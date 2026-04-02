using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class KeosAmazingSceneChanger : MonoBehaviour
{
    [HideInInspector]
    public List<string> scenes = new List<string>();
    [HideInInspector]
    public string SceneToLoad;

    [Header("Trigger")]
    public string HandTag = "HandTag";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(HandTag))
        {
            SceneManager.LoadScene(SceneToLoad);
        }
    }

    public void GetAllScenesAHHHHHHImBoutaCum()
    {
        scenes.Clear();
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string p = SceneUtility.GetScenePathByBuildIndex(i);
            string n = System.IO.Path.GetFileNameWithoutExtension(p);
            scenes.Add(n);
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(KeosAmazingSceneChanger))]
public class KeosAmazingSceneChangerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        KeosAmazingSceneChanger script = (KeosAmazingSceneChanger)target;
        base.OnInspectorGUI();

        GUILayout.Space(20);

        GUIStyle style = new GUIStyle();
        style.fontSize = 20;
        style.normal.textColor = Color.white;
        style.fontStyle = FontStyle.Bold;
        style.alignment = TextAnchor.MiddleCenter;

        string CurrentScene = string.IsNullOrEmpty(script.SceneToLoad) ? "None" : script.SceneToLoad;
        GUILayout.Label("Current Scene: " + CurrentScene, style);

        GUILayout.Space(20);

        if (GUILayout.Button("Get AllScenes"))
        {
            script.GetAllScenesAHHHHHHImBoutaCum();
        }

        GUILayout.Space(20);

        if (GUILayout.Button("None"))
        {
            script.SceneToLoad = "";
        }
        GUILayout.Space(10);

        foreach (string s in script.scenes)
        {
            if (GUILayout.Button(s))
            {
                script.SceneToLoad = s;
            }
        }
    }
}
#endif