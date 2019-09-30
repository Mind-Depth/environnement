using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Camera cam;
    public float sensitivity = 15f;
    public Rigidbody Rigid;
    public float MoveSpeed = 2f;

    private float rotationY = 0f;

    private void Start()
    {
        Teleporter._instance.player = this.transform;
    }

    void Update()
    {
        float rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivity;
        rotationY += Input.GetAxis("Mouse Y") * sensitivity;
        rotationY = Mathf.Clamp(rotationY, -80, 80);

        cam.transform.localEulerAngles = new Vector3(-rotationY, 0, 0);
        transform.localEulerAngles = new Vector3(0, rotationX, 0);

        Rigid.MovePosition(transform.position + (transform.forward * Input.GetAxis("Vertical") * MoveSpeed * Time.deltaTime) + (transform.right * Input.GetAxis("Horizontal") * MoveSpeed * Time.deltaTime));
    }
}
