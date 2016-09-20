using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    [SerializeField] GameObject clickEffect;

    [SerializeField] float moveSpeed;
    [SerializeField] float rotateSpeed;
    [SerializeField] float volume;

    AudioSource source;

	// Use this for initialization
	void Start () {
        source = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetMouseButtonDown(0)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit) && hit.transform.tag == "Ground") {
                StopCoroutine("MoveTo");
                StartCoroutine("MoveTo", new Vector3(hit.point.x, 1f, hit.point.z));
                clickEffect.transform.position = new Vector3(hit.point.x, 1f, hit.point.z);
                clickEffect.transform.GetChild(0).GetComponent<ParticleSystem>().Play();
            }
        }

	}

    IEnumerator MoveTo(Vector3 target) {


        if (!source.isPlaying) {
            source.Play();
            source.volume = 0;
        }

        // Rotate
        float angle = Vector3.Angle(target - transform.position, transform.forward);
        float sign = -Mathf.Sign(Vector3.Cross(target - transform.position, transform.forward).y);

        print(sign);

        float rotation = 0f;
        while(rotation < angle) {
            if(source.volume < volume)
                source.volume += 10f * Time.deltaTime;

            MoveWheels(false, sign);

            transform.Rotate(0, sign * rotateSpeed * Time.deltaTime, 0f);
            rotation += rotateSpeed * Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(0.15f);

        // Move
        Vector3 dir = target - transform.position;
        dir.Normalize();
        dir.y = 0f;
        float distance = Vector3.Distance(target, transform.position);
        float moved = 0f;

        while (moved < distance) {
            MoveWheels(true, 0f);

            transform.position += moveSpeed * dir * Time.deltaTime;
            moved += moveSpeed * Time.deltaTime;
            yield return null;
        }

        while (source.volume > 0f) {
            source.volume -= 10f * Time.deltaTime;
            yield return null;
        }
    }

    // TODO realismus ..
    void MoveWheels(bool type, float dir) {
        float m1 = 1f;
        float m2 = 1f;
        if (!type) {
            m1 = dir;
            m2 = dir;
        }
         
        transform.GetChild(1).transform.Rotate(m1 * 150f * Time.deltaTime, 0f, 0f);
        transform.GetChild(2).transform.Rotate(m2 * 150f * Time.deltaTime, 0f, 0f);

        transform.GetChild(3).transform.Rotate(m1 * 300f * Time.deltaTime, 0f, 0f);
        transform.GetChild(4).transform.Rotate(m2 * 300f * Time.deltaTime, 0f, 0f);

        transform.GetChild(5).transform.Rotate(m1 * 300f * Time.deltaTime, 0f, 0f);
        transform.GetChild(6).transform.Rotate(m2 * 300f * Time.deltaTime, 0f, 0f);
    }
}
