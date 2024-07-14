using Newtonsoft.Json;
using TMPro;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Linq;
using System;
using Unity.VisualScripting;
using UnityEngine.Events;
using System.Collections;
using UnityEngine.Networking;

[System.Serializable]
public class SavedData
{
    public int StashedTownholders { get ; set; }
    //public int InsideCurrency { get; set; }
    public double OutsideCurrency { get; set; }
    public int AllSettlements { get; set; }
    public int CapturedSettlements { get; set; }
    public DateTime TimeofExit { get; set; }

}

public class GameData : MonoBehaviour
{
    //public UnityEvent<double> OnResourceCountChanged;
    public TimeTracker tracker;
    [SerializeField] public SavedData data;
    [SerializeField] public double allmoney;
    private MoneyHandler moneyHandler;
    private DrawElements drawElements;
    [SerializeField] TMP_Text uncapturedSettlements;
    [SerializeField] TMP_Text capturedSettlements;
    public void Start()
    {
        StartCoroutine(LoadData());
    }
public IEnumerator LoadData()
    {
        string fileName = "GameData.json";
        string path = Path.Combine(Application.streamingAssetsPath, fileName);

        UnityWebRequest www = UnityWebRequest.Get(path);
        yield return www.SendWebRequest();
        string json = www.downloadHandler.text;
        GetStats(json);

        Initialization();

        double secondsPassed = Timemanager();

        data.OutsideCurrency = moneyHandler.PassedMoney(secondsPassed, data.OutsideCurrency); 
        allmoney = data.OutsideCurrency;
        DrawSettlements();

        yield return data;
    }

    private void GetStats(string json)
    {
        Dictionary<string, SavedData> GameDataDict = JsonConvert.DeserializeObject<Dictionary<string, SavedData>>(json);
        string key = GameDataDict.Keys.FirstOrDefault();
        GameDataDict.TryGetValue(key, out data);
    }



    private void Initialization()
    {
        tracker = new TimeTracker(); 
        moneyHandler = new MoneyHandler(); 
        drawElements = GetComponent<DrawElements>(); 
    }

    private double Timemanager()
    {
        DateTime newLogoutTime = tracker.UpdateLogoutTime();
        TimeSpan timedifference = newLogoutTime - data.TimeofExit;
        double secondsPassed = timedifference.TotalSeconds;
        return secondsPassed;
    }

    public void DrawSettlements()
    {
        uncapturedSettlements.text = (data.AllSettlements - data.CapturedSettlements).ToString("#");
        capturedSettlements.text = (data.CapturedSettlements).ToString("#");
    }

    //public void Update()
    //{
    //    //drawElements.DrawCapturedSettlements(data.CapturedSettlements); // Отрисовка данных через методы в DrawElements
    //    //drawElements.DrawunCapturedSettlements(data.AllSettlements - data.CapturedSettlements);
    //}


}

//private void SavingData(Dictionary<string, EntityData> entityDataDict)
//{
//    string json = JsonConvert.SerializeObject(entityDataDict);
//    string fileName = "GameData.json";
//    string path = Path.Combine(Application.dataPath, "Configs", fileName);
//    Debug.Log("Path: " + path);
//    File.WriteAllText(path, json);
//}