using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUIManager : MonoBehaviour
{
    [SerializeField] private bool is_pause_manager;
    [SerializeField] private GameObject pause;
    private bool is_pause_opened = false;

    private void Start()
    {
        Time.timeScale = 1;
    }

    private void Update()
    {
        if (is_pause_manager && Input.GetKeyDown(KeyCode.Tab))
        {
            if (!is_pause_opened)
            {
                Pause(pause);
            }
        }
    }

    public void LoadScene(int index)
    {
        SceneManager.LoadScene(index);
    }

    public void Pause(GameObject PauseMenu)
    {
        Time.timeScale = 0.000000000000000000000001f;
        is_pause_opened = true;
        PauseMenu.SetActive(true);
    }

    public void DeleteSaves()
    {
        PlayerPrefs.DeleteAll();
    }

    public void Resume(GameObject PauseMenu)
    {
        PauseMenu.SetActive(false);
        is_pause_opened = false;
        Time.timeScale = 1;
    }

    public void ToMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
