using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // Can change this to get the different images in the script without public prefabs like this
    public GameObject BusIndicatorUI;
    public GameObject SkyExposureUI;
    public GameObject LandmarkUI;
    public GameObject ShadowHeatmapUI;
    public TMPro.TMP_Dropdown UIdropdown;
    private GameObject currentUI;
    // Start is called before the first frame update
    void Start()
    {
        currentUI = new GameObject();
        UIdropdown.onValueChanged.AddListener(delegate
        {
            changeUI(UIdropdown.value);
        });
        hideAll();
    }

    // Update is called once per frame
    void Update()
    {

    }

    GameObject getUI(int ui)
    {
        var uiImage = new GameObject();

        switch (ui)
        {
            case 1:
                uiImage = SkyExposureUI;
                break;
            case 2:
                uiImage = ShadowHeatmapUI;
                break;
            case 3:
                uiImage = LandmarkUI;
                break;
            case 4:
                uiImage = BusIndicatorUI;
                break;
        }
        return uiImage;
    }

    void hideAll()
    {
        BusIndicatorUI.SetActive(false);
        SkyExposureUI.SetActive(false);
        ShadowHeatmapUI.SetActive(false);
        LandmarkUI.SetActive(false);
    }

    void toggleActiveUI(GameObject ui, bool b)
    {

        for (int i = 0; i < ui.transform.childCount; i++)
        {
            var child = ui.transform.GetChild(i).gameObject;
            if (child) child.SetActive(b);
        }
        ui.gameObject.SetActive(b);
        if (b) currentUI = ui;
    }

    void changeUI(int ui)
    {
        var newUI = getUI(ui);

        toggleActiveUI(currentUI, false);
        toggleActiveUI(newUI, true);
    }
}
