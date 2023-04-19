using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestAction : GoapAction { 
    //Our variables
    private float _startTime = 0; 
    private DiscontentmentData _discontentmentData;
    private GlobalData _globalData;
     
    public RestAction() { 
        addEffect("hasRested", true); 
    }
     
    public override void reset() {
        //Setting our component variables for the first time
        if (_discontentmentData == null) {
            _discontentmentData = GetComponent<DiscontentmentData>();
            _globalData = Init.instance.GlobalData;
        }
        _startTime = 0;
    }

    //Return if our tiredness is bellow 0.2f
    public override bool isDone() { 
        return _discontentmentData.Tiredness.Progress < 0.2f;
    }

    //Always in range
    public override bool requiresInRange() {
        return true;
    }

    //We set our target here
    public override bool checkProceduralPrecondition(GameObject agent) {
        target = gameObject;
        return true;
    }

    public override bool perform(GameObject agent) {
        //When we first run we get the start time
        if (_startTime == 0) {
            _startTime = Time.time;
        }

        //Decrease the tiredness progress
        _discontentmentData.Tiredness.Progress -= Time.deltaTime * 1 / (_globalData.discontentmentProgressSpeed / 10f);
        return true;
    }
}
