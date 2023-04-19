using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class TaskSearching : Node {
    private Transform _transform;
    private AgentData _agentData;
    public TaskSearching(Transform transform, AgentData preyData) {
        this._agentData = preyData;
        this._transform = transform; 
    }

    public override NodeState Evaluate() {
        state = NodeState.SUCCESS; 
        //Get a new position we should explore and move to
        if (_transform.position == _agentData.TargetMovement) {

            Vector3 targetPos = GetGoal(); 
            if (Init.gridManager.GetPartition(new Vector2Int((int)targetPos.x, (int)targetPos.z)).walkable == false) {
                targetPos = _transform.position;
            } 
            _agentData.TargetMovement = targetPos; 
        }

        //Move onto our movement node
        return state;
    }

    public Vector3 GetGoal() {
        //Chose a random position in a  circle
        Vector2 targetPos2 = (Random.insideUnitCircle * _agentData.WanderRadius);
        //get that as a vector3
        Vector3 targetPos = _transform.position + new Vector3(targetPos2.x, 0, targetPos2.y);

        //get our spawning bounds
        int height = Init.instance.GlobalData.SpawningBounds.y;
        int width = Init.instance.GlobalData.SpawningBounds.x;

        //Setting destination more towards the centre when out of bounds (Keeps AI within Bounds)
        if (targetPos.x < 0 | targetPos.x > height | targetPos.z < 0 | targetPos.z > width) {
            Vector3 DirectionToCentre = (new Vector3(height / 2, _transform.position.y, width / 2) - _transform.position).normalized;
            Vector3 NewPosition = _transform.position + (DirectionToCentre * 5);
            targetPos = NewPosition;
        }

        //return our new value
        return targetPos;
    }
}
