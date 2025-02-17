using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HoverButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject player;
    [SerializeField] private AttackSO attackSO;
    [SerializeField] private TMP_Text texto;
    [SerializeField] private string mensaje;
    private void Update()
    {
        
        mensaje = attackSO.nameAttack + "\n\n";

        if (attackSO.baseDamage != 0)
        {
            mensaje += (attackSO.baseDamage + player.GetComponent<StatisticsScript>().AditionalDamage) + " Att / ";
        }
        if (attackSO.healAmount != 0)
        {
            mensaje += attackSO.healAmount + " Hp / ";
        }
        if (attackSO.damageIncrease != 0)
        {
            if (attackSO.damageIncrease < 0)
            {
                mensaje += attackSO.damageIncrease + " DmgDown / ";
            }
            else
            {
                mensaje += attackSO.damageIncrease + " DmgUp / ";
            }
        }
        if (attackSO.manaCost != 0)
        {
            mensaje += attackSO.manaCost + " Mana / ";
        }
        if (attackSO.accuracy != 0)
        {
            mensaje += attackSO.accuracy + "% Accuracy.";
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (TryGetComponent(out Button b))
        {
            if (b.interactable)
            {
                texto.color = Color.blue;
            }
            else
            {
                texto.color = Color.yellow;
            }
        }
        texto.text = mensaje;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        texto.text = "";
    }
}
