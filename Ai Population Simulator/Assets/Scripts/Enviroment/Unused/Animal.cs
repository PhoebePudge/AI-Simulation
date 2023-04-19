using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Genes;
//public class Animal : Entity{
//    //Attributes
//    public float energy = 1;
//    public float hunger = 1;
//    public float thirst = 1;
//    public float reproductiveUrge = 1;

//    public float Health = 5;
//    public int maxHealth = 10;

//    public float Age = 0;
      
//    public float wanderRadius = 5;
//    public float speed = 1;
//    //each int refers to a month of in game time
//    public int JuvinileMinAge = 9;
//    public int AdulthoodMinAge = 60;
//    public int ElderlyMinAge = 144;

//    public const int maxViewDistance = 10;

//    public AnimalAction currentAction;

//    //Genetics
//    public DNA genes;
//    public Color maleColour = Color.gray;
//    public Color femaleColour = new Color(
//        255 / 255, 
//        255 / 250,
//        255 / 241);

//    float timeBetweenActionChoices = 1;

//    protected string foodTarget; 
//    // Move data:
//    public bool animatingMovement;
//    Vector3 moveTargetPos;

//    // Other
//    float lastActionChooseTime; 
//   /* public override void Init(Vector3 InitPosition){
//        base.Init(InitPosition);
//        genes = Genes.RandomGenes(5);

//        material.color = (genes.isMale) ? maleColour : femaleColour;

//        currentAction = AnimalAction.Exploring;

//        timeBetweenActionChoices = Random.Range(0.8f, 1.2f);
//        ChooseNextAction();

//        //change this to some cone
//        SphereCollider sc = gameObject.AddComponent<SphereCollider>();
//        sc.isTrigger = true;
//        sc.radius = 6f;
//    }*/
//    List<GameObject> nearbyTargets = new List<GameObject>();
//    List<GameObject> waterTargets = new List<GameObject>();
//    private void OnTriggerEnter(Collider other) {
//        if (other.gameObject.name == "WaterSource")
//        {
//            Debug.LogError("You found a water");
//            waterTargets.Add(other.gameObject);
//            return;
//        }
//        //not an entity target, we can exit out of here!
//        if (other.gameObject.GetComponent<Entity>() == null) {
//            return;
//        } 
//        if (other.gameObject.GetComponent<Entity>().name == foodTarget) {
//            //add to our target list
//            nearbyTargets.Add(other.gameObject);
//            return;
//        }
//    }
//    private void OnTriggerExit(Collider other) {
//        if (waterTargets.Contains(other.gameObject))
//        {
//            Debug.LogError("Removed a water");
//            waterTargets.Remove(other.gameObject);
//        }
//        if (nearbyTargets.Contains(other.gameObject)) {
//            //remove from our target lsit
//            nearbyTargets.Remove(other.gameObject);
//        }

//    }

//    public void ClampPosition()
//    {
//        bool outOfBounds = false;
//        if (transform.position.x < -10 | transform.position.x > 30)
//        {
//            outOfBounds = true; 
//        }
//        if (transform.position.z < -10 | transform.position.z > 30)
//        {
//            outOfBounds = true;
//        }
//        if (outOfBounds)
//        {
//            transform.position = new Vector3
//            (
//                Mathf.Clamp(transform.position.x, -10, 30),
//                transform.position.y,
//                Mathf.Clamp(transform.position.x, -10, 30)
//            );
//            animatingMovement = false;
//            //Vector3 ObjectOffset =  moveTargetPos - transform.position;
//            //Vector3 newPos = ObjectOffset - transform.position;
//            //StartMoveToCoord(newPos);
//        }
//        //transform.position = new Vector3
//        //(
//        //    Mathf.Clamp(transform.position.x, -10, 30),
//        //    transform.position.y,
//        //    Mathf.Clamp(transform.position.x, -10, 30)
//        //);

//        //if ()
//        //{

//        //}
//        //Vector3 ObjectOffset = transform.position - moveTargetPos;
//        //Vector3 newPos = ObjectOffset - transform.position;
//        //StartMoveToCoord(newPos);
//    }


//    protected virtual void Update() {
//        if (isPaused)
//        {
//            return;
//        }
//        ClampPosition();

//        float timeToDeathByHunger = 200;
//        float timeToDeathByThirst = 200;

//        // Increase hunger and thirst over time 
//        hunger -= Time.deltaTime * 1 / timeToDeathByHunger;
//        thirst -= Time.deltaTime * 1 / timeToDeathByThirst;

//        Age += Time.deltaTime;

//        Vector3 scale = Vector3.Lerp(new Vector3(0.3f, 0.3f, 0.3f), new Vector3(1.5f, 1.5f, 1.5f), Age / 30f);
//        transform.localScale = scale;

//        // Animate movement. After moving a single tile, the animal will be able to choose its next action
//        if (animatingMovement) {
//            AnimateMove();
//        } else {
//            float timeSinceLastActionChoice = Time.time - lastActionChooseTime;
//            if (timeSinceLastActionChoice > timeBetweenActionChoices) {
//                ChooseNextAction();
//            }
//        }
//        if (currentAction == AnimalAction.Resting) { 
//            energy += Time.deltaTime * 1 / 50;
//        }
//        //handle hunger, thirst and age death
//    }

//    protected virtual void ChooseNextAction() {
//        lastActionChooseTime = Time.time;

