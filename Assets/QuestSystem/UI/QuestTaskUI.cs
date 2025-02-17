using UnityEngine;
using UnityEngine.UI;

namespace QuestSystem.UI
{
    public class QuestTaskUI : MonoBehaviour
    {
        public Text questTaskDescription;

        public void UpdateData(QuestTask questTask)
        {
            questTaskDescription.text = questTask.description;
            questTaskDescription.color = questTask.state switch
            {
                QuestTaskState.Incomplete => Color.red,
                QuestTaskState.Complete => Color.green,
                _ => questTaskDescription.color
            };
        }
    }
}
