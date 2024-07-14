using NavMeshPlus.Components;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using static CustomTools;
public class TowerPlacementManager2D : MonoBehaviour
{
    public Settlement settlement;
    public ClassSettlementsData data;
    public GameObject towerPrefab; 
    private GameObject currentTower;
    public bool isActive = true;
    public GameObject controller;
    private float value;
    [SerializeField] public float resources;

    public void Update()
    {
        transform.rotation = Quaternion.identity;
    }

    public void BuilderTower(float valueoftower)
    {
        data = settlement.settlementsData;
        resources = settlement.resources;
        value = valueoftower;
        if (resources > value)
        {
            
            isActive = true;
            StartCoroutine(BuildTower());
            controller.SetActive(false);
            return;
        }
        
        ErrorWriter();
    }

    IEnumerator BuildTower()
    {
        yield return new WaitForSeconds(0.2f);
        while (isActive)
        {
            if (currentTower != null)
            {
                // Обрабатываем касания экрана
                if (Input.touchCount > 0)
                {
                    Touch touch = Input.GetTouch(0);
                    Vector3 touchPosition = touch.position;
                    Vector3 worldPosition = Camera.main.ScreenToWorldPoint(touchPosition);
                    worldPosition.z = 0; // Устанавливаем z на 0, чтобы башня находилась в плоскости 2D
                    currentTower.transform.position = worldPosition;



                    if (touch.position.x > Screen.width - 10 ||
                        touch.position.x < 10 ||
                        touch.position.y > Screen.height - 10 ||
                        touch.position.y < 10)
                    {
                        // Отменяем расстановку башни, если касание было отменено или завершено
                        CancelTowerPlacement();
                        //resources += value;
                        //settlement.resources = resources;
                        isActive = false;
                        controller.SetActive(true);
                        //settlement.resources = settlement.resources + value;
                    }
                    if (touch.phase == TouchPhase.Ended)
                    {
                        
                            PlaceTower();
                            resources -= value;
                            settlement.resources = resources;   
                            isActive = false;
                            controller.SetActive(true);
                            //settlement.resources = settlement.resources + value;
                        

                        
                    }
                }
            }
            else
            {
                Debug.Log("Ne Kasnylsya");
                StartTowerPlacement();
            }

            yield return new WaitForEndOfFrame();
        }
    }

    // Начинаем процесс расстановки новой башни
    public void StartTowerPlacement()
    {
        if (towerPrefab != null)
        {

            currentTower = GameObject.Instantiate(towerPrefab, transform);
        }
        else
        {
            Debug.LogError("Tower prefab is not assigned!");
        }
    }

    // Подтверждаем расстановку башни
    void PlaceTower()
    {
        currentTower = null;
    }

    // Отменяем расстановку башни
    void CancelTowerPlacement()
    {
        Destroy(currentTower);
        currentTower = null;
    }
    public void ErrorWriter()
    {
        // Находим все объекты с компонентом Transform, включая неактивные
        GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();
        GameObject errorMoney = allObjects.FirstOrDefault(obj => obj.CompareTag("UI"));

        if (errorMoney != null)
        {
            Debug.Log("Found UI object: " + errorMoney.name);
            errorMoney.SetActive(true);
            Debug.Log("Set active: " + errorMoney.activeSelf);

            // Дополнительная проверка: находится ли объект в правильном иерархическом контексте
            Canvas parentCanvas = errorMoney.GetComponentInParent<Canvas>();
            if (parentCanvas != null)
            {
                Debug.Log("Parent Canvas is active: " + parentCanvas.gameObject.activeSelf);
            }
            else
            {
                Debug.LogWarning("No parent Canvas found!");
            }
        }
        else
        {
            Debug.LogWarning("UI GameObject not found!");
        }
    }
}