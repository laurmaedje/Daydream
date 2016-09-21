using UnityEngine;
using System.Collections;

public class Bomb : MonoBehaviour {

    void Start() {
        Invoke("Destroy", 10f);
    }

    void OnCollisionEnter(Collision col) {

        if (col.gameObject.tag != "Player") {
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
