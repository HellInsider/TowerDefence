
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public enum EntityType
{
    Settler,
    Samurai,
    Haijin,
    Maekusha
}
[System.Serializable]   
public class EntityData
{
    public string id;
    public string name;
    public string type;
    public float moveSpeed;
    public float attackSpeed;
    public int health;
    public int durability;
    public int vulnerability;
    public int distanceToEnter;

    public EntityData(string id, string Name, string type, float MoveSpeed, float AttackSpeed, int Health, int Durability, int Vulnerability, int DistanceToEnter)
    {
        this.id = id;
        name = Name;
        this.type = type;
        moveSpeed = MoveSpeed;
        attackSpeed = AttackSpeed;
        health = Health;
        durability = Durability;
        vulnerability = Vulnerability;
        distanceToEnter = DistanceToEnter;
    }   
}

public class EntityinJson : MonoBehaviour
{
    [SerializeField] public EntityData entityData;
    [SerializeField] public EntityinJson entity;
    [SerializeField] public string idunit;

    //IEnumerator LoadData()
    //{
    //    string filePath = Path.Combine(Application.streamingAssetsPath, fileName);

    //    UnityWebRequest www = UnityWebRequest.Get(filePath);
    //    yield return www.SendWebRequest();

    //    if (www.result != UnityWebRequest.Result.Success)
    //    {
    //        Debug.LogError("Error reading JSON file: " + www.error);
    //    }
    //    else
    //    {
    //        string json = www.downloadHandler.text;
    //        Settlement data = JsonUtility.FromJson<Settlement>(json);
    //        // Дальнейшая обработка данных
    //        Debug.Log("Data loaded successfully");
    //    }
    //}

        private string fileName = "EntityList.json";
        private string json;
    public Dictionary<string, EntityData> entityDataDict;
    private void Start()
    {
        entity = new();
        entityData = GetStats(idunit);
    }
    public EntityData GetStats(string idunit)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        string path = Path.Combine(Application.streamingAssetsPath, fileName);
        WWW reader = new WWW(path);
        string json = reader.text;
        entityDataDict = JsonConvert.DeserializeObject<Dictionary<string, EntityData>>(json);
#endif

#if UNITY_EDITOR
        string path = Path.Combine(Application.streamingAssetsPath, fileName);
        string json = File.ReadAllText(path).Trim();
        entityDataDict = JsonConvert.DeserializeObject<Dictionary<string, EntityData>>(json);
#endif


        if (File.Exists(path))
        {

            if (entityDataDict != null)
            {
                if (entityDataDict.TryGetValue(idunit, out EntityData data))
                {
                    entityData = data;
                    return entityData;
                }
                else
                {
                    //Debug.LogError("Entity with ID " + idunit + " not found.");
                     return null;
                }
            }
            else
            {
                Debug.LogError("Failed to deserialize JSON to Dictionary<string, EntityData>.");
                 return null;
            }

        }
        else
        {
            Debug.Log("File " + fileName + " not found.");
             return null;
        }
        
    }
    //public void SaveStats()
    //{
    //    File.WriteAllText(Path.Combine("Configs", "EntityList.json"), JsonUtility.ToJson(_json));
    //}    
}
