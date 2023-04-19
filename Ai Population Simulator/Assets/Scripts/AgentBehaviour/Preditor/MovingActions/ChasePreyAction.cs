using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChasePreyAction : GoapAction {
    private bool _success = false; 
    private float _startTime = 0;
    private GlobalData _globalData;
    private AgentData _animalData;
    private float _movementSpeed;
    private Vector3 _targetPos;

    //Set precondition and effect
    public ChasePreyAction() { 
        addPrecondition("foundPrey", true);    
        addEffect("atPrey", true); 
    } 
    //Set our components the first time and reset our start time
    public override void reset() { 
        _startTime = 0;
        if (_animalData == null) {
            _animalData = GetComponent<AgentData>();
            _globalData = Init.instance.GlobalData;
            _movementSpeed = _animalData.Speed * _globalData.agentSpeedMultiplyer;
        }
    }
    //are we at our goal
    public override bool isDone() { 
        return _success;
    }

    public override bool requiresInRange() { 
        return true;
    }
    //Setting target
    public override bool checkProceduralPrecondition(GameObject agent) { 
        target = gameObject; 
        return true; 
    }

    public override bool perform(GameObject agent) {
        //Return if our mate does not exist
        if (_animalData.Memory.oppositeAgentType == null) {
            return false;
        } 
        //first time set goal
        if (_startTime == 0) { 
            _startTime = Time.time;
            _targetPos = _animalData.Memory.oppositeAgentType.position;
            _animalData.TargetMovement = _targetPos;
        }

        //Keep moving untill we get to the goal
        if (transform.position != _targetPos) { 
            transform.position = Vector3.MoveTowards(transform.position, _targetPos, Time.deltaTime * _movementSpeed);
            _success = false;
        } else { 
            //We are at the goal
            _success = true;
        }
        return true;
    }
}
