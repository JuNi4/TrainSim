using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Station : FixedObject
{

    public int maxPeopleEntering;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // move the station
        base.move();
    }
}
