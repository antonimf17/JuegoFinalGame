using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroUIPanel : MonoBehaviour
{
    public GameObject introPanel;
    public GameObject PanelGame;

    void Start()
    {
        // Pausa el tiempo al inicio si el panel est� activo
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
    }
    void OnDisable()
    {
        // Por seguridad, aseg�rate que el tiempo se reanude si el objeto se desactiva por otro motivo
        Time.timeScale = 1f;
    }
}

