using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam_MouseLook : MonoBehaviour
{
    [SerializeField] float mouseSensitivity = 100f;
    [SerializeField] float maxHeadUp = -70;
    [SerializeField] float maxHeadDown = 50;

    Vector2 mouseAxis;
    Transform playerTransform;
    float xRotation;
    private void Start()
    {
        playerTransform = transform.parent;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        mouseAxis.x = Input.GetAxis("Mouse X");
        mouseAxis.y = Input.GetAxis("Mouse Y");
        mouseAxis = mouseAxis * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseAxis.y;
        xRotation = Mathf.Clamp(xRotation, maxHeadUp,maxHeadDown);

        transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        playerTransform.Rotate(Vector3.up * mouseAxis.x);
    }
}
