using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class CustomWindow : EditorWindow
{  
    [MenuItem("Window/AI Manager")]
    public static void ShowWindow() {
        //Show existing window instance. If one doesn't exist, make one.
        EditorWindow.GetWindow(typeof(CustomWindow));
    }  
    private int _tabIndex;
    private GUIStyle _headerStyle;
    private Vector2 _scrollPos;
    private int _scale = 5;

    private void OnGUI() {
        //Our header style
        EditorGUI.BeginChangeCheck();
        _headerStyle = new GUIStyle();
        _headerStyle.alignment = TextAnchor.MiddleCenter;
        _headerStyle.fontStyle = FontStyle.Bold;
        _headerStyle.normal.textColor = Color.white;

        header("AI Manager");

        GUILayout.BeginHorizontal();

        //Call Init Begin (This is done on start by default), we also make sure you cant clear or play if that was done before
        if (GUILayout.Button("Begin") & !Init.playing)
        { 
            Init.Begin();
        };

        if (GUILayout.Button("Clear") & Init.playing)
        {
            Init.Clear();
        };

        GUILayout.EndHorizontal();

        HorizontalLine(Color.grey, 2f, new Vector2(0, 0));

        //Tab system for what to show
        _tabIndex = GUILayout.Toolbar(_tabIndex, new string[] { "Settings", "Data" });

        //Display switch
        _scrollPos = GUILayout.BeginScrollView(_scrollPos);
        switch (_tabIndex)
        {
            default:
                break;
            case 0:
                ShowSpawning();
                break;
            case 1:
                ShowData();
                break; 
        }


        GUILayout.EndScrollView(); 
    }
    //Some pretty editor stuff
    private void header(string text) {
        HorizontalLine(Color.grey, 2f, new Vector2(0, 0));
        GUILayout.Label(text, _headerStyle);
        HorizontalLine(Color.grey, 2f, new Vector2(0, 0));
    }
    public static void HorizontalLine(Color color, float height, Vector2 margin) {
        GUILayout.Space(margin.x);
        EditorGUI.DrawRect(EditorGUILayout.GetControlRect(false, height), color);
        GUILayout.Space(margin.y);
    }
    private void ShowSpawning()
    {
        //Show any debug things, such as showing smell or showing vision gizmos
        AgentSenses.showSmell = EditorGUILayout.Toggle("Show Smell", AgentSenses.showSmell);
        AgentSenses.showVision = EditorGUILayout.Toggle("Show Vision", AgentSenses.showVision);
        AgentSenses.showHearing = EditorGUILayout.Toggle("Show Hearing", AgentSenses.showHearing); 
        AgentData.showHistoryLines = EditorGUILayout.Toggle("Show History Lines", AgentData.showHistoryLines);
        if (Init.playing) { 
            QuadtreeComponent.ShowAllQuadTree = EditorGUILayout.Toggle("Show Quad Tree (for static collisions)", QuadtreeComponent.ShowAllQuadTree);
        }

        
    }
    private void ShowData()
    { 
        //Get our creation time taken
        GUILayout.Label("Time taken in Milliseconds: " + (Init.TimeTaken().Milliseconds));

        if (Init.playing)
        {

            EditorGUILayout.LabelField("Current Number Of Prey: " + Init.data.PreyAmount);
            EditorGUILayout.LabelField("Current Number Of Preditors: " + Init.data.PreditorAmount);

            int[] a = new int[Init.data.genes[0].gene.Length];
            foreach (var item in Init.data.genes) {
                if (a.Length == item.gene.Length) {
                    for (int i = 0; i < item.gene.Length; i++) {
                        a[i] = item.gene[i];
                    }
                }
            }
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Average genes: ", GUILayout.Width(120));


            foreach (var item in a) {
                EditorGUILayout.LabelField(item.ToString(), GUILayout.Width(25));
            }
            EditorGUILayout.EndHorizontal();

            //scale (Textures are odd to use so we get a rect area and scale its size by this to allow you to see it better)
            _scale = EditorGUILayout.IntSlider(_scale, 1, 10);

            //Draw camo
            header("Map Camoflauge Levels"); 
            Texture2D texture = Init.gridManager.CamoflaugeToTexture();
            GUILayout.Label("", GUILayout.Height(texture.height * _scale), GUILayout.Width(texture.width * _scale)); 
            GUI.DrawTexture(GUILayoutUtility.GetLastRect(), texture);

            //Draw landscape
            header("Landscape Texture");
            GUILayout.Label("", GUILayout.Height(texture.height * _scale), GUILayout.Width(texture.width * _scale));
            GUI.DrawTexture(GUILayoutUtility.GetLastRect(), Init.landscape);

            //Draw Walkable
            header("Walkable Areas");
            GUILayout.Label("", GUILayout.Height(texture.height * _scale), GUILayout.Width(texture.width * _scale));
            GUI.DrawTexture(GUILayoutUtility.GetLastRect(), Init.gridManager.WalkableToTexture());

            //Draw drinkable locations
            header("Drinkable Locations");
            GUILayout.Label("", GUILayout.Height(texture.height * _scale), GUILayout.Width(texture.width * _scale));
            GUI.DrawTexture(GUILayoutUtility.GetLastRect(), Init.gridManager.WaterToTexture());

            //Draw food locations
            header("Food Locations");
            GUILayout.Label("", GUILayout.Height(texture.height * _scale), GUILayout.Width(texture.width * _scale));
            GUI.DrawTexture(GUILayoutUtility.GetLastRect(), Init.gridManager.FoodToTexture());


        } 
    }  
}
