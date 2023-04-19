using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrinkAction : GoapAction {
    private bool _success = false; 
    private float _startTime = 0;
    private DiscontentmentData _discontentmentData;
    public DrinkAction() {
        addPrecondition("goDrink", true);
        addEffect("hasDrunk", true);
    }
    //Reseting stuff and setting component variables for the first time
    public override void reset() {
        if (_discontentmentData == null) {
            _discontentmentData = GetComponent<DiscontentmentData>();
        }
        _startTime = 0;
        _success = false;
    }
    //Did we finish?
    public override bool isDone() {
        return _success;
    }

    //We dont need to be in range
    public override bool requiresInRange() {
        return true;
    }

    //Setting our target
    public override bool checkProceduralPrecondition(GameObject agent) { 
        target = gameObject; 
        return true;
    }

    public override bool perform(GameObject agent) { 
        //Setting our time when we start
        if (_startTime == 0) {
            _startTime = Time.time;
        }
        //Reseting our third progress
        _success = true;
        _discontentmentData.Thirst.Progress = 0;
        return true;
    }
}