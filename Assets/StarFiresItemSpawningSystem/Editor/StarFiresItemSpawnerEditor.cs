#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(StarFiresItemSpawner))]
public class StarFiresItemSpawnerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        StarFiresItemSpawner spawner = (StarFiresItemSpawner)target;

        spawner.spawnManager = (StarFiresItemManager)EditorGUILayout.ObjectField(
            "Spawn Manager", spawner.spawnManager, typeof(StarFiresItemManager), true);

        if (spawner.spawnManager != null &&
            spawner.spawnManager.itemPrefabs != null &&
            spawner.spawnManager.itemPrefabs.Count > 0)
        {
            string[] options = spawner.spawnManager.GetItemNames();

            if (spawner.selectedItemIndex >= options.Length)
                spawner.selectedItemIndex = 0;

            spawner.selectedItemIndex = EditorGUILayout.Popup("Item To Spawn", spawner.selectedItemIndex, options);
        }
        else
        {
            EditorGUILayout.HelpBox("Assign The Item Manager Script To A GameObject.", MessageType.Warning);
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(spawner);
        }
    }
}
#endif
//This Script Was Made By StarFireVR.!!!!