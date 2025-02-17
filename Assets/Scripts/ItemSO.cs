using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ScriptableObjects/Item")]
public class ItemSO : ScriptableObject
{
    public float damage;
    public string nombre;
    public int nivelNecesario;
    public Sprite icono;
    
}
