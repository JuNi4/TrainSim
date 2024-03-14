/*

    Locomotive Simulator¹ by JuNi4 (https://github.com/juni4)

    Drive a locomotive with a custom input method for maximum imersion!

    *¹ This simulator uses estimations not real simulations!

 */

using UnityEngine;
using System.Collections;

using System;

// using Servo;

public class Train : MonoBehaviour
{   
    // Serial setup //
    [Header("Serial Comms Controller")] 
    public SerialController serialController;
    public bool useExternalControlls = true;

    // Settings //
    [Header("Train Stats")]

    // mass of the locomotive
    public double locomotiveMass = 10000;

    // locomotive tractive effort
    public double locomotiveTractiveEffort = 1000;

    // locomotive break force
    public double locomotiveBreakForce = 500;

    public double locomotiveMaxMainReservoirPressure = 5;
    public double locomotiveMainReservoirPressure = 0;
    public double locomotiveMaxBreakLinePressure = 5;
    public double locomotiveBreakPump = .1;
    public double locomotiveBreakValve = .2;

    // fuel & sand capacity
    public double locomotiveFuelCapacity = 300;
    public double locomotiveFuelConsumption = .1;

    public double locomotiveSandCapacity = 280;
    public double locomotiveSandConsumption = .1;

    // friction with air
    public double airFriction = 0.01;

    // wheelslip
    public double wheelSlipFactor = 1;

    public double wheelSlipSandFactor = 1;
    public double wheelSlipMassFactor = 1;

    // train values //
    [Header("Train Variables")]
    // velocity
    public double velocity = 0;
    
    // a list of directions
    public enum Directions
    { forward, neutral, reverse };

    // the direction the train will travel in
    public Directions direction;

    // automaticly set train vars //
    // total mass of the entire train
    public double trainMass;

    // break power of the carts
    public double trainBreakForce;

    public double trainTractiveEffort;

    public double trainMaxBreakLinePressure;
    public double trainBreakLinePressure = 0;

    public long chunkPos = 0;

    [Header("Cars")]
    public TrainCar[] cars;

    // Input Values //
    [Header("Controll Variables")]

    public Servo[] servos = {};

    public enum InputDeviceType
    {
        throttle,
        reverser,
        indipendantBreak_SL, trainBreak_SL,
        indipendantBreak_NSL, trainBreak_NSL
    }
    public InputDeviceType[] inputs = {};
    
