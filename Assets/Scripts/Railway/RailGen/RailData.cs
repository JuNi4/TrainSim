using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RailData
{
    // where the point is at
    public Vector3 position;

    // where the point leads to
    public List<int> next;

    public List<int> prev;

    public RailData( Vector3 pos )
    {
        position = pos;
    }

    public RailData( Vector3 pos, List<int> n )
    {
        position = pos;
        next = n;
    }

    public RailData( Vector3 pos, List<int> n, List<int> l )
    {
        position = pos;
        next = n;
        prev = l;
    }

    public RailData( Vector3 pos, int n )
    {
        position = pos;
        next = new List<int> {n};
    }

    public RailData( Vector3 pos, int n, int l )
    {
        position = pos;
        next = new List<int> {n};
        prev = new List<int> {l};
    }
}
