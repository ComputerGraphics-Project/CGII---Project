using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BusUIManager : MonoBehaviour
{
    [SerializeField] private Toggle busToggle;
    [SerializeField] private Slider busTimeSlider;
    [SerializeField] private TMP_Text busTimeLabel;
    private BusServiceAvailability busServiceAvailability;

    void Start() {
        busServiceAvailability = GameObject.Find("BusIndicator").GetComponent<BusServiceAvailability>();

        busToggle.onValueChanged.AddListener(delegate {OnBusToggleChanged(); });

        busTimeSlider.minValue = 0;
        busTimeSlider.maxValue = busServiceAvailability.GetNbOfSteps()-1;
        busTimeSlider.wholeNumbers = true;
        busTimeSlider.onValueChanged.AddListener(delegate {OnBusTimeSliderChanged(); });
        busTimeSlider.interactable = false;
    }

    public void ActivateBusIndicator() {
        busToggle.interactable = true;
    }

    private void OnBusToggleChanged() {
        if (busToggle.isOn) {
            busTimeSlider.interactable = true;
            OnBusTimeSliderChanged();
        } else {
            busTimeSlider.interactable = false;
            busServiceAvailability.RemoveModifier();
        }
    }
    private void OnBusTimeSliderChanged() {
        busServiceAvailability.SetCurrentStep((int) busTimeSlider.value);
        
        int beginningHour = (int) busTimeSlider.value * busServiceAvailability.GetHourStep();
        string firstHour = beginningHour.ToString();
        if (firstHour.Length < 2) {
            firstHour = "0" + firstHour;
        }
        string secondHour = (beginningHour + busServiceAvailability.GetHourStep()).ToString();
        if (secondHour.Length < 2) {
            secondHour = "0" + secondHour;
        }

        busTimeLabel.text = firstHour + ":00 - " + secondHour + ":00";
    }
}
