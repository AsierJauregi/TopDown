using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public enum EnemyAIDifficulty { Easy, Medium, Hard}
public enum EnemyState { Attack, Waiting, EndTurn}
public class EnemyManager : MonoBehaviour
{
    [SerializeField] private EnemyAIDifficulty enemyAIDifficulty;
    [SerializeField] private EnemyState actualEnemyState;
    [SerializeField] private List<AttackSO> attackMoves;

    [SerializeField] private float minManaThreshold;

    [Header("Medium Difficulty - Settings")]
    [SerializeField] private float mediumHealMoveProbability;
    [SerializeField] private float mediumBuffMoveProbability;


    private GameObject gameManager;
    private GameObject player;

    private StatisticsScript playerStatistics;
    private StatisticsScript enemyStatistics;

    private bool isThinking = false;
    private bool attackDone = false;

    public EnemyAIDifficulty EnemyAIDifficulty { get => enemyAIDifficulty; set => enemyAIDifficulty = value; }

    // Start is called before the first frame update
    void OnEnable()
    {
        GetComponent<StatisticsScript>().RecoverManaPerRound(TurnManager.manaRecovery);
        gameManager = gameManager = GameObject.Find("CombatManager");
        player = gameManager.GetComponent<TurnManager>().GetPlayer();
        actualEnemyState = EnemyState.Waiting;
        playerStatistics = player.GetComponent<StatisticsScript>();
        enemyStatistics = GetComponent<StatisticsScript>();
    }
    // Update is called once per frame
    void Update()
    {
        switch (actualEnemyState)
        {
            case EnemyState.Attack:
                AttackSO move = ChooseAction();
                AttackBehaviour(move);
                break;
            case EnemyState.Waiting:
                WaitingBehaviour();
                break;
            case EnemyState.EndTurn:
                EndTurnBehaviour();
                break;
        }
    }

    private void EndTurnBehaviour()
    {
        if (attackDone) return;
        StartCoroutine(EnemyAttackDone());
    }

    IEnumerator EnemyAttackDone()
    {
        attackDone = true;
        yield return new WaitForSeconds(2f);
        TurnManager.currentInformation = "···TURNO DEL JUGADOR···";
        gameManager.GetComponent<TurnManager>().actualCombatState = CombatState.Player;
        yield return new WaitForSeconds(1f);
        attackDone = false;
    }

    private void WaitingBehaviour()
    {
        if (isThinking) return;

        StartCoroutine(EnemyThinking());
    }

    IEnumerator EnemyThinking()
    {
        isThinking = true;
        yield return new WaitForSeconds(1.5f);
        TurnManager.currentInformation = "El enemigo esta escogiendo movimiento...";
        yield return new WaitForSeconds(2f);
        isThinking = false;
        actualEnemyState = EnemyState.Attack;
    }

    private void AttackBehaviour(AttackSO move)
    {
        if (move == null)
        {
            TurnManager.currentInformation = $"{enemyStatistics.NamePlayer} ha saltado su turno.";
            actualEnemyState = EnemyState.EndTurn;
            return;
        }

        if (Random.Range(0, 100) < move.accuracy)
        {
            TurnManager.currentInformation = $"{enemyStatistics.NamePlayer} ha usado {move.nameAttack}.";
            if (move.baseDamage != 0)
            {
                playerStatistics.HasDamageTheOpponent(move.baseDamage + enemyStatistics.AditionalDamage);
            }

            if (move.healAmount != 0)
            {
                enemyStatistics.IncreaseHealth(move.healAmount);
            }

            if (move.damageIncrease != 0)
            {
                enemyStatistics.IncreaseAditionalDamage(move.damageIncrease);
            }
        }
        else
        {
            TurnManager.currentInformation = $"{enemyStatistics.NamePlayer} ha usado {move.nameAttack}...pero ha fallado!";
        }
        GetComponent<StatisticsScript>().ReduceManaOfPlayer(move.manaCost);
        actualEnemyState = EnemyState.EndTurn;
    }

    private AttackSO ChooseAction()
    {
        switch (enemyAIDifficulty)
        {
            case EnemyAIDifficulty.Easy:
                return ChooseRandomAction();
            case EnemyAIDifficulty.Medium:
                return ChooseBalancedAction();
            case EnemyAIDifficulty.Hard:
                return ChooseSmartAction();
            default:
                return null;
        }
    }
    private AttackSO ChooseRandomAction()
    {
        var validMoves = attackMoves.Where(a => a.manaCost <= enemyStatistics.RemainingManaAmount).ToList();
        if (validMoves.Count == 0 || (enemyStatistics.RemainingManaAmount < minManaThreshold && Random.value < 0.5f))
        {
            return null;
        }
        return attackMoves[Random.Range(0, attackMoves.Count)];
    }

