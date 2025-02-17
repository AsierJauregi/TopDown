using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform player;   
    [SerializeField] private float moveDistanceH; 
    [SerializeField] private float moveDistanceV; 
    private Vector2 screenSize;

    void Start()
    {
        // Obtener la mitad del tamaño de la cámara en unidades del mundo
        Camera cam = Camera.main;
        screenSize = new Vector2(cam.orthographicSize * cam.aspect, cam.orthographicSize);

        moveDistanceH = GetComponent<BoxCollider2D>().size.x;
        moveDistanceV = GetComponent<BoxCollider2D>().size.y;
    }

    void Update()
    {
        CheckCameraBounds();
    }

    private void CheckCameraBounds()
    {
        Vector3 playerPos = player.position;
        Vector3 cameraPos = transform.position;

        if (playerPos.x > cameraPos.x + screenSize.x)
        {
            MoveCamera(Vector3.right, moveDistanceH);
        }
        else if (playerPos.x < cameraPos.x - screenSize.x)
        {
            MoveCamera(Vector3.left, moveDistanceH);
        }
        else if (playerPos.y > cameraPos.y + screenSize.y)
        {
            MoveCamera(Vector3.up, moveDistanceV);
        }
        else if (playerPos.y < cameraPos.y - screenSize.y)
        {
            MoveCamera(Vector3.down, moveDistanceV);
        }
    }

    private void MoveCamera(Vector3 direction, float moveDistance)
    {
        transform.position += direction * moveDistance;
    }
}
