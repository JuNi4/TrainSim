using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class Station : FixedObject
{

    // list of loadables
    public LoadingInfo[] freights;

    public double maxTrainDistance = 50;

    public double dist = 999;

    // Start is called before the first frame update
    void Start()
    {
        assignItems();
    }

    // a function for loading a car
    protected void load( int car, int freight )
    {
        // load the car
        float amount = Mathf.Min(Random.Range(freights[freight].loadingMin, freights[freight].loadingMax), base.train.cars[car].capacity - base.train.cars[car].fillLevel );
        float val = Mathf.Min( amount, freights[freight].fillLevel );
        Debug.Log("Loaded " + val.ToString());
        base.train.cars[car].fillLevel += freights[freight].dataType == DataType.Int? (int)val : val;
        freights[freight].fillLevel -= freights[freight].dataType == DataType.Int? (int)val : val;
    }

    // a function for unloading a car
    protected void unload( int car, int freight )
    {
        // unload the car
        float amount = Mathf.Min(Random.Range(freights[freight].unloadingMin, freights[freight].unloadingMax), freights[freight].loadingCapacity - freights[freight].fillLevel);
        float val = Mathf.Min( amount, base.train.cars[car].fillLevel );
        Debug.Log("Unloaded " + val.ToString());
        base.train.cars[car].fillLevel -= freights[freight].dataType == DataType.Int? (int)val : val;
        freights[freight].fillLevel += freights[freight].dataType == DataType.Int? (int)val : val;
    }

    public void loadCars()
    {
        // get the list of train cars
        TrainCar[] trainCars = base.train.cars;
        // go through all the resources
        for ( int f = 0; f < freights.Length; f++ ) {
            // get freight amount
            // find a train car
            for ( int c = 0; c < trainCars.Length; c++ ) {
                // check if car has correct freight
                if ( trainCars[c].freightType == freights[f].freight ) {
                    // load car
                    switch ( freights[f].load ) {
                        case LoadingType.load:
                            // load a random number of things
                            load(c, f);
                            break;
                        case LoadingType.unload:
                            // unload a random number of things
                            unload(c, f);
                            break;
                        case LoadingType.both:
                            // load & unload
                            load(c, f);
                            unload(c, f);
                            break;

                        default:
                            break;
                    }
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // move the station
        base.move();

        // calculate train is in range
        float distX = Mathf.Abs( gameObject.transform.position.x - train.transform.position.x );
        float distZ = Mathf.Abs( gameObject.transform.position.z - train.transform.position.z );
        dist = Mathf.Sqrt( Mathf.Pow( distX, 2 ) + Mathf.Pow( distZ, 2 ) );
        // check train distance
        if ( dist < maxTrainDistance ) {
            // show log message
            // Debug.Log("Train in Range");
            if ( Input.GetKeyDown("e") ) {
                loadCars();
            }
        }
    }
}