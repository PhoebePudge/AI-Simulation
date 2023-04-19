using UnityEngine;
using System.Collections.Generic;
//Framework created using tutorial by Brent Owens
//https://gamedevelopment.tutsplus.com/tutorials/goal-oriented-action-planning-for-a-smarter-ai--cms-20793
//This is the framework behind each our preditor actions
public abstract class GoapAction : MonoBehaviour { 
	//Our data
	private HashSet<KeyValuePair<string, object>> preconditions;
	private HashSet<KeyValuePair<string, object>> effects;
	private bool inRange = false;
	public float cost = 1f;

	// An action often has to perform on an object. This is that object. Can be null.
	public GameObject target;

	//Initilise
	public GoapAction() {
		preconditions = new HashSet<KeyValuePair<string, object>>();
		effects = new HashSet<KeyValuePair<string, object>>();
	}

	public void doReset() {
		inRange = false;
		target = null;
		reset();
	}

	//We write these within each action script
	public abstract void reset();
	public abstract bool isDone();
	public abstract bool checkProceduralPrecondition(GameObject agent);
	public abstract bool perform(GameObject agent);
	public abstract bool requiresInRange();

	public bool isInRange() {
		return inRange;
	}

	public void setInRange(bool inRange) {
		this.inRange = inRange;
	}

	//add this precondition
	public void addPrecondition(string key, object value) {
		preconditions.Add(new KeyValuePair<string, object>(key, value));
	}

	//Remove this precondition
	public void removePrecondition(string key) {
		KeyValuePair<string, object> remove = default(KeyValuePair<string, object>);
		foreach (KeyValuePair<string, object> kvp in preconditions) {
			if (kvp.Key.Equals(key))
				remove = kvp;
		}
		if (!default(KeyValuePair<string, object>).Equals(remove))
			preconditions.Remove(remove);
	}

	//Add this effect
	public void addEffect(string key, object value) {
		effects.Add(new KeyValuePair<string, object>(key, value));
	}

	//Remove this effect
	public void removeEffect(string key) {
		KeyValuePair<string, object> remove = default(KeyValuePair<string, object>);
		foreach (KeyValuePair<string, object> kvp in effects) {
			if (kvp.Key.Equals(key))
				remove = kvp;
		}
		if (!default(KeyValuePair<string, object>).Equals(remove))
			effects.Remove(remove);
	}

	//Preconditions of this action
	public HashSet<KeyValuePair<string, object>> Preconditions {
		get {
			return preconditions;
		}
	}

	//Effects of this action
	public HashSet<KeyValuePair<string, object>> Effects {
		get {
			return effects;
		}
	}
}