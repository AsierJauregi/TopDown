using UnityEngine;

namespace QuestSystem
{
    [RequireComponent(typeof(PolygonCollider2D))]
    public class ExploreAreaTrigger : MonoBehaviour, IQuestTaskCompleter, IQuestListener
    {
        public string questName = "Explorar B"; 
        public string taskDescription = "Ve a B";

        private void Start()
        {
            QuestManager.Instance.RegisterListener(this);
        }

        private void OnDisable()
        {
            QuestManager.Instance.UnregisterListener(this);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                CompleteTask();
            }
        }

        public void CompleteTask()
        {
            QuestManager.Instance.CompleteTask(questName, taskDescription);
        }

        public void OnQuestStarted(Quest quest) { }

        public void OnQuestCompleted(Quest quest) { }

        public void OnQuestTaskCompleted(QuestTask task)
        {
            if (task.description == taskDescription)
            {
                Destroy(gameObject, 1f);
            }
        }
    }
}
