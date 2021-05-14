using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class UIScript : MonoBehaviour
{
    ////////////////UI elements/////////////////////
    
    //SimParamPanel
    //UI elements - SimParamPanel
    

    //////Data plugin panel//////////

    public Button Save;
    public Button Load;

    //menu system 
    public GameObject fileBox;
    public TMP_Text option1;
    public TMP_Text option2;
    public TMP_Text option3;
    public TMP_Text option4;

    //temp variables
    private bool fileBoxActive;

    //interface variables
    [NonSerialized] public string ToFileName;  //file name where the data will be saved to
    [NonSerialized] public string FromFileName; //file name where the data will be retrieved from
    
    /////////////////////////////////

    //Simulation parameters panel////

    //OK and Cancel buttons
    public Button SimParamOKButton;
    public Button SimParamCancelButton;


    //User Inputs
    public TMP_InputField TMP_IF;

    //interface variables
    [NonSerialized] public float heatmapSize;

    ////////////////////////////////////

    // Start is called before the first frame update
    void Start()
    {
        fileBox.SetActive(false);
        fileBoxActive = false;
        
    }

    // Update is called once per frame
    void Update()
    {

    }



    //Data plugin

    //save
    public void onSave()
    {
        //show/hide the file box
        fileBox.SetActive(!fileBoxActive);

        //toggle state
        fileBoxActive = !fileBoxActive;    
    }

    public void onLoad()
    {
        
    }


    public void option1OnClick()
    {
        ToFileName = option1.text;
      
    }

    public void option2OnClick()
    {
        ToFileName = option2.text;
     
    }

    public void option3OnClick()
    {
        ToFileName = option3.text;
        
    }

    public void option4OnClick()
    {
        ToFileName = option4.text;
      
    }


}