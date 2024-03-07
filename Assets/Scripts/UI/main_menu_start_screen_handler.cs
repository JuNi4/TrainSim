using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class main_menu_start_screen_handler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void quit() {
        Application.Quit();
    }

    public void play() {
        gameObject.SetActive( false );
        GameObject other_menu = transform.parent.GetChild(1).gameObject;
        other_menu.SetActive( true );
    }
}
