using System.Collections;
using System.Collections.Generic;
using QuestSystem.UI;
using UnityEngine;
using UnityEngine.Serialization;

public class Player : MonoBehaviour
{
    [SerializeField] private GameManagerSO gameManagerSO;
    private float inputH;
    private float inputV;
    private bool moviendo;
    private Vector3 puntoDestino;
    private Vector3 puntoInteraccion;
    private Vector3 ultimoInput;
    private Collider2D colliderDelante;
    private Animator anim;

    [SerializeField] private float velocidadMovimiento;
    [SerializeField] private float radioInteraccion;
    [SerializeField] private LayerMask queEsColisionable;

    public bool interactuando;
    public Vector3 PuntoDestino { get => puntoDestino; set => puntoDestino = value; }

    [SerializeField]
    private QuestManagerUI questManagerUI;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        transform.position = gameManagerSO.NewPosition;
        anim.SetFloat("InputH", gameManagerSO.NewOrientation.x);
        anim.SetFloat("InputV", gameManagerSO.NewOrientation.y);
    }

    // Update is called once per frame
    void Update()
    {
        LecturaInputs();

        if (!moviendo && (inputH != 0 || inputV != 0))
        {
            anim.SetBool("IsMoving", true);
            anim.SetFloat("InputH", inputH);
            anim.SetFloat("InputV", inputV);

            ultimoInput = new Vector3(inputH, inputV, 0);
            puntoDestino = transform.position + ultimoInput;
            puntoInteraccion = puntoDestino;

            colliderDelante = LanzarCheck();

            if (!colliderDelante)
            {
                StartCoroutine(Mover());
            }
        }
        else if (inputV == 0 && inputH == 0)
        {
            anim.SetBool("IsMoving", false);
        }
    }

    private void LecturaInputs()
    {
        if (inputV == 0) inputH = Input.GetAxisRaw("Horizontal");
        if (inputH == 0) inputV = Input.GetAxisRaw("Vertical");
        if (Input.GetKeyDown(KeyCode.E)) LanzarInteraccion();
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ToggleQuestManagerUI();
        }
    }

    private void ToggleQuestManagerUI()
    {
        questManagerUI.Toggle();
    }

    private void LanzarInteraccion()
    {

        colliderDelante = LanzarCheck();
        if (colliderDelante)
        {
            if (colliderDelante.TryGetComponent(out Interactuable interactuable))
            {
                interactuable.Interactuar();
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
        //Refrescar punto de interacci�n al terminar de movernos
        puntoInteraccion = transform.position + ultimoInput;
        moviendo = false;
    }

    private Collider2D LanzarCheck()
    {
        return Physics2D.OverlapCircle(puntoInteraccion, radioInteraccion, queEsColisionable);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(puntoInteraccion, radioInteraccion);
    }
}
