using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Pathfinder : MonoBehaviour {

    static Pathfinder pathfinder;
    Grid grid;

    void Awake() {
        grid = GetComponent<Grid>();
        pathfinder = this;
    }

    public static Vector3[] FindPath(Vector3 startPos, Vector3 targetPos) {
        Node startNode = pathfinder.grid.NodeFromWorldPoint(startPos);
        Node targetNode = pathfinder.grid.NodeFromWorldPoint(targetPos);

        Heap<Node> openSet = new Heap<Node>(pathfinder.grid.MaxSize);
        HashSet<Node> closedSet = new HashSet<Node>();
        openSet.Add(startNode);

        while (openSet.Count > 0) {
            Node node = openSet.RemoveFirst();
            closedSet.Add(node);

            if (node == targetNode) {
                return pathfinder.RetracePath(startNode, targetNode);
            }

            foreach (Node neighbour in pathfinder.grid.GetNeighbours(node)) {
                if (!neighbour.walkable || closedSet.Contains(neighbour)) {
                    continue;
                }

                int newCostToNeighbour = node.gCost + pathfinder.GetDistance(node, neighbour);
                if (newCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour)) {
                    neighbour.gCost = newCostToNeighbour;
                    neighbour.hCost = pathfinder.GetDistance(neighbour, targetNode);
                    neighbour.parent = node;

                    if (!openSet.Contains(neighbour))
                        openSet.Add(neighbour);
                    else
                        openSet.UpdateItem(neighbour);
                }
            }
        }

        return null;
    }

    Vector3[] RetracePath(Node startNode, Node endNode) {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode) {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }

        Vector3[] waypoints = SimplifyPath(path);
        Array.Reverse(waypoints);
        return waypoints;

    }

    Vector3[] SimplifyPath(List<Node> path) {
        List<Vector3> waypoints = new List<Vector3>();
        Vector2 oldDirection = Vector2.zero;

        for (int i = 1; i < path.Count; i++) {
            Vector2 newDirection = new Vector2(path[i - 1].posX - path[i].posX, path[i - 1].posY - path[i].posY);
            if (newDirection != oldDirection) {
                waypoints.Add(path[i].worldPos);
            }
            oldDirection = newDirection;
        }

        return waypoints.ToArray();
    }

    int GetDistance(Node nodeA, Node nodeB) {
        int dstX = Mathf.Abs(nodeA.posX - nodeB.posX);
        int dstY = Mathf.Abs(nodeA.posY - nodeB.posY);

        if (dstX > dstY)
            return 14 * dstY + 10 * (dstX - dstY);
        return 14 * dstX + 10 * (dstY - dstX);
    }
}
