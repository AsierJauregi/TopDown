using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrulla_NPC : MonoBehaviour
{
    [SerializeField] private float velocidadMovimiento;
    [SerializeField] private float tiempoEntreEsperasMinimo;
    [SerializeField] private float tiempoEntreEsperasMaximo;
    [SerializeField] private float distanciaMaxima;

    [Header("Detecciones")]
    [SerializeField] private float radioDeteccion;
    [SerializeField] private LayerMask queEsObstaculo;

    private Vector3 posicionObjetivo;
    private Vector3 posicionInicial;

    private bool noEstaHablando = true;

    public bool NoEstaHablando { get => noEstaHablando; set => noEstaHablando = value; }
    public bool YaHaHablado { get => yaHaHablado; set => yaHaHablado = value; }

    private bool yaHaHablado = false;

    private bool vaAndar = true;

    private void Awake()
    {
        posicionInicial = transform.position;
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(IrHaciaDestinoYEsperar());
    }

    // Update is called once per frame
    void Update()
    {
        if (noEstaHablando && yaHaHablado && vaAndar)
        {
            vaAndar = false;
            StartCoroutine(IrHaciaDestinoYEsperar());
        }
    }


    private IEnumerator IrHaciaDestinoYEsperar()
    {
        while (noEstaHablando) //Por siempre.
        {
            CalcularNuevoDestino();
            while (transform.position != posicionObjetivo) //Va al ritmo de los frames pero corta bajo la condicion establecida
            {
                transform.position = Vector3.MoveTowards(transform.position, posicionObjetivo, velocidadMovimiento * Time.deltaTime);
                yield return null;
            }
            yield return new WaitForSeconds(Random.Range(tiempoEntreEsperasMinimo, tiempoEntreEsperasMaximo));
        }
        vaAndar = true;
    }

    private void CalcularNuevoDestino()
    {
        bool tileValido = false;
        int intentos = 0;
        while (!tileValido && intentos <15)
        {
            int probabilidad = Random.Range(0, 4);


            if (probabilidad == 0) //izq
            {
                posicionObjetivo = transform.position + Vector3.left;
            }
            else if (probabilidad == 1)//der
            {
                posicionObjetivo = transform.position + Vector3.right;
            }
            else if (probabilidad == 2)//arriba
            {
                posicionObjetivo = transform.position + Vector3.up;
            }
            else //abajo
            {
                posicionObjetivo = transform.position + Vector3.down;
            }



            tileValido = TileLibreYDentroDeDistancia();
            intentos++;
        }

    }


    private bool TileLibreYDentroDeDistancia()
    {
        if (Vector3.Distance(posicionInicial, posicionObjetivo) > distanciaMaxima)
        {
            return false; // no esta libre
        }
        else //El tile esta dentro de la distancia max
        {
            return !Physics2D.OverlapCircle(posicionObjetivo, radioDeteccion, queEsObstaculo);
        }
    }


}
