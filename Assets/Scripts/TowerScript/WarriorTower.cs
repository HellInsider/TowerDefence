using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

public class WarriorTower : Tower
{
    [SerializeField] GameObject warrior;
    [SerializeField] public Dictionary<GameObject, GameObject> warriors;
    [SerializeField] public float PatrolDistance;

    public void Awake()
    {
        warriors = new Dictionary<GameObject, GameObject>();
    }

    new void UpdateUnitList()
    {
        for (int i = warriors.Count - 1; i >= 0; i--)
        {
            var pair = warriors.ElementAt(i);
            if (pair.Value == null || pair.Value.GetComponent<MovableObject>().isDead)
            {
                pair.Key.SetActive(true);
                pair.Key.GetComponent<MovableObject>().ApplyDamage(1000);
                warriors.Remove(pair.Key);
            }
        }
        base.UpdateUnitList();
    }

    public new int GetUnitsCount()
    {
        UpdateUnitList();
        return units.Count;
    }

    public override void EnterUnit(GameObject unit)
    {
        UpdateUnitList();
        Debug.Log("Зашёл");
        units.Add(unit);
        var _warrior = Instantiate(warrior, unit.transform.position, Quaternion.identity, transform);
        _warrior.transform.position = unit.transform.position;
        _warrior.GetComponent<Warrior>().MotherTower = gameObject;
        warriors.Add(unit, _warrior);
        unit.SetActive(false);
    }

    public override void ReleaseUnits(int Count, GameObject target)
    {
        UpdateUnitList();
        int unitCount = math.min(Count, units.Count);
        for (int i = unitCount - 1; i >= 0; i--)
        {
            units[i].SetActive(true);
            units[i].transform.position = warriors[units[i]].transform.position;
            units[i].GetComponent<Unit>().SetTarget(target);

            var _warrior = warriors[units[i]];
            warriors.Remove(units[i]);
            Destroy(_warrior);

            units.RemoveAt(i);
        }
    }
}
