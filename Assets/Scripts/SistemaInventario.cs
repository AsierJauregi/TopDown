using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SistemaInventario : MonoBehaviour
{
    [SerializeField] private GameObject marcoInventario;
    [SerializeField] private Button[] botones;
    [SerializeField] private int itemsDisponibles = 0;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < botones.Length; i++) 
        {
            int indiceBoton = i;
            botones[i].onClick.AddListener(() => BotonClickado(indiceBoton));
        }
    }

    private void BotonClickado(int indiceBoton)
    {
        Debug.Log("Botón Clickado " + indiceBoton);
    }

    public void NuevoItem(ItemSO datosItem)
    {
        //Activar boton del inventario al tener nuevo item
        botones[itemsDisponibles].gameObject.SetActive(true);
        //Meter al botón los datos del item
        botones[itemsDisponibles].GetComponent<Image>().sprite = datosItem.icono;
        botones[itemsDisponibles].gameObject.GetComponentInChildren<TextMeshProUGUI>().SetText(datosItem.nombre);
        botones[itemsDisponibles].gameObject.GetComponentInChildren<TextMeshProUGUI>().fontSize = 14;
        itemsDisponibles++;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.I))
        {
            marcoInventario.SetActive(!marcoInventario.activeSelf);
        }
    }
}
