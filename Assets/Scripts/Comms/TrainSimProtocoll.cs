using UnityEngine;

using System;
using System.Collections;
using System.IO;
using Unity.VisualScripting;

public class TrainSimProtocoll : MonoBehaviour
{
    // train object
    public Train train;

    [Header("Serial Settings")]
    public uint baudRate = 9600;

    // a list of servos
    public Servo[] servos;

    // a list of devices //
    public Device[] devices;

    // TMP: only public tempoarely
    // keep track of if a device is connected
    public bool isConnected;
    public TSPSerialController serialController;

    public bool ranSetup = false;

    public int requestIndex = 0;

    // Serial message intents //
    // Invalid intent
    protected int TSP_INTENT_NONE = 0x00;

    // Send by game to get info about the connected components
    protected int TSP_INTENT_INFO = 0x01;

    // Intent for setting a value for a compenent
    protected int TSP_INTENT_SETVAL = 0x02;

    // Intent for reading a value
    protected int TSP_INTENT_GETVAL = 0x03;
    
    // Intent for clearing all registered components
    protected int TSP_INTENT_CLRCOMPONENT = 0x04;

    // Intent for adding a component to the arduino
    protected int TSP_INTENT_ADDCOMPONENT = 0x05;

    protected void removeAllDevices()
    {
        // send the remove devices
        this.serialController.SendSerialMessage( "" + TSP_INTENT_CLRCOMPONENT + (char)0 + (char)0 + (char)0 + (char)0 + (char)0 );
    }

    protected void addDevice( Device device )
    {
        // don't continue if the serial port is not connected
        if ( ! this.isConnected ) { return; }

        // remove all devices just to be sure
        removeAllDevices();

        // add the device
        int id = 0;
        switch ( device.deviceType )
        {
            case Device.DeviceType.Potentiometer:
                id = 1;
                break;
            case Device.DeviceType.Switch5:
                id = 2;
                break;
            case Device.DeviceType.Switch3:
                id = 3;
                break;
            case Device.DeviceType.Switch2:
                id = 4;
                break;
            default:
                return;
        }

        // send a message to the arduino to register the device //

        char sp = (char) device.secondaryPin;
        char mp = (char) device.primaryPin;

        char blank = (char)0;

        string msg = "" +(char)this.TSP_INTENT_ADDCOMPONENT + (char) id + blank + blank + sp + mp ;

        // for(int i=0;i<msg.Length;i++){Debug.Log(i+" "+(int)msg[i]);}

        // send message
        this.serialController.SendSerialMessage( msg );
        // for(int i=0;i<msg.Length;i++)
        //     this.serialController.SendSerialMessage( ""+msg[i] );
    }

    void Setup()
    {
        // add all the components
        for ( int i = 0; i < this.devices.Length; i++ )
        {
            // add the component
            this.addDevice( this.devices[i] );
        }

        this.ranSetup = true;
    }

    void Start()
    {
        // startup logic
        this.serialController = gameObject.GetComponent<TSPSerialController>();

        this.serialController.SetTearDownFunction(this.OnTearDown);
    }

    void Update()
    {
        // don't continue if the serial port is not connected
        if ( ! this.isConnected ) { return; }

        if ( ! this.ranSetup ) { Setup(); }

        // request all the values
        char blank = (char)0;
        this.serialController.SendSerialMessage( ""+ (char) TSP_INTENT_GETVAL + (char) requestIndex + blank + blank + blank + blank );
        requestIndex++;
        if ( requestIndex >= devices.Length )
            requestIndex = 0;

    }

    void OnApplicationQuit()
    {
        // remove all devices
        this.OnTearDown();
    }

    void OnConnectionEvent( bool status )
    {
        // set the connected status
        this.isConnected = status;
    }

    void OnMessageArrived(string message)
    {

        if (message == null) { return; }

        // Decode message

        int intent = (int) message[0];
        int id = (int) message[1];

        int msg = 0;

        for ( int i = 0; i < 4; i++ )
        {
            msg += message[2+i] << (8 * (3-i));
        }

        // Debug.Log(msg);

        // evaluate message

        // check if a device with the provided id is present
        if ( devices.Length < id || id < 0 )
            return;

        // determine by the type, what value should be extracted
        switch (devices[id].interpretationDeviceType)
        {
            
            case Device.InterpretationDeviceType.throttle:
                this.train.throttle = msg / 1023f;

                break;

            case Device.InterpretationDeviceType.reverser:
                // check if train is nearly not moving
                if ( this.train.velocity > 0.1 ) { break; }
                // set reverser
                if ( msg == 0 ) { this.train.direction = Train.Directions.neutral; }
                if ( msg == 1 ) { this.train.direction = Train.Directions.reverse; }
                if ( msg == 2 ) { this.train.direction = Train.Directions.forward; }

                break;

            case Device.InterpretationDeviceType.indipendantBreak:

                // positions
                //  release - increase break pressure
                //  hold - hold break pressure
                //  apply - decrease break pressure
                //  more apply - decrease break pressure a bit faster - not actually a thing
                //  emerghency - rapid decrease & emergency break

                // set position
                this.train.indipendantBreakPos = msg;

                break;

            case Device.InterpretationDeviceType.trainBreak:
                // positions
                //  release - increase break pressure
                //  hold - hold break pressure
                //  apply - decrease break pressure
                //  more apply - decrease break pressure a bit faster
                //  emerghency - rapid decrease
                
                // set break pos
                this.train.trainBreakPos = msg;

                break;

            default:
                // IDK
                break;
        }
    }

    public void OnTearDown()
    {
        // remove all devices
        this.removeAllDevices();

        // read a couple of messages
        this.serialController.ReadSerialMessage();
    }
}