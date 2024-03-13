using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TestMovement : MonoBehaviour
{

    public float velocity = 0;

    public float inbetweenPos;
    public int pos;

    public RailGen trackData;

    // Start is called before the first frame update
    void Start()
    {
        trackData = GameObject.Find("RailGenerator").GetComponent<RailGen>();
    }

    // Update is called once per frame
    void Update()
    {
        // check if segments are remainging
        if ( pos >= trackData.points.Count ) { return; }
        // get length of current segment
        Vector3 current = trackData.points[pos].position;
        Vector3 next = trackData.points[pos+1].position;

        // get segment direction
        Vector3 dir = ( next - current );
        float segmentLength = ( next - current ).x / dir.normalized.x;

        
        // progress train
        // velocity / length of segment
        inbetweenPos += velocity / segmentLength * Time.deltaTime;

        if ( float.IsNaN(inbetweenPos) ) { inbetweenPos = 1.1f; }

        while ( !(inbetweenPos < 1) ) {
            inbetweenPos -= 1;

            // recalculate inbetween pos
            inbetweenPos /= Time.deltaTime;
            inbetweenPos *= segmentLength;

            pos += 1;
            // recalculate distance
            current = trackData.points[pos].position;
            next = trackData.points[pos+1].position;

            // get segment direction
            dir = ( next - current );
            segmentLength = ( next - current ).x / dir.normalized.x;

            inbetweenPos /= segmentLength;
            inbetweenPos *= Time.deltaTime;

            
            if ( float.IsNaN(inbetweenPos) ) { inbetweenPos = 1.1f; break; }
        }

        // update position
        gameObject.transform.position = Vector3.Lerp(current,next,inbetweenPos);

        // rotate
        float tangle = Vector3.Angle(dir, new Vector3(1,0,0) );
        float cangle = 0;
        if (pos > 0) {
            cangle = Vector3.Angle( trackData.points[pos].position - trackData.points[pos-1].position, new Vector3(1,0,0) );
        }

        gameObject.transform.eulerAngles = new Vector3( 0, Mathf.Lerp(cangle, tangle, inbetweenPos), 0 );
    }
}
