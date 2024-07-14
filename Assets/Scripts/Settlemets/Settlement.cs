using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class Settlement : MonoBehaviour
{
    public SettlementfromJSON settlementfromJSON = new();
    [SerializeField] public float resources;
    [SerializeField] public string idsettlement;
    [SerializeField] public ClassSettlementsData settlementsData;
    public TMP_Text currency;
    public GameObject StartMessagePanel;
    public Base basa;
    float delay = 0.1f;
    public void Start()
    {
        settlementfromJSON = new();
        basa = basa.GetComponent<Base>();
        StartCoroutine(DelayLoaded(delay));
        
    }
    public IEnumerator StartMessage(GameObject StartMessagePanel)
    {
        yield return new WaitForSeconds(delay);
        if (settlementsData.captured == false)
        {
            StartMessagePanel.SetActive(true);
        }
    }
    private IEnumerator DelayLoaded(float delay)
    {
        yield return new WaitForSeconds(delay);
        settlementsData = settlementfromJSON.GetStats(idsettlement);
        resources = settlementsData.countofCurrency;
        basa.LoadedData(settlementsData);
        StartCoroutine(StartMessage(StartMessagePanel));
    }
    public void Update()
    { 
        List<GameObject> towerslist = GameObject.FindGameObjectsWithTag("Tower").ToList();
        for (int i = 0; i < towerslist.Count; i++)
        {
            Mine towerMine = towerslist[i].GetComponent<Mine>();
            if (towerslist[i].GetComponent("Mine") != null)
            {
                resources += towerMine.UpdateResources();
            }
        }
        settlementsData.countofCurrency = resources;
        currency.text = resources.ToString("#");
    }


}

