using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuGameplay : MonoBehaviour
{
    [SerializeField] private GameObject pausa;
    [SerializeField] private GameObject ajustes;
    [SerializeField] private GameObject guardar;
    [SerializeField] private GameObject inventario;
    [SerializeField] private GameObject ayuda;
    [SerializeField] private TextMeshProUGUI textoGuardar;

    private bool isPausa = false;
    private bool isAjustes = false;
    private bool isGuardar = false;
    private bool isInventario = false;
    private bool isAyuda = false;
    private bool isVolver = false;
    private bool isSalir = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    // Botón
    public void Pausa()
    {
        //activar pantalla pausa
        Time.timeScale = 0;
        pausa.SetActive(true);
        isPausa = true;
    }
    // Botón
    public void Ajustes()
    {
        //activar pantalla ajustes
        Time.timeScale = 0;
        ajustes.SetActive(true);
        isAjustes = true;

    }
    // Botón
    public void Volver()
    {
        //advertencia guardar antes de?
        //bool volver
        guardar.SetActive(true);
        isVolver= true;
        isGuardar = true;
        textoGuardar.text = "¿Desea Guardar antes de volver al inicio?";

    }
    // Botón
    public void Salir()
    {
        //advertencia pantalla guardar
        //bool salir
        guardar.SetActive(true);
        isSalir= true;
        isGuardar = true;
        textoGuardar.text = "¿Desea Guardar antes de salir?";
    }
    // Botón si
    public void Guardar()
    {
        //guardar partida 
        //if volver - menuprincipal
        //if salir - quit

        //Desactivar botones
        Debug.Log("Guardando");
        if (isVolver)
        {
            isVolver = false;
            textoGuardar.text = "Guardando y volviendo";
            StartCoroutine(Delay());
            SceneManager.LoadScene(0);
        }
        if (isSalir)
        {
            isSalir = false;
            textoGuardar.text = "Guardando y saliendo";
            StartCoroutine(Delay());
            Debug.Log("Saliendo");
            Application.Quit();
        }
    }
    // Botón no
    public void No()
    {
        //if volver
        //if salir

        //Desactivar botones
        if (isVolver)
        {
            textoGuardar.text = "Volviendo";
            StartCoroutine(Delay());
            SceneManager.LoadScene(0);
        }
        if (isSalir)
        {
            textoGuardar.text = "Saliendo";
            StartCoroutine(Delay());
            Debug.Log("Saliendo");
            Application.Quit();
        }
    }

    //botón
    public void Inventario()
    {
        //activa pantalla inventario
        inventario.SetActive(true);
        isInventario = true;
    }
    //botón
    public void Ayuda()
    {
        //activa pantalla ayuda
        ayuda.SetActive(true);
        Time.timeScale = 0;
        isAyuda = true;
    }
    //botón
    public void Cerrar()
    {
        Time.timeScale = 1;
        //cerrar el inventario
        //cerrar ayuda
        if (isPausa)
        {
            pausa.SetActive(false);
            isPausa = false;
        }
        if (isGuardar)
        {
            guardar.SetActive(false);
            isGuardar = false;
        }
        if (isAjustes)
        {
            ajustes.SetActive(false);
            isAjustes = false;
        }
    }

    //Delay
    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(5);
    }
}
