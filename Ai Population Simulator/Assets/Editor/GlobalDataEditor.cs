using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GlobalData))]
public class GlobalDataEditor : Editor {
    public override void OnInspectorGUI() { 
        base.OnInspectorGUI();

        GlobalData globalData = (GlobalData)target; 

        //styles
        var style = new GUIStyle(GUI.skin.label);
        style.fontStyle = FontStyle.Bold;

        var italic = new GUIStyle(GUI.skin.label);
        italic.fontStyle = FontStyle.Italic;
        italic.wordWrap = true; 

        //Texture
        GUILayout.BeginHorizontal();
        globalData.windTexture = TextureField("Texture", globalData.windTexture);

        GUILayout.BeginVertical();

        //Strength
        GUILayout.BeginHorizontal();
        GUILayout.Label("Strength", GUILayout.Width(75));
        globalData.WindStrength = EditorGUILayout.FloatField(globalData.WindStrength);
        GUILayout.EndHorizontal();

        //Speed
        GUILayout.BeginHorizontal();
        GUILayout.Label("Speed", GUILayout.Width(75));
        globalData.WindSpeed = EditorGUILayout.FloatField(globalData.WindSpeed);
        GUILayout.EndHorizontal();

        GUILayout.EndVertical();
        GUILayout.EndHorizontal(); 
        GUILayout.BeginHorizontal();
        //Rect we use for Simulation of wind offset
        GUILayout.Label("", GUILayout.Width(70), GUILayout.Height(70));
        Rect r = GUILayoutUtility.GetLastRect();
        //Notes
        GUILayout.TextArea("Simulation of effect of wind on smell. Black is the position of the agent, red is the middle of the smell radius, and yellow being areas in which you can smell.", italic);
        GUILayout.EndHorizontal();

        //Simulation of wind offset
        Texture2D windOffsetDemo = new Texture2D(20, 20);
        windOffsetDemo.filterMode = FilterMode.Point;
        Vector2Int windOFfset = new Vector2Int((int)((windOffsetDemo.width / 2) + globalData.GetWindOffset().x), (int)((windOffsetDemo.height / 2) - globalData.GetWindOffset().y));
        windOffsetDemo = Circle(windOffsetDemo, windOFfset.x, windOFfset.y, globalData.smellRange, Color.yellow);
        windOffsetDemo.SetPixel(windOFfset.x, windOFfset.y, Color.red);
        windOffsetDemo.SetPixel((int)((windOffsetDemo.width / 2)), (int)((windOffsetDemo.height / 2)), Color.black);
        windOffsetDemo.Apply();


        GUI.DrawTexture(r, windOffsetDemo);

        GUILayout.Space(20);

        GUILayout.Label("Spawning Chance", style);
        string[] names = new string[] { "Prey", "Preditor", "Bush", "Tree", "Flowers" };
        for (int i = 0; i < globalData.SpawnChance.Length; i++) { 
            GUILayout.BeginHorizontal();

            GUILayout.Label(names[i], GUILayout.Width(75));
            globalData.SpawnChance[i] = EditorGUILayout.IntField(globalData.SpawnChance[i]);
            GUILayout.Label(" out of " + (globalData.SpawningBounds.y * globalData.SpawningBounds.x).ToString() + " chance per partition");

            GUILayout.EndHorizontal();
        }

        //Makes sure stuff is updated
        EditorUtility.SetDirty(target);

        GUILayout.Space(20);

        GUILayout.Label("Notes", style);
        GUILayout.TextArea("For performance I reccomnd keeping spawning chances around 10 with default spawning bounds. Please keep in mind that not all variables can be changed during play mode. Some may required you to restart playmode to see changes", italic);
    }
    public static Texture2D Circle(Texture2D tex, int x, int y, int r, Color color) {
        //Draw a circle on a texture
        float rSquared = r * r;

        for (int u = 0; u < tex.width; u++) {
            for (int v = 0; v < tex.height; v++) {
                if ((x - u) * (x - u) + (y - v) * (y - v) < rSquared) tex.SetPixel(u, v, color);
            }
        }

        return tex;
    }
    private static Texture2D TextureField(string name, Texture2D texture) {
        //Code found online to make a texture slot
        GUILayout.BeginVertical();
        var style = new GUIStyle(GUI.skin.label);
        //style.alignment = TextAnchor.UpperCenter;
        style.fixedWidth = 70;
        GUILayout.Label(name, style);
        var result = (Texture2D)EditorGUILayout.ObjectField(texture, typeof(Texture2D), false, GUILayout.Width(70), GUILayout.Height(70));
        GUILayout.EndVertical();
        return result;
    }
}