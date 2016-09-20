using UnityEngine;

public class Grid : MonoBehaviour {

    public LayerMask obstacleMask;
    public Vector2 gridWorldSize;
    public float nodeRadius;

    Node[,] grid;
    int gridSizeX, gridSizeY;

    void Awake() {
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / (nodeRadius * 2));
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / (nodeRadius * 2));
        CreateGrid();
    }

    void CreateGrid() {
        grid = new Node[gridSizeX, gridSizeY];

        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;

        for (int x = 0; x < gridSizeX; x++) {
            for (int y = 0; y < gridSizeY; y++) {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeRadius * 2 + nodeRadius) + Vector3.forward * (y * nodeRadius * 2 + nodeRadius);
                bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, obstacleMask));
                grid[x, y] = new Node(walkable, worldPoint, x, y);
            }
        }
    }

    void OnDrawGizmos() {
        if (grid == null) return;

        foreach (Node n in grid) {
            Gizmos.color = n.walkable ? Color.green : Color.red;
            Gizmos.DrawCube(n.worldPos, Vector3.one * (nodeRadius * 2f - 0.1f));
        }
    }
}
