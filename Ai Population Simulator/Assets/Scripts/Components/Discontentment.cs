using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] public struct Discontentment {
    public string Name;
    public float Progress;
    public float Cost;
    public Discontentment(string name) {
        Name = name;
        Cost = 0;
        Progress = 0;
    }
    //Set our cost (distance to parition location)
    public void SetCost(Vector3 pos, GridPartition location) {
        if (location != null)
            Cost = CalculateCost(pos, location.CenterPosition);
        else
            Cost = 4;
    }
    //Set our cost (distance to vector3 location)
    private float CalculateCost(Vector3 pos, Vector3 location) {
        return 4 - Mathf.Clamp(Vector3.Distance(pos, location), 0, 4);
    }
    //Set our cost (Transform location)
    public void SetCost(Vector3 pos, Transform location) {
        if (location != null)
            Cost = CalculateCost(pos, location.position);
        else
            Cost = 4;
    }
    //Calculate discontentment
    public int Discontent {
        get { return (int)(Progress * Progress) + (int)(Cost * Cost); }
    }
    //To string
    public override string ToString() {
        return Progress.ToString() + " : " + Cost.ToString() + " = " + Discontent;
    }

};