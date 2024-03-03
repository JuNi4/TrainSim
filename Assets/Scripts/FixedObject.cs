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
        
    }

    // move the object to the position relative to the train
    public void move()
    {
        // teleport according to chunk position
        long chunkDist = chunkPos - train.chunkPos;

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
