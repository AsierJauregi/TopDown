namespace QuestSystem
{
    public interface IQuestListener
    {
        void OnQuestStarted(Quest quest);
        void OnQuestCompleted(Quest quest);
        void OnQuestTaskCompleted(QuestTask task);
    }
}
