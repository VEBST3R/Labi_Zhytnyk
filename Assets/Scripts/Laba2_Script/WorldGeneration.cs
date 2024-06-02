using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class WorldGeneration : MonoBehaviour
{
    public GameObject GrassPrefab;
    public GameObject StonePrefab;
    public GameObject DirtPrefab;
    public SaveData saveData;
    public List<GameObject> blocksInfo; // змінено на масив

    public int width = 500; // ширина світу в блоках
    public int length = 500; // довжина світу в блоках
    public int height = 4; // висота світу в блоках

    // Start is called before the first frame update
    void Start()
    {
        string savePath = Application.persistentDataPath + "/save.json";
        if (File.Exists(savePath))
        {
            saveData.LoadGame();
        }
        else
        {
            GenerateWorld();
        }
    }

    void GenerateWorld()
    {
        int stoneHeight = 3;
        int dirtHeight = 3;
        int grassHeight = 1;

        // Додаємо випадкове зсування
        float xOffset = Random.Range(0f, 1000f);
        float zOffset = Random.Range(0f, 1000f);

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < length; z++)
            {
                // Використовуємо зсування тут
                int baseHeight = Mathf.FloorToInt(Mathf.PerlinNoise(x * .05f + xOffset, z * .05f + zOffset) * (2 * height));
                for (int y = 0; y < baseHeight + stoneHeight + dirtHeight + grassHeight; y++)
                {
                    GameObject toInstantiate;
                    if (y < baseHeight + stoneHeight)
                    {
                        toInstantiate = StonePrefab;
                    }
                    else if (y < baseHeight + stoneHeight + dirtHeight)
                    {
                        toInstantiate = DirtPrefab;
                    }
                    else
                    {
                        toInstantiate = GrassPrefab;
                    }
                    GameObject newObj = Instantiate(toInstantiate, new Vector3(x, y, z), Quaternion.identity);
                    blocksInfo.Add(newObj);
                }
            }
        }
    }
}