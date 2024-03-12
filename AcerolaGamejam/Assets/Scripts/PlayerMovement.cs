using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //combination of: https://forum.unity.com/threads/third-person-camera-movement-script.858673/
    //and: https://docs.unity3d.com/ScriptReference/CharacterController.Move.html

    public float playerSpeed = 2.0f;
    public float jumpHeight = 1.0f;
    public float gravityValue = -9.81f;
    public float groundCheckRaycastDistance = 0.1f;

    public Transform Cam;

    private CharacterController controller;
    private Vector3 playerVelocity;
    [SerializeField]
    private bool groundedPlayer;


    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    bool CheckIsPlayerGrounded()
    {
        LayerMask groundLayer = 1 << 6;

        RaycastHit hit;
        if (Physics.Raycast(transform.position, -transform.up, out hit, groundCheckRaycastDistance, groundLayer))
        {
            return true;
        }
        return false;
    }

    void Update()
    {
        groundedPlayer = CheckIsPlayerGrounded();
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }


        float Horizontal = Input.GetAxis("Horizontal") ;
        float Vertical = Input.GetAxis("Vertical");

        Vector3 move = Cam.transform.right * Horizontal + Cam.transform.forward * Vertical;
        move.y = 0.0f;
        move.Normalize();

        controller.Move(move * playerSpeed * Time.deltaTime);

        if (move != Vector3.zero)
        {
            gameObject.transform.forward = move;
        }

        if (Input.GetButtonDown("Jump") && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);

        if (playerVelocity.magnitude != 0f)
        {
            transform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * Cam.GetComponent<CameraMovement>().sensivityX * Time.deltaTime);


            Quaternion CamRotation = Cam.rotation;
            CamRotation.x = 0f;
            CamRotation.z = 0f;

            transform.rotation = Quaternion.Lerp(transform.rotation, CamRotation, 0.1f);

        }
    }
}