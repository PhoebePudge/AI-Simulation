using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct AgentMemory {
    public GridPartition waterSourceHistory; //Location of water
    public GridPartition foodSourceHistory; // Location of food
    public Transform oppositeGenderHistory; //Location of a possible mate
    public Transform oppositeAgentType; //If you are a prey agent this will be the preditor to run from, for preditors this is the prey you may be chasing
}