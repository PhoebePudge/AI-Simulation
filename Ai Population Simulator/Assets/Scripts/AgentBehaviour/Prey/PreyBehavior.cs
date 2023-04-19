using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;  
public class PreyBehavior : BehaviorTree.Tree {
    public Node Root;
    protected override Node SetupTree(){
        //getting caches of components
        AgentData preyData = gameObject.GetComponent<AgentData>();
        AgentSenses senses = gameObject.GetComponent<AgentSenses>();
        DiscontentmentData data = gameObject.GetComponent<DiscontentmentData>();
        Transform _transform = GetComponent<Transform>();

        Root = new Selector( new List<Node>{

            //check of preditors and run away from them
            new Sequence(new List<Node>{
                new CheckForDanger(_transform, preyData, senses, Init.instance.GlobalData),
                new TaskFeeling(_transform, preyData, senses, Init.instance.GlobalData)
            }),

            //Custom node type of choose to mimic a basic Goal Orientated Behaviour
            new GoalOrientedBehaviour(new List<Node> {
                //Going to a food location and eating it
                new Sequence( new List<Node> {
                    new TaskTargetPosition(_transform, preyData),
                    new TaskEat(preyData, data)
                }),

                //Resting in place
                new TaskRest(preyData, data, Init.instance.GlobalData),

                //Going to your mate and reproducing
                new Sequence( new List<Node> {
                    new TaskTargetPosition(_transform, preyData),
                    new TaskReproduce(preyData, data)
                }),

                //Going to a water source and drinking
                new Sequence( new List<Node> {
                    new TaskTargetPosition(_transform, preyData),
                    new TaskDrink(data)
                }),

                //Choosing a random point as a destination and moving to it
                new Sequence( new List<Node> {
                    new TaskSearching(_transform, preyData),
                    new TaskTargetPosition(_transform, preyData)
                })

            }, preyData, data)
        });

        return Root;
    }
} 
 