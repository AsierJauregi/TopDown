using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    private float inputH;
    private float inputV;
    private bool moviendo;
    private Vector3 puntoDestino;
    [SerializeField] private float velocidadMovimiento;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        inputH = Input.GetAxis("Horizontal");
        inputV = Input.GetAxis("Vertical");
        if(!moviendo && (inputH != 0 || inputV != 0)) StartCoroutine(Mover());
    }

    IEnumerator Mover()
    {
        moviendo = true;
        puntoDestino = transform.position + new Vector3(inputH, inputV, 0);

        while (transform.position != puntoDestino)
        {
            transform.position = Vector3.MoveTowards(transform.position, puntoDestino, velocidadMovimiento * Time.deltaTime);
            yield return null;
        }
        moviendo = false;
    }
}
