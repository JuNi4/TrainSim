using System;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class TrainController : MonoBehaviour
{
    // physic values
    [Header("Physics")]
    public float gravity = 9.8F;

    // train values
    [Header("General Train Stuff")]
    public float velocity = 0;
    public Vector3Int chunkPos = new Vector3Int(0, 0, 0);

    // values for navigation
    [Header("Track Navigation Values")]
    public WorldGen world;
    public TrackPoint current;
    public TrackPoint next;
    [SerializeField]
    private double distanceBetweenNavPoints;
    [SerializeField]
    private double navProgress;

    private void setPointToIndex(int index)
    {
        this.current = ChunkData.trackPaths[index];
        this.next = this.current.next();

        // calculate the distance between the two points
        Vector3 diff = next.pos - current.pos;
        this.distanceBetweenNavPoints = Math.Sqrt( diff.x * diff.x +
                                                   diff.y * diff.y +
                                                   diff.z * diff.z );
    }

    private void calcDiff()
    {
        // calculate the distance between the two points
        Vector3 diff = next.pos - current.pos;
        this.distanceBetweenNavPoints = Math.Sqrt( diff.x * diff.x +
                                                   diff.y * diff.y +
                                                   diff.z * diff.z );
    }

    private void updatePos()
    {
        // next / previous segment stuff
        if ((this.navProgress > 1) && (world.x < (world.getWorld().Count - 1)))
        {
            this.navProgress -= 1;

            // recalculate inbetween pos
            // this.navProgress /= Time.deltaTime;
            this.navProgress *= this.distanceBetweenNavPoints;

            if (this.next.next().isEmpty())
            {
                this.current = ChunkData.trackPaths[ this.world.getWorld()[this.world.x + 1].trackStartIndex ];
                this.next = this.current.next();
            } else
            {
                this.current = new TrackPoint(this.next);
                this.next = this.next.next();
            }

            // go to the next point
            calcDiff();
            this.world.x++;

            // redo the progress
            this.navProgress /= this.distanceBetweenNavPoints;
            // this.navProgress *= Time.deltaTime;
        }
        else if (this.navProgress < 0 && world.x > 0)
        {
            // recalculate inbetween pos
            // this.navProgress /= Time.deltaTime;
            this.navProgress = Math.Abs(this.navProgress) * this.distanceBetweenNavPoints;

            if (this.current.previous().isEmpty())
            {
                this.next = ChunkData.trackPaths[ this.world.getWorld()[this.world.x - 1].trackEndIndex ];
                this.current = next.previous();
            } else
            {
                this.next = new TrackPoint(this.current);
                this.current = this.current.previous();
            }

            // go to the next point
            calcDiff();
            this.world.x--;

            // redo the progress
            this.navProgress = 1 - (this.navProgress / this.distanceBetweenNavPoints);
            // this.navProgress *= Time.deltaTime;
        }

        Vector3 dir = next.pos - current.pos;
        gameObject.transform.position = Vector3.Lerp(this.current.pos, this.next.pos, (float)this.navProgress);

        // rotate
        float tangle = Vector3.Angle(dir, new Vector3(1,0,0) );
        float cangle = tangle;
        if (!current.previous().isEmpty())
            cangle = Vector3.Angle(current.pos - current.previous().pos, new Vector3(1,0,0) );

        gameObject.transform.eulerAngles = new Vector3( 0, -Mathf.Lerp(cangle, tangle, (float)navProgress), 0 );
    }

    void Start()
    {
        world.runGeneration();
        // get the first nav point to put the train at
        setPointToIndex(world.getWorld()[0].trackStartIndex);
    }

    void Update()
    {
        updatePos();

        Vector3 dir = next.pos - current.pos;
        velocity -= gravity * dir.normalized.y * Time.deltaTime;
        navProgress += velocity / distanceBetweenNavPoints * Time.deltaTime;
    }
}