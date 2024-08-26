using UnityEngine;
using System.Collections.Generic;
using System;

using Random = UnityEngine.Random;

public class WorldGen : MonoBehaviour
{
    [Header("Worldgen Settings")]
    public int minWorldSize = 30;
    public int maxWorldSize = 500;

    public int seed = -1;

    [SerializeField]
    private List<Chunk> world;

    [Header("Render Settings")]
    public int renderDistance = 5;

    public int x = 0;

    [SerializeField]
    private int previousX = 0;

    [SerializeField]
    private List<GameObject> chunks;

    public List<Chunk> getWorld()
    {
        return this.world;
    }

    public void generate(int seed)
    {
        // apply the seed
        UnityEngine.Random.InitState(seed);

        // initialize / clear the world
        this.world = new List<Chunk>();

        // place the starting piece
        this.world.Add(ChunkData.chunks[0]);

        // generate a size for the world
        int size = Random.Range(minWorldSize, maxWorldSize);

        // add random pieces
        while (world.Count < size)
        {
            this.world.Add(Chunk.generateChunk());
        }
    }

    public void placeChunks()
    {
        Vector3 startPos = new Vector3(0,0,0);
        for (int i = /*-this.renderDistance + 1*/0; i < this.renderDistance; i++)
        {
            // calculate the chunk index
            int index = i;
            
            if ((index < 0) || (index > this.chunks.Count) || ((i + this.x) >= this.world.Count))
                continue;

            // add the mesh to the chunk
            this.chunks[index].AddComponent<MeshFilter>();
            this.chunks[index].AddComponent<MeshRenderer>();
            this.chunks[index].GetComponent<MeshFilter>().mesh = ChunkData.chunkModels[this.world[i + this.x].modelIndex];
            this.chunks[index].GetComponent<MeshRenderer>().material = ChunkData.chunkMaterials[this.world[i + this.x].modelIndex];

            // move the chunk
            this.chunks[index].transform.position = startPos;
            startPos += ChunkData.trackPaths[this.world[i + this.x].trackEndIndex].pos - ChunkData.trackPaths[this.world[i + this.x + 1].trackStartIndex].pos;
        }
    }

    public void createChunkObjects()
    {
        for (int i = 0; i < this.chunks.Count; i++)
        {
            Destroy(this.chunks[i]);
        }

        this.chunks = new List<GameObject>();

        for (int i = 0; i < this.renderDistance; i++)
        {
            this.chunks.Add(new GameObject());
            this.chunks[i].transform.SetParent(gameObject.transform);
            this.chunks[i].name = "Chunk " + i;
        }
    }

    public void runGeneration()
    {
        // create a "random seed" if the seed is -1
        if (this.seed == -1)
        {
            this.seed = (int)(Time.time * 10);
        }

        // initialize the chunk data
        ChunkData.initChunkData();

        this.generate(this.seed);
        this.createChunkObjects();
        this.placeChunks();
    }

    void Start()
    {
    }

    void Update()
    {
        if (previousX != x)
        {
            this.placeChunks();
            previousX = x;
        }
    }
}