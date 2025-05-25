using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroUIPanel : MonoBehaviour
{
    public GameObject introPanel;
    public GameObject PanelGame;
    public GameObject PausePanel;
    private bool isPaused = false;
    void Start()
    {
        // Pausa el tiempo al inicio si el panel está activo
        if (introPanel.activeSelf)
        {
            Time.timeScale = 0f;
        }
    }

    void Update()
    {
        if (introPanel.activeSelf && Input.GetKeyDown(KeyCode.Return))
        {
            introPanel.SetActive(false);
            Time.timeScale = 1f;
            PanelGame.SetActive(true);

        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                Resume();
            else
                Pause();
        }
    }

    void Pause()
    {
        PausePanel.SetActive(true);
        PanelGame.SetActive(false);
        Time.timeScale = 0f;
        isPaused = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void Resume()
    {
        PausePanel.SetActive(false);
        PanelGame.SetActive(true);
        Time.timeScale = 1f;
        isPaused = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    void OnDisable()
    {
        // Por seguridad, asegúrate que el tiempo se reanude si el objeto se desactiva por otro motivo
        Time.timeScale = 1f;
    }
    public void PauseGame()
    {
        PausePanel.SetActive(true);
        PanelGame.SetActive(false);
        Time.timeScale = 0f;
    }
    public void RenaudPuase()
    {
        PausePanel.SetActive(false);
        PanelGame.SetActive(true);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
    }
 
}

