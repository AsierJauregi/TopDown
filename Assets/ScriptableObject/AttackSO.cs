using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Attack")]
public class AttackSO : ScriptableObject
{
    public string nameAttack;
    public int baseDamage;
    public int healAmount;
    public int damageIncrease;
    public int manaCost;
    public int accuracy;
}
