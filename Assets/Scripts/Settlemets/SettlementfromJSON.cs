
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

[System.Serializable]
public class ClassSettlementsData
{
    public bool captured;
    public int countofUnits;
    public float countofCurrency;
    public int countofTownholders;
    public int countofBuildings;
    public string townholders;
    public ClassSettlementsData (bool Captured, int CountofUnits, float CountofCurrency, int CountofTownholders, int CountofBuildings, string Townholders) 
    {
        captured = Captured;
        countofUnits = CountofUnits;
        countofCurrency = CountofCurrency; 
        countofTownholders = CountofTownholders;
        countofBuildings = CountofBuildings;
        townholders = Townholders;
    }
}


public class SettlementfromJSON : MonoBehaviour
{
    public string idsettlement;
    public ClassSettlementsData settlementsData;
    public double currency;
    public Dictionary<string, ClassSettlementsData> settlementsDataDict;
    //void Start()
    //{
    //} 
    public ClassSettlementsData GetStats(string idsettlement)
    {

#if UNITY_ANDROID && !UNITY_EDITOR
        string path = Path.Combine(Application.streamingAssetsPath, fileName);
        WWW reader = new WWW(path);
        string json = reader.text;
        settlementsDataDict = JsonConvert.DeserializeObject<Dictionary<string, Settlements>>(json);
#endif

#if UNITY_EDITOR
        string path = Path.Combine(Application.streamingAssetsPath, "Settlements.json");
        string json = File.ReadAllText(path).Trim();
        settlementsDataDict = JsonConvert.DeserializeObject<Dictionary<string, ClassSettlementsData>>(json);
#endif

        if (settlementsDataDict.TryGetValue(idsettlement, out ClassSettlementsData data))
        {
            settlementsData = data;
            return settlementsData;
        }
        return settlementsData;
    }
}