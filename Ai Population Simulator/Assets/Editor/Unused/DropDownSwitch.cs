using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DropDownSwitch : MonoBehaviour
{
    public TMP_Dropdown dropdown;
    // Start is called before the first frame update
    void Start()
    {
        ShowSettings();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowStatistics()
    {
        gameObject.GetComponent<DiagnosisOverview>().enabled = false;
        gameObject.GetComponent<StatisticOverview>().enabled = true;
        //gameObject.GetComponent<Settings>().enabled = false;
        gameObject.transform.GetChild(1).gameObject.SetActive(false);
        gameObject.transform.GetChild(0).gameObject.SetActive(true);

    }
    public void ShowDiagnosisInformation()
    {
        gameObject.GetComponent<DiagnosisOverview>().enabled = true;
        gameObject.GetComponent<StatisticOverview>().enabled = false;
        //gameObject.GetComponent<Settings>().enabled = false;
        gameObject.transform.GetChild(1).gameObject.SetActive(false);
        gameObject.transform.GetChild(0).gameObject.SetActive(true);
    }
    public void ShowSettings()
    {
        gameObject.GetComponent<DiagnosisOverview>().enabled = false;
        gameObject.GetComponent<StatisticOverview>().enabled = false;
        //gameObject.GetComponent<Settings>().enabled = true;
        gameObject.transform.GetChild(1).gameObject.SetActive(true);
        gameObject.transform.GetChild(0).gameObject.SetActive(false);
    }

    public void OnDropDownChange()
    {
        int currentIndex = dropdown.value;
        Debug.LogError("Changed to " + currentIndex);

        //0 == diagnosis

        //1 == statistics

        //2 == settings

        switch (currentIndex)
        {
            default:
                break;
            case 0:
                ShowDiagnosisInformation();
                break;
            case 1:
                ShowStatistics();
                break;
            case 2:
                ShowSettings();
                break;
        }
    }
}
