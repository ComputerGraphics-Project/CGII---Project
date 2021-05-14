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
    public TMP_InputField TMP_IF;

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
    [NonSerialized] public string ToFile;  //file name where the data will be saved to
    [NonSerialized] public string FromFile; //file name where the data will be retrieved from
    
    /////////////////////////////////

    //OK and Cancel buttons
    public Button SimParamOKButton;
    public Button SimParamCancelButton;





    //interface variables
    [NonSerialized] public float heatmapSize;


    

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

    public void SimParamOK()
    {
        heatmapSize = float.Parse(TMP_IF.text);
        //Debug.Log(heatmapSize);
        //return heatmapSize;
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


    public void option1OnClick()
    {
        ToFile = option1.text;
      
    }

    public void option2OnClick()
    {
        ToFile = option2.text;
     
    }

    public void option3OnClick()
    {
        ToFile = option3.text;
        
    }

    public void option4OnClick()
    {
        ToFile = option4.text;
      
    }




}
