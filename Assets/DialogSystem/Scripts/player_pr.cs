using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_pr : MonoBehaviour
{

    private float inputH;
    private float inputV;
    private bool moviendo;
    private Vector3 puntoDestino;
    private Vector3 puntoInteraccion;
    private Vector3 ultimoInput;
    private Collider2D colliderDelante;

    [SerializeField] private float velocidadMovimiento;
    [SerializeField] private float radioInteraccion;
    [SerializeField] private LayerMask queEsColisionable;

    public bool interactuando;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        LecturaInputs();

        if (!moviendo && (inputH != 0 || inputV != 0))
        {
            ultimoInput = new Vector3(inputH, inputV, 0);
            puntoDestino = transform.position + ultimoInput;
            puntoInteraccion = puntoDestino;

            colliderDelante = LanzarCheck();

            if (!colliderDelante)
            {
                StartCoroutine(Mover());
            }

        }
    }

    private void LecturaInputs()
    {
        if (inputV == 0) inputH = Input.GetAxisRaw("Horizontal");
        if (inputH == 0) inputV = Input.GetAxisRaw("Vertical");
        if (Input.GetKeyDown(KeyCode.E)) LanzarInteraccion();

    }

    private void LanzarInteraccion()
    {
        colliderDelante = LanzarCheck();
        if (colliderDelante)
        {
            if (colliderDelante.gameObject.CompareTag("NPC"))
            {
                NPC npcScript = colliderDelante.gameObject.GetComponent<NPC>();
                npcScript.Interactuar();
            }

        }
    }

    IEnumerator Mover()
    {
        moviendo = true;
        while (transform.position != puntoDestino)
        {
            transform.position = Vector3.MoveTowards(transform.position, puntoDestino, velocidadMovimiento * Time.deltaTime);
            yield return null;
        }
        //Refrescar punto de interacción al terminar de movernos
        puntoInteraccion = transform.position + ultimoInput;
        moviendo = false;
    }

    private Collider2D LanzarCheck()
    {
        return Physics2D.OverlapCircle(puntoInteraccion, radioInteraccion);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(puntoInteraccion, radioInteraccion);
    }
}
