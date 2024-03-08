using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenMaster : MonoBehaviour
{
    [System.Serializable]
    public class Octave
    {
        public float noiseScale = 1.0f;
        public float frequency = 1.0f;
    }

    public Vector2Int chunkSize;
    public int vertexScale;
    public float baseScale = 1.0f;
    public List<Vector3> newVertices;
    public List<int> newTriangles;

    public List<Octave> octaves;

    new Transform transform;

    private float calculateNoise(float x, float y)
    {
        float ret = 0;
        for (int i = 0; i < octaves.Count; i++)
        {
            Octave oc = octaves[i];
            Vector2 sample = new Vector2(x,y);
            sample += new Vector2(transform.position.x, transform.position.y);
            sample /= new Vector2(chunkSize.x, chunkSize.y);
            ret += (Mathf.PerlinNoise(sample.x * oc.frequency, sample.y * oc.frequency)
                    - 0.5f) * oc.noiseScale;
        }
        return ret;
    }

    private void generateMeshData(int width, int height)
    {
        int w = width / vertexScale;
        int h = height / vertexScale;

        for (float x = 0; x < w; x++)
        {
            for (float y = 0; y < h; y++)
            {
                newVertices.Add(new Vector3(x*vertexScale, calculateNoise(x*vertexScale, y*vertexScale) * baseScale, y*vertexScale));
            }
        }

        for (int x = 0; x < w-1; x++)
        {
            for (int y = 0; y < h-1; y++)
            {
                int i = x*w + y;
                newTriangles.Add(i);
                newTriangles.Add(i+1);
                newTriangles.Add(i+w);
                newTriangles.Add(i+w);
                newTriangles.Add(i+1);
                newTriangles.Add(i+w+1);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        transform = GetComponent<Transform>();
        Mesh mesh = new Mesh();
        newVertices.Clear();
        newTriangles.Clear();
        generateMeshData(chunkSize.x, chunkSize.y);
        mesh.vertices = newVertices.ToArray();
        mesh.triangles = newTriangles.ToArray();
        mesh.RecalculateNormals();
        mesh.RecalculateTangents();
        GetComponent<MeshFilter>().mesh = mesh;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
