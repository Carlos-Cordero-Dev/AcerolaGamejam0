using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VoronoidGenerator : MonoBehaviour
{
    [SerializeField] private Color[] possibleColors;
    [SerializeField] private int gridSize = 10;
    [SerializeField] private float WAVELENGTH = 0.5f;

    private int imgSize;
    private int pixelsPerCell;
    private RawImage image;
    private Vector2Int[,] pointPositions;
    private Color[,] colors;
    private float[,] heights;

    public static float[,] VoroPerlin;
    public static float[,] VoroIsland;

    private void Awake()
    {
        image = GetComponent<RawImage>();
        imgSize = Mathf.RoundToInt(image.GetComponent<RectTransform>().sizeDelta.x);
        GenerateDiagram();
        GenerateHelperTextures();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            GenerateDiagram();
        }
    }

    private void GenerateDiagram()
    {
        Texture2D texture = new Texture2D(imgSize, imgSize);
        texture.filterMode = FilterMode.Point;
        pixelsPerCell = imgSize / gridSize;

        //for (int i = 0; i < imgSize; i++)
        //{
        //    for (int j = 0; j < imgSize; j++)
        //    {
        //        texture.SetPixel(i, j, Color.white);
        //    }
        //}

        GeneratePoints();


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
            }
        }


        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {
                texture.SetPixel(pointPositions[i, j].x, pointPositions[i, j].y, Color.black);
            }
        }

        texture.Apply();
        image.texture = texture;
        Debug.Log(Application.dataPath + "/Voronoid.png");
        System.IO.File.WriteAllBytes(Application.dataPath + "/Voronoid.png", texture.EncodeToPNG());
    }

    private void GeneratePoints()
    {
        pointPositions = new Vector2Int[gridSize, gridSize];
        colors = new Color[gridSize, gridSize];
        heights = new float[gridSize, gridSize];

        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {
                pointPositions[i, j] = new Vector2Int(i * pixelsPerCell + Random.Range(0, pixelsPerCell), j * pixelsPerCell + Random.Range(0, pixelsPerCell));

                Color currentColor = possibleColors[Random.Range(0, possibleColors.Length)];
                colors[i, j] = currentColor;

                //heights

                float nx = pointPositions[i, j].x / gridSize - 0.5f;
                float ny = pointPositions[i, j].y / gridSize - -0.5f;
                // start with noise:
                float noiseHeight = (1 + Mathf.PerlinNoise(nx / WAVELENGTH, ny / WAVELENGTH)) / 2;
                // modify noise to make islands:
                float d = 2 * Mathf.Max(Mathf.Abs(nx), Mathf.Abs(ny)); // should be 0-1
                heights[i, j] = (1 + noiseHeight - d) / 2;
                //Debug.Log("height " + heights[i, j]);


                float centerIsland = ((float)Mathf.Sin(((float)i / (float)gridSize) * Mathf.PI)) *
                    ((float)Mathf.Sin(((float)j / (float)gridSize) * Mathf.PI));

                centerIsland *= Mathf.PerlinNoise(i, j);

                //Debug.Log("centerIsland " + centerIsland);
                colors[i, j] = (heights[i, j] > -gridSize * 0.5f) ? Color.white : Color.black;

            }
        }
    }

    private void GenerateHelperTextures()
    {
        VoroPerlin = new float[imgSize, imgSize];

        for (int i = 0; i < imgSize; i++)
        {
            for (int j = 0; j < imgSize; j++)
            {
                float perl = Mathf.PerlinNoise(i, j);
                Debug.Log("perl " + perl + i + j);
                VoroPerlin[i, j] = perl;

            }
        }
    }
}
