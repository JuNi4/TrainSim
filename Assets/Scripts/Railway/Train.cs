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
    public float locomotiveMass = 10000f;

    // locomotive tractive effort
    public float locomotiveTractiveEffort = 1000f;

    // locomotive break force
    public float locomotiveBreakForce = 500f;

    public float locomotiveMaxMainReservoirPressure = 5f;
    public float locomotiveMainReservoirPressure = 0f;
    public float locomotiveMaxBreakLinePressure = 5f;
    public float locomotiveBreakPump = .1f;
    public float locomotiveBreakValve = .2f;

    // fuel & sand capacity
    public float locomotiveFuelCapacity = 300f;
    public float locomotiveFuelConsumption = .1f;

    public float locomotiveSandCapacity = 280f;
    public float locomotiveSandConsumption = .1f;

    // friction with air
    public float airFriction = 0.01f;

    // wheelslip
    public float wheelSlipFactor = 1f;

    public float wheelSlipSandFactor = 1f;
    public float wheelSlipMassFactor = 1f;

    public enum BreakStyle {
        NonSelfLapping,
        SelfLapping
    }
    public BreakStyle breakStyle;

    // train values //
    [Header("Train Variables")]
    // velocity
    public float velocity = 0;
    
    // a list of directions
    public enum Directions
    { forward, neutral, reverse };

    // the direction the train will travel in
    public Directions direction;

    // automaticly set train vars //
    // total mass of the entire train
    public float trainMass;

    // break power of the carts
    public float trainBreakForce;

    public float trainTractiveEffort;

    public float trainMaxBreakLinePressure;
    public float trainBreakLinePressure = 0;

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

    [Header("Life Values")]
    public float throttle;
    public int indipendantBreakPos;
    public int trainBreakPos;

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

    void applyBreaks()
    {
        // non self lapping breaks
        if ( breakStyle == BreakStyle.NonSelfLapping ) {
            // indipendant break
            if ( indipendantBreakPos == 4 ) { locomotiveMainReservoirPressure += locomotiveBreakPump * Time.deltaTime; }
            else if ( indipendantBreakPos == 2 ) { locomotiveMainReservoirPressure -= locomotiveBreakValve * .3f  * Time.deltaTime; }
            else if ( indipendantBreakPos == 1 ) { locomotiveMainReservoirPressure -= locomotiveBreakValve * .6f  * Time.deltaTime; }
            else if ( indipendantBreakPos == 0 ) { locomotiveMainReservoirPressure -= locomotiveBreakValve * Time.deltaTime; }

            // train break
            if ( trainBreakPos == 0 ) { trainBreakLinePressure += locomotiveBreakValve; }
            else if ( trainBreakPos == 2 ) { trainBreakLinePressure -= locomotiveBreakValve * .3f * Time.deltaTime; }
            else if ( trainBreakPos == 3 ) { trainBreakLinePressure -= locomotiveBreakValve * .6f * Time.deltaTime; }
            else if ( trainBreakPos == 4 ) { trainBreakLinePressure -= locomotiveBreakValve * Time.deltaTime; }
        }
        // limit breaks
        locomotiveMainReservoirPressure = Mathf.Max( locomotiveMainReservoirPressure, 0 );
        locomotiveMainReservoirPressure = Mathf.Min( locomotiveMainReservoirPressure, locomotiveMaxMainReservoirPressure );
        trainBreakLinePressure = Mathf.Max( trainBreakLinePressure, 0 );
        trainBreakLinePressure = Mathf.Min( trainBreakLinePressure, trainMaxBreakLinePressure );
    }


    // keyboard controlls
    void controlls()
    {
        // set reverser position
        if ( Input.GetKeyDown("w") && velocity < 0.1 ) {
            direction = Directions.forward;
        } else if ( Input.GetKeyDown("s") && velocity < 0.1 ) {
            direction = Directions.reverse;
        }
        // set throttle
        if ( Input.GetKey("e") ) {
            throttle += .01f;
        } else if ( Input.GetKey("d") ) {
            throttle -= .01f;
        }
        throttle = Mathf.Max( throttle, 0 );
        throttle = Mathf.Min( throttle, 1 );
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

        // key board controlls
        controlls();

        // apply breaks
        applyBreaks();

        // TRAIN //

        // throttle
        float acc =  trainTractiveEffort / trainMass * throttle * (Time.deltaTime * 10);
        // handle throttle
        if ( direction != Directions.neutral )
            velocity += acc;

        // apply breaks //
        // locomotive breaks
        float breakForce = locomotiveBreakForce * (( locomotiveMaxMainReservoirPressure - locomotiveMainReservoirPressure) / locomotiveMaxMainReservoirPressure );
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
        velocity -= Mathf.Pow(velocity, 2) * ( airFriction / trainMass ) * Time.deltaTime;

        // move train
        gameObject.transform.position += new Vector3( ( direction == Directions.forward? velocity : -velocity ) * Time.deltaTime,0,0);

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
        // go through all inputs
        for ( int i = 0; i < Math.Min( inputs.Length, msg.Length ); i++ )
        {
            // get value from message
            int val = (int) msg[i];
            // determine by the type, what value should be extracted
            switch (inputs[i])
            {
                
                case InputDeviceType.throttle:
                    throttle = val / 255f;

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
                    //  more apply - decrease break pressure a bit faster - not actually a thing
                    //  emerghency - rapid decrease & emergency break

                    // set position
                    indipendantBreakPos = val;                    

                    break;

                case InputDeviceType.trainBreak_NSL:
                    // positions
                    //  release - increase break pressure
                    //  hold - hold break pressure
                    //  apply - decrease break pressure
                    //  more apply - decrease break pressure a bit faster
                    //  emerghency - rapid decrease
                    
                    // set break pos
                    trainBreakPos = val;

                    break;

                default:
                    // IDK
                    break;
            }
        }
    }

}
