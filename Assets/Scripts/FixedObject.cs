using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedObject : MonoBehaviour
{

    public TrainBehavoir train;

    public long chunkPos = 0;

    public long chunkSize = 200;

    // Start is called before the first frame update
    void Start()
    {
        train = GameObject.Find("TrainBehavoir").GetComponent<TrainBehavoir>();
    }

    // move the object to the position relative to the train
    public void move()
    {
        // teleport according to chunk position
        long chunkDist = chunkPos - train.chunkPos;

        // check if train is about to leave the chunk
        if ( train.transform.position.x > chunkSize/2-.01 )
            chunkDist -= 1;

        if ( train.transform.position.x < -chunkSize/2+.01 )
            chunkDist += 1;

        // calculate x coodinate
        float x = (float) ( chunkDist * chunkSize );

        gameObject.transform.position = new Vector3(x, gameObject.transform.position.y, gameObject.transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        move();
    }
}
