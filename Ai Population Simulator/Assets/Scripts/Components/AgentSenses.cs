using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentSenses : MonoBehaviour
{
    /* Notes on each sense:

     Vision
        * Preditors and Prey have different FOV
        * We can detect water,
        * Agents (for opposite gender and opposite agent, these are only detected if they are in a parition bellow our camoflauge level)
        * Loose food (dropped meat and flowers)

     Smell
        * Smell has a static offset (based of sampling a distortion map and using a strength and speed, this is intended to mimic wind)
        * We can detect loose food in smelled partitions
        * Agents (for opposite gender and opposite agent)

     Hearing
        * If the sound level in that parition (modified with the distance so you are less likely to hear stuff further away)
        * This is used in things such as the prey's CheckForDanger and Fleeing node

     */

    //Locations you saw, smelled or heard
    private Vector2Int[] VisionCone;
    private Vector2Int[] Smell; 
    private Vector2Int[] Hearing; 

    //Show we be debugging these as Gizmos
    public static bool showVision = false;
    public static bool showSmell = false;
    public static bool showHearing = false;

    //Caching my components
    private AgentData _agentData;
    private GridManager _gridManager;
    private Transform _transform;
    private GlobalData _globalData;

    //Our location last sense (to remove our last location from)
    private Vector2Int PreviousLocation; 
    public List<GridPartition> hearingLocations = new List<GridPartition>();
    private float t = 0;

    private Vector2Int currentPosition() {
        return new Vector2Int((int)_transform.position.x, (int)_transform.position.z);
    }

    private void OnDestroy() {
        //We remove ourselfs from our last location when we die
        _gridManager.RemoveAgent(PreviousLocation, _agentData); 
    }
    void Start() {
        _agentData = GetComponent<AgentData>();
        _gridManager = Init.gridManager;
        _transform = transform;
        _globalData = Init.instance.GlobalData;
    }
    public GridPartition GetPartition() {
        return _gridManager.GetPartition(PreviousLocation);
    }
    private void Update() {

        //Increasing a time  and checking if its above our threashold to update our senses
        t += Time.deltaTime;
        bool update = false;
        if (t > _globalData.SenseUpdateSpeed) {
            update = true;
            t = 0;
        }

        if (update) { 
            //Get our currentPosition
            Vector2Int currentPos = currentPosition();

            //Update Agent location by removing its last, and adding it to the new location
            if (PreviousLocation != currentPos) {
                _gridManager.RemoveAgent(PreviousLocation, _agentData);
                PreviousLocation = currentPos;
                _gridManager.AddAgent(PreviousLocation, _agentData);
            }

            //Get visionCone (Preditors and prey have a different base FOV)
            if (_agentData.isPreditor) {
                VisionCone = _gridManager.GetPartitionsInCone(PreviousLocation, _globalData.visionRange, _transform.forward, _globalData.PreditorVisionFOV + _agentData.FieldOfVisionMultiplyer);
            } else {
                VisionCone = _gridManager.GetPartitionsInCone(PreviousLocation, _globalData.visionRange, _transform.forward, _globalData.PreyVisionFOV + _agentData.FieldOfVisionMultiplyer);
            }

            //Get our smell (Smell has a static offset) and hearing senses
            Smell = _gridManager.GetPartitionsInRadius(PreviousLocation + _globalData.GetWindOffset(), _globalData.smellRange);
            Hearing = _gridManager.GetPartitionsInRadius(PreviousLocation, _globalData.hearingRange);

            //Looping through each vision sample
            foreach (Vector2Int sample in VisionCone) {
                GridPartition SampledPartition = _gridManager.GetPartition(sample);

                //is it valid and does it contain anything?
                if (SampledPartition != null) {
                    if (SampledPartition.isFilled()) {

                        //Lets check for water
                        SampleForWater(SampledPartition);
                        
                        //If we can see the agent with the level on camoflauge
                        if (SampledPartition.CamoflaugeLevel >= _globalData.CamoflaugeVisionThreashold) {
                            SampleForAgents(SampledPartition);
                        }

                        //Update our food
                        SampleForFood(SampledPartition);
                    }
                }
            }

            //Loop through each smell sample
            foreach (Vector2Int sample in Smell) {
                GridPartition SampledPartition = _gridManager.GetPartition(sample);

                //If its worth looking at this sample
                if (SampledPartition != null) {
                    if (SampledPartition.isFilled()) {
                        SampleForFood(SampledPartition);
                        SampleForAgents(SampledPartition);
                    }
                }
            }

            //We clear the last locations we heard
            hearingLocations.Clear();


            //Looping through each hearing sample
            foreach (Vector2Int sample in Hearing) {
                GridPartition SampledPartition = _gridManager.GetPartition(sample);

                //If its worth looking at
                if (SampledPartition != null) {
                    if (SampledPartition.isFilled()) {

                        //Finding the distance
                        Vector3 newPos = new Vector3(sample.x, 0.5f, sample.y);
                        float DistanceToPos = 1f - ((float)Mathf.Clamp(Vector3.Distance(_transform.position, newPos), 0f, _globalData.hearingRange) / (float)_globalData.hearingRange);


                        //Using the distance and our sound level and finding if its above our threashold
                        if (SampledPartition.GetSoundLevel(_agentData) * DistanceToPos >= _globalData.hearingThreashold) {
                            //Debug.LogError("You Heared something, check it out");
                            hearingLocations.Add(SampledPartition);
                        }
                    }
                }
            }
        } 

    }
    private void SampleForWater(GridPartition SampledPartition) {  
        //If its drinkable, update our memory
        if (SampledPartition.drinkable) {
            if (_agentData.Memory.waterSourceHistory != SampledPartition)
                _agentData.Memory.waterSourceHistory = SampledPartition;
        }
    }
    private void SampleForAgents(GridPartition SampledPartition) {
        //There are agents here
        if (SampledPartition.agents.Count != 0) {
            //And its not use
            if (SampledPartition.agents[0] != _agentData) {
                //its the same kind as us
                if (SampledPartition.agents[0].isPreditor == _agentData.isPreditor) {
                    if (SampledPartition.agents[0].DNA.isMale != _agentData.DNA.isMale) {
                        _agentData.Memory.oppositeGenderHistory = SampledPartition.agents[0].GetComponent<Transform>();
                    } 
                } else {
                    //If its a different kind of agent, lets store it
                    _agentData.Memory.oppositeAgentType = SampledPartition.agents[0].transform; 
                }
            } 
        }
    }
    private void SampleForFood(GridPartition SampledPartition) {
        //If we found food
        if (SampledPartition.containsFood) {
            if (_agentData.Memory.foodSourceHistory != SampledPartition) {
                //if we are preditor, check if we have found meat and set it to our memory if it is
                if (_agentData.isPreditor) {
                    if (SampledPartition.food == Food.Meat) {
                        _agentData.Memory.foodSourceHistory = SampledPartition;
                    }

                //if we are prey, check if it is a flower and set it to our memory
                } else {
                    if (SampledPartition.food == Food.Flower) {
                        _agentData.Memory.foodSourceHistory = SampledPartition; 
                    }
                }
                
            }
        }
    }
    private void OnDrawGizmos() {
        //Drawing smell partitions
        if (Smell != null & showSmell) {
            foreach (var item in Smell) {
                if (item != null) {
                    _gridManager.DrawGizmos(item);
                }
            }
        }

        //Drawing Vision partitions
        if (VisionCone != null & showVision) {
            foreach (var item in VisionCone) {
                if (item != null) {
                    _gridManager.DrawGizmos(item);
                }
            }
        }

        //Showing hearing locations
        if (hearingLocations != null & showHearing) {
            Gizmos.color = Color.yellow;
            foreach (var item in hearingLocations) {
                Gizmos.DrawLine(item.CenterPosition, transform.position);
            }
        }

        //Drawing hearing partitions
        if (Hearing != null & showHearing) {
            foreach (var item in Hearing) {
                if (item != null) {
                    _gridManager.DrawGizmos(item);
                }
            }
        }

    }
}
