using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReproduceAction : GoapAction { 

    private float startTime = 0; 
    private AgentData _animalData;
    private DiscontentmentData _discontentmentData;
     
    public ReproduceAction() {
        addPrecondition("goReproduce", true);
        addEffect("hasReproduced", true);

    } 

    //Setting our variables if they are null and reseting out time
    public override void reset() {
        if (_discontentmentData == null) {
            _discontentmentData = GetComponent<DiscontentmentData>();
            _animalData = GetComponent<AgentData>();
        }
        startTime = 0;
    }

    public override bool isDone() {
        //return if you are a male or if you are a female carrying a child
        return _animalData.DNA.isMale == false | _animalData.isPreg;
    }
     
    //Always in range
    public override bool requiresInRange() {
        return true;
    }

    //Set our target here
    public override bool checkProceduralPrecondition(GameObject agent) { 
        target = gameObject;
        return true;
    }

    public override bool perform(GameObject agent) { 
        //Set the start time here each time we run
        if (startTime == 0) {
            startTime = Time.time;
        }

        //If we are a female, carry children
        if (!_animalData.DNA.isMale)
            _animalData.isPreg = true;

        //Set our progress back to 0
        _discontentmentData.Reproduction.Progress = 0;
        _animalData.OffspringDNA =_animalData.DNA.crossOver(_animalData.Memory.oppositeGenderHistory.GetComponent<AgentData>().DNA);
        return true;
    }
}