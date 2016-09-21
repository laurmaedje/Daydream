using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Player : MonoBehaviour {

    public enum Mode {
        MOVEMENT,
        BOMB
    }

    [SerializeField] GameObject clickEffect;
    [SerializeField] GameObject ModeInterface;

    [SerializeField] float moveSpeed;
    [SerializeField] float rotateSpeed;
    [SerializeField] float volume;

    [SerializeField] GameObject bombPrefab;

    Mode mode;
    AudioSource source;

	// Use this for initialization
	void Start () {
        source = GetComponent<AudioSource>();
        SetMode(0);
	}

    public void SetMode(int mode) {
        this.mode = (Mode)mode;

        for(int i = 0; i < System.Enum.GetNames(typeof(Mode)).Length; i++) {
            if (i == mode) {
                ModeInterface.transform.GetChild(i).GetComponent<Image>().color = Color.yellow;
            } else {
                ModeInterface.transform.GetChild(i).GetComponent<Image>().color = Color.white;
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
        bool hoverGui = UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(-1);

        // Change mode
        if (Input.GetKeyDown(KeyCode.Tab)) {
            SetMode(((int)mode + 1) % System.Enum.GetNames(typeof(Mode)).Length);
        }

        // Move
        if (Input.GetMouseButtonDown(0) && mode == Mode.MOVEMENT && !hoverGui) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit) && hit.transform.tag == "Ground") {
                clickEffect.transform.position = new Vector3(hit.point.x, 0.5f, hit.point.z);
                clickEffect.transform.GetChild(0).GetComponent<ParticleSystem>().Play();
                Vector3[] path = Pathfinder.FindPath(transform.position, hit.point);
                StopCoroutine("FollowPath");
                if (path != null) {
                    StartCoroutine("FollowPath", path);
                } else {
                    StartCoroutine("FadeSoundOut");
                }                    
            }
        } else if (Input.GetMouseButtonDown(0) && mode == Mode.BOMB && !hoverGui) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit)) {
                Vector3 dir = hit.point - transform.position;
                dir.Normalize();
                GameObject bomb = Instantiate(bombPrefab, transform.position, Quaternion.identity) as GameObject;
                bomb.GetComponent<Rigidbody>().AddForce(dir * 400f);
            }
        }

	}

    enum MovementAction {
        MOVE,
        ROTATE
    }

    IEnumerator FollowPath(Vector3[] path) {
        StartCoroutine("FadeSoundIn");

        int index = 0;
        MovementAction action = MovementAction.ROTATE;
        Vector3 waypoint = Vector3.zero;
        do {
            if (action == MovementAction.ROTATE) {
                if (index >= path.Length)
                    break;

                // Assign waypoint
                waypoint = path[index];
                waypoint.y = transform.position.y;
                index++;
                float angle = Vector3.Angle(waypoint - transform.position, transform.forward);
                float sign = -Mathf.Sign(Vector3.Cross(waypoint - transform.position, transform.forward).y);

                // Rotate towards waypoint
                float rotation = 0f;
                while (rotation < angle) {
                    if (source.volume < volume)
                        source.volume += 10f * Time.deltaTime;

                    MoveWheels();

                    transform.Rotate(0, sign * rotateSpeed * Time.deltaTime, 0f);
                    rotation += rotateSpeed * Time.deltaTime;
                    yield return null;
                }
                action = MovementAction.MOVE;

            } else {
                transform.position = Vector3.MoveTowards(transform.position, waypoint, moveSpeed * Time.deltaTime);
                MoveWheels();
                if (transform.position == waypoint) {
                    action = MovementAction.ROTATE;
                }
                yield return null;
            }
                

        } while (true);

        StartCoroutine("FadeSoundOut");
    }

    IEnumerator FadeSoundIn() {
        if (!source.isPlaying) {
            source.Play();
            source.volume = 0;
            while (source.volume < volume) {
                source.volume += 10f * Time.deltaTime;
                yield return null;
            }
        }      
    }

    IEnumerator FadeSoundOut() {
        if (source.isPlaying) {
            while (source.volume > 0f) {
                source.volume -= 10f * Time.deltaTime;
                yield return null;
            }
            source.Stop();
        }
        
    }

    void MoveWheels() {
        transform.GetChild(1).transform.Rotate(150f * Time.deltaTime, 0f, 0f);
        transform.GetChild(2).transform.Rotate(150f * Time.deltaTime, 0f, 0f);

        transform.GetChild(3).transform.Rotate(300f * Time.deltaTime, 0f, 0f);
        transform.GetChild(4).transform.Rotate(300f * Time.deltaTime, 0f, 0f);

        transform.GetChild(5).transform.Rotate(300f * Time.deltaTime, 0f, 0f);
        transform.GetChild(6).transform.Rotate(300f * Time.deltaTime, 0f, 0f);
    }
}
