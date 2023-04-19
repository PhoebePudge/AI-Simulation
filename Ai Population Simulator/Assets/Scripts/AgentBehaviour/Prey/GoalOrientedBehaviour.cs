using System.Collections.Generic; 
using BehaviorTree;
//This node is intended to a simplistic version of goal orientated behaviour, choosing a node child based on its respective discontentment being highest
public class GoalOrientedBehaviour : Node {
    private DiscontentmentData _discontentmentData;
    private AgentData _animalData;

    public GoalOrientedBehaviour() : base() { }

    //Caching our data to use in this node
    public GoalOrientedBehaviour(List<Node> children, AgentData animalData, DiscontentmentData data) : base(children) {
        this._animalData = animalData;
        this._discontentmentData = data;
    }
    public override NodeState Evaluate() {
        //Finding our discontentment sorted by priority
        DiscontentmentData.DiscontentStates[] SelectedValue = _discontentmentData.FindHighestDiscontentment();

        //Loop through each, This allows a second or third choice to be chosen should we not have the information to perform that action
        //Example: if our Hunger is the highest priority and we do not know any food sources, we may default to our second highest prority drink, or if that doesnt work our third which may be explore.  
        foreach (var discontentment in SelectedValue) {
            bool failed = false;

            //Switch between them
            switch (discontentment) {
                //If we have a hood source set our target position to this and evaluate this node
                case DiscontentmentData.DiscontentStates.Hunger:
                    if (_animalData.Memory.foodSourceHistory != null) {
                        _animalData.TargetMovement = _animalData.Memory.foodSourceHistory.CenterPosition;
                        children[(int)discontentment].Evaluate();
                    } else {
                        failed = true;
                    }
                    break;
                //Evaluate our rest node
                case DiscontentmentData.DiscontentStates.Tiredness:
                    children[(int)discontentment].Evaluate();
                    break;
                //if we have found a mate, lets evaluate that node
                case DiscontentmentData.DiscontentStates.Reproduction:
                    if (_animalData.Memory.oppositeGenderHistory != null) {
                        _animalData.TargetMovement = _animalData.Memory.oppositeGenderHistory.position;
                        children[(int)discontentment].Evaluate();
                    } else {
                        failed = true;
                    }
                    break;
                //If we know where water is, lets evaluate that node
                case DiscontentmentData.DiscontentStates.Thirst:
                    if (_animalData.Memory.waterSourceHistory != null) {
                        _animalData.TargetMovement = _animalData.Memory.waterSourceHistory.CenterPosition;
                        children[(int)discontentment].Evaluate();
                    } else {
                        failed = true;
                    }
                    break;
                //Evaluate explore
                case DiscontentmentData.DiscontentStates.Explore:
                    children[(int)discontentment].Evaluate();
                    break;
                default:
                    break;
            }

            //If we havnt failed (we found something we can call, lets exit this loop and finish up this node)
            if (!failed) {
                break;
            }
        }

        //We cant fail this node
        state = NodeState.FAILURE;
        return state;
    }
}
