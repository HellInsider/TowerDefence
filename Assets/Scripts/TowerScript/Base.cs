
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Base : Tower
{
    public TowerPlacementManager2D TowerPlacementManager;
    [SerializeField] public Settlement settlement;
    [SerializeField] public ClassSettlementsData settlementsdata;
    [SerializeField] public GameObject unitprefab;
    [SerializeField] public int unitcounter;
    [SerializeField] public float resources;
    public void Loaded()
    {
        if (unitprefab == null)
        {
            Debug.LogError("unitprefab is null. Please assign a prefab in the inspector.");
            return;
        }

        if (transform == null)
        {
            Debug.LogError("transform is null. This should not happen.");
            return;
        }

        resources = settlementsdata.countofCurrency;
        for(int i =0; i < unitcounter; i++)
        {
            var unit = GameObject.Instantiate(unitprefab, transform);
            unit.GetComponent<MovableObject>().SetTarget(gameObject);
            //units.Add(unit);
        }

    }
    public int LoadedData(ClassSettlementsData data)
    {
        settlementsdata = data;
        unitcounter = data.countofUnits;
        Loaded();
        return unitcounter;
    }
    
    public void AddUnit(float value)
    {

        if(resources > value)
        {
            for( int i=0; i < 20; i++)
            {
                var unit = GameObject.Instantiate(unitprefab, transform);
                unit.GetComponent<MovableObject>().SetTarget(gameObject);
                unit = null;
            }
            resources -= value;
            settlement.resources = resources; 
        }else { TowerPlacementManager.ErrorWriter(); }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            // Добавьте здесь логику воздействия на юнита
            Debug.Log("Unit has entered the tower trigger zone");
            //ApplyEffect(other.gameObject);
        }
    }

}
