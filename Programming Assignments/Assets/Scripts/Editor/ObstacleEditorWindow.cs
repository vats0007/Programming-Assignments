using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ObstacleEditorWindow : EditorWindow
{
    private const string PREFS_OBS_SO_INFO = "ObstacleEditorWindow_ObstacleInfo";
    private ObstacleInfoSO obstacleInfoSO;

    [MenuItem("Window/Obstacle Editor")]

    public static void ShowWindow()
    {
        GetWindow<ObstacleEditorWindow>("Obstacle Editor Window");
    }

    private void OnEnable()
    {
        int instanceID = EditorPrefs.GetInt(PREFS_OBS_SO_INFO, 0);
        if(instanceID != 0)
        {
            obstacleInfoSO = EditorUtility.InstanceIDToObject(instanceID) as ObstacleInfoSO;
        }
    }

    private void OnDisable()
    {
        if(obstacleInfoSO != null)
        {
            EditorPrefs.SetInt(PREFS_OBS_SO_INFO, obstacleInfoSO.GetInstanceID());
        }
    }

    private void OnGUI()
    {
        GUILayout.Label("Obstacle Grid Editor", EditorStyles.boldLabel);
        obstacleInfoSO = (ObstacleInfoSO)EditorGUILayout.ObjectField("Obstacle Information", obstacleInfoSO, typeof(ObstacleInfoSO), false);
        EditorGUILayout.Space();

        if(obstacleInfoSO != null)
        {
            EditorGUILayout.Space();
            GUILayout.Label("Toggle the button to spawn/Destroy Obstacles", EditorStyles.label);
            DrawGridToggle();
            OnGUIChange();
        }
        else
        {
            EditorGUILayout.HelpBox("assign obstaceInfoSO", MessageType.Warning);
        }
    }

    private void DrawGridToggle()
    {
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("", GUILayout.Width(20));//empty space for index corner
        for (int xLab = 0; xLab < 10; xLab++)
        {
            GUILayout.Label(xLab.ToString(), GUILayout.Width(20));
        }
        EditorGUILayout.EndHorizontal();
        for (int y = 0; y < 10; y++)
        {

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label(y.ToString(), GUILayout.Width(20));
            for (int x = 0; x < 10; x++)
            {
                obstacleInfoSO.obstacleGrid[x, y] = EditorGUILayout.Toggle(obstacleInfoSO.obstacleGrid[x, y], GUILayout.Width(20));
            }
            EditorGUILayout.EndHorizontal();

        }
    }

    public void OnGUIChange()
    {
        if (GUI.changed)
        {
            EditorUtility.SetDirty(obstacleInfoSO);
            //Debug.Log(obstacleInfoSO.obstacleGrid[0, 0]);
        }
    }
}
