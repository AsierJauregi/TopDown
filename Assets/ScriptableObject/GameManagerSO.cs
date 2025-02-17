using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(menuName = "GameManager")]
public class GameManagerSO : ScriptableObject
{
    [NonSerialized]
    private Vector3 newPosition = new Vector3(0.5f, 0.5f,0);

    [NonSerialized]
    private Vector2 newOrientation = new Vector2(0,-1);

    public Vector2 NewOrientation { get => newOrientation; set => newOrientation = value; }
    public Vector3 NewPosition { get => newPosition; set => newPosition = value; }

    public void LoadNewScene(Vector3 newPosition, Vector2 newOrientation, int newSceneIndex)
    {
        this.newPosition = newPosition;
        this.newOrientation = newOrientation;
        SceneManager.LoadScene(newSceneIndex);
    }
}
