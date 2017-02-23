using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public float zoomSpeed = 40f;
    public float maxZoom = 1f;
    public float minZoom = 100f;

    public int xMargin = 10, yMargin = 10;

    public GameObject worldControllerObject;
    public WorldController worldController;

    // Use this for initialization
    void Start () {
        if (worldControllerObject == null)
        {
            Debug.LogError("No WorldControllerObject");
        }
        worldController = worldControllerObject.GetComponent<WorldController>();
        if(worldController == null)
        {
            Debug.LogError("No WorldController");
        }
        transform.position = new Vector3(worldController.world.size.x / 2, -10, -40);
        transform.rotation = Quaternion.Euler(-60, 0, 0);
	}
	
	// Update is called once per frame
	void Update () {
        // Camera pan map left and right
        float hscroll = Input.GetAxis("Horizontal");
        float vscroll = Input.GetAxis("Vertical");
        // Camera rotate
        float rotate = Input.GetAxis("Rotation");
        float tilt = Input.GetAxis("Tilt");
        // Camera zoom
        float zoom = zoomSpeed * Input.GetAxis("Zoom");

        // Mouse drag
        if (Input.GetKey(KeyCode.Mouse0))
        {
        }
        else if (Input.GetKey(KeyCode.Mouse1))
        {
            rotate -= Input.GetAxis("Mouse X");
            tilt -= Input.GetAxis("Mouse Y");
        }
        else if (Input.GetKey(KeyCode.Mouse2))
        {
            hscroll -= Input.GetAxis("Mouse X");
            vscroll -= Input.GetAxis("Mouse Y");
        }

        float speedScale = Mathf.Clamp(-transform.position.z / 10f, 1, 7); ;

        // Camera rotate: rotate along x axis, and tilt along vertical plane (with normal right)
        transform.rotation = Quaternion.AngleAxis(rotate, Vector3.forward) * transform.rotation;
        transform.rotation *= Quaternion.AngleAxis(-tilt, Vector3.right);

        // Horizontal translation
        transform.Translate(hscroll * speedScale, 0, 0);
        // Vertical movement, forward camera but projected onto plane (use mean to avoid corner cases)
        Vector3 position = transform.position;
        position += vscroll * speedScale * (Vector3.ProjectOnPlane(transform.forward, Vector3.forward) +
            Vector3.ProjectOnPlane(transform.up, Vector3.forward)).normalized;
        // Zoom
        position += transform.forward * zoom * speedScale;

        // Clamp to avoid too far camera
        // TODO: dynamic clamp
        position.x = Mathf.Clamp(position.x, -xMargin, worldController.world.size.x + xMargin);
        position.y = Mathf.Clamp(position.y, -yMargin, worldController.world.size.y + yMargin);
        position.z = Mathf.Clamp(position.z, -minZoom, -maxZoom);
        transform.position = position;
    }
}
