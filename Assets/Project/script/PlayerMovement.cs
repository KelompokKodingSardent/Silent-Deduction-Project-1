using UnityEngine;

public class movement : MonoBehaviour
{
    public CharacterController controller;
    public Animator anim; 
    
    public float walkSpeed = 5f;  
    public float runSpeed = 10f;  
    public float jumpHeight = 3f; 
    public float gravity = -9.81f;
    
    // --- SISTEM DETEKSI TANAH ---
    public Transform groundCheck; 
    public float groundDistance = 0.8f; 
    public LayerMask groundMask; 
    public bool isGrounded; 

    Vector3 velocity;
    float currentSpeed;

    void Start()
    {
        currentSpeed = walkSpeed;
    }

    void Update()
    {
        // Sensor: Mengecek apakah menginjak tanah
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // Fitur Lari
        if (Input.GetKey(KeyCode.LeftShift)) { currentSpeed = runSpeed; }
        else { currentSpeed = walkSpeed; }

        // Fitur Jalan
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        
        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * currentSpeed * Time.deltaTime);

        // --- [BARU] LOGIKA ANIMASI MENGGUNAKAN BLEND TREE ---
        if (anim != null)
        {
            // Mengirim nilai pergerakan secara spesifik (-1 hingga 1) ke Animator
            anim.SetFloat("InputX", x);
            anim.SetFloat("InputY", z); 
        }
        // -----------------------------------------------------

// Fitur Lompat
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            if (anim != null)
            {
                anim.SetTrigger("lompatAktif");
            }
            
            // Menunda lonjakan fisik selama 0.2 detik (sesuaikan angka ini dengan animasi Anda)
            Invoke("TerapkanLompatan", 0.5f); 
        }

        // Menerapkan Gravitasi
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
    
    // --- [BARU] FUNGSI PENERAP LOMPATAN SETELAH JEDA ---
    void TerapkanLompatan()
    {
        if (isGrounded) // Pastikan karakter masih di tanah saat jeda selesai
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    // --- FITUR X-RAY UNTUK MELIHAT SENSOR ---
    void OnDrawGizmos()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundDistance);
        }
    }
}