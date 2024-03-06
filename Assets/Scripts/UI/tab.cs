using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class tab : MonoBehaviour
{

    public int activeElement = 0;

    [Header("Standard / Inactive tab button colors")]
    public Color stdColor = new Color(255, 255, 255, 255);
    public Color stdHoverColor = new Color(200, 200, 200, 255);
    public Color stdClickColor = new Color(150, 150, 150, 255);
    public Color stdDisabledColor = new Color(100, 100, 100, 255);
    public Color stdTextColor = new Color(0, 0, 0, 255);

    [Header("Standard / Inactive tab button colors")]
    public Color activeColor = new Color(0, 0, 0, 255);
    public Color activeHoverColor = new Color(100, 100, 100, 255);
    public Color activeClickColor = new Color(150, 150, 150, 255);
    public Color activeDisabledColor = new Color(200, 200, 200, 255);
    public Color activeTextColor = new Color(255, 255, 255, 255);

    public GameObject tabs;
    public GameObject contents;

    // Start is called before the first frame update
    void Start()
    {
        // get tabs and contents
        tabs = gameObject.transform.GetChild(0).gameObject;
        contents = gameObject.transform.GetChild(1).gameObject;

        disableAll();
        setActiveElement(activeElement);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // change the active element
    public void setActiveElement(int element)
    {
        // disable current element
        contents.transform.GetChild(activeElement).gameObject.SetActive( false );

        // set the color of the tab button
        Button btn = tabs.transform.GetChild(activeElement + 1).GetComponent<Button>();
        TextMeshProUGUI txt = tabs.transform.GetChild(activeElement + 1).transform.GetChild(0).GetComponent<TextMeshProUGUI>();

        ColorBlock cb = btn.colors;
        cb.normalColor = stdColor;
        cb.highlightedColor = stdHoverColor;
        cb.pressedColor = stdClickColor;
        cb.selectedColor = stdColor;
        cb.disabledColor = stdDisabledColor;

        txt.color = stdTextColor;

        btn.colors = cb;

        // enable next element
        activeElement = element;
        contents.transform.GetChild(activeElement).gameObject.SetActive( true );

        // set the color of the tab button
        btn = tabs.transform.GetChild(activeElement + 1).GetComponent<Button>();
        txt = tabs.transform.GetChild(activeElement + 1).transform.GetChild(0).GetComponent<TextMeshProUGUI>();


        cb = btn.colors;

        cb.normalColor = activeColor;
        cb.highlightedColor = activeHoverColor;
        cb.pressedColor = activeClickColor;
        cb.selectedColor = activeColor;
        cb.disabledColor = activeDisabledColor;

        txt.color = activeTextColor;

        btn.colors = cb;

    }

    public void disableAll()
    {
        for ( int i = 0; i < contents.transform.childCount; i++)
        {
            // disable them
            contents.transform.GetChild(i).gameObject.SetActive( false );

            // set button
            Button btn = tabs.transform.GetChild(i + 1).GetComponent<Button>();
            TextMeshProUGUI txt = tabs.transform.GetChild(i + 1).transform.GetChild(0).GetComponent<TextMeshProUGUI>();

            ColorBlock cb = btn.colors;
            cb.normalColor = stdColor;
            cb.highlightedColor = stdHoverColor;
            cb.pressedColor = stdClickColor;
            cb.selectedColor = stdColor;
            cb.disabledColor = stdDisabledColor;

            txt.color = stdTextColor;

            btn.colors = cb;
        }
    }
}
