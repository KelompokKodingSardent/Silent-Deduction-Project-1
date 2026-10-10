using UnityEngine;

public class PickupSystem : MonoBehaviour
{
    public Camera playerCamera; // Kamera untuk menembak Raycast
    public Transform holdPosition; // Tempat benda diletakkan saat dipegang
    public float pickupRange = 3f; // Jarak jangkauan tangan
    public float throwForce = 10f; // Kekuatan lemparan

    private GameObject heldObject; // Menyimpan data benda yang sedang dipegang

    void Update()
    {
        // Jika tangan KOSONG
        if (heldObject == null)
        {
            // Mengecek apakah tombol E ditekan
            if (Input.GetKeyDown(KeyCode.E))
            {
                RaycastHit hit;
                // Menembakkan sinar laser tak terlihat dari kamera ke depan
                if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, pickupRange))
                {
                    // Jika benda yang kena laser memiliki Tag "Pickup"
                    if (hit.transform.CompareTag("Pickup"))
                    {
                        PickUpObject(hit.transform.gameObject);
                    }
                }
            }
        }
        else // Jika sedang MEMEGANG BENDA
        {
            // Mengecek apakah tombol R ditekan
            if (Input.GetKeyDown(KeyCode.R))
            {
                DropObject();
            }
        }
    }

    void PickUpObject(GameObject pickObj)
    {
        heldObject = pickObj; // Daftarkan benda ke memori

        // Matikan fisik (gravitasi dll) agar benda tidak meronta-ronta/jatuh saat dipegang
        Rigidbody rb = heldObject.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
        }

        // Pindahkan benda ke lokasi HoldPosition dan jadikan "anak" dari HoldPosition
        heldObject.transform.position = holdPosition.position;
        heldObject.transform.rotation = holdPosition.rotation;
        heldObject.transform.parent = holdPosition;
    }

    void DropObject()
    {
        Rigidbody rb = heldObject.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false; // Nyalakan gravitasi lagi
            // Beri gaya dorong ke depan untuk melempar
            rb.AddForce(playerCamera.transform.forward * throwForce, ForceMode.Impulse);
        }

        // Lepaskan benda dari HoldPosition
        heldObject.transform.parent = null;
        heldObject = null; // Kosongkan tangan
    }
}