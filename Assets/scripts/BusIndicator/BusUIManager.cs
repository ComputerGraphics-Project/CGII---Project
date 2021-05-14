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
    [SerializeField] private TMP_InputField hourStep;
    [SerializeField] private TMP_InputField maxDistance;
    [SerializeField] private TMP_InputField reliabilityFactor;
    private BusServiceAvailability busServiceAvailability;

    void Start() {
        busServiceAvailability = GameObject.Find("BusIndicator").GetComponent<BusServiceAvailability>();

        busToggle.onValueChanged.AddListener(delegate {OnBusToggleChanged(); });

        busTimeSlider.minValue = 0;
        busTimeSlider.maxValue = busServiceAvailability.GetNbOfSteps()-1;
        busTimeSlider.wholeNumbers = true;
        busTimeSlider.onValueChanged.AddListener(delegate {OnBusTimeSliderChanged(); });
        busTimeSlider.interactable = false;

        hourStep.text = (busServiceAvailability.hourStep).ToString();
        maxDistance.text = (busServiceAvailability.maxDistance).ToString();
        reliabilityFactor.text = (busServiceAvailability.reliabilityFactor).ToString();

        hourStep.onValueChanged.AddListener(delegate {OnInputChanged(); });
        maxDistance.onValueChanged.AddListener(delegate {OnInputChanged(); });
        reliabilityFactor.onValueChanged.AddListener(delegate {OnInputChanged(); });
    }

    public void ActivateBusIndicator() {
        busToggle.interactable = true;
    }

    private void OnBusToggleChanged() {
        if (busToggle.isOn) {
            busTimeSlider.interactable = true;
            busServiceAvailability.SetStops();
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

    private void OnInputChanged() {
        busServiceAvailability.hourStep = int.Parse(hourStep.text);
        busServiceAvailability.maxDistance = float.Parse(maxDistance.text);
        busServiceAvailability.reliabilityFactor = float.Parse(reliabilityFactor.text);
    }
}
