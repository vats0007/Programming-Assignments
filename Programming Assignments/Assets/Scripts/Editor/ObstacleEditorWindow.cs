//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEditor;

//public class ObstacleEditorWindow : EditorWindow
//{
//    //for setting editorPrefs for setting SO 
//    private const string PREFS_OBS_SO_INFO = "ObstacleEditorWindow_ObstacleInfo";
//    private ObstacleInfoSO obstacleInfoSO;

//    //can be enabled from path...
//    [MenuItem("Window/Obstacle Editor")]
//    public static void ShowWindow()
//    {
//        GetWindow<ObstacleEditorWindow>("Obstacle Editor Window");
//    }

//    private void Awake()
//    {
//        obstacleInfoSO = FindAnyObjectByType<ObstacleInfoSO>();//finding SO object if on the scene(But not allowed)
//    }

//    private void OnEnable()
//    {
//        //gettings for editorPrefs
//        int instanceID = EditorPrefs.GetInt(PREFS_OBS_SO_INFO, 0);
//        if(instanceID != 0)
//        {
//            obstacleInfoSO = EditorUtility.InstanceIDToObject(instanceID) as ObstacleInfoSO;
//        }
//    }

//    private void OnDisable()
//    {
//        //settings for editorPrefs
//        if(obstacleInfoSO != null)
//        {
//            EditorPrefs.SetInt(PREFS_OBS_SO_INFO, obstacleInfoSO.GetInstanceID());
//        }
//    }
//    private void OnGUI()
//    {
//        //lable for name
//        GUILayout.Label("Obstacle Grid Editor", EditorStyles.boldLabel);
//        //gettinf SO info
//        obstacleInfoSO = (ObstacleInfoSO)EditorGUILayout.ObjectField("Obstacle Information", obstacleInfoSO, typeof(ObstacleInfoSO),false);//scene odject is not allowed
//        EditorGUILayout.Space();

//        if(obstacleInfoSO != null)//if not null
//        {
//            EditorGUILayout.Space();
//            GUILayout.Label("Toggle the button to spawn/Destroy Obstacles", EditorStyles.label);
//            DrawGridToggle();
//            OnGUIChange();
//        }
//        else//warning Message
//        {
//            EditorGUILayout.HelpBox("assign obstaceInfoSO", MessageType.Warning);
//        }
//    }

//    //for ToggleButtons
//    private void DrawGridToggle()
//    {
//        EditorGUILayout.BeginHorizontal();
//        GUILayout.Label("", GUILayout.Width(20));//empty space for index corner
//        //Lables for X
//        for (int xLab = 0; xLab < 10; xLab++)
//        {
//            GUILayout.Label(xLab.ToString(), GUILayout.Width(20));
//        }
//        EditorGUILayout.EndHorizontal();
//        for (int y = 0; y < 10; y++)
//        {
//            EditorGUILayout.BeginHorizontal();
//            //lables for Y
//            GUILayout.Label(y.ToString(), GUILayout.Width(20));
//            for (int x = 0; x < 10; x++)
//            {
//                obstacleInfoSO.obstacleGrid[x, y] = EditorGUILayout.Toggle(obstacleInfoSO.obstacleGrid[x, y], GUILayout.Width(20));
//            }
//            EditorGUILayout.EndHorizontal();

//        }
//    }

//    //on certain change 
//    public void OnGUIChange()
//    {
//        if (GUI.changed)
//        {
//            EditorUtility.SetDirty(obstacleInfoSO);//setting changes to SO as well
//            //Debug.Log(obstacleInfoSO.obstacleGrid[0, 0]);
//        }
//    }
//}
