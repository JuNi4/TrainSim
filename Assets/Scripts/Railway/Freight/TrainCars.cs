using UnityEngine;

[System.Serializable]
public class TrainCar {
    // freight type
    public FreightType freightType;

    public CarType carType;

    // capacity
    public float capacity;

    [Header("Car Stats")]
    public double carMaxBrakeLinePressure = 2.5;
    public double carBreakForce = 400;

    public double carMass = 500;

}