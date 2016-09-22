using UnityEngine;

public class FollowCam : MonoBehaviour {

    [SerializeField] Transform target;
    [SerializeField] float distance;
    [SerializeField] float rotateSpeed;
	
	void LateUpdate() {
        Rotate();
        Follow();
    }

    void Rotate() {
        if (Input.GetKey(KeyCode.Mouse2)) {
            transform.RotateAround(target.transform.position, Vector3.up, Input.GetAxis("Mouse X") * rotateSpeed * Time.deltaTime);
        }
    }

    void Follow() {

        var heightBuffer = transform.position.y;

        // Convert the angle into a rotation
        var currentRotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);

        // Set the position of the camera on the x-z plane to: distance meters behind the target
        transform.position = target.position;
        transform.position -= currentRotation * Vector3.forward * distance;

        // Set the height of the camera
        transform.position = new Vector3(transform.position.x, heightBuffer, transform.position.z);

        // Always look at the target
        transform.LookAt(target);
    }
    
}
