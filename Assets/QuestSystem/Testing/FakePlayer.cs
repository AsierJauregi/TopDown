using UnityEngine;

// TODO: REMOVE IT! For testing.
namespace QuestSystem.Testing
{
    public class FakePlayer : MonoBehaviour
    {
        public string questName = "Hablar con X";
        public string taskDescription1 = "Ve al pueblo"; 
        public string taskDescription2 = "Habla con X";

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                StartQuest();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                CompleteTask(taskDescription1);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                CompleteTask(taskDescription2);
            }
        }

        private void StartQuest()
        {
            QuestManager.Instance.StartQuest(questName);
        }

        private void CompleteTask(string taskDescription)
        {
            QuestManager.Instance.CompleteTask(questName, taskDescription);
        }
    }
}
