using UnityEngine;

public class Node {

    public bool walkable;
    public Vector3 worldPos;
    public int posX;
    public int posY;

    public int gCost;
    public int hCost;
    public int fCost {
        get {
            return gCost + hCost;
        }
    }

    public Node parent;

    public Node(bool walkable, Vector3 worldPos, int posX, int posY) {
        this.walkable = walkable;
        this.worldPos = worldPos;
        this.posX = posX;
        this.posY = posY;
    }


}
