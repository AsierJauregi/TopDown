using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ParaBorrar
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private GameManagerSO gameManagerSO;
        private float inputH;
        private float inputV;
        private Vector3 puntoDestino;
        private Vector3 ultimoInput;
        private Vector3 puntoInteraccion;
        private Collider2D colliderDelante;
        private Animator anim;

        [SerializeField] private float velocidadMovimiento;
        [SerializeField] private float radioInteraccion;
        [SerializeField] private LayerMask queEsColisionable;
        private bool isMoving;

        public Vector3 PuntoDestino { get => puntoDestino; set => puntoDestino = value; }

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
            MovimientoYAnimaciones();
        }

        private void MovimientoYAnimaciones()
        {
            //Ejecuto el moviemiento si estoy en una casilla y solo si hay input.
            if (!isMoving && (inputV != 0 || inputH != 0))
            {
                anim.SetBool("IsMoving", true);
                anim.SetFloat("InputH", inputH);
                anim.SetFloat("InputV", inputV);

                //Actualizo cual fue mi primer input, cual va a ser el puntoDestino y cual el puntoInteraccion
                ultimoInput = new Vector3(inputH, inputV, 0);
                puntoDestino = transform.position + ultimoInput;
                puntoInteraccion = puntoDestino;

                colliderDelante = LanzarCheck();
                if (colliderDelante == false)
                {
                    StartCoroutine(MoverPersonaje());
                }
            }
            else if (inputV == 0 && inputH == 0)
            {
                anim.SetBool("IsMoving", false);
            }
        }

        private void LecturaInputs()
        {
            if (inputV == 0)
            {
                inputH = Input.GetAxisRaw("Horizontal");
            }

            if (inputH == 0)
            {
                inputV = Input.GetAxisRaw("Vertical");
            }
        }

        IEnumerator MoverPersonaje()
        {
            isMoving = true;

            while (transform.position != puntoDestino)
            {
                transform.position = Vector3.MoveTowards(transform.position, puntoDestino, velocidadMovimiento * Time.deltaTime);
                yield return null;
            }

            puntoInteraccion = transform.position + ultimoInput;
            isMoving = false;
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
}

