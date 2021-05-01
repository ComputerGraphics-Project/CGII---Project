using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEditor;
using Mapbox.Unity.Map;
using Mapbox.Unity.MeshGeneration.Modifiers;

public class BusServiceAvailability: MonoBehaviour
{
    [SerializeField] private int hourStep = 2;
	[SerializeField] private float frequencyWeight = 0.5f;
	[SerializeField] private float distanceWeight = 0.5f;

    [Tooltip("Distance from which the distance indicator will be equal to 0 (in meters).")]
	[SerializeField] private int maxDistance = 555;
    [Tooltip("Duration from which the frequency indicator will be equal to 0 (in minutes).")]
	[SerializeField] private int maxWaitingTime = 30;

    private List<Stop> stops;
    private int currentStep;
    private GameObjectModifier busModifier;
    private VectorSubLayerProperties buildingLayer;


    void Start() {
        SetStops();

        currentStep = 0;
        busModifier = ScriptableObject.CreateInstance<BusIndicatorModifier>();
        buildingLayer = GameObject.Find("Map").GetComponent<AbstractMap>().VectorData.GetFeatureSubLayerAtIndex(0);
    }

    public float[] GetIndicator(float lat, float lon) {
        SetFrequencyIndicator();

        Point point = new Point(new Tuple<float, float>(lat, lon), this.stops, 24/hourStep, maxDistance);
        point.ComputeGlobalIndicator(frequencyWeight, distanceWeight);

        return point.globalIndicator;
    }

    public int GetHourStep() {
        return hourStep;
    }
    public int GetNbOfSteps() {
        return 24 / hourStep;
    }
    public int GetCurrentStep() {
        return currentStep;
    }
    public void SetCurrentStep(int currentStep) {
        this.currentStep = currentStep;

        RemoveModifier();
        AddModifier();
    }
    public void AddModifier() {
        buildingLayer.BehaviorModifiers.AddGameObjectModifier(busModifier);
    }
    public void RemoveModifier() {
        buildingLayer.BehaviorModifiers.RemoveGameObjectModifier(busModifier);
    }

    private void SetStops() {
        BusData busData = SaverLoader.Load();

        if (busData == null || busData.GetHourStep() != this.hourStep) {
            SetStopsFromFile();
        } else {
            this.stops = busData.GetStops();
        }
        GameObject.Find("UIBusIndicator").GetComponent<BusUIManager>().ActivateBusIndicator();
    }

    private void SetStopsFromFile() {
        string text = loadFile("Assets/Resources/BusIndicator/stops.txt");
        string[] lines = Regex.Split(text, "\n");

        int nbStops = lines.Length - 2;
        this.stops = new List<Stop>();

        for (int i=0; i < nbStops; i++) {
            string line = lines[i+1];

            if (!line.Contains("NSR:Quay")) {
                continue;
            }

            string[] quotes = Regex.Split(line, "\"");
            if (quotes.Length > 1) {
                line = quotes[0] + quotes[2];
            }

            string[] values = Regex.Split(line, ",");
            string id = values[0];
            float lat = float.Parse(values[4], System.Globalization.CultureInfo.InvariantCulture);
            float lon = float.Parse(values[5], System.Globalization.CultureInfo.InvariantCulture);

            this.stops.Add(new Stop(id, new Tuple<float, float>(lat, lon), 24/hourStep));
        }

        SetNumberOfStops();
        SaverLoader.Save(this.stops, this.hourStep);
    }
    private void SetNumberOfStops() {
        string text = loadFile("Assets/Resources/BusIndicator/stop_times.txt");
        string[] lines = Regex.Split(text, "\n");

        int nbStopsTimes = lines.Length - 2;

        for (int i=0; i < nbStopsTimes; i++) {
            string line = lines[i+1];

            string[] values = Regex.Split(line, ",");
            string id = values[1];
            string time = values[4];
            int hour = int.Parse(Regex.Split(time, ":")[0]);
            hour %= 24;
            int indexStep = hour / hourStep;

            foreach (Stop stop in this.stops) {
                if (stop.id == id) {
                    stop.IncreaseNbOfStops(indexStep);
                    break;
                }
            }
        }
    }
    private void SetFrequencyIndicator() {
        int maxNbOfStops = hourStep * 60 / maxWaitingTime;

        foreach (Stop stop in this.stops) {
            stop.SetFrequencyIndicator(maxNbOfStops);
        }
    }

    private string loadFile(string filename) {
        TextAsset file = (TextAsset) AssetDatabase.LoadAssetAtPath(filename, typeof(TextAsset));
        if (file == null) {
            throw new Exception(filename + " not found");
        }
        return file.text;
    }
}