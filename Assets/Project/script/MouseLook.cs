using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public Transform playerBody;

    float xRotation = 0f;

    void Start()
    {
        // Mengunci kursor mouse di tengah layar
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Membatasi nengok atas-bawah

        // Rotasi kamera (atas/bawah)
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        // Rotasi badan (kiri/kanan)
        playerBody.Rotate(Vector3.up * mouseX);
    }
}