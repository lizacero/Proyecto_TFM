using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Controla el HUD durante el juego: pausa, ajustes, inventario, ayuda, volver/salir.
// Singleton al que acceden BaulDropZone, InterruptorDropZone, etc. para ActualizarInventario.
// Gestiona los paneles, el inventario visual y el diálogo de guardar antes de volver/salir.
public class MenuGameplay : MonoBehaviour
{
    public static MenuGameplay instance;

    [Header("Paneles")]
    [SerializeField] private GameObject pausa;
    [SerializeField] private GameObject ajustes;
    [SerializeField] private GameObject guardar;
    [SerializeField] private GameObject inventario;
    [SerializeField] private GameObject ayuda;
    [SerializeField] private GameObject inventarioOverlay;
    [SerializeField] private GameObject ayudaOverlay;

    [Header("Inventario")]
    [SerializeField] private TextMeshProUGUI textoInventario;
    [SerializeField] private Image[] slotsInventario;
    [SerializeField] private List<ItemInventario> itemsInventario = new List<ItemInventario>();

    [SerializeField] private TextMeshProUGUI textoGuardar;

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

    void Start()
    {
        // CRÍTICO: Asegurar que Time.timeScale esté en 1 al iniciar la escena
        Time.timeScale = 1;
        InicializarPaneles();
        ResetearEstados();
    }

    // Inicializa todos los paneles de UI
    private void InicializarPaneles()
    {
        SetActivo(pausa, false);
        SetActivo(ajustes, false);
        SetActivo(guardar, false);
        SetActivo(inventario, false);
        SetActivo(ayuda, true); 
        SetActivo(inventarioOverlay, false);
        SetActivo(ayudaOverlay, false);
    }

    private void SetActivo(GameObject go, bool activo)
    {
        if (go != null) go.SetActive(activo);
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
        Time.timeScale = 0;
        SetActivo(pausa, true);
        ToggleZonasSensibles(false);
        isPausa = true;
    }

    // Botón
    public void Ajustes()
    {
        Time.timeScale = 0;
        SetActivo(ajustes, true);
        isAjustes = true;

    }
    // Botón
    public void Volver()
    {
        SetActivo(guardar, true);
        isVolver = true;
        isGuardar = true;
        if (textoGuardar != null) textoGuardar.text = "¿Desea Guardar antes de volver al inicio?";

    }
    // Botón
    public void Salir()
    {
        SetActivo(guardar, true);
        isSalir = true;
        isGuardar = true;
        if (textoGuardar != null) textoGuardar.text = "¿Desea Guardar antes de salir?";
    }
    // Botón si
    public void Guardar()
    {
        //Desactivar botones
        Debug.Log("Guardando");

        // CRÍTICO: Restaurar Time.timeScale antes de cargar la escena
        Time.timeScale = 1;

        if (isVolver)
        {
            isVolver = false;
            textoGuardar.text = "Guardando y volviendo";
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            SceneManager.LoadScene(0);
        }
        if (isSalir)
        {
            isSalir = false;
            if (textoGuardar != null)
            textoGuardar.text = "Guardando y saliendo";
            Debug.Log("Saliendo");
            Application.Quit();
        }
    }
    // Botón no
    public void No()
    {
        // CRÍTICO: Restaurar Time.timeScale antes de cargar la escena
        Time.timeScale = 1;

        if (isVolver)
        {
            textoGuardar.text = "Volviendo";
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            SceneManager.LoadScene(0);
        }
        if (isSalir)
        {
            textoGuardar.text = "Saliendo";
            Debug.Log("Saliendo");
            Application.Quit();
        }
    }

    //botón
    public void Inventario()
    {
        SetActivo(inventario, true);
        SetActivo(inventarioOverlay, true);
        isInventario = true;
        ActualizarInventario();

        var puerta1 = FindAnyObjectByType<Puerta1Manager>();
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
        SetActivo(inventario, false);
        SetActivo(inventarioOverlay, false);
        isInventario = false;
    }

    //botón
    public void Ayuda()
    {
        SetActivo(ayuda, true);
        SetActivo(ayudaOverlay, true);
        isAyuda = true;
    }
    public void CerrarAyuda()
    {
        SetActivo(ayuda, false);
        SetActivo(ayudaOverlay, false);
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
            if (pausa != null) pausa.SetActive(false);
            isPausa = false;
        }
        if (isGuardar)
        {
            if (guardar != null) guardar.SetActive(false);
            isGuardar = false;
        }
        if (isAjustes)
        {
            if (ajustes != null) ajustes.SetActive(false);
            isAjustes = false;
        }
        if (isInventario)
        {
            if (inventario != null) inventario.SetActive(false);
            isInventario = false;
        }
        if (isAyuda)
        {
            if (ayuda != null) ayuda.SetActive(false);
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
