using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//A* pathfinding
public class Pathfinder : MonoBehaviour {
	//Our agents can call this to find a path for them to follow
	public static List<GridPartition> FindPath(Vector2Int startPos, Vector2Int targetPos) {

		//Getting our start and target partition
		GridPartition startNode = Init.gridManager.GetPartition(startPos);
		GridPartition targetNode = Init.gridManager.GetPartition(targetPos);

		//If they are valid
		if (startNode != null & targetNode != null) {
			List<GridPartition> openSet = new List<GridPartition>();
			HashSet<GridPartition> closedSet = new HashSet<GridPartition>();
			openSet.Add(startNode);

			//while we still have paritions remaining in the open set
			while (openSet.Count > 0) {
				GridPartition node = openSet[0];
				for (int i = 1; i < openSet.Count; i++) {
					if (openSet[i].fCost < node.fCost || openSet[i].fCost == node.fCost) {
						if (openSet[i].hCost < node.hCost)
							node = openSet[i];
					}
				}

				openSet.Remove(node);
				closedSet.Add(node);

				//If we are at end of path
				if (node == targetNode) {

					return RetracePath(startNode, targetNode);
				}

				//Looking at each neighbour
				foreach (GridPartition neighbour in Init.gridManager.GetNeighboursPartitions(node.Position)) {
					//valid
					if (neighbour != null) {
						//walkable
						if (!neighbour.walkable || closedSet.Contains(neighbour)) {
							continue;
						}

						//get our cost
						int newCostToNeighbour = node.gCost + GetDistance(node, neighbour);
						//if its cheaper to use, lets use it
						if (newCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour)) {
							neighbour.gCost = newCostToNeighbour;
							neighbour.hCost = GetDistance(neighbour, targetNode);
							neighbour.parent = node;

							if (!openSet.Contains(neighbour))
								openSet.Add(neighbour);
						}
					}
				}

			}
		}
		return null;
	}

	//Retracing the path we took
	static List<GridPartition> RetracePath(GridPartition startNode, GridPartition endNode) {
		List<GridPartition> path = new List<GridPartition>();
		GridPartition currentNode = endNode;

		while (currentNode != startNode) {
			path.Add(currentNode);
			currentNode = currentNode.parent;
		}
		path.Reverse();

		return path;

	}

	//Get the distance between partitions
	static int GetDistance(GridPartition nodeA, GridPartition nodeB) {
		int dstX = Mathf.Abs(nodeA.Position.x - nodeB.Position.x);
		int dstY = Mathf.Abs(nodeA.Position.y - nodeB.Position.y);

		if (dstX > dstY)
			return 14 * dstY + 10 * (dstX - dstY);
		return 14 * dstX + 10 * (dstY - dstX);
	}
}
