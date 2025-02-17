using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GrassBehaviour : MonoBehaviour
{
    [SerializeField] private GameManagerSO gameManager;

    [SerializeField] private GameObject player;

    [SerializeField] private int nextSceneIndex;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Player player))
        {
            float chance = Random.Range(0f, 1f);
            if (chance < 0.01f)
            {
                TurnManager.previusPosition = player.PuntoDestino;
                SceneManager.LoadScene(nextSceneIndex);
            }
        }
    }
}
