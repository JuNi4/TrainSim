using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class start_in_state : MonoBehaviour
{

    public enum State {
        Active,
        Inactive
    };
    public State startState = State.Active;

    // Start is called before the first frame update
    void Start()
    {
        if ( startState == State.Active ) {
            gameObject.SetActive( true );
        }
        if ( startState == State.Inactive ) {
            gameObject.SetActive( false );
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
