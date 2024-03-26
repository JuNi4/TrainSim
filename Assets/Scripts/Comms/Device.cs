using UnityEditor.EditorTools;
using UnityEngine;


// Base Device class for all electrical devices
[System.Serializable]
public class Device
{
    // all types of the component
    public enum DeviceType
    {
        Potentiometer,
        Switch5,
        Switch3,
        Switch2
    }

    // the types of devices hooked up as the component
    public enum InterpretationDeviceType
    {
        throttle,
        reverser,
        indipendantBreak,
        trainBreak
    }

    // the pin for the device
    [Header("Pins")]
    [Tooltip("Main Pin, always needed")]
    public uint primaryPin;
    [Tooltip("Secondary Pin, only needed for Switch3")]
    public uint secondaryPin;

    [Header("Settings")]
    // the device type
    [Tooltip("The type of device.\n Potentiometer - Used for throttle\n Switch 5 - Used for breaks\n Switch 3 - Used for the reverser\n Switch 2 - Standart ON/OFF switch")]
    public DeviceType deviceType;

    [Tooltip("What the device should be interpreted as")]
    public InterpretationDeviceType interpretationDeviceType;
}