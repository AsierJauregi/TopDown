using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PlayerState { Attack1, Attack2, Attack3, Attack4, Waiting, EndTurn}
public class PlayerManager : MonoBehaviour
{
    public PlayerState actualPlayerState;
    [SerializeField] private GameObject attackPanel;

    [SerializeField] private List<Button> listButtons;
    [SerializeField] private Button botonSaltar;
    [SerializeField] private List<AttackSO> listAttacks;

    private GameObject gameManager;
    private GameObject enemy;
    private bool attackDone = false;
    // Start is called before the first frame update
    private void OnEnable()
    {
        GetComponent<StatisticsScript>().RecoverManaPerRound(TurnManager.manaRecovery);
        gameManager = GameObject.Find("CombatManager");
        enemy = gameManager.GetComponent<TurnManager>().GetEnemy();
        actualPlayerState = PlayerState.Waiting;
        ActivateAttackButtons();
        botonSaltar.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        switch (actualPlayerState)
        {
            case PlayerState.Attack1:
                Attack1Behaviour();
                break;
            case PlayerState.Attack2:
                Attack2Behaviour();
                break;
            case PlayerState.Attack3:
                Attack3Behaviour();
                break;
            case PlayerState.Attack4:
                Attack4Behaviour();
                break;
            case PlayerState.Waiting:
                WaitingBehaviour();
                break;
            case PlayerState.EndTurn:
                EndTurnBehaviour();
                break;
        }
    }

    private void ActivateAttackButtons()
    {
        for (int i = 0; i < listButtons.Count; i++)
        {
            if (GetComponent<StatisticsScript>().RemainingManaAmount < listAttacks[i].manaCost)
            {
                listButtons[i].interactable = false;
            }
            else
            {
                listButtons[i].interactable = true;
            }
        }
    }

    private void Attack1Behaviour()
    {
        if (UnityEngine.Random.Range(0, 100) < listAttacks[0].accuracy)
        {
            TurnManager.currentInformation = $"Has usado {listAttacks[0].nameAttack}.";
            enemy.GetComponent<StatisticsScript>().HasDamageTheOpponent(listAttacks[0].baseDamage + GetComponent<StatisticsScript>().AditionalDamage);
        }
        else
        {
            TurnManager.currentInformation = $"Has usado {listAttacks[0].nameAttack}...pero has fallado!";
        }
        GetComponent<StatisticsScript>().ReduceManaOfPlayer(listAttacks[0].manaCost);
        actualPlayerState = PlayerState.EndTurn;

    }
    private void Attack2Behaviour()
    {
        if (UnityEngine.Random.Range(0, 100) < listAttacks[1].accuracy)
        {
            TurnManager.currentInformation = $"Has usado {listAttacks[1].nameAttack}.";
            enemy.GetComponent<StatisticsScript>().HasDamageTheOpponent(listAttacks[1].baseDamage + GetComponent<StatisticsScript>().AditionalDamage);
        }
        else
        {
            TurnManager.currentInformation = $"Has usado {listAttacks[1].nameAttack}...pero has fallado!";
        }
        GetComponent<StatisticsScript>().ReduceManaOfPlayer(listAttacks[1].manaCost);
        actualPlayerState = PlayerState.EndTurn;
    }
    private void Attack3Behaviour()
    {
        if (UnityEngine.Random.Range(0, 100) < listAttacks[2].accuracy)
        {
            TurnManager.currentInformation = $"Has usado {listAttacks[2].nameAttack}.";
            GetComponent<StatisticsScript>().IncreaseAditionalDamage(listAttacks[2].damageIncrease);
            GetComponent<StatisticsScript>().IncreaseHealth(listAttacks[2].healAmount);
        }
        else
        {
            TurnManager.currentInformation = $"Has usado {listAttacks[2].nameAttack}...pero has fallado!";
        }
        GetComponent<StatisticsScript>().ReduceManaOfPlayer(listAttacks[2].manaCost);
        actualPlayerState = PlayerState.EndTurn;
    }
    private void Attack4Behaviour()
    {
        if (UnityEngine.Random.Range(0, 100) < listAttacks[3].accuracy)
        {
            TurnManager.currentInformation = $"Has usado {listAttacks[3].nameAttack}.";
            GetComponent<StatisticsScript>().IncreaseAditionalDamage(listAttacks[3].damageIncrease);
            GetComponent<StatisticsScript>().IncreaseHealth(listAttacks[3].healAmount);
        }
        else
        {
            TurnManager.currentInformation = $"Has usado {listAttacks[3].nameAttack}...pero has fallado!";
        }
        GetComponent<StatisticsScript>().ReduceManaOfPlayer(listAttacks[3].manaCost);
        actualPlayerState = PlayerState.EndTurn;
    }

    private void WaitingBehaviour()
    {
        if (!attackPanel.activeSelf)
        {
            attackPanel.SetActive(true);
        }
    }

    private void EndTurnBehaviour()
    {
        if (attackDone)
        {
            return;
        }
        StartCoroutine(AttackDone());
    }

    IEnumerator AttackDone()
    {
        attackDone = true;
        for (int i = 0; i < listButtons.Count; i++)
        {
            listButtons[i].interactable = false;
        }
        botonSaltar.gameObject.SetActive(false);
        yield return new WaitForSeconds(2f);
        TurnManager.currentInformation = "···TURNO DE ENEMIGO···";
        gameManager.GetComponent<TurnManager>().actualCombatState = CombatState.Enemy;
        yield return new WaitForSeconds(1f);
        attackDone = false;
    }

    public void ChooseAttack(int index)
    {
        switch (index)
        {
            case 1:
                actualPlayerState = PlayerState.Attack1;
                break;
            case 2:
                actualPlayerState = PlayerState.Attack2;
                break;
            case 3:
                actualPlayerState = PlayerState.Attack3;
                break;
            case 4:
                actualPlayerState = PlayerState.Attack4;
                break;
        }
    }

    public void SaltarTurno()
    {
        TurnManager.currentInformation = "Has saltado el turno.";
        actualPlayerState = PlayerState.EndTurn;
    }
}
