using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Genes;

public class AgentData : MonoBehaviour {
    //Are we a preditor
    public bool isPreditor;

    //Amount of sound generated
    public float soundGeneration = 0;
    public Vector3 TargetMovement;

    public float soundGenerated;

    public float Age = 0;

    public float WanderRadius = 5;
    public float Speed = 2;

    public DNA DNA;
    public DNA OffspringDNA;

    public bool isPreg = false;
    [SerializeField] private float GestationProgress = 0;
    [SerializeField] private int offSpringAmount = 1;

    [SerializeField] private Color maleColour = Color.gray;
    [SerializeField] private Color femaleColour = Color.red;

    public AgentMemory Memory;
    DiscontentmentData data;
    GlobalData globalData;

    //Show history lines
    public static bool showHistoryLines = false;

    [SerializeField] private float BaseScale = 1f;
    public int FieldOfVisionMultiplyer;

    private void OnDestroy() {
        //Returning if this wasnt killed
        if (!this.gameObject.scene.isLoaded) return;
        if (Init.playing == false) return;

        //Stats
        if (isPreditor) Init.data.PreditorAmount--;
        else Init.data.PreyAmount--;

        //GameObject gm = GameObject.Instantiate(Resources.Load<GameObject>("Meat"));
        //gm.transform.parent = Init.instance.SpawningTransform;
        //Vector2Int location = new Vector2Int((int)transform.position.x, (int)transform.position.y);
        //Init.gridManager.SetFood(location, Food.Meat, gm.AddComponent<Meat>());
        //gm.transform.position = transform.position;
    }
    public void Create(DNA dna) {
        if (isPreditor) {
            Init.data.PreditorAmount++;
        } else {
            Init.data.PreyAmount++;
        }

        data = GetComponent<DiscontentmentData>();
        globalData = Init.instance.GlobalData;

        DNA = dna;

        offSpringAmount = Mathf.CeilToInt(Mathf.Clamp(offSpringAmount * DNA.gene[0], 1, 2));

        Speed = Speed * (DNA.gene[1] / 2 + 0.5f);

        BaseScale = DNA.gene[2] / 6f + 0.75f;

        //starts of at 1, 2 and 3. So its just some extra degrees of vision added ontop
        FieldOfVisionMultiplyer = DNA.gene[3];


        //Setting up other stuff
        Age = 0f;
        if (gameObject.GetComponent<MeshRenderer>() == null) {
            gameObject.transform.GetChild(0).GetComponent<MeshRenderer>().material.color = (DNA.isMale) ? maleColour : femaleColour;
        } else {
            gameObject.GetComponent<MeshRenderer>().material.color = (DNA.isMale) ? maleColour : femaleColour;
        } 
    }
    public void RandomizeData() {
        
        Age = Mathf.Clamp(Random.Range(0f, 2f), 0f, 1f);
        data.RandomizeData(Age >= 1);
    }
    Vector3 previousLocation; 
    public void Update() {
        //You only produce sound if you have moved this update
        if (previousLocation != transform.position) {
            soundGeneration = 1f;
        } else {
            soundGeneration = 0f;
        }

        //Updating our discontentment data
        data.UpdateData(Age >= 1, Memory);

        //Increasing our age
        Age += Time.deltaTime * 1 / 25;
        Age = Mathf.Clamp(Age, .2f, 1);

        //Having children
        if (isPreg) {
            GestationProgress += Time.deltaTime * 1 / 25;
            if (GestationProgress > 1) {
                CreateChildren();
            }
        }

        //Find our target direction
        Vector3 targetDirection = TargetMovement - transform.position;
        //Rotate towards
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, Time.deltaTime * Speed * globalData.agentRotationMultiplyer, 0.0f);
        // Calculate a rotation a step closer to the target and applies rotation to this object
        transform.rotation = Quaternion.LookRotation(newDirection);
        //Scale with age
        transform.localScale = new Vector3(BaseScale * Age, BaseScale * Age, BaseScale * Age);
        previousLocation = transform.position;
    }
    private void CreateChildren() {
        //Loop through how many children
        //Debug.LogError(offSpringAmount);
        if (OffspringDNA != null) {
            for (int i = 0; i < offSpringAmount; i++) {

                GameObject agent;
                if (isPreditor) {
                    //Spawn a preditor
                    agent = GameObject.Instantiate(Resources.Load<GameObject>("Preditor"), Init.instance.SpawningTransform);
                    agent.GetComponent<Transform>().position = transform.position;
                    agent.GetComponent<AgentData>().Create(OffspringDNA);
                    agent.GetComponent<AgentData>().RandomizeData(); 
                    agent.GetComponent<AgentData>().TargetMovement = transform.position;

                } else {
                    //Spawn a prey
                    agent = GameObject.Instantiate(Resources.Load<GameObject>("Prey"), Init.instance.SpawningTransform);
                    agent.GetComponent<Transform>().position = transform.position;
                    agent.GetComponent<AgentData>().Create(OffspringDNA);
                    agent.GetComponent<AgentData>().RandomizeData(); 
                    agent.GetComponent<AgentData>().TargetMovement = transform.position;

                } 
                //Set its DNA 
                agent.GetComponent<AgentData>().DNA.Mutate(); 
            }
        }
        isPreg = false;
        GestationProgress = 0;
    }

    private void OnDrawGizmos() {
        if (TargetMovement != null) {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, TargetMovement);
        }

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + (transform.forward * 2));

        if (showHistoryLines) {
            Gizmos.color = Color.Lerp(Color.cyan, Color.blue, 0f);
            if (Memory.oppositeAgentType != null) {
                Gizmos.DrawLine(transform.position, Memory.oppositeAgentType.position);
            }
            Gizmos.color = Color.Lerp(Color.cyan, Color.blue, .4f);
            if (Memory.foodSourceHistory != null) {
                Gizmos.DrawLine(transform.position, Memory.foodSourceHistory.CenterPosition);
            }
            Gizmos.color = Color.Lerp(Color.cyan, Color.blue, .6f);
            if (Memory.waterSourceHistory != null) {
                Gizmos.DrawLine(transform.position, Memory.waterSourceHistory.CenterPosition);
            }
            Gizmos.color = Color.Lerp(Color.cyan, Color.blue, 1f);
            if (Memory.oppositeGenderHistory != null) {
                Gizmos.DrawLine(transform.position, Memory.oppositeGenderHistory.position);
            }
        }
    }
}
