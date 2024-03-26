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
    // [Header("Serial Comms Controller")] 
    // public SerialController serialController;
    // public bool useExternalControlls = true;

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

    [Header("Life Values")]
    public float throttle;
    public int indipendantBreakPos;
    public int trainBreakPos;

    // function for calculating the tractive effort int the train
    public void recalculateTrainTractiveEffort()
    {
        // add current locomotive tractive effort
        this.trainTractiveEffort = this.locomotiveTractiveEffort;
    }
    public void recalculateTrainBreakForce()
    {
        this.trainBreakForce = 0;
        // add all break forces
        for ( int i = 0; i < cars.Length; i++ ) {
            this.trainBreakForce += this.cars[i].carBreakForce;
        }
    }
    public void recalculateTrainMaxBreakLinePressure()
    {
        // add current locomotive break pressure
        this.trainMaxBreakLinePressure = this.locomotiveMaxBreakLinePressure;
        // add all break forces
        for ( int i = 0; i < cars.Length; i++ ) {
            this.trainMaxBreakLinePressure += this.cars[i].carMaxBrakeLinePressure;
        }
    }
    public void recalculateTrainMass()
    {
        // add current locomotive mass
        this.trainMass = this.locomotiveMass;
        // add all break forces
        for ( int i = 0; i < this.cars.Length; i++ ) {
            this.trainMass += this.cars[i].carMass;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // go to 0 0 0
        gameObject.transform.position = new Vector3(0,0,0);


        this.recalculateTrainTractiveEffort();
        this.recalculateTrainBreakForce();
        this.recalculateTrainMaxBreakLinePressure();
        this.recalculateTrainMass();
    }

    void applyBreaks()
    {
        // non self lapping breaks
        if ( this.breakStyle == BreakStyle.NonSelfLapping ) {
            // indipendant break
            if ( this.indipendantBreakPos == 4 ) { this.locomotiveMainReservoirPressure += this.locomotiveBreakPump * Time.deltaTime; }
            else if ( this.indipendantBreakPos == 2 ) { this.locomotiveMainReservoirPressure -= this.locomotiveBreakValve * .3f  * Time.deltaTime; }
            else if ( this.indipendantBreakPos == 1 ) { this.locomotiveMainReservoirPressure -= this.locomotiveBreakValve * .6f  * Time.deltaTime; }
            else if ( this.indipendantBreakPos == 0 ) { this.locomotiveMainReservoirPressure -= this.locomotiveBreakValve * Time.deltaTime; }

            // train break
            if ( this.trainBreakPos == 0 ) { this.trainBreakLinePressure += this.locomotiveBreakValve; }
            else if ( this.trainBreakPos == 2 ) { this.trainBreakLinePressure -= this.locomotiveBreakValve * .3f * Time.deltaTime; }
            else if ( this.trainBreakPos == 3 ) { this.trainBreakLinePressure -= this.locomotiveBreakValve * .6f * Time.deltaTime; }
            else if ( this.trainBreakPos == 4 ) { this.trainBreakLinePressure -= this.locomotiveBreakValve * Time.deltaTime; }
        }
        // limit breaks
        this.locomotiveMainReservoirPressure = Mathf.Max( this.locomotiveMainReservoirPressure, 0 );
        this.locomotiveMainReservoirPressure = Mathf.Min( this.locomotiveMainReservoirPressure, this.locomotiveMaxMainReservoirPressure );
        this.trainBreakLinePressure = Mathf.Max( this.trainBreakLinePressure, 0 );
        this.trainBreakLinePressure = Mathf.Min( this.trainBreakLinePressure, this.trainMaxBreakLinePressure );
    }


    // keyboard controlls
    void controlls()
    {
        // set reverser position
        if ( Input.GetKeyDown("w") && velocity < 0.1 ) {
            this.direction = Directions.forward;
        } else if ( Input.GetKeyDown("s") && velocity < 0.1 ) {
            this.direction = Directions.reverse;
        }
        // set throttle
        if ( Input.GetKey("e") ) {
            this.throttle += .01f;
        } else if ( Input.GetKey("d") ) {
            this.throttle -= .01f;
        }
        this.throttle = Mathf.Max( this.throttle, 0 );
        this.throttle = Mathf.Min( this.throttle, 1 );
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
        this.controlls();


        // TRAIN //

        // throttle
        float acc =  this.trainTractiveEffort / this.trainMass * this.throttle * (Time.deltaTime * 10);
        // handle throttle
        if ( this.direction != Directions.neutral )
            this.velocity += acc;

        // apply breaks
        this.applyBreaks();

        // apply breaks //
        // locomotive breaks
        float breakForce = this.locomotiveBreakForce * (( this.locomotiveMaxMainReservoirPressure - this.locomotiveMainReservoirPressure) / this.locomotiveMaxMainReservoirPressure );
        breakForce /= this.trainMass;
        breakForce *= Time.deltaTime;
        if ( this.velocity - breakForce >= 0 )
            this.velocity -= breakForce;
        // train breaks
        breakForce = this.trainBreakForce * (( this.trainMaxBreakLinePressure - this.trainBreakLinePressure) / this.trainMaxBreakLinePressure );
        breakForce /= this.trainMass;
        breakForce *= Time.deltaTime;
        if ( this.velocity - breakForce >= 0 )
            this.velocity -= breakForce;

        // natural deacceleration
        //       Friction
        //  V² * --------
        //         Mass
        this.velocity -= Mathf.Pow(this.velocity, 2) * ( this.airFriction / this.trainMass ) * Time.deltaTime;

        // move train
        gameObject.transform.position += new Vector3( ( this.direction == Directions.forward? this.velocity : -this.velocity ) * Time.deltaTime,0,0);

        // // forwards teleportation
        if ( gameObject.transform.position.x > 100 ) {
            gameObject.transform.position -= new Vector3(200,0,0);
            this.chunkPos += 1;
        }
        if ( gameObject.transform.position.x < -100 ) {
            gameObject.transform.position += new Vector3(200,0,0);
            this.chunkPos -= 1;
        }
    }
}
