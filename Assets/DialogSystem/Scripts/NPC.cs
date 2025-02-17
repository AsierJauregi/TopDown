using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NPC : MonoBehaviour, Interactuable
{
    [SerializeField, TextArea(1,5)] private string[] frases; //num min de lineas y num máximo el text area (1,5)
    [SerializeField] private float tiempoEntreLetras;
    [SerializeField] private GameObject cuadroDialogo;
    [SerializeField] private TextMeshProUGUI textoDialogo;

    [SerializeField] private GameManagerSO gameManager;
    private bool hablandoNPC = false;
    private int indiceActual = -1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Interactuar()
    {
        gameManager.CambiarEstadoPlayer(false);
        cuadroDialogo.SetActive(true);
        if (!hablandoNPC)
        {
            SiguienteFrase();
        }
        else
        {
            CompletarFrase();
        }
        
    }

    IEnumerator EscribirFrasePorLetras()
    {
        hablandoNPC =true;
        textoDialogo.text = ""; // limpieza cuadro

       char[] caracteresFrase = frases[indiceActual].ToCharArray(); // subdividir la frase en caracteres 
        foreach (char caracter in caracteresFrase)
        {
            textoDialogo.text += caracter;
            yield return new WaitForSeconds(tiempoEntreLetras);
        }
        hablandoNPC=false;
    }


    private void CompletarFrase()
    {
        StopAllCoroutines();
        textoDialogo.text = frases[indiceActual];
        hablandoNPC = false;

    }

    private void SiguienteFrase()
    {
        indiceActual++;
        if(indiceActual >= frases.Length)
        {
            TerminarDialogo();
        }
        else
        {
            StartCoroutine(EscribirFrasePorLetras());
        }


    }

    private void TerminarDialogo()
    {
        hablandoNPC = false;
        textoDialogo.text = "";
        indiceActual = -1;
        cuadroDialogo.SetActive(false);
        gameManager.CambiarEstadoPlayer(true);
    }
}
