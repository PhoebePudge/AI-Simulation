using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class TaskFeeling : Node {
    private Transform _transform;
    private AgentData _agentData;
    private AgentSenses _agentSenses;
    private GlobalData _globalData;
    public TaskFeeling(Transform transform, AgentData preyData, AgentSenses sense, GlobalData globalData) {
        _transform = transform;
        this._agentData = preyData;
        this._agentSenses = sense;
        this._globalData = globalData;
    }

    public override NodeState Evaluate() {

        //catching null varibles
        if (_agentData.Memory.oppositeAgentType == null & _agentSenses.hearingLocations == null) {
            state = NodeState.FAILURE;
            return state;
        }

        //List to average run direction from
        List<Vector3> Preditors = new List<Vector3>();

        if (_agentSenses.hearingLocations != null) {
            //Looping though each thing we heard
            foreach (var item in _agentSenses.hearingLocations) {
                //If its a preditor, lets add it to our list if its within range
                if (item.agents.Count != 0) {
                    if (item.agents[0].isPreditor) {
                        if (Vector3.Distance(item.CenterPosition, _transform.position) < _globalData.PreyFleeRadius) {
                            Preditors.Add(_transform.position - item.agents[0].transform.position);
                        }
                    }
                }
            }
        }

        if (_agentData.Memory.oppositeAgentType != null) {
            //adding our memory if its within range
            if (Vector3.Distance(_agentData.Memory.oppositeAgentType.position, _transform.position) < _globalData.PreyFleeRadius) {
                Preditors.Add(_transform.position - _agentData.Memory.oppositeAgentType.position);
            }
        }

        //add each preditor position into our offset, and divide it by the amount we have
        //This creates an average value
        Vector3 offset = Vector3.zero;
        foreach (var item in Preditors) {
            offset += item;
        }
        offset = new Vector3(offset.x / Preditors.Count, 0, offset.z / Preditors.Count);

        //Lets set our position away from this
        _agentData.TargetMovement = _transform.position + offset;
        _agentData.TargetMovement =
            new Vector3(
                Mathf.Clamp(_agentData.TargetMovement.x, 0, 30),
                _transform.position.y,
                Mathf.Clamp(_agentData.TargetMovement.z, 0, 30));

        //If we are there we can call success, if not lets keep running
        if (Vector3.Distance(_transform.position, _agentData.TargetMovement) < 0.1f) {
            state = NodeState.SUCCESS;
            return state;
        } else {
            state = NodeState.RUNNING;
        }

        //Move towards the position
        _transform.position = Vector3.MoveTowards(_transform.position, _agentData.TargetMovement, Time.deltaTime * _agentData.Speed * Init.instance.GlobalData.agentSpeedMultiplyer);
        return state;
    }
}