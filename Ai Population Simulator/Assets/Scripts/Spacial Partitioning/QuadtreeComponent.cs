using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadtreeComponent : MonoBehaviour
{ 
    //The max depth we can go to
    public int Depth = 10; 

    //Our tree (We use this for static collision detection)
    public QuadT<bool> Collisionquadtree;  
     
    //We use this in the custom window to show the entire
    public static bool ShowAllQuadTree = false;

    //Called in init
    public void Create() {
        //Get our bounds
        int width = Init.instance.GlobalData.SpawningBounds.x;
        int height = Init.instance.GlobalData.SpawningBounds.y;

        //Create our collision tree
        Collisionquadtree = new QuadT<bool>(new Vector2(this.transform.position.x, this.transform.position.z), new Vector2(height, width), Depth);

        //Subscribe an update function
        Collisionquadtree.QuadtreeUpdated += (obj, args) => { UpdateGeneration(); };
         
    }
    private void UpdateGeneration() { 
        //This is not used anymore but was very useful during testing
    }
    private void OnDrawGizmos() {
        if (ShowAllQuadTree) {
            if (Collisionquadtree != null) { 
                //Start drawing root node
                DrawNode(Collisionquadtree.GetRoot()); 
            }
        }

    }
    //The colours we lerp between for gizmos
    private Color _minColor = new Color(1, 1, 1, 1f);
    private Color _maxColor = new Color(0, 0.5f, 1, 0.25f); 
    public void DrawNode(QuadT<bool>.QuadtreeNode<bool> node, int nodeDepth = 0) {
        //If we are not at the end leaf
        if (!node.isLeaf()) {
            //And everything is valid
            if (node.Nodes != null) {
                //Foreach subnodes
                foreach (var subnode in node.Nodes) {
                    //If its valid
                    if (subnode != null) { 
                        //Draw that node
                        DrawNode(subnode, nodeDepth + 1);
                    }
                }
            }
        } 

        //If this is collidable
        if (node.Data == true) { 
            Gizmos.color = Color.blue; 
            Gizmos.DrawCube(new Vector3(node.Position.x, 0, node.Position.y), new Vector3(node.Size.x, 0.1f, node.Size.y));
        //If its not collidable
        } else { 
            Gizmos.color = Color.Lerp(_minColor, _maxColor, nodeDepth / (float)Depth);
            Gizmos.DrawWireCube(new Vector3(node.Position.x, 0, node.Position.y), new Vector3(node.Size.x, 0.1f, node.Size.y)); 
        }
    }  
}
