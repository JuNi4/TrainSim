// a list of all freight types
using System.Collections.Generic;

public enum FreightType {
    People,
    Stone, Gravel, Sand,
    Coal, Wood,
    Ethanol, Disel, Oil, Argon, Hydrogen
};

// a list of all train car types
public enum CarType {
    PassengerCar,
    HopperCar,
    FlatCar,
    SteakFlatCar, // probably not spelled that way
    TankCar
};

public enum LoadingType
{
    load, unload,
    both
};

public enum DataType {
    Int, Float
};

public class FreightTypeWeights {
    public Dictionary<FreightType, float> weights = new Dictionary<FreightType, float>();
    
    public void createLookup() {
        weights[FreightType.People] = 80;

        weights[FreightType.Stone] = 1;
        weights[FreightType.Gravel] = 1;
        weights[FreightType.Sand] = 1;
        weights[FreightType.Coal] = 1;

        weights[FreightType.Wood] = 100;

        weights[FreightType.Ethanol] = 1;
        weights[FreightType.Disel] = .85f;
        weights[FreightType.Oil] = .88f;

        weights[FreightType.Argon] = .001784f;
        weights[FreightType.Hydrogen] = .00008375f;
    }
}