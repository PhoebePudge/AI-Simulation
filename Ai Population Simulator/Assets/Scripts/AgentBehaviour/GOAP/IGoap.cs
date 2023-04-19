using UnityEngine;
using System.Collections;
//Framework created using tutorial by Brent Owens
//https://gamedevelopment.tutsplus.com/tutorials/goal-oriented-action-planning-for-a-smarter-ai--cms-20793
//We use this for our preditor behaviour
/**
 * Collect the world data for this Agent that will be
 * used for GOAP planning.
 */
using System.Collections.Generic;
public interface IGoap {
	HashSet<KeyValuePair<string, object>> getWorldState();
	HashSet<KeyValuePair<string, object>> createGoalState();
	void planFailed(HashSet<KeyValuePair<string, object>> failedGoal);
	void planFound(HashSet<KeyValuePair<string, object>> goal, Queue<GoapAction> actions);
	void actionsFinished();
	void planAborted(GoapAction aborter);
	bool moveAgent(GoapAction nextAction);
}

