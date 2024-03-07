using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class tab : MonoBehaviour
{

    [Header("Tab System Setup")]
    public int tabIndex = 0;
    public GameObject tabs;

    public int tabIndexOffset = 1;

    [Space(10)]
    public int contentIndex = 1;
    public GameObject contents;
    public int activeElement = 0;

    [Space(10)]
    public string leftKey = "";
    public string rightKey = "";

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

    // Start is called before the first frame update
    void Start()
    {
        // get tabs and contents
        if ( tabs == null ) {
            tabs = gameObject.transform.GetChild(tabIndex).gameObject;
        }
        if ( contents == null ) {
            contents = gameObject.transform.GetChild(contentIndex).gameObject;
        }

        disableAll();
        setActiveElement(activeElement);
    }

    // Update is called once per frame
    void Update()
    {
        if ( leftKey != "" && rightKey != "" ) {
            // keyboard navigation to the left
            if ( Input.GetKeyDown(leftKey) && activeElement > 0 ) {
                setActiveElement(activeElement-1);
            }
            // keyboard navigation to the right
            if ( Input.GetKeyDown(rightKey) && activeElement < contents.transform.childCount-1 ) {
                setActiveElement(activeElement+1);
            }
        }
    }

    // change the active element
    public void setActiveElement(int element)
    {
        // disable current element
        contents.transform.GetChild(activeElement).gameObject.SetActive( false );

        // set the color of the tab button
        Button btn = tabs.transform.GetChild(activeElement + tabIndexOffset).GetComponent<Button>();
        TextMeshProUGUI txt = tabs.transform.GetChild(activeElement + tabIndexOffset).transform.GetChild(0).GetComponent<TextMeshProUGUI>();

        ColorBlock cb = btn.colors;
        cb.normalColor = stdColor;
        cb.highlightedColor = stdHoverColor;
        cb.pressedColor = stdClickColor;
        cb.selectedColor = stdClickColor;
        cb.disabledColor = stdDisabledColor;

        txt.color = stdTextColor;

        btn.colors = cb;

        // enable next element
        activeElement = element;
        contents.transform.GetChild(activeElement).gameObject.SetActive( true );

        // set the color of the tab button
        btn = tabs.transform.GetChild(activeElement + tabIndexOffset).GetComponent<Button>();
        txt = tabs.transform.GetChild(activeElement + tabIndexOffset).transform.GetChild(0).GetComponent<TextMeshProUGUI>();


        cb = btn.colors;

        cb.normalColor = activeColor;
        cb.highlightedColor = activeHoverColor;
        cb.pressedColor = activeClickColor;
        cb.selectedColor = activeClickColor;
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
            Button btn = tabs.transform.GetChild(i + tabIndexOffset).GetComponent<Button>();
            TextMeshProUGUI txt = tabs.transform.GetChild(i + tabIndexOffset).transform.GetChild(0).GetComponent<TextMeshProUGUI>();

            ColorBlock cb = btn.colors;
            cb.normalColor = stdColor;
            cb.highlightedColor = stdHoverColor;
            cb.pressedColor = stdClickColor;
            cb.selectedColor = stdClickColor;
            cb.disabledColor = stdDisabledColor;

            txt.color = stdTextColor;

            btn.colors = cb;
        }
    }
}
