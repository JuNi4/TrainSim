using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class set_state : MonoBehaviour
{

    public state_object[] obj;

    // Start is called before the first frame update
    void Start()
    {
        foreach (var item in obj)
        {
            item.target.SetActive(item.startEnabled);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
