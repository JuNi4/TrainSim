using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenMaster : MonoBehaviour
{
    //store the camera to track
    public Camera camera;
    //store the material for the chuncks
    public Material chunkMaterial;

    Vector3[] newVertices;
    int[] newTriangles;

    void generateMeshData(int width, int height)
    {
        int i = 0;
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                newVertices[i] = new Vector3(x, 0, y);
                i++;
            }
        }

        i = 0;
        for (int x = 0; x < width-1; x++)
        {
            for (int y = 0; y < height-1; y++)
            {
                int j = x*width + y;
                newTriangles[i] = j;         i++;
                newTriangles[i] = j+1;       i++;
                newTriangles[i] = j+width;   i++;
                newTriangles[i] = j+1;       i++;
                newTriangles[i] = j+width;   i++;
                newTriangles[i] = j+width+1; i++;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Mesh mesh = new Mesh();
        generateMeshData(16,16);
        mesh.RecalculateNormals();
        GetComponent<MeshFilter>().mesh = mesh;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
