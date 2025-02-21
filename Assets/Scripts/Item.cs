using System.Collections;
using System.Collections.Generic;
using QuestSystem;
using UnityEngine;

public class Item : MonoBehaviour, Interactuable, IQuestTaskCompleter
{
    [SerializeField] private ItemSO misDatos;
    [SerializeField] private GameManager gameManager;

    [Header("Quests")]
    [SerializeField]
    private string questName = string.Empty;
    [SerializeField]
    private string taskDescription = string.Empty;
    
    public ItemSO MisDatos { get => misDatos; }

    public void Interactuar()
    {
        gameManager.Inventario.NuevoItem(misDatos);
        CompleteTask();
        Destroy(this.gameObject);
    }


    public void CompleteTask()
    {
        QuestManager.Instance.CompleteTask(questName, taskDescription);
    }
}
