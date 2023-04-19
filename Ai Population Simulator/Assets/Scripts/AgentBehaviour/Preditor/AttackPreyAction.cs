using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPreyAction : GoapAction {
    private bool _success = false; 
    private float _startTime = 0;
    DiscontentmentData _discontentmentData;
    AgentData _agentData;

    public AttackPreyAction() {
        addPrecondition("atPrey", true);
        addEffect("foundFood", true);
    } 
    //Reset our variables and set our components to variables for the first time
    public override void reset() {
        _startTime = 0;
        _success = false;
        if (_discontentmentData == null) {
            _discontentmentData = GetComponent<DiscontentmentData>();
            _agentData = GetComponent<AgentData>();
        }
    }
    //Have we finished
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
        //Setting our start time when we start attacking
        if (_startTime == 0) {
            _startTime = Time.time; 
        }

        //Setting our food source to be the grid parition of our prey
        _agentData.Memory.foodSourceHistory = _agentData.Memory.oppositeAgentType.GetComponent<AgentSenses>().GetPartition();

        //Destroy the prey (This should make some food appear at the partition we got before)
        Destroy(_agentData.Memory.oppositeAgentType.gameObject);

        //Spawn in food
        GameObject gm = GameObject.Instantiate(Resources.Load<GameObject>("Meat"));
        gm.transform.parent = Init.instance.SpawningTransform; 
        Init.gridManager.SetFood(_agentData.Memory.foodSourceHistory.Position, Food.Meat, gm.AddComponent<Meat>());
        gm.transform.position = _agentData.Memory.foodSourceHistory.CenterPosition; 
        _success = true; 
        return true;
    }
}
