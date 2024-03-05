using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VoronoidGenerator : MonoBehaviour
{
    [SerializeField] private Color[] possibleColors;
    [SerializeField] private int gridSize = 10;

    private int imgSize;
    private int pixelsPerCell;
    private RawImage image;
    private Texture2D texture;
    private Vector2Int[,] pointPositions;
    private Color[,] colors;
    private float[,] heights;

    private float timer = 0.0f;
    
    public static float[,] VoroPerlin;
    public static float[,] VoroIsland;
    public static float[,] completeHeights;

    private void Awake()
    {
        image = GetComponent<RawImage>();
        imgSize = Mathf.RoundToInt(image.GetComponent<RectTransform>().sizeDelta.x);

        pointPositions = new Vector2Int[gridSize, gridSize];
        colors = new Color[gridSize, gridSize];
        heights = new float[gridSize, gridSize];
        texture = new Texture2D(imgSize, imgSize);
        texture.filterMode = FilterMode.Point;
        VoroPerlin = new float[imgSize, imgSize];
        VoroIsland = new float[imgSize, imgSize];
        completeHeights = new float[imgSize, imgSize];
        pixelsPerCell = imgSize / gridSize;

        GenerateHelperTextures(0.0f);
        GeneratePoints();
        GeneratePointColorsHeights();
        GenerateDiagram();
    }

    private void Update()
    {
        timer += Time.deltaTime;
        GenerateHelperTextures(timer * 10.0f);
        GeneratePointColorsHeights();
        GenerateDiagram();


        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //}
    }

    private void GenerateDiagram()
    {

        for (int i = 0; i < imgSize; i++)
        {
            for (int j = 0; j < imgSize; j++)
            {
                int gridX = i / pixelsPerCell;
                int gridY = j / pixelsPerCell;

                float nearestDistance = Mathf.Infinity;
                Vector2Int nearestPoint = new Vector2Int();

                for (int a = -1; a < 2; a++)
                {
                    for (int b = -1; b < 2; b++)
                    {

                        int X = gridX + a;
                        int Y = gridY + b;

                        if (X < 0 || Y < 0 || X >= gridSize || Y >= gridSize) continue;

                        float distance = Vector2Int.Distance(new Vector2Int(i, j), pointPositions[X, Y]);
                        if (distance < nearestDistance)
                        {
                            nearestDistance = distance;
                            nearestPoint = new Vector2Int(X, Y);
                        }
                    }
                }

                texture.SetPixel(i, j, colors[nearestPoint.x, nearestPoint.y]);
                completeHeights[i, j] = heights[nearestPoint.x, nearestPoint.y];
            }
        }


        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {
                texture.SetPixel(pointPositions[i, j].x, pointPositions[i, j].y, Color.red);
            }
        }

        texture.Apply();
        image.texture = texture;
        //Debug.Log(Application.dataPath + "/Scenes/Voronoid/Resources/Textures/Voronoid.png");
        System.IO.File.WriteAllBytes(Application.dataPath + "/Scenes/Voronoid/Resources/Textures/Voronoid.png", texture.EncodeToPNG());
    }

    private void GeneratePoints()
    {
        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {
                pointPositions[i, j] = new Vector2Int(i * pixelsPerCell + Random.Range(0, pixelsPerCell), j * pixelsPerCell + Random.Range(0, pixelsPerCell));
            }
        }
    }

    private void GeneratePointColorsHeights()
    {
        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {
                //Color currentColor = possibleColors[Random.Range(0, possibleColors.Length)];
                //colors[i, j] = currentColor;

                float island = VoroIsland[pointPositions[i, j].x, pointPositions[i, j].y];
                float perlin = VoroPerlin[pointPositions[i, j].x, pointPositions[i, j].y];

                float currentNoiseValue = (island + perlin) * 0.5f;
                currentNoiseValue = Mathf.Clamp(currentNoiseValue, 0.0f, 1.0f);
                //Debug.Log("combined color " + currentNoiseValue);
                //Debug.Log("centerIsland " + centerIsland);
                heights[i, j] = currentNoiseValue;
                //colors[i, j] = (currentNoiseValue > 0.5f) ? Color.white : Color.black;
                colors[i, j] = new Color(currentNoiseValue, currentNoiseValue, currentNoiseValue, 1.0f);

            }
        }
    }

    private void GenerateHelperTextures(float t)
    {

        for (int i = 0; i < imgSize; i++)
        {
            for (int j = 0; j < imgSize; j++)
            {
                float perl = Mathf.PerlinNoise((float)i/ (float)imgSize, (float)j / (float)imgSize);
                //Debug.Log("perl " + perl + i + j);
                VoroPerlin[i, j] = perl;


                float centerIsland = Mathf.Sin(t)+ ((float)Mathf.Sin(((float)i / (float)imgSize) * Mathf.PI)) *
                      -Mathf.Sin(t) + ((float)Mathf.Sin(((float)j / (float)imgSize) * Mathf.PI));

                VoroIsland[i, j] = centerIsland;
            }
        }



    }
}
