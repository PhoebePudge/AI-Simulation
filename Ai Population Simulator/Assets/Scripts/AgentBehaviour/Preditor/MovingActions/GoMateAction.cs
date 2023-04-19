using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoMateAction : GoapAction {
    private float _startTime = 0;
    DiscontentmentData _discontentmentData;
    AgentData _agentData;
    public bool _usePath = false;
    public List<GridPartition> _path;
    public int _pathProgress = 0;
    Vector3 _targetPos;
    private float _speed;
    private QuadT<bool> _collisionTree;
    public GoMateAction() { 
        addEffect("goReproduce", true);
        addPrecondition("foundMate", true);
    } 
    public override void reset() {
        if(_discontentmentData == null) {
            _discontentmentData = GetComponent<DiscontentmentData>();
            _agentData = GetComponent<AgentData>();
            _collisionTree = Init.instance.quadtree.Collisionquadtree;
        }
        _speed = _agentData.Speed * Init.instance.GlobalData.agentSpeedMultiplyer;
    }

    public override bool isDone() {
        return transform.position == _targetPos;
    }

    public override bool requiresInRange() {
        return true;
    }

    public override bool checkProceduralPrecondition(GameObject agent) {
        if (_agentData.Memory.oppositeGenderHistory == null) {
            return false;
        }
        _startTime = 0;
        target = gameObject; 
        return true;
    }

    public override bool perform(GameObject agent) {
        if (_startTime == 0) {
            _startTime = Time.time;
            Redo();
        }

        if (_usePath == false) {
            //simple point to point movement
            Vector3 currentPosition = transform.position;
            transform.position = Vector3.MoveTowards(currentPosition, _targetPos, Time.deltaTime * _speed);
            QuadT<bool>.QuadtreeNode<bool> n = _collisionTree.GetRoot().LowestData(new Vector2(currentPosition.x, currentPosition.z));
            if (n.Data == true) {
                _usePath = true;
                _path = Pathfinder.FindPath(new Vector2Int((int)currentPosition.x, (int)currentPosition.z), new Vector2Int((int)_targetPos.x, (int)_targetPos.z));

                //We got a null path
                if (_path == null) {
                    return false;
                }

                //We got a empty path
                if (_path.Count == 0) {
                    return false;
                }

                _targetPos = _path[_path.Count - 1].CenterPosition;
                _pathProgress = 0;
            }


        } else {
            //moving between path points
            Vector3 currentDestination = _path[_pathProgress].CenterPosition;
            _agentData.TargetMovement = currentDestination;

            transform.position = Vector3.MoveTowards(transform.position, currentDestination, Time.deltaTime * _speed);

            //Move to the next position in our path
            if (_pathProgress < _path.Count - 1) {
                if (transform.position == _path[_pathProgress].CenterPosition) {
                    _pathProgress++;
                }

            }
        }


        return true;
    }
    private void Redo() {
        _usePath = false;
        _pathProgress = 0;
        _path = null;
        _targetPos = Init.gridManager.GetPartition(new Vector2Int((int)_agentData.Memory.oppositeGenderHistory.position.x, (int)_agentData.Memory.oppositeGenderHistory.position.y)).CenterPosition;
        _agentData.TargetMovement = _targetPos;
    }
}