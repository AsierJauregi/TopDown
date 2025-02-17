using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


[CreateAssetMenu(fileName = "Scriptable Objects/GameManager")]
public class GameManagerSO:ScriptableObject
{
    private player_pr player; // CUIDADO que es el de ejemplo !!!!
    private void OnEnable() // llamadas por EVENTO
    {
        SceneManager.sceneLoaded += NuevaEscenaCargada;
    }

    private void NuevaEscenaCargada(Scene arg0, LoadSceneMode arg1)
    {
        player = FindObjectOfType<player_pr>();
    }

    public void CambiarEstadoPlayer(bool estado)
    {
        player.interactuando = !estado;
    }
}
