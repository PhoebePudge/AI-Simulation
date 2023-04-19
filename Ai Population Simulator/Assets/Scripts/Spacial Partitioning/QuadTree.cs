using System.Collections;
using System.Collections.Generic; 
using UnityEngine;
using System.Linq;
using System;
//I orgionally based my quad tree of one create by Danny Goodayle, however I felt this solution was lacking. Instead I chose to follow a tutorial but world of zero who took this concept further.
//Although my initial structure was based of this tutorial, I make several custom functions to take this solution further from the origional
//World of Zero - https://worldofzero.com/videos/building-the-quadtree-lets-make-2d-voxel-terrain-part-1/
//Danny Goodayle - https://gist.github.com/MohHeader/270acd1224e35b89e9f411785ba43562#file-quadtreetest-cs-L12 
public class QuadT<TType> where TType : IComparable{
	//root node
	private QuadtreeNode<TType> node;

	//Max depth
	private int depth;

	//An event we can subscribe to for each update
	public event EventHandler QuadtreeUpdated;

	//Create our quad tree
	public QuadT(Vector2 position, Vector2 size, int depth) {
		node = new QuadtreeNode<TType>(position, size, depth, -1, null);
		//node.Subdivide(depth);
		this.depth = depth;

	}
	//Insert a position
	public void Insert(Vector2 position, TType value) {
		var leafnode = node.Subdivide(position, value, depth);
		leafnode.Data = value;
		NotifyQuadtreeUpdate();
	}
	//Insert a circle
	public void InsertCircle(Vector2 position, float radius, TType value) {
		var leafnodes = new LinkedList<QuadtreeNode<TType>>();
		node.CircleSubdivide(leafnodes, position, radius, value, depth);
		
		NotifyQuadtreeUpdate();
	}
	//Insert a square
	public void InsertSquare(Vector2 position, float radius, TType value) {
		var leafnodes = new LinkedList<QuadtreeNode<TType>>();
		node.SquareSubdivide(leafnodes, position, radius, value, depth);

		NotifyQuadtreeUpdate();
	}
	//Call update
	private void NotifyQuadtreeUpdate() {
        if (QuadtreeUpdated != null) {
			QuadtreeUpdated(this, new EventArgs());

		}
    }  
	public class QuadtreeNode<TType> where TType : IComparable {
		//its center position
		private Vector2 _position; 
		//Its bounds size
		private Vector2 _size; 
		//Children
		private QuadtreeNode<TType>[] _subNodes; 
		//Get out parent
		private QuadtreeNode<TType> _parent; 
		//Current depth
		private TType _data; 
		//Current depth (This decreases when you get furthe down)
		private int _depth; 
		//node location relative to our parent (e.g is it lower left, upper right...)
		private int _location;

		//Find the lowest data in a position (Used to sample collision)
		public QuadtreeNode<TType> LowestData(Vector2 location) {
			//If there are no more children, lets return this
			if (_subNodes == null) {
				return this;
			}
			 
			//Loop through each child
			for (int i = 0; i < _subNodes.Length; i++) {
				//If our location is within that childs bounds
				if (_subNodes[i].Contained(location)) {  
					//Search that child
					return _subNodes[i].LowestData(location);
				}
			}
			 
			//default
			return _subNodes[0].LowestData(location); 
		}

		//Initialise the node
		public QuadtreeNode(Vector2 position, Vector2 size, int depth, int index, QuadtreeNode<TType> parent, TType value = default(TType)) {
			this._position = position;
			this._size = size;
			this._depth = depth;
			this._data = value;
			this._location = index; 
			this._parent = parent;
		}
		//Return our depth
		public int Depth
        {
			get { return _depth; }
        }
		//Return our parent
		public QuadtreeNode<TType> Parent {
			get { return _parent; }
		}
		//Return our location from our parent
		public int SubDivideIndex {
			get {return _location; }
        } 
		//Return our child nodes
		public IEnumerable<QuadtreeNode<TType>> Nodes {
			get { return _subNodes; }
		}
		//Return our position
		public Vector3 Position {
			get { return _position; }
		}
		//Return our size
		public Vector2 Size {
			get { return _size; }
		}
		//Return our data
		public TType Data {
			get { return _data; }
			internal set { this._data = value; }
		}
		//Basic subdivision
		public QuadtreeNode<TType> Subdivide(Vector2 targetPosition, TType value, int depth = 0) {
			//If we are at the end, lets return
			if (depth == 0) {
				return this;
			}
			//Our location index
			int subDivideIndex = GetIndexOfPosition(targetPosition, _position);

			//If we dont have a child
			if (_subNodes == null) {
				//Create some children nodes
				_subNodes = new QuadtreeNode<TType>[4];

				for (int i = 0; i < _subNodes.Length; i++) {
					Vector2 newPos = _position;
					if ((i & 2) == 2) {
						newPos.y -= _size.y * 0.25f;
					} else {
						newPos.y += _size.y * 0.25f;
					}

					if ((i & 1) == 1) {
						newPos.x += _size.x * 0.25f;
					} else {
						newPos.x -= _size.x * 0.25f;
					}

					_subNodes[i] = new QuadtreeNode<TType>(newPos, _size * 0.5f, depth - 1, i, this);
				}
			}

			//Subdivide the child who is in the target position
			return _subNodes[subDivideIndex].Subdivide(targetPosition, value, depth - 1);
		}

