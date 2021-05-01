using System;
using System.Collections.Generic;
using UnityEngine;

public class Point
{
    public Tuple<float, float> coordinates { get; }
    public float distanceToNearestStop { get; private set; }
    public Stop nearestStop {get; private set; }
    public float distanceIndicator { get; set; }
    public float[] globalIndicator { get; set; }

    public Point(Tuple<float, float> coordinates, List<Stop> stops, int nbSteps, int maxDistance)
    {
        this.coordinates = coordinates;
        this.distanceToNearestStop = Mathf.Infinity;
        this.globalIndicator = new float[nbSteps];
        SetNearestStop(stops);
        this.distanceIndicator = Mathf.Min(1, 1 - this.distanceToNearestStop / (maxDistance * 0.00001f));
    }
    private void SetNearestStop(List<Stop> stops) {
        foreach (Stop stop in stops) {
            float distance = Point.Distance(this.coordinates, stop.coordinates);

            if (distance < this.distanceToNearestStop) {
                this.distanceToNearestStop = distance;
                this.nearestStop = stop;
            }
        }
    }

    public static float Distance(Tuple<float, float> from, Tuple<float, float> to) {
        return Mathf.Sqrt(Mathf.Pow(from.Item1 - to.Item1, 2) + Mathf.Pow(from.Item2 - to.Item2, 2));
    }

    public void ComputeGlobalIndicator(float frequencyWeight, float distanceWeight) {
        int n = this.globalIndicator.Length;
        for (int i=0; i<n; i++) {
            this.globalIndicator[i] = frequencyWeight*this.nearestStop.frequencyIndicators[i] + distanceWeight*this.distanceIndicator;
        }
    }
}
