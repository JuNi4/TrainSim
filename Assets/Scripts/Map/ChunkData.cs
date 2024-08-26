using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ChunkData
{
    public static List<string> chunkSources = new List<string>{
        // Starting piece
        "terrain",
        // Other Pieces //
    };

    // a list of waypoint along the tracks of each chunk
    public static List<TrackPoint> trackPaths = new List<TrackPoint>();
    // a list of chunks
    public static List<Chunk> chunks = new List<Chunk>();
    // The meshs for the chunks
    public static List<Mesh> chunkModels = new List<Mesh>();
    // the materials for the chunks
    public static List<Material> chunkMaterials = new List<Material>();

    // helper functions //

    // create a chunk
    private static int addChunk(string model)
    {
        Chunk chunk = new Chunk(chunkSources.Count, 0, 0);

        // add the model
        if (!chunkSources.Contains(model))
            chunkSources.Add(model);
        else
            chunk.modelIndex = chunkSources.FindIndex(i => i == model);

        // add it to the chunks
        int index = chunks.Count;
        chunks.Add(chunk);

        return index;
    }

    // create a starting trackpoint
    private static int addStartTrackPoint(Vector3 pos)
    {
        int index = trackPaths.Count;

        // add the point
        trackPaths.Add(new TrackPoint(pos, -1, index+1));

        return index;
    }

    // create an end trackpoint
    private static int addEndTrackPoint(Vector3 pos)
    {
        int index = trackPaths.Count;

        // add the point
        trackPaths.Add(new TrackPoint(pos, index-1, -1));

        return index;
    }

    // create a trackpoint
    private static int addTrackPoint(Vector3 pos)
    {
        int index = trackPaths.Count;

        // add the point
        trackPaths.Add(new TrackPoint(pos, index-1, index+1));

        return index;
    }

    public static void initChunkData()
    {
        int x;
        // create the starting tile
        x = addChunk("terrain");
        addStartTrackPoint(new Vector3(-50, 0, 0));
        chunks[x].trackStartIndex = addTrackPoint(new Vector3(0, 0, 0));
        chunks[x].trackEndIndex = addEndTrackPoint(new Vector3(100, 0, 0));

        // create the straight tile
        x = addChunk("terrain");
        chunks[x].trackStartIndex = addStartTrackPoint(new Vector3(-100, 0, 0));
        chunks[x].trackEndIndex = addEndTrackPoint(new Vector3(100, 10, 0));

        // create the straight tile
        x = addChunk("dev_terrain_01");
        chunks[x].trackStartIndex = addStartTrackPoint(new Vector3(-500, 0, 0));
        chunks[x].trackEndIndex = addEndTrackPoint(new Vector3(500, 10, 0));

        // initialize all meshes and materials
        for (int i = 0; i < chunkSources.Count; i++)
        {
            chunkModels.Add(Resources.Load<Mesh>(chunkSources[i]));
            chunkMaterials.Add(Resources.Load<Material>(chunkSources[i]));
        }
    }

}