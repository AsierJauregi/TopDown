using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "ScriptableObjects/GameManager")]

public class GameManager : ScriptableObject
{
    private Player player;
    private SistemaInventario inventario;

    public SistemaInventario Inventario { get => inventario; }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += NuevaEscenaCargada;
    }

    private void NuevaEscenaCargada(Scene arg0, LoadSceneMode arg1)
    {
        player = FindObjectOfType<Player>();
        inventario = FindObjectOfType<SistemaInventario>();
    }

}
