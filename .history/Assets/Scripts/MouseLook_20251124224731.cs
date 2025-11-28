using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public Transform playerBody;
    public Weapon weaponScript; // ⚠️ Ezt az Editorban be kell állítani!

    private float xRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Vertikális forgás (fel-le) - recoil hozzáadása!
        xRotation -= mouseY;
        
        // Ha van weaponScript, adjuk hozzá a recoil-t
        float recoilOffset = 0f;
        if (weaponScript != null)
        {
            recoilOffset = weaponScript.GetRecoil();
        }
        
        xRotation = Mathf.Clamp(xRotation - recoilOffset, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Horizontális forgás (jobbra-balra)
        playerBody.Rotate(Vector3.up * mouseX);
    }
}