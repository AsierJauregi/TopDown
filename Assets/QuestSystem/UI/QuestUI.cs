using UnityEngine;
using UnityEngine.UI;

namespace QuestSystem.UI
{
    public class QuestUI : MonoBehaviour
    {
        public Text questName;
        public Text questDescription;
        public Text questState;

        public Transform questTasksContainer;

        public void UpdateData(Quest quest)
        {
            questName.text = quest.questName;
            questDescription.text = quest.description;
            questState.text = quest.state.ToString();
            
            questState.color = quest.state switch
            {
                QuestState.Active => Color.white,
                QuestState.Completed => Color.green,
                _ => questState.color
            };
        }
    }
}