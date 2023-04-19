using BehaviorTree;
public class TaskReproduce : Node {
    private AgentData _agentData;
    private DiscontentmentData _discontentmentData;

    public TaskReproduce(AgentData preyData, DiscontentmentData data) {
        this._discontentmentData = data;
        this._agentData = preyData;
    }

    public override NodeState Evaluate() {

        _discontentmentData.Reproduction.Progress = 0;

        //If we are female, have a child
        if (!_agentData.DNA.isMale) {
            _agentData.isPreg = true;
        }
        _agentData.OffspringDNA = _agentData.DNA.crossOver(_agentData.Memory.oppositeGenderHistory.GetComponent<AgentData>().DNA);
        //finish
        state = NodeState.SUCCESS;
        return state;
    }
}