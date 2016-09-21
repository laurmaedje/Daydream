using UnityEngine;
using System.Collections;

public class Bomb : MonoBehaviour {

    bool collided = false;

    void Start() {
        Invoke("Destroy", 10f);
    }

    void OnCollisionEnter(Collision col) {
        if (collided) return;

        collided = true;


        if (col.gameObject.tag != "Player") {
            foreach (Collider collider in Physics.OverlapSphere(transform.position, 3f)) {
                if (collider.tag == "Destroyable") {
                    Transform parent = collider.transform.parent;
                    parent.GetComponent<Destruct>().DestructAround(transform.position, 3f);
                    break;
                }
            }

            Destroy(GetComponent<MeshRenderer>());
            Destroy(GetComponent<SphereCollider>());
            Destroy(GetComponent<Rigidbody>());
            transform.GetChild(0).gameObject.SetActive(true);
            Invoke("Destroy", 2f);

                 
        }

    }

    void Destroy() {
        Destroy(gameObject);
    }
}
