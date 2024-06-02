using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class GameData
{
    public List<GameObjectData> gameObjectDataList = new List<GameObjectData>();
}

[Serializable]
public class GameObjectData
{
    public Vector3 position;
    public Quaternion rotation;
    public string prefabName;
}

public class SaveData : MonoBehaviour
{
    public LayerMask saveableLayerMask;
    public GameObject[] blockPrefabs;
    private string saveFilePath;
    public WorldGeneration worldGeneration;

    private void Awake()
    {
        saveFilePath = Path.Combine(Application.persistentDataPath, "save.json");

        if (File.Exists(saveFilePath))
        {
            Debug.Log("Save file exists at: " + saveFilePath);
        }
    }

    public void SaveGame()
    {
        GameData data = new GameData();

        // Отримуємо всі об'єкти з шару Land
        Collider[] allObjectsInLandLayer = Physics.OverlapSphere(Vector3.zero, Mathf.Infinity, saveableLayerMask);

        foreach (Collider collider in allObjectsInLandLayer)
        {
            GameObject saveableObject = collider.gameObject;
            GameObjectData gameObjectData = new GameObjectData();
            gameObjectData.position = saveableObject.transform.position;
            gameObjectData.rotation = saveableObject.transform.rotation;
            gameObjectData.prefabName = saveableObject.name; // зберігаємо ім'я префаба
            data.gameObjectDataList.Add(gameObjectData);
        }

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(saveFilePath, json);
    }

    public void LoadGame()
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            GameData data = JsonUtility.FromJson<GameData>(json);

            foreach (GameObjectData gameObjectData in data.gameObjectDataList)
            {
                // Знаходимо префаб, ім'я якого міститься в імені збереженого об'єкта
                GameObject prefab = Array.Find(blockPrefabs, p => gameObjectData.prefabName.Contains(p.name));
                if (prefab != null)
                {
                    Instantiate(prefab, gameObjectData.position, gameObjectData.rotation);
                }
            }
        }
        else
        {
            Debug.Log("Save file does not exist.");
        }
    }
}