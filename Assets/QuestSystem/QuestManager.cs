using System;
using System.Collections.Generic;
using UnityEngine;

namespace QuestSystem
{
    public class QuestManager : MonoBehaviour
    {
        // JSON models
        [Serializable]
        private class QuestList
        {
            public List<Quest> quests;
        }

        public static QuestManager Instance;

        public List<Quest> quests = new();

        private readonly List<IQuestListener> _listeners = new();

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
        private void Start()
        {
            LoadQuestsFromJson();
        }

        private void LoadQuestsFromJson()
        {
            var jsonFile = Resources.Load<TextAsset>("quests");
            if (!jsonFile)
            {
                Debug.LogError("No se ha cargado el archivo quests JSON.");
                return;
            }

            var wrapper = JsonUtility.FromJson<QuestList>(jsonFile.text);
            if (wrapper == null || wrapper.quests == null)
            {
                Debug.LogError("Formato JSON incorrecto.");
                return;
            }

            foreach (var quest in wrapper.quests)
            {
                quest.questTasks ??= new List<QuestTask>();
            }

            quests = wrapper.quests;
        }

        public void RegisterListener(IQuestListener listener)
        {
            if (!_listeners.Contains(listener))
            {
                _listeners.Add(listener);
            }
        }

        public void UnregisterListener(IQuestListener listener)
        {
            if (_listeners.Contains(listener))
            {
                _listeners.Remove(listener);
            }
        }
        
        public void StartQuest(string questName)
        {
            var quest = quests.Find(q => q.questName == questName);
            if (quest == null)
            {
                return;
            }

            if(quest.state is QuestState.Active or QuestState.Completed)
            {
                return;
            }

            quest.state = QuestState.Active;

            foreach (var task in quest.questTasks)
            {
                task.OnQuestTaskCompleted += (questTask) => quest.CheckCompletion();
            }

            foreach (var listener in _listeners)
            {
                listener.OnQuestStarted(quest);
            }
        }

        public void CompleteTask(string questName, string taskDescription)
        {
            var quest = quests.Find(q => q.questName == questName);
            if (quest == null)
            {
                return;
            }

            if (quest.state != QuestState.Active)
            {
                return;
            }

            var task = quest.questTasks.Find(t => t.description == taskDescription);
            if (task == null)
            {
                return;
            }

            if (task.state == QuestTaskState.Complete)
            {
                return;
            }

            task.Complete();

            foreach (var listener in _listeners)
            {
                listener.OnQuestTaskCompleted(task);
            }

            quest.CheckCompletion();

            if (quest.state != QuestState.Completed)
            {
                return;
            }

            foreach (var listener in _listeners)
            {
                listener.OnQuestCompleted(quest);
            }
        }
    }
}
