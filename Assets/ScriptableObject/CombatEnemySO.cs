using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CombatEnemy")]
public class CombatEnemySO : ScriptableObject
{
    public string nameEnemy;
    public Sprite sprite;
    public float health;
    public float mana;
    public float aditionalDamage;
}
