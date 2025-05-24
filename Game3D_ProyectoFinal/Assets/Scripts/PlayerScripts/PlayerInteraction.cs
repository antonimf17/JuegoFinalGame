using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerInteraction : MonoBehaviour
{
    public bool cercaDeLlave = false;
    public bool cercaDePuerta = false;
    public GameObject llave;
    public bool tieneLlave = false;

    public string nombreSiguienteEscena;

    private bool cercaDeMaria, cercaDeMaria2, cercaDeMaria3;

    private bool tieneMaria, tieneMaria2, tieneMaria3;

    private GameObject Maria, Maria2, Maria3;

    public TextMeshProUGUI interactionText;
    public TextMeshProUGUI interaction2Text;

    void Update()
    {
        if (cercaDeLlave && Input.GetKeyDown(KeyCode.F))
        {
            RecogerLlave();
        }

        if (cercaDePuerta && Input.GetKeyDown(KeyCode.F) && tieneLlave)
        {
            AbrirPuerta();
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            if (cercaDeMaria && !tieneMaria)
            {
                tieneMaria = true;
                Maria.SetActive(false);
                Debug.Log("Maria recogida");
            }

            if (cercaDeMaria2 && !tieneMaria2)
            {
                tieneMaria2 = true;
                Maria2.SetActive(false);
                Debug.Log("Maria2 recogida");
            }

            if (cercaDeMaria3 && !tieneMaria3)
            {
                tieneMaria3 = true;
                Maria3.SetActive(false);
                Debug.Log("Maria3 recogida");
            }

            if (cercaDePuerta)
            {
     
                if (TieneTodasLasMarias())
                {
                    AbrirPuerta();
                }
                else
                {
                    Debug.Log("Aún no has recogido todas las Marias.");
                }
            }
        }

        // 👇 Mueve esta parte AQUÍ dentro del Update
        if (cercaDeLlave && !tieneLlave)
        {
            interactionText.text = "Press F to collect the key";
            interactionText.gameObject.SetActive(true);
        }
        else if (cercaDeMaria && !tieneMaria)
        {
            interactionText.text = "Press F to collect Maria";
            interactionText.gameObject.SetActive(true);
        }
        else if (cercaDeMaria2 && !tieneMaria2)
        {
            interactionText.text = "Press F to collect Maria";
            interactionText.gameObject.SetActive(true);
        }
        else if (cercaDeMaria3 && !tieneMaria3)
        {
            interactionText.text = "Press F to collect Maria";
            interactionText.gameObject.SetActive(true);
        }
        else
        {
            interactionText.gameObject.SetActive(false);
        }
        if (cercaDePuerta)
        {
            string nombreEscena = SceneManager.GetActiveScene().name;

            if (nombreEscena == "Parte1Juego") // Cambia "Escena1" por el nombre real de tu escena
            {
                if (tieneLlave)
                {
                    interaction2Text.text = "Press F to open the door";
                }
                else
                {
                    interaction2Text.text = "You need the key!";
                }
            }
            else if (nombreEscena == "Parte2Juego") // Cambia "Escena2" por el nombre real de tu segunda escena
            {
                if (TieneTodasLasMarias())
                {
                    interaction2Text.text = "Press F to open the door";
                }
                else
                {
                    interaction2Text.text = "You need to collect Maria!";
                }
            }

            interaction2Text.gameObject.SetActive(true);
        }
        else
        {
            interaction2Text.gameObject.SetActive(false);
        }
    }



    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Llave"))
        {
            cercaDeLlave = true;
            llave = other.gameObject;
        }

        if (other.CompareTag("Puerta"))
        {
            cercaDePuerta = true;
        }
        switch (other.tag)
        {
            case "Puerta":
                cercaDePuerta = true;
                break;
            case "Maria":
                cercaDeMaria = true;
                Maria = other.gameObject;
                break;
            case "Maria2":
                cercaDeMaria2 = true;
                Maria2 = other.gameObject;
                break;
            case "Maria3":
                cercaDeMaria3 = true;
                Maria3 = other.gameObject;
                break;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Llave"))
        {
            cercaDeLlave = false;
            llave = null;
        }

        if (other.CompareTag("Puerta"))
        {
            cercaDePuerta = false;
        }
        switch (other.tag)
        {
            case "Puerta":
                cercaDePuerta = false;
                break;
            case "Maria":
                cercaDeMaria = false;
                break;
            case "Maria2":
                cercaDeMaria2 = false;
                break;
            case "Maria3":
                cercaDeMaria3 = false;
                break;
        }
    }

    void RecogerLlave()
    {
        tieneLlave = true;
        llave.SetActive(false); ; // elimina el objeto llave del mundo
        Debug.Log("Llave recogida");
    }

    void AbrirPuerta()
    {
        Debug.Log("Puerta abierta, cambiando de escena...");
        SceneManager.LoadScene(nombreSiguienteEscena);
    }
    bool TieneTodasLasMarias()
    {
        return tieneMaria && tieneMaria2 && tieneMaria3;
    }

}
