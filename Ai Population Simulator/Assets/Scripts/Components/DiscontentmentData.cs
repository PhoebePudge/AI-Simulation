using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscontentmentData : MonoBehaviour
{
     
    public float Health = 1f;

    public Discontentment Hunger;
    public Discontentment Tiredness;
    public Discontentment Reproduction;
    public Discontentment Thirst;
    public Discontentment Explore;

    public enum DiscontentStates { Hunger, Tiredness, Reproduction, Thirst, Explore, None }
    public DiscontentStates state = DiscontentStates.Explore;

    //Initialising
    void Start()
    {
        Hunger = new Discontentment("Hunger");
        Tiredness = new Discontentment("Tiredness");
        Reproduction = new Discontentment("Reproduction");
        Thirst = new Discontentment("Thirst");
        Explore = new Discontentment("Exploration");
        Explore.Cost = 5;
    }
    //Randomosing our values
    public void RandomizeData(bool adult) {
        Tiredness.Progress = Random.Range(0.5f, 1f);
        Thirst.Progress = Random.Range(0.5f, 1f);
        Hunger.Progress = Random.Range(0.5f, 1f);

        //Randomise reproduction if we are a adult
        if (adult)
            Reproduction.Progress = Random.Range(0.5f, 1f); 
    }
    // Update is called once per frame
    public void UpdateData(bool adult, AgentMemory history)
    {
        //Increase the progress and calculate our costs
        Tiredness.Progress += (Time.deltaTime * GetComponent<AgentData>().Speed) / Init.instance.GlobalData.discontentmentProgressSpeed;

        Thirst.Progress += Time.deltaTime * 1 / Init.instance.GlobalData.discontentmentProgressSpeed;
        Thirst.SetCost(transform.position, history.waterSourceHistory);

        Hunger.Progress += Time.deltaTime * 1 / Init.instance.GlobalData.discontentmentProgressSpeed;
        Hunger.SetCost(transform.position, history.foodSourceHistory);

        if (adult) {
            Reproduction.Progress += Time.deltaTime * 1 / Init.instance.GlobalData.discontentmentProgressSpeed;
            Reproduction.SetCost(transform.position, history.oppositeGenderHistory);
        }

        //Checking if we are dying
        int MaxDiscontentment = Init.instance.GlobalData.discontentmentDamageThreashold;
        if (Tiredness.Discontent >= MaxDiscontentment || Thirst.Discontent >= MaxDiscontentment || Hunger.Discontent >= MaxDiscontentment) {
            Health -= Time.deltaTime * 1 / Init.instance.GlobalData.discontentmentProgressSpeed;


            //We died
            if (Health <= 0) { 
                

                GameObject gm = GameObject.Instantiate(Resources.Load<GameObject>("Meat"));
                gm.transform.parent = Init.instance.SpawningTransform;
                Vector2Int location = new Vector2Int((int)transform.position.x, (int)transform.position.y);
                Init.gridManager.SetFood(location, Food.Meat, gm.AddComponent<Meat>());
                gm.transform.position = Init.gridManager.GetPartition(location).CenterPosition;

                Destroy(gameObject);

            }
        }
    }
    //Find our discontentmnet sorted by the priority
    public DiscontentStates[] FindHighestDiscontentment() {
        DiscontentStates[] values = new DiscontentStates[5];
        
        bool[] toggled = new bool[5] { true, true, true, true, true};
        for (int i = 0; i < 5; i++) {
            DiscontentStates SelectedValue = DiscontentStates.None;
            int HighestDiscontentment = -1;

            if (Hunger.Discontent >= HighestDiscontentment && toggled[0] == true) {
                HighestDiscontentment = Hunger.Discontent;
                SelectedValue = DiscontentStates.Hunger;
                
            }

            if (Tiredness.Discontent >= HighestDiscontentment && toggled[1] == true) {
                HighestDiscontentment = Tiredness.Discontent;
                SelectedValue = DiscontentStates.Tiredness;
                
            }

            if (Reproduction.Discontent >= HighestDiscontentment && toggled[2] == true) {
                HighestDiscontentment = Reproduction.Discontent;
                SelectedValue = DiscontentStates.Reproduction;
                
            }

            if (Thirst.Discontent >= HighestDiscontentment && toggled[3] == true) {
                HighestDiscontentment = Thirst.Discontent;
                SelectedValue = DiscontentStates.Thirst;
                
            }

            if (Explore.Discontent >= HighestDiscontentment && toggled[4] == true) {
                HighestDiscontentment = Explore.Discontent;
                SelectedValue = DiscontentStates.Explore;
                
            }
            toggled[(int)SelectedValue] = false;
            values[i] = SelectedValue;
        }

        return values;
    }
}
