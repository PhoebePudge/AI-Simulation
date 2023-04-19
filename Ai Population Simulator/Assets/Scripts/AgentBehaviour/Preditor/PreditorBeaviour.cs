using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreditorBeaviour : MonoBehaviour, IGoap {
	public bool DebugData = false;
	private DiscontentmentData _discontentmentData;
	private AgentData _animalData;
	public string CurrentPlan;
	void Start() {
		//Caches variables
		_discontentmentData = GetComponent<DiscontentmentData>();
		_animalData = GetComponent<AgentData>();
	}
	 
	public HashSet<KeyValuePair<string, object>> getWorldState() {
		//Preconditions for several use real world data
		HashSet<KeyValuePair<string, object>> worldData = new HashSet<KeyValuePair<string, object>>(); 
		worldData.Add(new KeyValuePair<string, object>("foundPrey", _animalData.Memory.oppositeAgentType != null)); 

		worldData.Add(new KeyValuePair<string, object>("foundWater", _animalData.Memory.waterSourceHistory != null)); 

		worldData.Add(new KeyValuePair<string, object>("foundMate", _animalData.Memory.oppositeGenderHistory != null)); 

		worldData.Add(new KeyValuePair<string, object>("foundFood", _animalData.Memory.foodSourceHistory != null)); 

		worldData.Add(new KeyValuePair<string, object>("shouldEat",	_discontentmentData.Hunger.Discontent > 25)); 
		return worldData;
	}
	 
	public HashSet<KeyValuePair<string, object>> createGoalState() {
		HashSet<KeyValuePair<string, object>> goal = new HashSet<KeyValuePair<string, object>>();

		//We filter out many of the goals to only when we can do them, to save resources

		//Its only worth adding the hunger goal is its more of a priority than exploring, and it we can meet the preconditions
        if (_discontentmentData.Hunger.Discontent > 25 && (_animalData.Memory.oppositeAgentType != null || _animalData.Memory.foodSourceHistory != null)) { 
            goal.Add(new KeyValuePair<string, object>("hasEaten", true));
        }

		//Rest if its a higher priority than exploring
		if (_discontentmentData.Tiredness.Discontent > 25) {
            goal.Add(new KeyValuePair<string, object>("hasRested", true));
        }

		//Reproduce if we should and we know a mate
        if (_discontentmentData.Reproduction.Discontent > 25 && _animalData.Memory.oppositeGenderHistory != null) {
            goal.Add(new KeyValuePair<string, object>("hasReproduced", true));
        }

		//Drink if its a high enough priority, and we know a water source
		if (_discontentmentData.Thirst.Discontent > 25 && _animalData.Memory.waterSourceHistory != null) {
            goal.Add(new KeyValuePair<string, object>("hasDrunk", true));
        }

		//We always have the goal of exploring
        goal.Add(new KeyValuePair<string, object>("goExplore", true));
		 
		return goal;
	}
	 
	public void planFailed(HashSet<KeyValuePair<string, object>> failedGoal) { 
	}

	public void planFound(HashSet<KeyValuePair<string, object>> goal, Queue<GoapAction> actions) {
		//Debugging it we need to
		if (DebugData)
			Debug.Log("<color=green>Plan found</color> " + GoapAgent.prettyPrint(actions));

		CurrentPlan = GoapAgent.prettyPrint(actions);
	}

	public void actionsFinished() {
		//Logging completition if we want to
		if (DebugData)
			Debug.Log("<color=blue>Actions completed</color>"); 
	}

	public void planAborted(GoapAction aborter) {
		// Logging plan abortion if we want to
		if (DebugData)
			Debug.Log("<color=red>Plan Aborted</color> " + GoapAgent.prettyPrint(aborter));
	}

	public bool moveAgent(GoapAction nextAction) {
		//We ignore range requirments, these are not used
		nextAction.setInRange(true);
		return true;
	} 
}
