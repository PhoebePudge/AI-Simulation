using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EatAction : GoapAction {

    private bool _success = false;
    private float _startTime = 0;
    DiscontentmentData _discontentmentData;
    AgentData _agentData;

    public EatAction() {
        addPrecondition("goEat", true);
        addEffect("hasEaten", true); 
    } 
    //Reset our data
    public override void reset() {
        _startTime = 0;
        _success = false;
        //set our components are variables if we havnt already
        if (_discontentmentData == null) {
            _discontentmentData = GetComponent<DiscontentmentData>();
            _agentData = GetComponent<AgentData>();
        }
    }

    //Did we finish it?
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
        //Set our start time
        if (_startTime == 0) {
            _startTime = Time.time;
        }

        _success = true;

        //Check if our food source exists
        if (_agentData.Memory.foodSourceHistory.foodSource != null) {
            //Reset our progress and eat it
            _discontentmentData.Hunger.Progress = 0;
            _agentData.Memory.foodSourceHistory.foodSource.Eat();
            _agentData.Memory.foodSourceHistory.RemoveFood();
            _agentData.Memory.foodSourceHistory = null; 
        } 

        return true;
    }
}