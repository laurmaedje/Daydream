using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    [SerializeField] GameObject clickEffect;

    [SerializeField] float moveSpeed;
    [SerializeField] float rotateSpeed;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetMouseButtonDown(0)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit)) {
                StopCoroutine("MoveTo");
                StartCoroutine("MoveTo", new Vector3(hit.point.x, 1f, hit.point.z));
                clickEffect.transform.position = new Vector3(hit.point.x, 1f, hit.point.z);
                clickEffect.transform.GetChild(0).GetComponent<ParticleSystem>().Play();
            }
        }

	}

    IEnumerator MoveTo(Vector3 target) {

        // Rotate
        float angle = Vector3.Angle(target - transform.position, transform.forward);
        float sign = -Mathf.Sign(Vector3.Cross(target - transform.position, transform.forward).y); 
        float rotation = 0f;
        while(rotation < angle) {
            transform.Rotate(0, sign * rotateSpeed * Time.deltaTime, 0f);
            rotation += rotateSpeed * Time.deltaTime;
            yield return null;
        }

        // Move
        Vector3 dir = target - transform.position;
        dir.y = 0f;
        float distance = Vector3.Distance(target, transform.position);
        float moved = 0f;
        dir.Normalize();
        while (moved < distance) {
            transform.position += moveSpeed * dir * Time.deltaTime;
            moved += moveSpeed * Time.deltaTime;
            yield return null;
        }   
    }
}
