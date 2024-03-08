using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Vector2 textCoord;
    public GameObject ground;
    public Texture2D splatTex;

    private Material myMat;

    //debug
    public Vector4 currentCoords;
    public Vector4 splatRgba;

    void Start()
    {
        myMat = GetComponent<Renderer>().material;
    }

    Vector4 PlayerPosToTexCoords()
    {
        float playerU = 0.0f;
        float playerV = 0.0f;

        Bounds groundBounds = ground.GetComponent<MeshRenderer>().bounds;

        float groundScaleX = Mathf.Abs(groundBounds.min.x - groundBounds.max.x);
        float groundScaleZ = Mathf.Abs(groundBounds.min.z - groundBounds.max.z);
        Debug.Log("scale x " + groundScaleX + " scale z " + groundScaleZ);

        float downLeftX = ground.transform.position.x - (groundScaleX / 2.0f);
        float upRightX = ground.transform.position.x + (groundScaleX / 2.0f);
        float offsetX = 0.0f;
        if (downLeftX < 0.0f)
        {
            //positive offset for corner to be in x = 0
            offsetX = -downLeftX;
        }
        float totalX = (upRightX + offsetX) - (downLeftX + offsetX);
        float playerX = (transform.position.x + offsetX) - (downLeftX + offsetX);
        playerU = playerX / totalX;

        float downLeftZ = ground.transform.position.z - (groundScaleZ / 2.0f);
        float upRightZ = ground.transform.position.z + (groundScaleZ / 2.0f);
        float offsetZ = 0.0f;
        if (downLeftZ < 0.0f)
        {
            //positive offset for corner to be in z = 0
            offsetZ = -downLeftZ;
        }
        float totalZ = (upRightZ + offsetZ) - (downLeftZ + offsetZ);
        float playerZ = (transform.position.z + offsetZ) - (downLeftZ + offsetZ);
        playerV = playerZ / totalZ;

        return new Vector4(playerU, playerV, 1.0f, 1.0f);

    }

    // Update is called once per frame
    void Update()
    {
        currentCoords = PlayerPosToTexCoords();
        myMat.color = splatTex.GetPixelBilinear(currentCoords.x, currentCoords.y);
    }
}
