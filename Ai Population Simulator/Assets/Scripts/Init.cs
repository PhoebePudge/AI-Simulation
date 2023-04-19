using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Genes;
public class GraphedData {
    public int PreyAmount;
    public int PreditorAmount;
    public List<DNA> genes;
    public GraphedData() { PreditorAmount = 0; PreyAmount = 0; genes = new List<DNA>(); }

}
public class Init : MonoBehaviour {
    //Our instance
    public static Init instance;

    //Our instance's public variables
    public GlobalData GlobalData;
    public Transform SpawningTransform;
    public Transform _graph;
    public Transform PlaneTransform;
    public Gradient _gradient;
    public QuadtreeComponent quadtree;

    //Used to find time taken to genderate map
    private static System.DateTime _startTime;
    private static System.DateTime _endTime;
    public static System.TimeSpan TimeTaken() {
        return _startTime.Subtract(_endTime);
    }

    public static GraphedData data = new GraphedData();

    //Some public stuff we use in other places
    public static Texture2D landscape;
    public static GridManager gridManager;

    public static bool playing = false;
    private void Start() {
        instance = this;
        Begin();
    }

    public static void Begin() {
        playing = true;
        //Geting our width and height
        int height = instance.GlobalData.SpawningBounds.y;
        int width = instance.GlobalData.SpawningBounds.x;

        //Creating our quadTree
        instance.quadtree.transform.position = new Vector3(height / 2f, 0f, width / 2f);
        instance.quadtree.Create();

        //Setting our start time
        _startTime = System.DateTime.Now;

        //Creating our grid manager
        gridManager = new GridManager(width, height);

        //Creating our landscape texture
        landscape = new Texture2D(height, width);
        landscape.filterMode = FilterMode.Point;
        bool a = true;

        float totalPartitions = height * width;
        //Going though each parition
        for (int x = 0; x < height; x++) {
            for (int y = 0; y < width; y++) {

                //Calculate our colour for our texture
                float col = Mathf.PerlinNoise(instance.GlobalData.MapOffset.x + (((float)x / height) * instance.GlobalData.Scale), instance.GlobalData.MapOffset.y + (((float)y / width) * instance.GlobalData.Scale));
                landscape.SetPixel(x, y, instance._gradient.Evaluate(col));


                //Find our current position
                Vector3 position = new Vector3(x, 0, y);
                Vector2Int position2Int = new Vector2Int(x, y);

                //If its water (This value comes from where our gradient has its water)
                if (col <= 0.22) {
                    gridManager.SetWater(position2Int);
                    instance.quadtree.Collisionquadtree.InsertSquare(position2Int + new Vector2(0.5f, 0.5f), 0.5f, true);
                } else {

                    //If we are within the chance to spawn prey
                    if (Random.Range(0, totalPartitions) < instance.GlobalData.SpawnChance[0]) {

                        GameObject agent = GameObject.Instantiate(Resources.Load<GameObject>("Prey"), instance.SpawningTransform);
                        agent.GetComponent<Transform>().position = position;
                        agent.GetComponent<AgentData>().Create(new DNA(11));
                        agent.GetComponent<AgentData>().RandomizeData();
                        agent.GetComponent<AgentData>().TargetMovement = position;
                        data.genes.Add(agent.GetComponent<AgentData>().DNA);
                        //If not, are we within the chance to spawn a preditor
                        //} else if (Random.Range(0, 200) < instance.GlobalData.SpawnChance[1]) {
                    } else if (Random.Range(0, totalPartitions) < instance.GlobalData.SpawnChance[1]) {
                        a = false;
                        GameObject agent = GameObject.Instantiate(Resources.Load<GameObject>("Preditor"), instance.SpawningTransform);
                        agent.GetComponent<Transform>().position = position;
                        agent.GetComponent<AgentData>().Create(new DNA(11));
                        agent.GetComponent<AgentData>().RandomizeData();
                        agent.GetComponent<AgentData>().TargetMovement = position;
                        data.genes.Add(agent.GetComponent<AgentData>().DNA);

                    }

                    //Calculate our folliage scale and make a variable
                    float folliageSizeMulti = Random.Range(0.8f, 1.2f);
                    GameObject gm = null;

                    //Spawn grass (Provides camoflauge)
                    if (Random.Range(0, totalPartitions) < instance.GlobalData.SpawnChance[2]) {

                        gm = GameObject.Instantiate(Resources.Load<GameObject>("grass"), instance.SpawningTransform);
                        gm.name = "grass";
                        gm.transform.rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
                        gridManager.SetCamoflauge(position2Int, 3);

                        //Spawn birch
                    } else if (Random.Range(0, totalPartitions) < instance.GlobalData.SpawnChance[3]) {

                        gm = GameObject.Instantiate(Resources.Load<GameObject>("Birch"), instance.SpawningTransform);
                        gm.transform.rotation = gm.transform.rotation * Quaternion.Euler(0, 0, Random.Range(0, 360));
                        gm.name = "Bush";

                        //Spawn loose food
                    } else if (Random.Range(0, totalPartitions) < instance.GlobalData.SpawnChance[4]) {
                        //More likely to spawn flowers than meat
                        if (Random.Range(0, 4) == 1) {
                            gm = GameObject.Instantiate(Resources.Load<GameObject>("Meat"), instance.SpawningTransform);
                            gm.name = "Meat";
                            gm.transform.rotation = gm.transform.rotation * Quaternion.Euler(0, 0, Random.Range(0, 360));
                            gridManager.SetFood(position2Int, Food.Meat, gm.AddComponent<Meat>());
                        } else {
                            gm = GameObject.Instantiate(Resources.Load<GameObject>("Flower"), instance.SpawningTransform);
                            gm.name = "Flower";
                            gm.transform.rotation = gm.transform.rotation * Quaternion.Euler(0, 0, Random.Range(0, 360));
                            gridManager.SetFood(position2Int, Food.Flower, gm.AddComponent<Flower>());
                        }
                    }

                    //If we did create one, lets set its position and scale
                    if (gm != null) {
                        gm.transform.position = position;
                        gm.transform.localScale = new Vector3(folliageSizeMulti, folliageSizeMulti, folliageSizeMulti);
                    }
                }
            }
        }

        //Apply its landscape, position and scale
        landscape.Apply();
        instance.PlaneTransform.gameObject.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", landscape);
        instance.PlaneTransform.position = new Vector3(height / 2f, 0f, width / 2f);
        instance.PlaneTransform.localScale = new Vector3(height / 10f, 1f, width / 10f);

        //Setting end date
        _endTime = System.DateTime.Now;

    }
    public static void Clear() {
        playing = false;
        //Removing each item we have spawned before
        foreach (Transform item in instance.SpawningTransform) {
            Destroy(item.gameObject);
        }
    }
}
