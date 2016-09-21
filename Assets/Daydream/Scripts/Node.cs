using UnityEngine;

public class Node : IHeapItem<Node> {

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

    int heapIndex;
    public int HeapIndex {
        get {
            return heapIndex;
        }
        set {
            heapIndex = value;
        }
    }
        

    public Node parent;

    public Node(bool walkable, Vector3 worldPos, int posX, int posY) {
        this.walkable = walkable;
        this.worldPos = worldPos;
        this.posX = posX;
        this.posY = posY;
    }


    public int CompareTo(Node nodeToCompare) {
        int compare = fCost.CompareTo(nodeToCompare.fCost);
        if (compare == 0) {
            compare = hCost.CompareTo(nodeToCompare.hCost);
        }
        return -compare;
    }
}
