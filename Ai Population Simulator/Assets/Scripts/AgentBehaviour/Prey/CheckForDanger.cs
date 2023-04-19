using UnityEngine;
using BehaviorTree;
public class CheckForDanger : Node {
    private Transform _transform;
    private AgentData _agentData;
    private AgentSenses _agentSense;
    private GlobalData _globalData;

    //Caching the data we will need
    public CheckForDanger(Transform transform, AgentData preyData, AgentSenses sense, GlobalData data) {
        _transform = transform;
        this._agentData = preyData;
        this._agentSense = sense;
        this._globalData = data;
    }
    public override NodeState Evaluate() {
        //No preditor has been seen or heard yet, lets exit from here
        if (_agentData.Memory.oppositeAgentType == null & _agentSense.hearingLocations == null) {
            state = NodeState.FAILURE;
            return state;
        }

        //if we have heard anything
        if (_agentSense.hearingLocations != null) {
            //loop through each thing we have heard
            foreach (var item in _agentSense.hearingLocations) {
                //if there is a agent and its a preditor, lets successfully go to our next node
                if (item.agents.Count != 0) {
                    if (item.agents[0].isPreditor) {
                        if (Vector3.Distance(item.CenterPosition, _transform.position) < _globalData.PreyFleeRadius) {
                            state = NodeState.SUCCESS;
                            return state;
                        }
                    }
                }
            }
        }

        //If we have a agent stored in memory
        if (_agentData.Memory.oppositeAgentType != null) {
            //Check if its in a reasonable radius, if it is, lets successfully go to our next node 
            if (Vector3.Distance(_agentData.Memory.oppositeAgentType.position, _transform.position) < _globalData.PreyFleeRadius) {
                state = NodeState.SUCCESS;
                return state;
            }
        }

        //We default to failure, this could have in some odd cases such as hearing locations being empty, or some other conditions
        state = NodeState.FAILURE;
        return state;
    }

}