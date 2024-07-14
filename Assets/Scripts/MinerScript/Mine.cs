using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Mine : Tower
{
    [SerializeField] public float resourcespersecond;
    [SerializeField] public float resources;
    float efficiency;

    public new float UpdateResources()
    {
        efficiency = CalcEfficiency();
        resources = resourcespersecond * efficiency * Time.deltaTime;
        return resources;
    }
    public void EnableTower(GameObject gameObject)
    {
        GameObject.Instantiate(gameObject, transform);
    }


}
