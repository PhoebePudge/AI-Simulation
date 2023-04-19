using BehaviorTree;


public class TaskEat : Node {
    private AgentData _agentData;
    private DiscontentmentData _discontentmentData;
    //Caching the data we know
    public TaskEat(AgentData preyData, DiscontentmentData data) {
        this._discontentmentData = data;
        this._agentData = preyData;
    }

    public override NodeState Evaluate() {

        //if our food source is still there
        if (_agentData.Memory.foodSourceHistory != null) {
            if (_agentData.Memory.foodSourceHistory.foodSource != null) {

                //lets eat it if there are still some remaining
                if (_agentData.Memory.foodSourceHistory.foodSource.isEdible()) {
                    _discontentmentData.Hunger.Progress = 0;
                    _agentData.Memory.foodSourceHistory.foodSource.Eat();

                    //Success
                    state = NodeState.SUCCESS;
                    return state;

                }
            }
        }

        //We failed (No food source able to eat)
        state = NodeState.FAILURE;
        return state;
    }
}
