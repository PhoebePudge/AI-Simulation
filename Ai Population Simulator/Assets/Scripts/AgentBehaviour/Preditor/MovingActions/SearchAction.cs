using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchAction : GoapAction { 
    private float _startTime = 0;
    private AgentData _agentData;
    private bool _usePath = false;
    private List<GridPartition> _path;
    private int _pathProgress = 0;
    private float _speed;
    private QuadT<bool> _collisionTree;
    public Vector3 _targetPos;
    public SearchAction() {
        addEffect("goExplore", true); 
    }
    
    //Setting our componets to variables if we havnt already, and calcualte our speed and new goal
    public override void reset() {
        if (_agentData == null) {
            _agentData = GetComponent<AgentData>();
            _collisionTree = Init.instance.quadtree.Collisionquadtree;
        }
        
        _speed = _agentData.Speed * Init.instance.GlobalData.agentSpeedMultiplyer; 
        Redo();
    }

    //Are we are our destinaton
    public override bool isDone() { 
        return transform.position == _targetPos;
    }

    public override bool requiresInRange() {
        return true;
    }

    //Set our target
    public override bool checkProceduralPrecondition(GameObject agent) {
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
        _targetPos = GetGoal();
        _agentData.TargetMovement = _targetPos; 
    }
    public Vector3 GetGoal() {
        Vector2 targetPos2 = (Random.insideUnitCircle * _agentData.WanderRadius);
        _targetPos = transform.position + new Vector3(targetPos2.x, 0, targetPos2.y);

        int height = Init.instance.GlobalData.SpawningBounds.y;
        int width = Init.instance.GlobalData.SpawningBounds.x;

        //Setting destination more towards the centre when out of bounds (Keeps AI within Bounds)
        if (_targetPos.x < 0 | _targetPos.x > height | _targetPos.z < 0 | _targetPos.z > width) {
            Vector3 DirectionToCentre = (new Vector3(height / 2, transform.position.y, width / 2) - transform.position).normalized;
            Vector3 NewPosition = transform.position + (DirectionToCentre * 5);
            _targetPos = NewPosition;
        }

        if (Init.gridManager.GetPartition(new Vector2Int((int)_targetPos.x, (int)_targetPos.z)).walkable == false) {
            return transform.position;
        } 
        return _targetPos;
    }
}