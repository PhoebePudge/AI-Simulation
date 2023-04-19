using System.Collections.Generic;
using UnityEngine;
using System.Collections;
//Framework created using tutorial by Brent Owens
//https://gamedevelopment.tutsplus.com/tutorials/goal-oriented-action-planning-for-a-smarter-ai--cms-20793
/**
 * Stack-based Finite State Machine.
 * Push and pop states to the FinitestateMachine.
 * 
 * States should push other states onto the stack 
 * and pop themselves off.
 */
using System;


public class FinitestateMachine {

	private Stack<FSMState> stateStack = new Stack<FSMState>();

	public delegate void FSMState(FinitestateMachine FinitestateMachine, GameObject gameObject);


	public void Update(GameObject gameObject) {
		if (stateStack.Peek() != null)
			stateStack.Peek().Invoke(this, gameObject);
	}

	public void pushState(FSMState state) {
		stateStack.Push(state);
	}

	public void popState() {
		var state = stateStack.Pop();
	}
}


public interface FSMState {

	void Update(FinitestateMachine FinitestateMachine, GameObject gameObject);
}
