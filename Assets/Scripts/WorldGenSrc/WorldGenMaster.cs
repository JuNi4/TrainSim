using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Octave
{
    public float noiseScale = 1.0f;
    public float frequency = 1.0f;
}

public class WorldGenMaster : MonoBehaviour
{
    public Vector3Int chunkSize;
    public uint renderDistance = 5;
    public int vertexScale;
    public Vector2 baseScale = new Vector2(1,1);

    public List<Octave> octaves;

    public Material chunkMat;

    new Transform transform;

    private void addChunk(Vector3 pos)
    {
        GameObject obj = new GameObject("Chunk");
        obj.GetComponent<Transform>().position = pos;
        obj.AddComponent<MeshRenderer>();
        obj.GetComponent<MeshRenderer>().material = chunkMat;
        obj.AddComponent<MeshFilter>();
        //setup the custom script
        obj.AddComponent<WorldGenSlave>();
        obj.GetComponent<WorldGenSlave>().chunkSize = chunkSize;
        obj.GetComponent<WorldGenSlave>().vertexScale = vertexScale;
        obj.GetComponent<WorldGenSlave>().baseScale = baseScale;
        obj.GetComponent<WorldGenSlave>().octaves = octaves;
        obj.GetComponent<WorldGenSlave>().initValues();
        obj.GetComponent<WorldGenSlave>().build();
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int x = 0; x < renderDistance; x++)
        {
            for (int y = 0; y < renderDistance; y++)
            {
                for (int z = 0; z < renderDistance; z++)
                {
                    addChunk(new Vector3(x*(chunkSize.x-1),y*(chunkSize.y-1),z*(chunkSize.z-1)));
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
