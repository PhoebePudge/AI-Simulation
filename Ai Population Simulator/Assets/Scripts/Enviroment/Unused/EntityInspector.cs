using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class EntityInspector : MonoBehaviour {
    //Prefabs for filling out inspector
    [SerializeField] GameObject SliderBlock;
    [SerializeField] GameObject TextBlock;
    [SerializeField] GameObject HeaderBlock;


    [SerializeField] GameObject HighlightedObject;


    //Currently displayed inspector
    private bool isEmpty = true;
    Coroutine UD = null;
    //private void Start() {
    //    transform.GetComponent<Image>().enabled = false;
    //}
    //void Update() {
    //    if (Input.GetMouseButtonDown(0)) {
    //        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    //        RaycastHit hit;

    //        if (Physics.Raycast(ray, out hit, 100)) {
    //            if (hit.transform.gameObject.GetComponent<Entity>() != null) {
    //                DisplayEntity(hit.transform.gameObject);

    //            }
    //        }
    //    }
    //}
    //public void DisplayEntity(GameObject hit) {

    //    //Clear old one
    //    if (!isEmpty) {
    //        foreach (Transform item in transform) {
    //            Destroy(item.gameObject);
    //        }
    //        isEmpty = true;
    //    }

    //    if (isEmpty) {
    //        //Create new stuff
    //        GameObject ClassBlock = Instantiate(HeaderBlock, transform);
    //        ClassBlock.GetComponent<TextMeshProUGUI>().text = hit.transform.gameObject.GetComponent<Entity>().GetType().ToString();

            
    //        if (hit.GetComponent<Animal>() != null) {
    //            Animal an = hit.GetComponent<Animal>();

    //            if (UD != null) {
    //                //one already exists, close old one
    //                StopCoroutine(UD);
    //            }

    //            DisplayAnimal(an);



    //            //Start new one
    //            UD = StartCoroutine(UpdateAnimal(an));

    //        } else {
                
    //            if (UD != null & isDisplayAnimal == true) {
    //                Debug.LogError(isDisplayAnimal == true); 
    //                //one already exists, close old one
    //                StopCoroutine(UD);
    //            }
    //        }
            
    //        if (hit.GetComponent<Plant>() != null) {
    //            Plant an = hit.GetComponent<Plant>();

    //            if (UD != null) {
    //                //one already exists, close old one
    //                StopCoroutine(UD);
    //            }

    //            DisplayPlant(an);

    //            //Start new one
    //            UD = StartCoroutine(UpdatePlant(an));

    //        } else {

    //            if (UD != null & isDisplayAnimal == false) {
    //                Debug.LogError(isDisplayAnimal == false);
    //                //one already exists, close old one
    //                StopCoroutine(UD);
    //            }
    //        }

    //        isEmpty = false;
    //    }
    //}
    //bool isDisplayAnimal;
    //public void DisplayPlant(Plant an) {
    //    transform.GetComponent<Image>().enabled = true;
    //    isDisplayAnimal = false;
    //    GameObject AgeBlock = Instantiate(SliderBlock, transform);
    //    AgeBlock.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Growth Progress"; 
    //    AgeBlock.transform.GetChild(1).GetComponent<Slider>().value = an.growth;


    //    GameObject GrowthTime = Instantiate(HeaderBlock, transform);
    //    GrowthTime.GetComponent<TextMeshProUGUI>().text = "Growth Time: ";
    //    //Debug.LogError("Displaying a plant detail");
    //}
    //public IEnumerator UpdatePlant(Plant an) {
    //    yield return new WaitForSeconds(0.1f);
    //    HighlightedObject.SetActive(true);
    //    for (int i = 0; i < 200; i++) {
    //        if (isDisplayAnimal == false) {
    //            HighlightedObject.transform.position = an.transform.position;

    //            transform.GetChild(1).transform.GetChild(1).GetComponent<Slider>().value = an.growth;

    //            if (an.growth != 1) {

    //                float timeInSectionds = (1 - an.growth) / Time.smoothDeltaTime;
    //                //Debug.LogError(timeInSectionds);
    //                int timeInMins = (int)(timeInSectionds / 60);
    //                //Debug.LogError(timeInMins);
    //                string output = "";
    //                if (timeInSectionds == 0) {
    //                    output = " < 1 minutes untill fully grown";
    //                } else {
    //                    output = timeInMins.ToString() + " minutes untill fully grown";
    //                } 
    //                transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = output;


    //            } else {
    //                transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "";
    //            }
    //            //Debug.LogError("Updating a plant detail");
    //            yield return new WaitForSeconds(0.1f);
    //        }
    //    }

    //    ClearDisplay();
    //    yield return null;
    //}
    //public void DisplayAnimal(Animal an) {
    //    transform.GetComponent<Image>().enabled = true;
    //    isDisplayAnimal = true;
    //    GameObject HealthBlock = Instantiate(SliderBlock, transform);
    //    HealthBlock.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Health";
    //    HealthBlock.transform.GetChild(1).GetComponent<Slider>().maxValue = an.maxHealth;
    //    HealthBlock.transform.GetChild(1).GetComponent<Slider>().value = an.Health;

    //    GameObject EnergyBlock = Instantiate(SliderBlock, transform);
    //    EnergyBlock.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Energy"; 
    //    EnergyBlock.transform.GetChild(1).GetComponent<Slider>().value = an.energy;

    //    GameObject HungerBlock = Instantiate(SliderBlock, transform);
    //    HungerBlock.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Hunger";
    //    HungerBlock.transform.GetChild(1).GetComponent<Slider>().value = an.hunger;

    //    GameObject ThirstBlock = Instantiate(SliderBlock, transform);
    //    ThirstBlock.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Thirst";
    //    ThirstBlock.transform.GetChild(1).GetComponent<Slider>().value = an.thirst;

    //    GameObject GenderBlock = Instantiate(HeaderBlock, transform);
    //    GenderBlock.transform.GetComponent<TextMeshProUGUI>().text = "Gender: " + ((an.genes.isMale) ? "Male" : "Female");

    //    GameObject AgeBlock = Instantiate(HeaderBlock, transform);
    //    AgeBlock.transform.GetComponent<TextMeshProUGUI>().text = "Age: " + GetAgeInFormat((int)an.Age, an);

    //    GameObject GenesBlock = Instantiate(TextBlock, transform);
    //    GenesBlock.transform.GetComponent<TextMeshProUGUI>().text = "Genes: " + an.genes.ToString();

    //    GameObject ActionBlock = Instantiate(HeaderBlock, transform);
    //    ActionBlock.transform.GetComponent<TextMeshProUGUI>().text = "Current Action: " + an.currentAction.ToString();

    //}
    //public IEnumerator UpdateAnimal(Animal an) {
    //    yield return new WaitForSeconds(0.1f);
    //    HighlightedObject.SetActive(true);
    //    //sort out while loop
    //    for (int i = 0; i < 200; i++) {
    //        if (isDisplayAnimal == true) {
    //            HighlightedObject.transform.position = an.transform.position;


    //            transform.GetChild(1).transform.GetChild(1).GetComponent<Slider>().value = an.Health;
    //            transform.GetChild(2).transform.GetChild(1).GetComponent<Slider>().value = an.energy;
    //            transform.GetChild(3).transform.GetChild(1).GetComponent<Slider>().value = an.hunger;
    //            transform.GetChild(4).transform.GetChild(1).GetComponent<Slider>().value = an.thirst;

    //            transform.GetChild(6).GetComponent<TextMeshProUGUI>().text = "Age: " + GetAgeInFormat((int)an.Age, an);

    //            transform.GetChild(8).GetComponent<TextMeshProUGUI>().text = "Current Action: " + an.currentAction.ToString();
    //            yield return new WaitForSeconds(0.1f);
    //        }
    //    }


    //    ClearDisplay();
    //    yield return null;
    //}

    //void ClearDisplay() {
    //    foreach (Transform item in transform) {
    //        Destroy(item.gameObject);
    //    }
    //    isEmpty = true;
    //    transform.GetComponent<Image>().enabled = false;
    //    HighlightedObject.SetActive(false);
    //}
    //public string GetAgeInFormat(int months, Animal an) {
    //    string output = "";
    //    int years = months / 12;
    //    if (years != 0) {
    //        output = years.ToString() + " years " + (months - (years * 12)) + " months ";
    //    } else {
    //        output = months.ToString() + " months ";
    //    }

    //    if (months < an.JuvinileMinAge) {
    //        output += " (Infant) ";
    //    } else if (months < an.AdulthoodMinAge) {
    //        output += " (Juvinile) ";
    //    } else if (months < an.ElderlyMinAge) {
    //        output += " (Adult) ";
    //    } else {
    //        output += " (Elderly) ";
    //    }

    //    return output;
    //}
}
