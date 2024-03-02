using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Servo
{

    // min and max angle of the servo
    public int servoMinAngle = 0;
    public int servoMaxAngle = 180;

    // the minimum and maximum amount the dial will display
    public int minValue = 0;
    public int maxValue = 1;

    // weather or not to flip the direction the dial travels in
    bool flip = true;

    // Dial type ( what value is displayed on it )
    public enum DialType
    {
        Speed,
        Fuel, Sand,

        BreakLinePressure, MainResPressure,

        Temp
    }

    public DialType type;

    // init method
    public void servo() { }

    // calculate to wich angle the servo needs to go to
    public int getAngle( double value )
    {
        // get the position of the value in the value range
        double x = (value - minValue) / maxValue;
        int angle = (int)(servoMinAngle + x * ( servoMaxAngle - servoMinAngle ));

        if ( flip ) { return 180 - angle; }
        
        return angle;
    }
}
