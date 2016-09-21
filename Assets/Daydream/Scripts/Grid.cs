using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Grid : MonoBehaviour {

    static Grid instance;

    public LayerMask obstacleMask;
    public Vector2 gridWorldSize;
    public float nodeRadius;

    Node[,] grid;
    int gridSizeX, gridSizeY;
    public int MaxSize {
        get {
            return gridSizeX * gridSizeY;
        }
    }

    void Awake() {
        instance = this;

        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / (nodeRadius * 2));
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / (nodeRadius * 2));
        CreateGrid();
    }

    public static void CreateGrid() {
        instance.grid = new Node[instance.gridSizeX, instance.gridSizeY];

        Vector3 worldBottomLeft = instance.transform.position - Vector3.right * instance.gridWorldSize.x / 2 - Vector3.forward * instance.gridWorldSize.y / 2;

        for (int x = 0; x < instance.gridSizeX; x++) {
            for (int y = 0; y < instance.gridSizeY; y++) {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * instance.nodeRadius * 2 + instance.nodeRadius) + Vector3.forward * (y * instance.nodeRadius * 2 + instance.nodeRadius);
                bool walkable = !(Physics.CheckCapsule(worldPoint, new Vector3(worldPoint.x, worldPoint.y + 2, worldPoint.z), instance.nodeRadius * 2, instance.obstacleMask));
                instance.grid[x, y] = new Node(walkable, worldPoint, x, y);
            }
        }
    }

    public List<Node> GetNeighbours(Node node) {
        List<Node> neighbours = new List<Node>();

        for (int x = -1; x <= 1; x++) {
            for (int y = -1; y <= 1; y++) {
                if (x == 0 && y == 0)
                    continue;

                int checkX = node.posX + x;
                int checkY = node.posY + y;

                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY) {
                    neighbours.Add(grid[checkX, checkY]);
                }
            }
        }

        return neighbours;
    }

    public Node NodeFromWorldPoint(Vector3 worldPosition) {
        float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentY = (worldPosition.z + gridWorldSize.y / 2) / gridWorldSize.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);
        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
        return grid[x, y];
    }

    void OnDrawGizmos() {
        if (grid == null) return;

        /*Vector3 worldBottomLeft = instance.transform.position - Vector3.right * instance.gridWorldSize.x / 2 - Vector3.forward * instance.gridWorldSize.y / 2;
        for (int x = 0; x < instance.gridSizeX; x++) {
            for (int y = 0; y < instance.gridSizeY; y++) {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * instance.nodeRadius * 2 + instance.nodeRadius) + Vector3.forward * (y * instance.nodeRadius * 2 + instance.nodeRadius);
                Gizmos.DrawSphere(worldPoint, instance.nodeRadius * 2);
            }
        }*/

        foreach(var n in grid) {
            Gizmos.color = n.walkable ? Color.green : Color.red;
            Gizmos.DrawCube(n.worldPos, nodeRadius * Vector3.one);
        }
    }
}
