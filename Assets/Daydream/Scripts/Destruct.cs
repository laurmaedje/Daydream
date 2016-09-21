using UnityEngine;

public class Destruct : MonoBehaviour {

    public void DestructAround(Vector3 center, float radius) {

        // Check for every block if it's center is in the explosion radius
        for (int i = 0; i < transform.childCount; i++) {
            Vector3 point = transform.GetChild(i).localPosition;
            point = transform.TransformPoint(point);
            float distance = Vector3.Distance(point, center);

            if (distance < radius) {
                Destroy(transform.GetChild(i).gameObject);
            }
        }

        Invoke("RecreateGrid", .1f);
    }

    void RecreateGrid() {
        Grid.CreateGrid();
    }

}
