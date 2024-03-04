using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IslandVisualization : MonoBehaviour
{
    private int imgSize;
    private int pixelsPerCell;
    private RawImage image;
    
    private void Awake()
    {
        image = GetComponent<RawImage>();
        imgSize = Mathf.RoundToInt(image.GetComponent<RectTransform>().sizeDelta.x);
    }

    private void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
