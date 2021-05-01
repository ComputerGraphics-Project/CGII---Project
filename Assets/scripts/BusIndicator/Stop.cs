using System;
using System.Linq;
using UnityEngine;

public class Stop
{
    public string id { get; }
    public Tuple<float, float> coordinates { get; }
    public int[] nbOfStopsPerHourStep { get; private set; }
    public float[] frequencyIndicators { get; set; }

    public Stop(string id, Tuple<float, float> coordinates, int nbSteps) {
        this.id = id;
        this.coordinates = coordinates;
        this.nbOfStopsPerHourStep = new int[nbSteps];
        this.frequencyIndicators = new float[nbSteps];
    }
    public Stop(string id, Tuple<float, float> coordinates, int[] nbOfStopsPerHourStep, float[] frequencyIndicators) {
        this.id = id;
        this.coordinates = coordinates;
        this.nbOfStopsPerHourStep = nbOfStopsPerHourStep;
        this.frequencyIndicators = frequencyIndicators;
    }

    public void IncreaseNbOfStops(int indexStep) {
        this.nbOfStopsPerHourStep[indexStep]++;
    }

    public int GetMaxNumberOfStops() {
        return this.nbOfStopsPerHourStep.Max();
    }

    public void SetFrequencyIndicator(int maxNbOfStops) {
        for (int i=0; i<this.frequencyIndicators.Length; i++) {
            this.frequencyIndicators[i] = Mathf.Min(1f, (float) this.nbOfStopsPerHourStep[i] / maxNbOfStops);
        }
    }

}
