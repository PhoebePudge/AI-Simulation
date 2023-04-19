using UnityEngine;

public class FoodSource : MonoBehaviour {
    public virtual void Eat() { 
        Destroy(gameObject);
    }  
    public virtual bool isEdible() {
        return true;
    }
}
public class Meat : FoodSource { 
}
public class Flower : FoodSource {
    private int foodAmount;
    private GameObject[] children;
    // Start is called before the first frame update
    void Start() {
        children = new GameObject[9];
        foodAmount = Random.Range(0, 9);

        for (int i = 0; i < 9; i++) {
            children[i] = transform.GetChild(0).transform.GetChild(i).gameObject;
        }

        for (int i = 0; i < 8; i++) {
            int rnd = Random.Range(0, i);

            GameObject temp = children[i];

            children[i] = children[rnd];
            children[rnd] = temp;
        }

        UpdateChildrenDisplay();
    }
    public override bool isEdible() { 
        return foodAmount > 0; 
    }
    float time;
    public float GrowthSpeed = 1f;
    private void Update() {
        if (foodAmount < 9) {
            time += Time.deltaTime * GrowthSpeed;

            if (time > 1) {
                time = 0;
                foodAmount++;
                UpdateChildrenDisplay();
            }
        }
    }
    private void UpdateChildrenDisplay() {
        if (foodAmount >= 0 & foodAmount < 9) {
            for (int i = 0; i < 9; i++) {
                children[i].SetActive(foodAmount > i);
            }
        }
    } 
    public override void Eat() { 
        foodAmount--;
        UpdateChildrenDisplay();
    }
}