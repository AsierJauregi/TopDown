using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBehaviour : MonoBehaviour
{
    [SerializeField] private GameManagerSO gameManagerSO;

    [SerializeField] private int nextSceneIndex;

    [SerializeField] private Vector3 nextScenePosition;

    [SerializeField] private Vector2 nextSceneOrientation;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Player player))
        {
            gameManagerSO.LoadNewScene(nextScenePosition, nextSceneOrientation, nextSceneIndex);
        }
    }
}