    // function for calculating the tractive effort int the train
    public void recalculateTrainTractiveEffort()
    {
        // add current locomotive tractive effort
        trainTractiveEffort = locomotiveTractiveEffort;
    }
    public void recalculateTrainBreakForce()
    {
        trainBreakForce = 0;
        // add all break forces
        for ( int i = 0; i < cars.Length; i++ ) {
            trainBreakForce += cars[i].carBreakForce;
        }
    }
    public void recalculateTrainMaxBreakLinePressure()
    {
        // add current locomotive break pressure
        trainMaxBreakLinePressure = locomotiveMaxBreakLinePressure;
        // add all break forces
        for ( int i = 0; i < cars.Length; i++ ) {
            trainMaxBreakLinePressure += cars[i].carMaxBrakeLinePressure;
        }
    }
    public void recalculateTrainMass()
    {
        // add current locomotive mass
        trainMass = locomotiveMass;
        // add all break forces
        for ( int i = 0; i < cars.Length; i++ ) {
            trainMass += cars[i].carMass;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // go to 0 0 0
        gameObject.transform.position = new Vector3(0,0,0);


        recalculateTrainTractiveEffort();
        recalculateTrainBreakForce();
        recalculateTrainMaxBreakLinePressure();
        recalculateTrainMass();
    }

    // Update is called once per frame
    void Update()
    {
        // INPUTS //

        // if ( useExternalControlls ) { // external setup
        //     if ( sx > 13 ) { velocity += (sx-13)*acceleration; }        // throttle input
        //     else if ( sx < 8 ) { velocity -= (8-sx)*deacceleration; }   // brake input
        // }

        // if      ( Input.GetKey("w") ) { velocity += maxAcceleration* acceleration;     } // keyboard throttle
        // else if ( Input.GetKey("s") ) { velocity -= maxDeacceleration* deacceleration; }
    
        // // update speed dial
        // if ( useExternalControlls ) {
        //     char x = (char)((int)velocity);
        //     serialController.SendSerialMessage(""+ x);
        // }

        // TRAIN //

        // apply breaks //
        // locomotive breaks
        double breakForce = locomotiveBreakForce * (( locomotiveMaxMainReservoirPressure - locomotiveMainReservoirPressure) / locomotiveMaxMainReservoirPressure );
        breakForce /= trainMass;
        breakForce *= Time.deltaTime;
        if ( velocity - breakForce >= 0 )
            velocity -= breakForce;
        // train breaks
        breakForce = trainBreakForce * (( trainMaxBreakLinePressure - trainBreakLinePressure) / trainMaxBreakLinePressure );
        breakForce /= trainMass;
        breakForce *= Time.deltaTime;
        if ( velocity - breakForce >= 0 )
            velocity -= breakForce;

        // natural deacceleration
        //       Friction
        //  V² * --------
        //         Mass
        velocity -= Math.Pow(velocity, 2) * ( airFriction / trainMass ) * Time.deltaTime;

        // move train
        gameObject.transform.position += new Vector3( (float)( direction == Directions.forward? velocity : -velocity ) * Time.deltaTime,0,0);

        // // forwards teleportation
        if ( gameObject.transform.position.x > 100 ) {
            gameObject.transform.position -= new Vector3(200,0,0);
            chunkPos += 1;
        }
        if ( gameObject.transform.position.x < -100 ) {
            gameObject.transform.position += new Vector3(200,0,0);
            chunkPos -= 1;
        }
    }

    void OnMessageArrived(string msg)
    {
        // if ( msg.Length >= 1 ) {
        //     sx = msg[0];
        // }
        // if ( msg.Length >= 2 ) {
        //     sy = msg[1];
        // }

        // go through all inputs
        for ( int i = 0; i < Math.Min( inputs.Length, msg.Length ); i++ )
        {
            // get value from message
            int val = (int) msg[i];
            // determine by the type, what value should be extracted
            switch (inputs[i])
            {
                
                case InputDeviceType.throttle:
                    double acc =  trainTractiveEffort / trainMass * ( val / (float)255 ) * (Time.deltaTime * 10) * (val > 2? 1:0);
                    // handle throttle
                    if ( direction != Directions.neutral )
                        velocity += acc;

                    break;

                case InputDeviceType.reverser:
                    // check if train is nearly not moving
                    if ( velocity > 0.1 ) { break; }
                    // set reverser
                    if ( val == 0 ) { direction = Directions.neutral; }
                    if ( val == 1 ) { direction = Directions.reverse; }
                    if ( val == 2 ) { direction = Directions.forward; }

                    break;

                case InputDeviceType.indipendantBreak_NSL:

                    // positions
                    //  release - increase break pressure
                    //  hold - hold break pressure
                    //  apply - decrease break pressure
                    //  more apply - decrease break pressure a bit faster
                    //  emerghency - rapid decrease
                    if ( val == 4 ) { locomotiveMainReservoirPressure += locomotiveBreakPump * Time.deltaTime; }
                    else if ( val == 2 ) { locomotiveMainReservoirPressure -= locomotiveBreakValve * .3  * Time.deltaTime; }
                    else if ( val == 1 ) { locomotiveMainReservoirPressure -= locomotiveBreakValve * .6  * Time.deltaTime; }
                    else if ( val == 0 ) { locomotiveMainReservoirPressure -= locomotiveBreakValve * Time.deltaTime; }

                    break;

                case InputDeviceType.trainBreak_NSL:
                    // positions
                    //  release - increase break pressure
                    //  hold - hold break pressure
                    //  apply - decrease break pressure
                    //  more apply - decrease break pressure a bit faster
                    //  emerghency - rapid decrease
                    if ( val == 4 ) { trainBreakLinePressure += locomotiveBreakValve * Time.deltaTime; }
                    else if ( val == 2 ) { trainBreakLinePressure -= locomotiveBreakValve * .3 * Time.deltaTime; }
                    else if ( val == 1 ) { trainBreakLinePressure -= locomotiveBreakValve * .6 * Time.deltaTime; }
                    else if ( val == 0 ) { trainBreakLinePressure -= locomotiveBreakValve * Time.deltaTime; }

                    break;

                case InputDeviceType.indipendantBreak_SL:

                    // positions
                    //  release - increase break pressure
                    //  hold - hold break pressure
                    //  apply - decrease break pressure
                    //  more apply - decrease break pressure a bit faster
                    //  emerghency - rapid decrease
                    if ( val == 4 ) { locomotiveMainReservoirPressure += locomotiveBreakPump * Time.deltaTime; }
                    else if ( val == 2 ) { locomotiveMainReservoirPressure -= locomotiveBreakValve * .3  * Time.deltaTime; }
                    else if ( val == 1 ) { locomotiveMainReservoirPressure -= locomotiveBreakValve * .6  * Time.deltaTime; }
                    else if ( val == 0 ) { locomotiveMainReservoirPressure -= locomotiveBreakValve * Time.deltaTime; }

                    break;

                case InputDeviceType.trainBreak_SL:
                    // positions
                    //  release - increase break pressure
                    //  hold - hold break pressure
                    //  apply - decrease break pressure
                    //  more apply - decrease break pressure a bit faster
                    //  emerghency - rapid decrease
                    if ( val == 4 ) { trainBreakLinePressure += locomotiveBreakValve * Time.deltaTime; }
                    else if ( val == 2 ) { trainBreakLinePressure -= locomotiveBreakValve * .3 * Time.deltaTime; }
                    else if ( val == 1 ) { trainBreakLinePressure -= locomotiveBreakValve * .6 * Time.deltaTime; }
                    else if ( val == 0 ) { trainBreakLinePressure -= locomotiveBreakValve * Time.deltaTime; }

                    break;

                default:
                    // IDK
                    break;
            }
        }
        // limit breaks
        locomotiveMainReservoirPressure = (double)Mathf.Max( (float)locomotiveMainReservoirPressure, 0 );
        locomotiveMainReservoirPressure = (double)Mathf.Min( (float)locomotiveMainReservoirPressure, (float)locomotiveMaxMainReservoirPressure );
        trainBreakLinePressure = (double)Mathf.Max( (float)trainBreakLinePressure, 0 );
        trainBreakLinePressure = (double)Mathf.Min( (float)trainBreakLinePressure, (float)trainMaxBreakLinePressure );
    }

}
