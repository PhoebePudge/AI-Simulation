using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum Food { None, Meat, Flower }
public class GridPartition {
    //can we drink here
    public bool drinkable = false;

    //Can we walk here
    public bool walkable = true;

    //Does this node contain food
    public bool containsFood {
        get {
            return (food != Food.None);
        }
    }

    //Food stuff
    public Food food = Food.None;
    public FoodSource foodSource = null;

    public void RemoveFood() {
        food = Food.None;
        foodSource = null;
    }
    //Agents stored here
    public List<AgentData> agents = new List<AgentData>();

    //Camo level of this parition
    public float CamoflaugeLevel;

    //partitioning costs
    public int gCost;
    public int hCost; 
    public GridPartition parent;

    //A quick way to figure out if a parition has something we can use for vision and hearing checks
    public bool isFilled() {
        bool value = drinkable == true | containsFood == true | agents.Count > 0;
        return value;
    }

    //Calculate parition costs
    public int fCost {
        get {
            return gCost + hCost;
        }
    }

    //our position
    private Vector2Int m_position;
    public Vector2Int Position {
        get { return m_position; }
    }
    public Vector3 CenterPosition {
        get { return new Vector3(m_position.x + 0.5f, 0, m_position.y + 0.5f); }
    }
    //Initialise
    public GridPartition(Vector2Int Position) {
        this.m_position = Position;
    }
    //Draw this parition
    public void DrawGizmo(Color? col = null) {
        //Different colours for data stored
        if (col == null) {
            if (!walkable) {
                Gizmos.color = Color.red;
                Gizmos.DrawWireCube(new Vector3(m_position.x + 0.5f, 0, m_position.y + 0.5f), Vector3.one);
            } else if (agents.Count != 0) {
                Gizmos.color = Color.yellow;
                Gizmos.DrawCube(new Vector3(m_position.x + 0.5f, 0, m_position.y + 0.5f), Vector3.one);
            } else if (drinkable) {
                Gizmos.color = Color.blue;
                Gizmos.DrawWireCube(new Vector3(m_position.x + 0.5f, 0, m_position.y + 0.5f), Vector3.one);
            } else {
                Gizmos.color = Color.white;
                Gizmos.DrawWireCube(new Vector3(m_position.x + 0.5f, 0, m_position.y + 0.5f), Vector3.one);
            }
        } else {
            //Override colour
            Gizmos.color = col.Value;
            Gizmos.DrawWireCube(new Vector3(m_position.x + 0.5f, 0, m_position.y + 0.5f), Vector3.one);
        }
    }
    //Get the sound level of every agent in our node
    public float GetSoundLevel(AgentData mydata) {
        float sound = 0;
        if (agents != null) {
            foreach (var item in agents) {
                if (mydata != item) {
                    sound += item.soundGeneration;
                }
            }
        }
        return sound;
    }
}