		public void CircleSubdivide(LinkedList<QuadtreeNode<TType>> selectedNodes, Vector2 targetPosition, float radius, TType value, int depth = 0) {
			//If we are the end, return and add this on the list
			if (depth == 0) {
				this._data = value;
				selectedNodes.AddLast(this); 
				return;
			}
			int subDivideIndex = GetIndexOfPosition(targetPosition, _position);

			//If we dont have children
			if (_subNodes == null) {
				//Lets make children
				_subNodes = new QuadtreeNode<TType>[4];

				for (int i = 0; i < _subNodes.Length; i++) {
					Vector2 newPos = _position;
					if ((i & 2) == 2) {
						newPos.y -= _size.y * 0.25f;
					} else {
						newPos.y += _size.y * 0.25f;
					}

					if ((i & 1) == 1) {
						newPos.x += _size.x * 0.25f;
					} else {
						newPos.x -= _size.x * 0.25f;
					}

					_subNodes[i] = new QuadtreeNode<TType>(newPos, _size * 0.5f, depth - 1, i, this, Data);
				}
			}

			//Look through each child
			for (int i = 0; i < _subNodes.Length; i++) {
				//If its within our circle, lets subdivide it

				if (_subNodes[i].ContainedInCircle(targetPosition, radius)) {
					_subNodes[i].CircleSubdivide(selectedNodes, targetPosition, radius, value, depth - 1);
				}


			}

			//Trying to find if we need to unSubdivide (takes longer to add data, but makes sampling the quadtree quicker for us)
			var shouldReduce = true;
			var initialValue = _subNodes[0].Data;

			for (int i = 0; i < _subNodes.Length; i++) {
				shouldReduce &= initialValue.CompareTo(_subNodes[i].Data) == 0;
				shouldReduce &= _subNodes[i].isLeaf();
			}

			//Actually unsubdivide
			if (shouldReduce) {
				this._data = initialValue;
				_subNodes = null;
			} 
		}
		//doing the same as a circle subdivide but using a contained in square check
		public void SquareSubdivide(LinkedList<QuadtreeNode<TType>> selectedNodes, Vector2 targetPosition, float radius, TType value, int depth = 0) {
			if (depth == 0) {
				this._data = value;
				selectedNodes.AddLast(this);
				//yield return this;
				return;
			}
			int subDivideIndex = GetIndexOfPosition(targetPosition, _position);

			if (_subNodes == null) {

				_subNodes = new QuadtreeNode<TType>[4];

				for (int i = 0; i < _subNodes.Length; i++) {
					Vector2 newPos = _position;
					if ((i & 2) == 2) {
						newPos.y -= _size.y * 0.25f;
					} else {
						newPos.y += _size.y * 0.25f;
					}

					if ((i & 1) == 1) {
						newPos.x += _size.x * 0.25f;
					} else {
						newPos.x -= _size.x * 0.25f;
					}

					_subNodes[i] = new QuadtreeNode<TType>(newPos, _size * 0.5f, depth - 1, i, this, Data);
				}
			}
			for (int i = 0; i < _subNodes.Length; i++) {
				 
				if (_subNodes[i].ContainedInSquare(targetPosition, radius)) {
					_subNodes[i].SquareSubdivide(selectedNodes, targetPosition, radius, value, depth - 1);
				}


			}
			var shouldReduce = true;
			var initialValue = _subNodes[0].Data;

			for (int i = 0; i < _subNodes.Length; i++) {
				shouldReduce &= initialValue.CompareTo(_subNodes[i].Data) == 0;
				shouldReduce &= _subNodes[i].isLeaf();
			}

			if (shouldReduce) {
				this._data = initialValue;
				_subNodes = null;
			} 
		}

		
		//Is this square contained in our bounds
		public bool ContainedInSquare(Vector2 lookup, float r) {
			Vector2 difference = this._position - _position;
			//Debug.LogError(difference);
			if (lookup.x < this._position.x + (_size.x / 2f) + r) {
				if (lookup.x > this._position.x - (_size.x / 2f) - r) {
					if (lookup.y < this._position.y + (_size.y / 2f)+ r) {
						if (lookup.y > this._position.y - (_size.y / 2f) - r) {
							return true;
						}
					}


				}
			}
			return false;
		}
		//Is this sample contained in our bounds
		public bool Contained(Vector2 lookup) {
			if (lookup.x < this._position.x + (_size.x / 2f)) {
				if (lookup.x > this._position.x - (_size.x/2f)) {

					if (lookup.y < this._position.y + (_size.y/2f)) {
						if (lookup.y > this._position.y - (_size.y/2f)) {

							return true;
						}
					}
				}
			}
			return false;
		}
		//Is this circle contained in our bounds
		public bool ContainedInCircle(Vector2 position, float radius) {
			Vector2 difference = this._position - position;
			difference.x = MathF.Max(0, MathF.Abs(difference.x) - _size.x / 2);
			difference.y = MathF.Max(0, MathF.Abs(difference.y) - _size.y / 2);
			return difference.magnitude < radius;
		}
		//Is this a leaf node
		public bool isLeaf() {
			return Nodes == null;
		}
		//Find all leaf nodes bellow this node
		public IEnumerable<QuadtreeNode<TType>> GetLeafNodes() {
            if (isLeaf()) {
				yield return this;

			} else {
                if (Nodes != null) {
                    foreach (var node in Nodes) {
                        foreach (var leaf in node.GetLeafNodes()) {
							yield return leaf;
						} 
                    }
                }
            }
		}
	}
	//What index location would it be if we lookup this position
	private static int GetIndexOfPosition(Vector2 lookupPosition, Vector2 nodePosition) {
		int index = 0;

		index |= lookupPosition.y < nodePosition.y ? 2 : 0;
		index |= lookupPosition.x > nodePosition.x ? 1 : 0;

		return index;
	}
	//Get our root node
	public QuadtreeNode<TType> GetRoot() {
		return node;
	}
	//Call gettin a leaf nodes
	public IEnumerable<QuadtreeNode<TType>> GetLeafNodes() {
		return node.GetLeafNodes();
    }
}