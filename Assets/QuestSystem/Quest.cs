using System;
using System.Collections.Generic;

namespace QuestSystem
{
    public enum QuestState { Inactive, Active, Completed }

    [Serializable]
    public class Quest
    {
        public string questName;
        public string description;
        public QuestState state = QuestState.Inactive;
        public List<QuestTask> questTasks = new();

        public event Action<Quest> OnQuestCompleted;

        public void CheckCompletion()
        {
            if (!questTasks.TrueForAll(t => t.state == QuestTaskState.Complete))
            {
                return;
            }

            state = QuestState.Completed;
            OnQuestCompleted?.Invoke(this);
        }
    }
}
