using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class SceneSwitcher : MonoBehaviour
{
    public string scenename;
    public void OnButtonClick(string sceneName)
    {
       
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }

        // Метод для выхода из приложения
        public void QuitGame()
        {
            // Проверяем, запущено ли приложение на устройстве Android

            // Для других платформ используем стандартный метод Quit
            Application.Quit();
        }
}

