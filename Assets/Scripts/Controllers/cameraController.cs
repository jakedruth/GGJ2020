using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraController : MonoBehaviour
{
    public Transform targetToFollow;
    [MinMaxRange(0.1f, 10)]
    public RangedFloat zoomMinMax;
    [Range(0, 1)]
    public float zoomPercentage; // clamp 0 to 1
    public float zoomSpeed;
    public float moveSpeed;

    private Camera camera;

    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        controlZoom();
        followTarget();
    }
    public void setNewTarget(Transform newTarget)
    {
        targetToFollow = newTarget;
    }
    private void controlZoom()
    {
        if (Input.GetAxis("Mouse ScrollWheel") != 0f)
        {
            zoomPercentage -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
            zoomPercentage = Mathf.Clamp01(zoomPercentage);
            camera.orthographicSize = zoomMinMax.Lerp(zoomPercentage);
        }
    }
    private void followTarget()
    {
        transform.position = Vector3.Lerp(transform.position, 
            new Vector3(targetToFollow.position.x, targetToFollow.position.y, transform.position.z), 
            moveSpeed * Time.deltaTime);
    }
}
