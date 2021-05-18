using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BusUIManager : MonoBehaviour
{
    [SerializeField] private Slider busTimeSlider;
    [SerializeField] private TMP_Text busTimeLabel;
    [SerializeField] private TMP_InputField hourStep;
    [SerializeField] private TMP_InputField maxDistance;
    [SerializeField] private TMP_InputField reliabilityFactor;
    [SerializeField] private Button OKButton;
    [SerializeField] private Button cancelButton;

    private BusServiceAvailability busServiceAvailability;
    private const string defaultHourStep = "2";
    private const string defaultMaxDistanceStep = "640";
    private const string defaultReliabilityFactor = "2";

    void Awake() {
        busTimeSlider.minValue = 0;
        busTimeSlider.wholeNumbers = true;

        hourStep.text = defaultHourStep;
        maxDistance.text = defaultMaxDistanceStep;
        reliabilityFactor.text = defaultReliabilityFactor;

        OKButton.onClick.AddListener(delegate {OnOKButtonClicked(); });
        cancelButton.onClick.AddListener(delegate {OnCancelButtonClicked(); });
    }

    public void OnOKButtonClicked() {
        if (busServiceAvailability == null) {
            busServiceAvailability = GameObject.Find("BusIndicator").GetComponent<BusServiceAvailability>();
        }

        busServiceAvailability.hourStep = int.Parse(hourStep.text);
        busTimeSlider.maxValue = busServiceAvailability.GetNbOfSteps()-1;
        busServiceAvailability.maxDistance = float.Parse(maxDistance.text);
        busServiceAvailability.reliabilityFactor = float.Parse(reliabilityFactor.text);
        busServiceAvailability.SetStops();
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

    private void OnCancelButtonClicked() {
        hourStep.text = defaultHourStep;
        maxDistance.text = defaultMaxDistanceStep;
        reliabilityFactor.text = defaultReliabilityFactor;
        OnOKButtonClicked();
    }
}
