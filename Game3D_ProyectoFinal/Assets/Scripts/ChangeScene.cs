using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class ChangeScene : MonoBehaviour
{
 public void LoadScene(string Part1_game)
    {
        SceneManager.LoadScene(Part1_game);
    }

    public void ExitGame()
    {
        Application.Quit(); //Salir de la aplicación, cierra el juego completamente
    }

  
}
