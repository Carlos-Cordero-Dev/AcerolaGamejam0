using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PerlinVisualization : MonoBehaviour
{
    private int imgSize;
    private int pixelsPerCell;
    private RawImage image;
    private int gridSize = 10;
    private Texture2D texture;
    private void Awake()
    {
        image = GetComponent<RawImage>();
        imgSize = Mathf.RoundToInt(image.GetComponent<RectTransform>().sizeDelta.x);
    }

    private void Start()
    {
        texture = new Texture2D(imgSize, imgSize);
        texture.filterMode = FilterMode.Point;
        pixelsPerCell = imgSize / gridSize;
        SetTexture();
    }
    private void Update()
    {
        if (GameManager.gameState == GameManager.GameState.Gameplay)
        {
            SetTexture();
        }
    }
    private void SetTexture()
    {
        for (int i = 0; i < imgSize; i++)
        {
            for (int j = 0; j < imgSize; j++)
            {
                float perl = VoronoidGenerator.VoroPerlin[i, j];
                //Debug.Log("perl " + perl);
                texture.SetPixel(i, j, new Color(perl, perl, perl,1.0f));
            }
        }

        texture.Apply();
        image.texture = texture;
    }
}
