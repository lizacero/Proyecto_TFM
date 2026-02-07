using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuGameplay : MonoBehaviour
{
    public static MenuGameplay instance;
    [SerializeField] private GameObject pausa;
    [SerializeField] private GameObject ajustes;
    [SerializeField] private GameObject guardar;
    [SerializeField] private GameObject inventario;
    [SerializeField] private GameObject ayuda;
    [SerializeField] private TextMeshProUGUI textoGuardar;

    // Referencia al overlay/background del inventario para detectar clics fuera
    [SerializeField] private GameObject inventarioOverlay;
    [SerializeField] private GameObject ayudaOverlay;
    [SerializeField] private TextMeshProUGUI textoInventario;

    [SerializeField] private Image[] slotsInventario;
    [SerializeField] private List<ItemInventario> itemsInventario = new List<ItemInventario>();


    private bool isPausa = false;   
    private bool isAjustes = false;
    private bool isGuardar = false;
    private bool isInventario = false;
    private bool isAyuda = false;
    private bool isVolver = false;
    private bool isSalir = false;

    private void Awake()
    {
        instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // CRÍTICO: Asegurar que Time.timeScale esté en 1 al iniciar la escena
        Time.timeScale = 1;

        // Asegurar que todos los paneles estén desactivados al inicio
        InicializarPaneles();

        // Resetear todas las variables de estado
        ResetearEstados();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Inicializa todos los paneles de UI desactivándolos al inicio
    private void InicializarPaneles()
    {
        if (pausa != null)
        {
            pausa.SetActive(false);
        }

        if (ajustes != null)
        {
            ajustes.SetActive(false);
        }

        if (guardar != null)
        {
            guardar.SetActive(false);
        }

        if (inventario != null)
        {
            inventario.SetActive(false);
        }

        if (ayuda != null)
        {
            //ayuda.SetActive(false);
            ayuda.SetActive(true);
        }
        // Desactivar el overlay del inventario también
        if (inventarioOverlay != null)
        {
            inventarioOverlay.SetActive(false);
        }
        if (ayudaOverlay != null)
        {
            ayudaOverlay.SetActive(false);
        }
    }

    /// Resetea todas las variables de estado a sus valores iniciales
    private void ResetearEstados()
    {
        isPausa = false;
        isAjustes = false;
        isGuardar = false;
        isInventario = false;
        isAyuda = false;
        isVolver = false;
        isSalir = false;
    }

    // Botón
    public void Pausa()
    {
        //activar pantalla pausa
        Time.timeScale = 0;
        if (pausa != null)
        {
            pausa.SetActive(true);
        }
        ToggleZonasSensibles(false);
        isPausa = true;
    }

    // Botón
    public void Ajustes()
    {
        //activar pantalla ajustes
        Time.timeScale = 0;
        if (ajustes != null)
        {
            ajustes.SetActive(true);
        }
        isAjustes = true;

    }
    // Botón
    public void Volver()
    {
        //advertencia guardar antes de?
        //bool volver
        if (guardar != null)
        {
            guardar.SetActive(true);
        }
        isVolver = true;
        isGuardar = true;
        if (textoGuardar != null)
        {
            textoGuardar.text = "¿Desea Guardar antes de volver al inicio?";
        }

    }
    // Botón
    public void Salir()
    {
        //advertencia pantalla guardar
        //bool salir
        if (guardar != null)
        {
            guardar.SetActive(true);
        }
        isSalir = true;
        isGuardar = true;
        if (textoGuardar != null)
        {
            textoGuardar.text = "¿Desea Guardar antes de salir?";
        }
    }
    // Botón si
    public void Guardar()
    {
        //guardar partida 
        //if volver - menuprincipal
        //if salir - quit

        //Desactivar botones
        Debug.Log("Guardando");

        // CRÍTICO: Restaurar Time.timeScale antes de cargar la escena
        Time.timeScale = 1;

        if (isVolver)
        {
            isVolver = false;
            if (textoGuardar != null)
            {
                textoGuardar.text = "Guardando y volviendo";
            }
            // Restaurar cursor del sistema antes de cambiar de escena
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            // No usar corrutina aquí, cargar directamente
            // StartCoroutine(Delay());
            SceneManager.LoadScene(0);
        }
        if (isSalir)
        {
            isSalir = false;
            if (textoGuardar != null)
            {
                textoGuardar.text = "Guardando y saliendo";
            }
            // No usar corrutina aquí, salir directamente
            // StartCoroutine(Delay());
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

        // CRÍTICO: Restaurar Time.timeScale antes de cargar la escena
        Time.timeScale = 1;

        if (isVolver)
        {
            if (textoGuardar != null)
            {
                textoGuardar.text = "Volviendo";
            }
            // Restaurar cursor del sistema antes de cambiar de escena
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            // No usar corrutina aquí, cargar directamente
            // StartCoroutine(Delay());
            SceneManager.LoadScene(0);
        }
        if (isSalir)
        {
            if (textoGuardar != null)
            {
                textoGuardar.text = "Saliendo";
            }
            // No usar corrutina aquí, salir directamente
            // StartCoroutine(Delay());
            Debug.Log("Saliendo");
            Application.Quit();
        }
    }

    //botón
    public void Inventario()
    {
        //activa pantalla inventario
        if (inventario != null)
        {
            inventario.SetActive(true);
        }
        // Activar el overlay para detectar clics fuera
        if (inventarioOverlay != null)
        {
            inventarioOverlay.SetActive(true);
        }

        isInventario = true;
        ActualizarInventario();
        var puerta1 = Object.FindAnyObjectByType<Puerta1Manager>();
        if (puerta1 != null) puerta1.OnInventarioAbierto();
    }

    private Sprite ObtenerSprite(string nombreObjeto)
    {
        foreach (var item in itemsInventario)
            if (item.nombreObjeto == nombreObjeto)
                return item.sprite;
        return null;
    }

    public void ActualizarInventario()
    {
        var objetos = InventarioManager.instance.ObtenerObjetos();

        if (objetos.Count == 0)
        {
            textoInventario.text = "Inventario vacío";
            if (textoInventario != null) textoInventario.gameObject.SetActive(true);
            foreach (var slot in slotsInventario)
            {
                if (slot != null)
                {
                    var arrastrable = slot.GetComponent<ObjetoInventarioArrastrable>();
                    if (arrastrable != null)
                        arrastrable.nombreObjeto = "";
                    slot.gameObject.SetActive(false);
                }
            }
            return;
        }
        else
        {
            textoInventario.gameObject.SetActive(false);
        }

        int i = 0;
        foreach (string nombre in objetos)
        {
            if (i >= slotsInventario.Length) break;
            var sprite = ObtenerSprite(nombre);
            if (sprite != null && slotsInventario[i] != null)
            {
                slotsInventario[i].sprite = sprite;
                slotsInventario[i].gameObject.SetActive(true);
                var arrastrable = slotsInventario[i].GetComponent<ObjetoInventarioArrastrable>();
                if (arrastrable == null)
                    arrastrable = slotsInventario[i].gameObject.AddComponent<ObjetoInventarioArrastrable>();
                arrastrable.nombreObjeto = nombre;
                i++;
            }
        }
        for (; i < slotsInventario.Length; i++)
        {
            if (slotsInventario[i] != null)
            {
                var arrastrable = slotsInventario[i].GetComponent<ObjetoInventarioArrastrable>();
                if (arrastrable != null)
                    arrastrable.nombreObjeto = "";  // Limpiar el nombre
                slotsInventario[i].gameObject.SetActive(false);
            }
        }
    }

    // Cierra el inventario cuando se hace clic fuera del panel
    public void CerrarInventario()
    {
        var puerta1 = Object.FindAnyObjectByType<Puerta1Manager>();
        if (puerta1 != null) puerta1.OnInventarioCerrado();
        if (inventario != null)
        {
            inventario.SetActive(false);
        }
        // Desactivar el overlay también
        if (inventarioOverlay != null)
        {
            inventarioOverlay.SetActive(false);
        }

        isInventario = false;
    }

    //botón
    public void Ayuda()
    {
        //activa pantalla ayuda
        if (ayuda != null)
        {
            ayuda.SetActive(true);
        }
        // Activar el overlay para detectar clics fuera
        if (ayudaOverlay != null)
        {
            ayudaOverlay.SetActive(true);
        }

        isAyuda = true;
    }
    public void CerrarAyuda()
    {
        if (ayuda != null)
        {
            ayuda.SetActive(false);
        }
        //desactivarOverlay
        if (ayudaOverlay != null)
        {
            ayudaOverlay.SetActive(false);
        }

        isAyuda = false;
    }
    //botón
    public void Cerrar()
    {
        Time.timeScale = 1;
        //cerrar el inventario
        //cerrar ayuda
        if (isPausa)
        {
            if (pausa != null)
            {
                pausa.SetActive(false);
            }
            isPausa = false;
        }
        if (isGuardar)
        {
            if (guardar != null)
            {
                guardar.SetActive(false);
            }
            isGuardar = false;
        }
        if (isAjustes)
        {
            if (ajustes != null)
            {
                ajustes.SetActive(false);
            }
            isAjustes = false;
        }
        if (isInventario)
        {
            if (inventario != null)
            {
                inventario.SetActive(false);
            }
            isInventario = false;
        }
        if (isAyuda)
        {
            if (ayuda != null)
            {
                ayuda.SetActive(false);
            }
            isAyuda = false;
        }
        ToggleZonasSensibles(true);
    }

    private void ToggleZonasSensibles(bool habilitar)
    {
        ZonaInteractuable[] zonas = Object.FindObjectsByType<ZonaInteractuable>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        foreach (ZonaInteractuable zona in zonas)
        {
            if (zona != null)
            {
                zona.SetInteraccionesActivas(habilitar);
            }
        }
    }

    [System.Serializable]
    public class ItemInventario
    {
        public string nombreObjeto;
        public Sprite sprite;
    }


}
