using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeightmapToMesh : MonoBehaviour
{
    public float heightMultiplyer = 1.0f;

    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    private MeshCollider meshCollider;

    private MeshData meshData;
    private Mesh mesh;
    private void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();


        meshData = MeshGenerator.GenerateTerrainMesh(VoronoidGenerator.completeHeights, heightMultiplyer);
        mesh = meshData.CreateMesh();

        meshFilter.sharedMesh = mesh;
        //meshCollider.sharedMesh = mesh;

        meshRenderer.material = Resources.Load("Materials/VoronoidMat") as Material;

    }
    private void Update()
    {
        meshData = MeshGenerator.GenerateTerrainMesh(VoronoidGenerator.completeHeights, heightMultiplyer);
        mesh = meshData.CreateMesh();

        meshFilter.sharedMesh = mesh;
        //meshCollider.sharedMesh = mesh;
    }
}