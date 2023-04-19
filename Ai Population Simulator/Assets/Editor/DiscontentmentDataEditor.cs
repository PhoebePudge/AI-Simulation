using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DiscontentmentData))]
public class DiscontentmentDataEditor : Editor
{
    public override void OnInspectorGUI() {
        //Style
        GUIStyle header = new GUIStyle();
        header.normal.textColor = Color.white;
        header.fontStyle = FontStyle.Bold;

        //Display order of priority
        DiscontentmentData data = (DiscontentmentData)target;
        DiscontentmentData.DiscontentStates[] a = data.FindHighestDiscontentment();
        GUILayout.BeginHorizontal();
        foreach (var item in a) { 
            GUILayout.Label(item.ToString(), header);
        }
        GUILayout.EndHorizontal();

        //Display health
        GUILayout.Label("Health: " + data.Health);

        //Display all values
        displayDiscontentment(data.Hunger);
        displayDiscontentment(data.Tiredness);
        displayDiscontentment(data.Reproduction);
        displayDiscontentment(data.Thirst);
        displayDiscontentment(data.Explore);
    }

    //Creates a nice visual display
    private void displayDiscontentment(Discontentment display) {
        //Styles
        GUIStyle header = new GUIStyle(); 
        header.normal.textColor = Color.white;
        header.fontStyle = FontStyle.Bold;

        GUIStyle style = new GUIStyle();
        style.alignment = TextAnchor.MiddleCenter;
        style.normal.textColor = Color.white;

        GUIStyle bg = GUI.skin.window; 
        int guiWidth = (int)((EditorGUIUtility.currentViewWidth - 25) / 4f);

        //Name
        GUILayout.Label(display.Name, header);

        EditorGUILayout.BeginHorizontal();
         //Display cost
        EditorGUILayout.BeginVertical(bg);
        GUILayout.Label("Cost", style, GUILayout.Width(guiWidth));
        display.Cost = EditorGUILayout.FloatField(round(display.Cost), GUILayout.Width(guiWidth));
        EditorGUILayout.LabelField(((int)(display.Cost * display.Cost)).ToString(), style, GUILayout.Width(guiWidth));
        EditorGUILayout.EndVertical();

        GUILayout.Space(10);

        //Display progress
        EditorGUILayout.BeginVertical(bg);
        GUILayout.Label("Progress", style, GUILayout.Width(guiWidth));
        display.Progress = EditorGUILayout.FloatField(round(display.Progress), GUILayout.Width(guiWidth));
        EditorGUILayout.LabelField(((int)(display.Progress * display.Progress)).ToString(), style, GUILayout.Width(guiWidth));
        EditorGUILayout.EndVertical();

        GUILayout.Space(10);

        //Display discontent
        EditorGUILayout.BeginVertical(bg);
        GUILayout.Label("Discontent", style, GUILayout.Width(guiWidth));
        EditorGUILayout.FloatField(display.Discontent, GUILayout.Width(guiWidth));
        EditorGUILayout.EndVertical ();

        GUILayout.Space(10);

        EditorGUILayout.EndHorizontal();
    }
    private float round(float input) {
        return Mathf.Round(input * 100f) / 100f;
    }
}

