using System;
using System.Collections;
using System.Collections.Generic;
using ParaBorrar;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SistemaInventario : MonoBehaviour
{
    [SerializeField] private GameObject marcoInventario;
    [SerializeField] private Button[] botones;
    [SerializeField] private int itemsDisponibles = 0;
    [SerializeField] private GameManagerSO gameManagerSO;
    private static SistemaInventario instancia;

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
        botones[itemsDisponibles].gameObject.GetComponentInChildren<TextMeshProUGUI>().fontSize = 24;
        itemsDisponibles++;
    }

    public bool IsInInventory(string nombreItem)
    {
        foreach (var item in botones)
        {
            if (item.gameObject.GetComponentInChildren<TextMeshProUGUI>().text == nombreItem) return true;
        }
        return false;
    }

    public SistemaInventario GetInstance()
    {
        instancia = this;
        return instancia;
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