//        //need to switch to goal orintated
//        if (hunger <= 0.5 & nearbyTargets.Count != 0) {
//            if (currentAction == AnimalAction.GoingToFood) {
//                //do a check here to be sure we are near a food source
//                Debug.LogError("is this even in use rn??");
//                currentAction = AnimalAction.Eating;
//            }

//            currentAction = AnimalAction.GoingToFood;
//        }
//        // More thirsty than hungry
//        if (thirst <= 0.5 & waterTargets.Count != 0)
//        {
//            if (currentAction == AnimalAction.GoingToWater)
//            {
//                //do a check here to be sure we are near a food source
//                Debug.LogError("is this even in use rn??");
//                currentAction = AnimalAction.Drinking;
//            }

//            currentAction = AnimalAction.GoingToWater;
//        }

//        if (energy < 0.2f) {
//            Debug.LogError("Tired Bunny");
//            currentAction = AnimalAction.Resting;
//        }

//        Act();

//    }
//    GameObject nearestFoodSource;
//    protected virtual void SearchFood() {
//        if (nearbyTargets.Count == 0)
//        {

//            Debug.Log("No food here, maybe try wander? ");
//            currentAction = AnimalAction.Exploring;
//            Act();
//            return;
//        }

//        nearbyTargets = nearbyTargets.OrderBy( x => Vector2.Distance(this.transform.position, x.transform.position) ).ToList();
//        nearestFoodSource = nearbyTargets[0];
//        Debug.LogError("New produced " + nearestFoodSource.name);
         
//        //We can eat it from here
//        if (Vector3.Distance(transform.position, nearestFoodSource.transform.position) < .2f) { 
//            currentAction = AnimalAction.Eating;
//            Act();
//        } else
//        {//We are not close enough. We need to move to it
//            StartMoveToCoord(nearestFoodSource.transform.position);
//        }
//        //Debug.LogError("Nearest target is " + nearestFoodSource.name + " at dist " + dist); 
//    }
//    protected virtual void SearchWater()
//    {
//        if (waterTargets.Count == 0)
//        {

//            Debug.Log("No water here, maybe try wander? ");
//            currentAction = AnimalAction.Exploring;
//            Act();
//            return;
//        }

//        waterTargets = waterTargets.OrderBy(x => Vector2.Distance(this.transform.position, x.transform.position)).ToList();
//        nearestFoodSource = waterTargets[0];
//        Debug.LogError("New produced " + nearestFoodSource.name);

//        //We can eat it from here
//        if (Vector3.Distance(transform.position, nearestFoodSource.transform.position) < .2f)
//        {
//            currentAction = AnimalAction.Drinking;
//            Act();
//        }
//        else
//        {//We are not close enough. We need to move to it
//            StartMoveToCoord(nearestFoodSource.transform.position);
//        }
//        //Debug.LogError("Nearest target is " + nearestFoodSource.name + " at dist " + dist); 
//    }
//    protected void Act() {
//        //animatingMovement = false;
//        switch (currentAction) {
//            case AnimalAction.Exploring:
//                Vector2 targetPos = (Random.insideUnitCircle * wanderRadius);
//                Vector3 targetPos3 = new Vector3(targetPos.x, 0, targetPos.y) + transform.position;
//                StartMoveToCoord(targetPos3);
//                break;
//            case AnimalAction.GoingToFood:
//                SearchFood();
//                break;
//            case AnimalAction.GoingToWater:
//                SearchWater();
//                break;
//            case AnimalAction.Resting:
//                if (energy > .9f) {
//                    currentAction = AnimalAction.Exploring;
//                }
//                break;
//            case AnimalAction.Eating:
//                Debug.LogError("Eating now");
//                hunger += 0.5f;
//                Destroy(nearestFoodSource);
//                currentAction = AnimalAction.Resting;
//                break;
//            case AnimalAction.Drinking:
//                Debug.LogError("Drinking now");
//                thirst += 0.5f;
//                Destroy(nearestFoodSource);
//                currentAction = AnimalAction.Resting;
//                break;
//        }
//    } 
//    Vector3 moveStartPos;
//    float moveProgress = 0;
//    float rotateProgress = 0;
//    float targetDistance;

//    Quaternion toRotation;
//    float rotationDistance;
//    protected void StartMoveToCoord(Vector3 target) { 
//        moveTargetPos = target;
//        moveStartPos = gameObject.transform.position;
//        targetDistance = Vector2.Distance(moveStartPos, moveTargetPos);
//        moveProgress = 0;
//        rotateProgress = 0;
//        animatingMovement = true;
//        Vector3 direction = moveTargetPos - transform.position;
//        toRotation = Quaternion.LookRotation(direction);

//        rotationDistance = Quaternion.Dot(transform.rotation, toRotation);
//    }

//    protected virtual void AnimateMove() {
//        energy -= Time.deltaTime * speed / 50;

        
//        //Debug.LogWarning("Moving from " + moveStartPos + " to " + moveTargetPos + " at " + moveProgress);
//        moveProgress += (Time.deltaTime * speed);
//        rotateProgress += Time.deltaTime * speed;

        

//        transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, rotateProgress);
//        //transform.LookAt(moveTargetPos);
//        transform.position = Vector3.Lerp(moveStartPos, moveTargetPos, moveProgress);

//        if (moveProgress > 0.99f) {
             
//            //we are at destination
//            animatingMovement = false;
//        }
         
//    }
//}
