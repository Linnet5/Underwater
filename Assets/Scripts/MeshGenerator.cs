using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshGenerator : MonoBehaviour
{
    // Brackeys Tutorial https://www.youtube.com/watch?v=eJEpeUH1EMg , https://www.youtube.com/watch?v=64NblGkAabk
    Mesh mesh;
    Vector2[] newUV;
    Vector3[] vertices;
    int[] triangles;
    public int resolution;
    [SerializeField] float height;
    [SerializeField] float noiseZoom;
    int xSize;
    int zSize;
    DecorPlacement decorPlacement;
    Vector3 worldPos;

    // Functions CreateShape() & UpdateMesh() based on Brackeys tutorial https://www.youtube.com/watch?v=64NblGkAabk 
    void Awake() {
        decorPlacement = gameObject.GetComponent<DecorPlacement>();
    }
    void CreateShape(bool firstTime, GameObject tile) {
        vertices = new Vector3[(xSize + 1)*(zSize + 1)];
        newUV = new Vector2[vertices.Length];   // Added UV generation

        for(int i = 0, z = 0; z <= zSize; z++) {
            for(int x = 0; x <= xSize; x++) {
                float y = Mathf.PerlinNoise((worldPos.x +  x) *  noiseZoom, (worldPos.z + z)*  noiseZoom) * height;
                vertices[i] = new Vector3(x, y, z);
                decorPlacement.PlaceDecor(x, y, z, worldPos, firstTime, tile);
                i++;
            }
        }

        int vert = 0;
        int tris = 0;

        triangles = new int[xSize * zSize * 6];
        for(int z = 0; z < zSize; z++) {
            for(int x = 0; x < xSize; x++) {
                triangles[tris] = vert;
                triangles[tris + 1] = vert + xSize + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + xSize + 1;
                triangles[tris + 5] = vert + xSize + 2;

                vert++;
                tris += 6;
            }
            vert++;
        }
    }

    void CreateUV() {
        for (int i = 0; i < newUV.Length; i++)
        {
            newUV[i] = new Vector2(vertices[i].x, vertices[i].z);
        }
    }
    
    void UpdateMesh() {
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = newUV;

        mesh.RecalculateNormals();
    }

    public void Generate(bool firstTime, GameObject tile) {
        worldPos = gameObject.transform.position;
        mesh = new Mesh();

        xSize = zSize = resolution;

        CreateShape(firstTime, tile);
        CreateUV();
        UpdateMesh();

        GetComponent<MeshFilter>().mesh = mesh;
        GetComponent<MeshCollider>().sharedMesh = mesh;
        GetComponent<MeshCollider>().enabled = true; 
    }
}
