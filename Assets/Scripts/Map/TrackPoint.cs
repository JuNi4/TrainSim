using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TrackPoint
{
    // empty trackpoint
    public static TrackPoint EMPTY = new TrackPoint();

    // fields
    public Vector3 pos = new Vector3();
    public List<int> prevPoint = new List<int>();
    public List<int> nextPoint = new List<int>();

    public int switchIndex = -1;

    public TrackPoint()
    {/* Do Nothing */}

    public TrackPoint(Vector3 pos, List<int> prev, List<int> next)
    {
        this.pos = pos;
        this.prevPoint = prev;
        this.nextPoint = next;
    }

    public TrackPoint(Vector3 pos, int prev, int next)
    {
        this.pos = pos;
        this.prevPoint.Add(prev);
        this.nextPoint.Add(next);
    }

    public TrackPoint(TrackPoint point)
    {
        this.pos = point.pos;
        this.prevPoint = point.prevPoint;
        this.nextPoint = point.nextPoint;
    }

    // returns true, if the point is "invalid" or "empty"
    public bool isEmpty()
    {
        return (this.prevPoint.Count == 0 && this.nextPoint.Count == 0) ||
               (this.prevPoint[0] == -1 && this.nextPoint[0] == -1);
    }

    // returns the next track point
    public TrackPoint next()
    {
        if (this.nextPoint.Count > 0 && this.nextPoint[0] != -1)
            return ChunkData.trackPaths[this.nextPoint[0]];
        else
            return TrackPoint.EMPTY;
    }

    public TrackPoint previous()
    {
        if (this.prevPoint.Count > 0 && this.prevPoint[0] != -1)
            return ChunkData.trackPaths[this.prevPoint[0]];
        else
            return TrackPoint.EMPTY;
    }
}