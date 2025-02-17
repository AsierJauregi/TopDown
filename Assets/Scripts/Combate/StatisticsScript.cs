using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatisticsScript : MonoBehaviour
{
    [SerializeField] private string namePlayer;
    [SerializeField] private float aditionalDamage;
    [SerializeField] private float remainingHealthValue;
    [SerializeField] private float maxHealthValue;
    [SerializeField] private float remainingManaAmount;
    [SerializeField] private float manaAmount;

    [Header("UI")]
    [SerializeField] private GameObject playerText;
    [SerializeField] private GameObject healthBar;
    [SerializeField] private GameObject manaBar;

    public float RemainingHealthValue { get => remainingHealthValue; set => remainingHealthValue = value; }
    public float MaxHealthValue { get => maxHealthValue; set => maxHealthValue = value; }
    public float RemainingManaAmount { get => remainingManaAmount; set => remainingManaAmount = value; }
    public float ManaAmount { get => manaAmount; set => manaAmount = value; }
    public float AditionalDamage { get => aditionalDamage; set => aditionalDamage = value; }
    public string NamePlayer { get => namePlayer; set => namePlayer = value; }

    // Start is called before the first frame update
    void Start()
    {
        playerText.GetComponent<TMP_Text>().text = namePlayer;
        Color colorTexto = Color.black;
        if (TryGetComponent(out EnemyManager enemyManager))
        {
            switch (enemyManager.EnemyAIDifficulty)
            {
                case EnemyAIDifficulty.Easy:
                    colorTexto = Color.green;
                    break;
                case EnemyAIDifficulty.Medium:
                    colorTexto = Color.yellow;
                    break;
                case EnemyAIDifficulty.Hard:
                    colorTexto = Color.red;
                    break;
            }
        }
        playerText.GetComponent<TMP_Text>().color = colorTexto;
        healthBar.transform.GetChild(0).GetComponent<Image>().fillAmount = remainingHealthValue / maxHealthValue;
        healthBar.transform.GetChild(1).GetComponent<TMP_Text>().text = remainingHealthValue + "/" + maxHealthValue;
        manaBar.transform.GetChild(0).GetComponent<Image>().fillAmount = remainingManaAmount / manaAmount;
        manaBar.transform.GetChild(1).GetComponent<TMP_Text>().text = remainingManaAmount + "/" + manaAmount;
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.transform.GetChild(0).GetComponent<Image>().fillAmount = remainingHealthValue / maxHealthValue;
        healthBar.transform.GetChild(1).GetComponent<TMP_Text>().text = remainingHealthValue + "/" + maxHealthValue;

        manaBar.transform.GetChild(0).GetComponent<Image>().fillAmount = remainingManaAmount / manaAmount;
        manaBar.transform.GetChild(1).GetComponent<TMP_Text>().text = remainingManaAmount + "/" + manaAmount;
        if (remainingHealthValue <= 0)
        {
            GameObject.Find("CombatManager").GetComponent<TurnManager>().actualCombatState = CombatState.GameOver;
            if (gameObject.name.Equals("Player"))
            {
                TurnManager.currentInformation = "Enemy wins the combat!";
                TurnManager.playerWins = false;
            }
            else
            {
                TurnManager.currentInformation = "You win the combat!";
                TurnManager.playerWins = true;
            }
        }
    }

    public void HasDamageTheOpponent(float amountDmg)
    {
        if ((remainingHealthValue-amountDmg)<=0)
        {
            remainingHealthValue = 0;
        }
        else
        {
            remainingHealthValue -= amountDmg;
        }
    }

    public void ReduceManaOfPlayer(float mana)
    {
        if ((remainingManaAmount-mana)<=0)
        {
            remainingManaAmount = 0;
        }
        else
        {
            remainingManaAmount -= mana;
        }
    }

    public void RecoverManaPerRound(float amount)
    {
        if ((remainingManaAmount + amount)>=manaAmount)
        {
            remainingManaAmount = manaAmount;
        }
        else
        {
            remainingManaAmount += amount;
        }
    }

    public void IncreaseAditionalDamage(float extra)
    {
        aditionalDamage += extra;
    }

    public void IncreaseHealth(float value)
    {
        if ((remainingHealthValue + value) > maxHealthValue)
        {
            remainingHealthValue = maxHealthValue;
        }
        else
        {
            remainingHealthValue += value;
        }
        
    }
}
