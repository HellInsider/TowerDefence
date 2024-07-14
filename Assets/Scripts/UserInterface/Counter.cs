using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class Counter : MonoBehaviour
{
    [SerializeField] TMP_Text minerscounter = null;
    [SerializeField] TMP_Text minersunitcounter = null;
    [SerializeField] TMP_Text warriorscounter = null;
    [SerializeField] TMP_Text warriorsunitcounter = null;
    [SerializeField] Settlement settlementdata;
    [SerializeField] ClassSettlementsData settlements;
    int warriorcounter=0;
    int warriorunitcounter=0;
    int minerunitcounter=0;
    int minercounter=0;

    private void Awake()
    {
         
    }
    public void Update()
    {
        minercounter = 0;
        warriorcounter = 0;
        minerunitcounter = 0;
        warriorunitcounter = 0;
        var data = settlementdata.settlementsData;
        List<GameObject> towerslist = GameObject.FindGameObjectsWithTag("Tower").ToList();
        for( int i =0; i<towerslist.Count; i++ ) 
        {
            if(towerslist[i].GetComponent("WarriorTower") != null)
            {
                warriorcounter++;
                WarriorTower warriorTower = towerslist[i].GetComponent<WarriorTower>();
                warriorunitcounter += warriorTower.GetComponent<WarriorTower>().GetUnitsCount();
            }
            if (towerslist[i].GetComponent("ArcherTower") != null)
            {
                warriorcounter++;
                ArcherTower archerTower = towerslist[i].GetComponent<ArcherTower>();
                warriorunitcounter += archerTower.GetUnitsCount();
                    //warriorunitcounter += towerslist[i].GetComponent<ArcherTower>().GetUnitsCount();
            }
            if (towerslist[i].GetComponent("Mine") != null)
            {
                minercounter++;
                Mine mine = towerslist[i].GetComponent<Mine>();
                minerunitcounter += mine.GetComponent<Mine>().GetUnitsCount();
            }
            
        }
        minersunitcounter.text = minerunitcounter.ToString("#");
        warriorsunitcounter.text = warriorunitcounter.ToString("#");
        if (minerunitcounter == 0 || warriorunitcounter == 0)
        {
            if (minerunitcounter == 0)
            {
                minersunitcounter.text = minerunitcounter.ToString("0");
            }
            if (warriorunitcounter == 0)
            {
                warriorsunitcounter.text = warriorunitcounter.ToString("0");
            }
        }
        minerscounter.text = minercounter.ToString("#");
        warriorscounter.text = warriorcounter.ToString("#");
        if(minercounter == 0 || warriorcounter == 0)
        {
            if (minercounter == 0)
            {
                minerscounter.text = minercounter.ToString("0");
            }
            if (warriorcounter == 0)
            {
                warriorscounter.text = warriorcounter.ToString("0");
            }
        }

    }
}