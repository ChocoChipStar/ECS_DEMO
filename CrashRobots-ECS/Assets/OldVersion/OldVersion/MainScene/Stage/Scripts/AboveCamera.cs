using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AboveCamera : MonoBehaviour
{
    public Camera mainCamera;

    public int yDrawingArea = 30;
    [Range(1f, 50f)]
    public float cameraSize = 8;

    [SerializeField]
    private float rotationValue;

    [SerializeField]
    private float zValue;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        Camera.main.orthographicSize = cameraSize;
    }

    // Update is called once per frame
    void Update()
    {
        Camera.main.orthographicSize = cameraSize;

        mainCamera.transform.position = new Vector3(transform.position.x, transform.position.y + yDrawingArea, transform.position.z - zValue);
        mainCamera.transform.rotation = Quaternion.Euler(rotationValue, 0.0f, 0.0f);
    }
}
