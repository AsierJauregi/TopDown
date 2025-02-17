using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public enum CombatState { Start, Player, Enemy, Waiting, GameOver}
public class TurnManager : MonoBehaviour
{
    public CombatState actualCombatState;
    [SerializeField] private GameManagerSO gM;
    [SerializeField] private CombatEnemySO[] combatEnemies;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject enemy;

    [SerializeField] private GameObject infoPanel;
    public static string currentInformation = "...";
    public static float manaRecovery = 10;

    public static bool playerWins;
    private bool hasEnded = false;
    public static Vector3 previusPosition;
    // Start is called before the first frame update
    void Start()
    {
        EnemyAIDifficulty[] valores = (EnemyAIDifficulty[]) System.Enum.GetValues(typeof(EnemyAIDifficulty));
        enemy.GetComponent<EnemyManager>().EnemyAIDifficulty = valores[Random.Range(0, valores.Length)];

        CombatEnemySO cEnemy = combatEnemies[Random.Range(0, combatEnemies.Length)];

        enemy.GetComponent<SpriteRenderer>().sprite = cEnemy.sprite;

        enemy.GetComponent<StatisticsScript>().NamePlayer = cEnemy.nameEnemy;
        enemy.GetComponent<StatisticsScript>().AditionalDamage = cEnemy.aditionalDamage;
        enemy.GetComponent<StatisticsScript>().RemainingHealthValue = cEnemy.health;
        enemy.GetComponent<StatisticsScript>().MaxHealthValue = cEnemy.health;
        enemy.GetComponent<StatisticsScript>().RemainingManaAmount = cEnemy.mana;
        enemy.GetComponent<StatisticsScript>().ManaAmount = cEnemy.mana;

    }

    // Update is called once per frame
    void Update()
    {
        infoPanel.transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text = currentInformation;
        switch (actualCombatState)
        {
            case CombatState.Start:
                break;
            case CombatState.Player:
                PlayerBehaviour();
                break;
            case CombatState.Enemy:
                EnemyBehaviour();
                break;
            case CombatState.Waiting:
                WaitingBehaviour();
                break;
            case CombatState.GameOver:
                GameOverBehaviour();
                break;
        }
    }

    private void PlayerBehaviour()
    {
        player.GetComponent<PlayerManager>().enabled = true;
        enemy.GetComponent<EnemyManager>().enabled = false;
    }

    private void EnemyBehaviour()
    {
        player.GetComponent<PlayerManager>().enabled = false;
        enemy.GetComponent<EnemyManager>().enabled = true;
    }

    private void WaitingBehaviour()
    {

    }

    private void GameOverBehaviour()
    {
        if (!hasEnded)
        {
            StartCoroutine(EndTheGame());
        }
    }

    public GameObject GetEnemy()
    {
        return enemy;
    }

    public GameObject GetPlayer()
    {
        return player;
    }

    IEnumerator EndTheGame()
    {
        hasEnded = true;
        player.GetComponent<PlayerManager>().enabled = false;
        enemy.GetComponent<EnemyManager>().enabled = false;
        if (playerWins)
        {
            enemy.GetComponent<SpriteRenderer>().color = Color.black;
        }
        else
        {
            player.GetComponent<SpriteRenderer>().color = Color.black;
        }
        yield return new WaitForSeconds(3f);
        gM.LoadNewScene(previusPosition,new Vector2(0,-1),0);
    }
}
