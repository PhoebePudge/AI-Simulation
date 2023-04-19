using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
public class TaskTargetPosition : Node {
    private Transform _transform;
    private AgentData _agentData;
    private bool _usePath = false;
    private List<GridPartition> _path;
    private int _pathProgress = 0;

    public TaskTargetPosition(Transform transform, AgentData preyData) {
        this._agentData = preyData;
        this._transform = transform;
    }
    public override NodeState Evaluate() {
        //if we dont have a target position lets fail this node
        if (_agentData.TargetMovement == null) {
            state = NodeState.FAILURE;
            return state;
        }

        //if we are not using the pathfinding
        if (_usePath == false) {

            //simple point to point movement
            _transform.position = Vector3.MoveTowards(_transform.position, _agentData.TargetMovement, Time.deltaTime * _agentData.Speed * Init.instance.GlobalData.agentSpeedMultiplyer);

            //We are at final node
            if (_transform.position == _agentData.TargetMovement) {
                _usePath = false;
                _pathProgress = 0;
                _path = null;
                state = NodeState.SUCCESS;
                return state;
            }
            
            //Checking for collision and should we need pathfinding help
            Vector2 pos = new Vector2(_transform.position.x, _transform.position.z);
            QuadT<bool>.QuadtreeNode<bool> n = Init.instance.quadtree.Collisionquadtree.GetRoot().LowestData(pos);
            //If our sampled from the quad tree is not walkable, switch to pathfinding
            if (n.Data == true) {
                _usePath = true;
                _path = Pathfinder.FindPath(new Vector2Int((int)_transform.position.x, (int)_transform.position.z), new Vector2Int((int)_agentData.TargetMovement.x, (int)_agentData.TargetMovement.z));
                _pathProgress = 0;
            }

            //Use pathfinding
        } else {
            //We fail if we our path is empty or null
            if (_path == null) {
                _usePath = false;
                _pathProgress = 0;
                _path = null;
                state = NodeState.FAILURE;
                return state;
            }
            if (_path.Count == 0) {
                _usePath = false;
                _pathProgress = 0;
                _path = null;
                state = NodeState.FAILURE;
                return state;
            }

            //moving between path points
            Vector3 destination = _path[Mathf.Clamp(_pathProgress, 0, _path.Count)].CenterPosition;
            _transform.position = Vector3.MoveTowards(_transform.position, destination, Time.deltaTime * _agentData.Speed * Init.instance.GlobalData.agentSpeedMultiplyer);

            //Increasing our path progress
            if (_pathProgress < _path.Count - 1) {
                if (_transform.position == _path[_pathProgress].CenterPosition) {
                    _pathProgress++;
                }
            } else { 
                //We are at final node, lets finish this
                if (_transform.position == _path[_pathProgress].CenterPosition) {
                    _usePath = false;
                    _pathProgress = 0;
                    _path = null;
                    state = NodeState.SUCCESS;
                    return state; 
                }
            }
        }
        state = NodeState.RUNNING;
        return state;
    }
}