using UnityEngine;

[System.Serializable]
public class Chunk
{
    // Values of the chunk
    public int modelIndex = 0;
    public int trackStartIndex = 0;
    public int trackEndIndex = 0;

    // Construct a new Chunk object
    public Chunk(int modelIndex, int trackStartIndex, int trackEndIndex)
    {
        this.modelIndex = modelIndex;
        this.trackStartIndex = trackStartIndex;
        this.trackEndIndex = trackEndIndex;
    }

    // Generate a chunk object
    public static Chunk generateChunk()
    {
        return ChunkData.chunks[Random.Range(1, ChunkData.chunkModels.Count)];
    }
}