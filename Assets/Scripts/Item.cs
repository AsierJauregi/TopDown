using QuestSystem;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Item : MonoBehaviour, Interactuable, IQuestTaskCompleter
{
    [SerializeField] private ItemSO misDatos;
    [SerializeField] private GameManager gameManager;

    private AudioSource _audioSource;
    
    [Header("Quests")]
    [SerializeField]
    private string questName = string.Empty;
    [SerializeField]
    private string taskDescription = string.Empty;
    
    public ItemSO MisDatos { get => misDatos; }

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void Interactuar()
    {
        gameManager.Inventario.NuevoItem(misDatos);
        CompleteTask();

        _audioSource.volume = 0.25f;
        _audioSource.Play();

        Destroy(this.gameObject, _audioSource.clip.length);
    }


    public void CompleteTask()
    {
        QuestManager.Instance.CompleteTask(questName, taskDescription);
    }
}
