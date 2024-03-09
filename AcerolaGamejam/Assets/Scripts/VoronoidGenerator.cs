using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VoronoidGenerator : MonoBehaviour
{
    [SerializeField] private Color[] possibleColors;
    [SerializeField] private int gridSize = 10;
    [SerializeField] private float animDuration;
    [SerializeField] private GameObject dst_object;
    private MeshRenderer dst_renderer;

    private int imgSize;
    private int pixelsPerCell;
    private RawImage image;
    private Vector2Int[,] pointPositions;
    private Color[,] colors;
    private float[,] heights;

    private float timer = 0.0f;
    
    public static float[,] VoroPerlin;
    public static float[,] VoroIsland;
    public static float[,] completeHeights;
    public static Texture2D textureSplat;
    public static Texture2D gameplayTexture;
    public static Texture2D previewTexture;

    private void Awake()
    {
        dst_renderer = dst_object.GetComponent<MeshRenderer>();

        image = GetComponent<RawImage>();
        imgSize = Mathf.RoundToInt(image.GetComponent<RectTransform>().sizeDelta.x);

        pointPositions = new Vector2Int[gridSize, gridSize];
        colors = new Color[gridSize, gridSize];
        heights = new float[gridSize, gridSize];

        gameplayTexture = new Texture2D(imgSize, imgSize);
        gameplayTexture.filterMode = FilterMode.Point;

        previewTexture = new Texture2D(imgSize, imgSize);
        previewTexture.filterMode = FilterMode.Point;

        textureSplat = new Texture2D(imgSize, imgSize);
        textureSplat.filterMode = FilterMode.Point;

        VoroPerlin = new float[imgSize, imgSize];
        VoroIsland = new float[imgSize, imgSize];
        completeHeights = new float[imgSize, imgSize];
        pixelsPerCell = imgSize / gridSize;

        GeneratePoints();
    }

    private void Update()
    {
        timer += Time.deltaTime;
        GenerateHelperTextures(timer, animDuration);
        GeneratePointColorsHeights();
        GenerateDiagram(previewTexture);

        //UpdateAndShowTextures
    }

    //TODO: Make this function adapt to level craetion (maybe summon a different function to set level beforehand?
    public void UpdateAndShowTextures(float timer, int gameState)
    {
        if(gameState == 0) //preview
        {
            GenerateHelperTextures(timer, animDuration);
            GeneratePointColorsHeights();
            GenerateDiagram(previewTexture);
        }
        else if(gameState == 1) //gameplay
        {
            GenerateHelperTextures(timer, animDuration);
            GeneratePointColorsHeights();
            GenerateDiagram(gameplayTexture);
        }
    }

    private void GenerateDiagram(Texture2D texture)
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

                float currentHeight = heights[nearestPoint.x, nearestPoint.y];
                textureSplat.SetPixel(i, j, (currentHeight > 0.5f) ? Color.white : Color.black);
                completeHeights[i, j] = currentHeight;
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

        textureSplat.Apply();
        //Debug.Log(Application.dataPath + "/Scenes/Voronoid/Resources/Textures/Voronoid.png");
        //System.IO.File.WriteAllBytes(Application.dataPath + "/Scenes/Voronoid/Resources/Textures/Voronoid.png", texture.EncodeToPNG());

        dst_renderer.material.SetTexture("_MainTex", textureSplat);
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

    private void GenerateHelperTextures(float t, float animDuration)
    {

        for (int i = 0; i < imgSize; i++)
        {
            for (int j = 0; j < imgSize; j++)
            {
                float perl = Mathf.PerlinNoise((float)i/ (float)imgSize, (float)j / (float)imgSize);
                //Debug.Log("perl " + perl + i + j);
                VoroPerlin[i, j] = perl;

                //x=sen(y*4)*10
                float interpTime = t / animDuration;
                float interpPxl = interpTime * imgSize;

                float offsetY = interpPxl;
                float offsetX = Mathf.Sin(offsetY * 0.1f) * 40.0f;


                float centerIsland = ((float)Mathf.Sin(((float)(i + offsetX) / (float)imgSize) * Mathf.PI)) *
                       ((float)Mathf.Sin(((float)(j+ offsetY) / (float)imgSize) * Mathf.PI));

                VoroIsland[i, j] = centerIsland;
            }
        }



    }
}
