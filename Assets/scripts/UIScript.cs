using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class UIScript : MonoBehaviour
{
    //UI elements - SimParamPanel
    public TMP_InputField TMP_IF;

    
    //OK and Cancel buttons
    public Button SimParamOKButton;
    public Button SimParamCancelButton;


    //interface variables
    [NonSerialized] public float heatmapSize;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SimParamOK()
    {
        heatmapSize = float.Parse(TMP_IF.text);
        //Debug.Log(heatmapSize);
        //return heatmapSize;
    }


}
