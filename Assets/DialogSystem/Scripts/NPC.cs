using System;
using System.Collections;
using System.Collections.Generic;
using QuestSystem;
using TMPro;
using UnityEngine;

public class NPC : MonoBehaviour, Interactuable, IQuestTaskCompleter
{
    [Header("Dialogo")]
    [SerializeField, TextArea(1,5)] private string[] frases; //num min de lineas y num m�ximo el text area (1,5)
    [SerializeField] private float tiempoEntreLetras;
    [SerializeField] private GameObject cuadroDialogo;
    [SerializeField] private TextMeshProUGUI textoDialogo;


    [Header("Opciones")]
    [SerializeField] private bool usarOpciones;
    [SerializeField] private GameObject cuadroOpciones;
    [SerializeField] private GameObject[] botonesOpciones;
    [SerializeField,TextArea(1, 5)] private string[] opciones;
    private bool opcionesMostradas = false;
    private bool terminarConversacion = false;

    [Header("Otros")]
    [SerializeField] private GameManagerSO gameManager;
    private bool patrullaNPCEncontrada = false;
    private bool hablandoNPC = false;
    private int indiceActual = -1;

    [Header("Quests")]
    [SerializeField]
    private int optionToGiveQuest;
    [SerializeField]
    private string questName = string.Empty;
    
    [Header("Quest Tasks")]
    [SerializeField]
    private int optionToCompleteQuestTask;
    [SerializeField]
    private string questTaskName = string.Empty;
    [SerializeField]
    private string taskDescription = string.Empty;
    
    private void Start()
    {
        Patrulla_NPC patrullaNPC;
        if(this.gameObject.TryGetComponent<Patrulla_NPC>(out patrullaNPC)){
            patrullaNPCEncontrada = true;
        }
    }

    public void Interactuar()
    {
        if (patrullaNPCEncontrada)
        {
            this.gameObject.GetComponent<Patrulla_NPC>().NoEstaHablando = false;
            this.gameObject.GetComponent<Patrulla_NPC>().YaHaHablado = true;
        }
    //    gameManager.CambiarEstadoPlayer(false);
        cuadroDialogo.SetActive(true);
        if(opcionesMostradas && !terminarConversacion)
        {
            indiceActual = frases.Length - 1;
          //  Debug.Log("Salto dialogo");
        }
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
        if (usarOpciones && !opcionesMostradas)
        {
            MostrarOpciones();
        }
    }


    private void CompletarFrase()
    {
        StopAllCoroutines();
        textoDialogo.text = frases[indiceActual];
        hablandoNPC = false;
        if (usarOpciones && !opcionesMostradas)
        {
            MostrarOpciones();
        }
        if (terminarConversacion)
        {
            indiceActual = frases.Length - 1;
        }
        
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
    //    gameManager.CambiarEstadoPlayer(true);
        if (patrullaNPCEncontrada)
        {
            this.gameObject.GetComponent<Patrulla_NPC>().NoEstaHablando = true;
        }
        if (usarOpciones)
        {
            cuadroOpciones.SetActive(false);
        }
        opcionesMostradas = false;
    }
    private void MostrarOpciones()
    {
        cuadroOpciones.SetActive(true);
        opcionesMostradas = true; 

        // Configurar los botones con las opciones disponibles
        for (int i = 0; i < botonesOpciones.Length; i++)
        {
            if (i < opciones.Length)
            {
               // Debug.Log("MUESTRATEEE");
                botonesOpciones[i].GetComponentInChildren<TextMeshProUGUI>().text = opciones[i];
                int index = i;
                botonesOpciones[i].GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => SeleccionarOpcion(index));
            }

        }
    }
    public void SeleccionarOpcion(int index)
    {
        terminarConversacion = true;
        cuadroOpciones.SetActive(false);
        indiceActual = index + 1;
        CompletarFrase();
        
        if (index == optionToGiveQuest)
        {
            StartQuestOnPlayer();
        }
        
        if (index == optionToCompleteQuestTask)
        {
            CompleteTask();
        }
    }

    private void StartQuestOnPlayer()
    {
        QuestManager.Instance.StartQuest(questName);
    }


    public void CompleteTask()
    {
        QuestManager.Instance.CompleteTask(questName, taskDescription);
    }
}