    private AttackSO ChooseBalancedAction()
    {
        if (enemyStatistics.RemainingManaAmount < minManaThreshold && Random.value < 0.3f) return null; // 30% de probabilidad de pasar turno para ahorrar maná

        if ((enemyStatistics.RemainingHealthValue < enemyStatistics.MaxHealthValue * 0.3f) && (Random.value < mediumHealMoveProbability)) // Si la vida está baja, curarse
        {
            var healMove = attackMoves.FirstOrDefault(a => a.healAmount > 0 && a.manaCost <= enemyStatistics.RemainingManaAmount);
            if (healMove != null) return healMove;
        }

        if (enemyStatistics.AditionalDamage < 20 && (Random.value < mediumBuffMoveProbability)) // Si aún no se ha acumulado suficiente buff, aumentar daño
        {
            var buffMove = attackMoves.FirstOrDefault(a => a.damageIncrease > 0 && a.manaCost <= enemyStatistics.RemainingManaAmount);
            if (buffMove != null) return buffMove;
        }

        return attackMoves.FirstOrDefault(a => a.baseDamage > 0 && a.manaCost <= enemyStatistics.RemainingManaAmount); // Atacar si no hay mejor opción
    }

    private AttackSO ChooseSmartAction()
    {
        // 1) Si no tiene maná para ningún ataque, salta turno
        var validMoves = attackMoves.Where(a => a.manaCost <= enemyStatistics.RemainingManaAmount).ToList();
        if (validMoves.Count == 0)
            return null;
       
        // 2) Si con el daño puede matar al rival, atacar
        var lethalMove = validMoves
            .Where(a => a.baseDamage > 0)
            .OrderByDescending(a => a.baseDamage + enemyStatistics.AditionalDamage)
            .FirstOrDefault(a => a.baseDamage + enemyStatistics.AditionalDamage >= playerStatistics.RemainingHealthValue);
        if (lethalMove != null) return lethalMove;
       
        // 3) Si algún ataque deja al rival con menos del 20% de vida, atacarlo si tiene mana
        var finishingMove = validMoves
            .Where(a => a.baseDamage > 0)
            .OrderByDescending(a => a.baseDamage + enemyStatistics.AditionalDamage)
            .FirstOrDefault(a => playerStatistics.RemainingHealthValue - (a.baseDamage + enemyStatistics.AditionalDamage) < playerStatistics.MaxHealthValue * 0.2f);
       
        // 4) Si la vida de la IA es baja, considerar curarse
        var healMove = validMoves
            .Where(a => a.healAmount > 0)
            .OrderByDescending(a => a.healAmount)
            .FirstOrDefault(a => (enemyStatistics.MaxHealthValue - enemyStatistics.RemainingHealthValue) > 20);

        // **Nueva Lógica: Decidir si saltar turno**
        if (enemyStatistics.RemainingManaAmount < minManaThreshold && Random.value < 0.2f) return null;

        if (finishingMove != null && healMove != null)
        {
            // Si la IA tiene menos del 15% de vida, prioriza curarse
            if (enemyStatistics.RemainingHealthValue < enemyStatistics.MaxHealthValue * 0.15f) 
                return healMove;

            // Si la IA está por debajo del 30% de vida, pero el ataque casi asegura la victoria, prioriza el ataque
            if (playerStatistics.RemainingHealthValue - (finishingMove.baseDamage + enemyStatistics.AditionalDamage) < playerStatistics.MaxHealthValue * 0.2f)
                return finishingMove;

            // En cualquier otra situación, si la vida de la IA es menor al 30%, prefiere curarse
            if (enemyStatistics.RemainingHealthValue < enemyStatistics.MaxHealthValue * 0.3f)
                return healMove;

            // Si la IA no está en peligro extremo y puede atacar con un buen ataque, lo hará
            return finishingMove;
        }

        // Si solo puede curarse, se cura
        if (healMove != null) return healMove;

        // Si solo puede atacar al rival y dejarlo con <20% de vida, lo hace
        if (finishingMove != null) return finishingMove;

        var attackMove = validMoves
            .Where(a => a.baseDamage > 0)
            .OrderByDescending(a => a.baseDamage + enemyStatistics.AditionalDamage)
            .FirstOrDefault(a => Random.value < 0.4f);
        if (attackMove != null) return attackMove;

        // 5) Si no se cumplen las anteriores, subir el daño permanente
        var buffMove = validMoves
            .Where(a => a.damageIncrease > 0)
            .OrderByDescending(a => a.damageIncrease)
            .FirstOrDefault();
        if (buffMove != null) return buffMove;

        return null; // Si no hay movimientos válidos, pasa turno
    }

    
}
