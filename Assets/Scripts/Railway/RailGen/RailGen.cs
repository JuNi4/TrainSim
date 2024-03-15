using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailGen : MonoBehaviour
{
    // random generator settings
    public float minRailLength = 10;
    public float maxRailLength = 200;

    // grade in percent ( m up, 100 over )
    public float minRailGrade = 0;
    public float maxRailGrade = 5;

    // curve settings
    public float currentAngle = 0;
    public float maxAngle = 180;
    public float maxCurveAnlge = 45;
    public float curvePercentage = 5;
    public float curveResoloution = 10;

    // a list of points
    public List<RailData> points = new List<RailData>();

    public List<Vector3> newVerts;
    public List<int> newTriangles;

    [Header("Debug")]
    public bool rebuildTrack = false;

    public bool showDebug = true;
    public bool showPoints = true;
    public Color pointColor = Color.yellow;
    public bool showTrack = true;
    public Color trackColor = Color.blue;
    public bool showTargetHeight = true;
    public Color theightColor = Color.green;

    // Calculates the direction of the last generated rail segmen. Returns a random direction for the first element
    protected Vector3 calculateLastSegmentDirection()
    {
        if ( points.Count < 2 ) { return new Vector3(1,0,0); }
        // get the last two points
        Vector3 a = points[ points.Count-1 ].position;
        Vector3 b = points[ points.Count-2 ].position;

        // get the vector between the two points and normalize it
        return ( a - b ).normalized;
    }

    // get height at point
    protected float getHeightAtPoint( Vector3 pos )
    {
        return 3f;
    }

    // Bezier Function ( src: https://catlikecoding.com/unity/tutorials/curves-and-splines/ )
    protected static Vector3 Bezier (Vector3 p0, Vector3 p1, Vector3 p2, float t) {
		t = Mathf.Clamp01(t);
		float oneMinusT = 1f - t;
		return
			oneMinusT * oneMinusT * p0 +
			2f * oneMinusT * t * p1 +
			t * t * p2;
	}

    // function for generating a single rail segment
    protected void generateSegment()
    {
        // calculate the direction of the last rail segment
        Vector3 dir = calculateLastSegmentDirection(); 

        // generate random length
        float length = Random.Range(minRailLength, maxRailLength);

        // generate point
        Vector3 start = points[ points.Count -1 ].position;
        Vector3 pos = start + dir*length;


        // height checks //

        // get target height
        float height = getHeightAtPoint( pos );

        // calculate grade
        float grade = height - start.y;

        // check if height is low enough for an incline
        if ( grade > 0 )
        {
            // check if low enough for an incline
            if ( grade <= maxRailGrade ) {
                pos.y = start.y + grade /100 *length;
            } else {
                // do a tunnel
            }
        }
        else if ( grade < 0 )
        {
            // decline
            float decline = Mathf.Max( grade, -maxRailGrade ) /100 *length;
            pos.y = start.y + decline;
        }

        // curves //
        // generate chance for curve
        float doCurve = Random.Range(0f, 101f);

        if ( doCurve <= curvePercentage ) { length *= .5f; }
        Vector3 pos2 = start + dir*length;

        if ( doCurve <= curvePercentage ) {
            // generate next point
            //points.Add( new RailData(pos2) );

            // calculate the direction of the last rail segment
            dir = calculateLastSegmentDirection();

            // generate angle
            float turnTo = Random.Range(-maxAngle, maxAngle);

            turnTo = Mathf.Clamp(currentAngle - turnTo, -maxCurveAnlge, maxCurveAnlge);

            currentAngle = turnTo;

            // rotate direction vector
            dir = Quaternion.Euler( 0, currentAngle, 0 ) * new Vector3( 1, dir.y, 0).normalized;

            pos = pos2 + dir*length;

            // make bezier curve
            for ( float i = 0; i < 1; i += 1 / curveResoloution )
            {
                // add the points
                points.Add( new RailData( Bezier(start, pos2, pos, i), points.Count+1, points.Count-1 ) );
            }

        } else {
            // create a new point
            points.Add( new RailData( pos, points.Count+1, points.Count-1 ) );
        }

        // newVerts.Add( start );
        // newVerts.Add( pos );
        // newVerts.Add( pos+ new Vector3(20,0,0) );

        // newTriangles.Add(newVerts.Count -1);
        // newTriangles.Add(newVerts.Count -2);
        // newTriangles.Add(newVerts.Count -3);
    }

    // Start is called before the first frame update
    void Start()
    {
        // Mesh mesh = new Mesh();
        
        // add start position to list of points
        points.Add( new RailData( gameObject.transform.position, points.Count+1, -1 ));

        // generate all rail segments
        for ( int i = 0; i < 500; i ++ ) {
            generateSegment();
        }

        // mesh.vertices = newVerts.ToArray();
        // mesh.triangles = newTriangles.ToArray();

        // GetComponent<MeshFilter>().mesh = mesh;
    }

    // Update is called once per frame
    void Update()
    {
        if ( rebuildTrack ) {
            points = new List<RailData>
            {
                // add start position to list of points
                new RailData( gameObject.transform.position, 1 )
            };

            // generate all rail segments
            for ( int i = 0; i < 500; i ++ ) {
                generateSegment();
            }

            rebuildTrack = false;
        }
    }

    void OnDrawGizmos()
    {
        if (!showDebug) { return; }
        for ( int i = 0; i < points.Count; i ++ ) {
            // draw generated points
            if (showPoints) {
                Gizmos.color = pointColor;
                Gizmos.DrawSphere(points[i].position, 1);
            }
            if ( showTrack ) {
                // draw line to next element
                if ( i < points.Count -1 ) {
                    Gizmos.color = trackColor;
                    Gizmos.DrawLine(points[i].position, points[i+1].position);
            }}
            if ( showTargetHeight ) {
                // draw line to the target height
                Gizmos.color = theightColor;
                Vector3 p = points[i].position;
                p.y = getHeightAtPoint(p);
                Gizmos.DrawLine(points[i].position, p);
            }
        }
    }
}