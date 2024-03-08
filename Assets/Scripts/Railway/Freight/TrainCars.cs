using UnityEngine;

[System.Serializable]
public class TrainCar {
    // freight type
    public FreightType freightType;

    public CarType carType;

    // capacity
    public float capacity;

    public float fillLevel;

    [Header("Car Stats")]
    public float carMaxBrakeLinePressure = 2.5f;
    public float carBreakForce = 400;

    public float carMass = 500;

}