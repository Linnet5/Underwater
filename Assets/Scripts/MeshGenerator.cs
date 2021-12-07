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

    // Start is called before the first frame update

    void Start()
    { 
    }

    // Function from Brackeys
    void CreateShape() {
        decorPlacement = gameObject.GetComponent<DecorPlacement>();
        vertices = new Vector3[(xSize + 1)*(zSize + 1)];
        newUV = new Vector2[vertices.Length];   // Added UV generation
        float startX = 0.0f;          //Random.Range(256.0f, 256.0f*1000);   // Random mesh every time
        float startY = 0.0f;      //Random.Range(256.0f, 256.0f*1000);

        for(int i = 0, z = 0; z <= zSize; z++) {
            for(int x = 0; x <= xSize; x++) {
                float y = Mathf.PerlinNoise((worldPos.x +  x + startX) *  noiseZoom, (worldPos.z + z + startY)*  noiseZoom) * height;
                vertices[i] = new Vector3(x, y, z);
                decorPlacement.PlaceDecor(x, y, z, worldPos);
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
    
    // Function from Brackeys
    void UpdateMesh() {
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = newUV;

        mesh.RecalculateNormals();
    }

    public void Generate() {
        worldPos = gameObject.transform.position;
        mesh = new Mesh();

        xSize = zSize = resolution;

        CreateShape();
        CreateUV();
        UpdateMesh();

        GetComponent<MeshFilter>().mesh = mesh;
        GetComponent<MeshCollider>().sharedMesh = mesh;
        GetComponent<MeshCollider>().enabled = true; 
    }
}
