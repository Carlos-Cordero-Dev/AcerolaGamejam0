using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PreviewTextureVisualization : MonoBehaviour
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
        pixelsPerCell = imgSize / gridSize;
        SetTexture();
    }
    private void Update()
    {
        SetTexture();
    }
    private void SetTexture()
    {
        image.texture = VoronoidGenerator.previewTexture;
    }
}
