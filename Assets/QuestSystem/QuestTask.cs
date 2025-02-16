using System;

namespace QuestSystem
{
    public enum QuestTaskState { Incomplete, Complete }
    
    [Serializable]
    public class QuestTask
    {
        public string description;
        public QuestTaskState state = QuestTaskState.Incomplete;
        
        public event Action<QuestTask> OnQuestTaskCompleted;

        public void Complete()
        {
            if (state == QuestTaskState.Complete)
            {
                return;
            }

            state = QuestTaskState.Complete;
            OnQuestTaskCompleted?.Invoke(this);
        }
    }
}
