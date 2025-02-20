using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour, Interactuable
{
    [SerializeField] private ItemSO misDatos;
    [SerializeField] private GameManager gameManager;

    public ItemSO MisDatos { get => misDatos; }

    public void Interactuar()
    {
        gameManager.Inventario.NuevoItem(misDatos);
        Destroy(this.gameObject);
    }



}
