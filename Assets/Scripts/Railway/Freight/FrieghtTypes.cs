// a list of all freight types
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
    SteakFlatCar, // probably not speeled that way
    TankCar
};

public enum LoadingType
{
    load, unload,
    both
};