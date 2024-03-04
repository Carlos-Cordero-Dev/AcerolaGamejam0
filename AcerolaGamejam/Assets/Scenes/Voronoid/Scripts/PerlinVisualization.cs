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
    private void Awake()
    {
        image = GetComponent<RawImage>();
        imgSize = Mathf.RoundToInt(image.GetComponent<RectTransform>().sizeDelta.x);
    }

    private void Start()
    {
        SetTexture();
    }
    private void SetTexture()
    {
        Texture2D texture = new Texture2D(imgSize, imgSize);
        texture.filterMode = FilterMode.Point;
        pixelsPerCell = imgSize / gridSize;

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
