using UnityEngine;
using BehaviorTree;

public class TaskRest : Node {
    private AgentData preyData;
    private DiscontentmentData data;
    private GlobalData _globalData;
    public TaskRest(AgentData preyData, DiscontentmentData data, GlobalData globaldata) {
        this.data = data;
        this.preyData = preyData;
        this._globalData = globaldata;
    }

    public override NodeState Evaluate() {
        if (data.Tiredness.Progress < 0) {
            data.Tiredness.Progress -= Time.deltaTime * 1 / (_globalData.discontentmentProgressSpeed / 2);
            state = NodeState.RUNNING;
            return state;
        } else {
            data.Tiredness.Progress = 0;
            state = NodeState.SUCCESS;
            return state;
        }
    }
}