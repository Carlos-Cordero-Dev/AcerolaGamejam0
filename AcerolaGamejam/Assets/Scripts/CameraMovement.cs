using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    //combination of: https://forum.unity.com/threads/third-person-camera-movement-script.858673/
    //and: https://docs.unity3d.com/ScriptReference/CharacterController.Move.html

    public float sensivityX = 4.0f;
    public float sensivityY = 4.0f;
    public float distance = 10.0f;

    public Transform lookAt;
    public Transform Player;

    private const float YMin = -50.0f;
    private const float YMax = 50.0f;
    private float currentX = 180.0f;
    private float currentY = 0.0f;

    private void Start()
    {
        currentY = Player.transform.position.y + distance;
        currentY = Mathf.Clamp(currentY, YMin, YMax);

        Vector3 Direction = new Vector3(0, 0, distance);
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);

        //transform.position = lookAt.position + rotation * Direction;
        transform.position = lookAt.position + rotation * Direction;

        transform.LookAt(lookAt.position);
    }
    // Update is called once per frame
    void LateUpdate()
    {

        if (Input.GetMouseButton(1))
        {
            currentX += Input.GetAxis("Mouse X") * sensivityX * Time.deltaTime;
            currentY += Input.GetAxis("Mouse Y") * sensivityY * Time.deltaTime;

            currentY = Mathf.Clamp(currentY, YMin, YMax);

        }

        Vector3 Direction = new Vector3(0, 0, -distance);
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        transform.position = lookAt.position + rotation * Direction;
        transform.LookAt(lookAt.position);

    }
}