using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SistemaInventario : MonoBehaviour
{
    [SerializeField] private GameObject marcoInventario;
    [SerializeField] private Button[] botones;
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

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.I))
        {
            marcoInventario.SetActive(!marcoInventario.activeSelf);
        }
    }
}
