using System;
using System.Collections.Generic;
using UnityEngine;

namespace QuestSystem.UI
{
    public class QuestManagerUI : MonoBehaviour, IQuestListener
    {
        public GameObject questsUIPanel;

        public Transform questsContainer;
        public QuestUI questPrefab;

        private readonly Dictionary<string, QuestUI> _questUIElements = new();

        public QuestTaskUI questTaskPrefab;

        private readonly Dictionary<string, Dictionary<string, QuestTaskUI>> _questTaskUIElements = new();

        private void Start()
        {
            QuestManager.Instance?.RegisterListener(this);
            questsUIPanel.SetActive(false);

            // Esto es necesario para er cambio de escena, se pierde la información.
            GetQuests();
        }

        private void OnDisable()
        {
            QuestManager.Instance?.UnregisterListener(this);
        }

        // TODO: TEMPORARY SOLUTION
        private void GetQuests()
        {
            foreach (var quest in QuestManager.Instance.quests)
            {
                if (quest.state == QuestState.Inactive)
                {
                    continue;
                }

                AddQuestUI(quest);
            }
        }

        public void Toggle()
        {
            questsUIPanel.SetActive(!questsUIPanel.activeSelf);
        }

        public void OnQuestStarted(Quest quest)
        {
            questsUIPanel.SetActive(true);

            if (_questUIElements.ContainsKey(quest.questName))
            {
                return;
            }

            AddQuestUI(quest);
        }

        private void AddQuestUI(Quest quest)
        {
            var questUI = Instantiate(questPrefab, questsContainer);
            questUI.UpdateData(quest);
            _questUIElements[quest.questName] = questUI;
            _questTaskUIElements[quest.questName] = new Dictionary<string, QuestTaskUI>();

            foreach (var questTask in quest.questTasks)
            {
                var questTaskUI = Instantiate(questTaskPrefab, questUI.questTasksContainer);
                questTaskUI.UpdateData(questTask);
                _questTaskUIElements[quest.questName].Add(questTask.description, questTaskUI);
            }
        }

        public void OnQuestTaskCompleted(QuestTask questTask)
        {
            foreach (var quest in _questTaskUIElements)
            {
                if (!quest.Value.ContainsKey(questTask.description))
                {
                    continue;
                }

                quest.Value[questTask.description].UpdateData(questTask);
            }
        }

        public void OnQuestCompleted(Quest quest)
        {
            
            if (_questUIElements.TryGetValue(quest.questName, out var element))
            {
                element.UpdateData(quest);
            }
        }
    }
}