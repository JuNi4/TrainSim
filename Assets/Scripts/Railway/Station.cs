using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Station : FixedObject
{

    // list of loadables
    public LoadingInfo[] freights;

    public double maxTrainDistance = 50;

    // Start is called before the first frame update
    // void Start()
    // {
        
    // }

    public void load()
    {
        // go through all the resources
        for ( int f = 0; f < freights.Length; f++ ) {
            // get the list of train cars
            TrainCar[] trainCars = base.train.cars;
            // find a train car
            for ( int c = 0; c < trainCars.Length; c++ ) {
                // check if car has correct freight
                if ( trainCars[c].freightType == freights[f].freight ) {
                    // load car
                    switch ( freights[f].load ) {
                        case LoadingType.load:
                            // load a random number of things
                            
                        case LoadingType.unload:
                            // unload a random number of things
                        case LoadingType.both:
                            // load & unload

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
    }
}
