using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RailData
{
    // where the point is at
    public Vector3 position;

    // where the point leads to
    public List<ulong> next;

    public List<ulong> prev;

    public RailData( Vector3 pos )
    {
        position = pos;
    }

    public RailData( Vector3 pos, List<ulong> n )
    {
        position = pos;
        next = n;
    }

    public RailData( Vector3 pos, List<ulong> n, List<ulong> l )
    {
        position = pos;
        next = n;
        prev = l;
    }
}
