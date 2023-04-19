using BehaviorTree;


public class TaskDrink : Node {
    private DiscontentmentData _discontentmentData;
    public TaskDrink(DiscontentmentData data) {
        this._discontentmentData = data;
    }

    public override NodeState Evaluate() {
        //drink
        _discontentmentData.Thirst.Progress = 0;
        state = NodeState.SUCCESS;
        return state;
    }
}