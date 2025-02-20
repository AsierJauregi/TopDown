using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


[CreateAssetMenu(fileName = "Scriptable Objects/GameManager")]
public class GameManagerSO:ScriptableObject
{
    [NonSerialized]
    private Vector3 newPosition = new Vector3(0.5f, 0.5f, 0);

    [NonSerialized]
    private Vector2 newOrientation = new Vector2(0, -1);

    public Vector2 NewOrientation { get => newOrientation; set => newOrientation = value; }
    public Vector3 NewPosition { get => newPosition; set => newPosition = value; }

    private Player player;
    private SistemaInventario inventario;
    private void OnEnable() // llamadas por EVENTO
    {
        SceneManager.sceneLoaded += NuevaEscenaCargada;
    }

    private void NuevaEscenaCargada(Scene arg0, LoadSceneMode arg1)
    {
        player = FindObjectOfType<Player>();
        inventario = FindObjectOfType<SistemaInventario>();
    }

    public void LoadNewScene(Vector3 newPosition, Vector2 newOrientation, int newSceneIndex)
    {
        this.inventario = inventario.GetInstance();
        this.newPosition = newPosition;
        this.newOrientation = newOrientation;
        SceneManager.LoadScene(newSceneIndex);
    }

    public void CambiarEstadoPlayer(bool estado)
    {
        player.interactuando = !estado;
    }
}
