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

        // ����� ��� ������ �� ����������
        public void QuitGame()
        {
            // ���������, �������� �� ���������� �� ���������� Android

            // ��� ������ �������� ���������� ����������� ����� Quit
            Application.Quit();
        }
}

