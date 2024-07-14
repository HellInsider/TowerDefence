using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using Unity.VisualScripting;
using UnityEngine;

public class SaverScript : MonoBehaviour
{
    [SerializeField] List<GameObject> units;
    [SerializeField] List<GameObject> towers;

    [SerializeField] string data;


    void Awake()
    {
        units = GameObject.FindGameObjectsWithTag("Unit").ToList();
        towers = GameObject.FindGameObjectsWithTag("Tower").ToList();
    }

    void Update()
    {
        
    }
}
