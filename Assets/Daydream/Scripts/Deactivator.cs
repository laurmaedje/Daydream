using UnityEngine;

public class Deactivator : MonoBehaviour {

    public enum Action {
        Destroy, 
        Deactivate
    }

    [SerializeField] float delay;
    [SerializeField] Action type;

    float timer;

	// Use this for initialization
	void Start () {
        timer = delay;
	}
	
	// Update is called once per frame
	void Update () {

        if (timer > 0) {
            timer -= Time.deltaTime;
        }
        else {
            if (type == Action.Deactivate) {
                timer = delay;
                gameObject.SetActive(false);
            } else {
                Destroy(gameObject);
            }

        }
    }
}
