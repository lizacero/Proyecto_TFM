using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PreGameManager : MonoBehaviour
{
    [Header("Referencias UI")]
    [SerializeField] private TextMeshProUGUI textoAyuda;
    [SerializeField] private TextMeshProUGUI textoGuion;
    [SerializeField] private GameObject mensajes;
    [SerializeField] private TextMeshProUGUI textoMensaje;

    [Header("Referencias Objetos")]
    [SerializeField] private GameObject personaje;
    [SerializeField] private GameObject ventana;
    [SerializeField] private GameObject abierta;
    [SerializeField] private GameObject cerrada;
    [SerializeField] private GameObject ventana2;
    [SerializeField] private GameObject cama;
    [SerializeField] private GameObject estrella;
    [SerializeField] private GameObject papel;

    [Header("Referencia Animator")]
    //[SerializeField] private Animator animVentana;

    [Header("Referencias Escenas")]
    [SerializeField] private GameObject escenaHabitacion;
    [SerializeField] private GameObject escenaVentana;

    [Header("Referencias Cámara")]
    [SerializeField] private Camera camaraPrincipal;

    [Header("Configuración Fade")]
    [SerializeField] private GameObject panelFade; // Panel UI para el fade out (Image con color negro)
    [SerializeField] private float duracionFade = 5f;

    // Estado del guion
    private enum EtapaGuion
    {
        Inicio,
        WaitVentana1,
        WaitEstrella,
        WaitVentana2,
        WaitPapel,
        WaitSalirPapel,
        WaitCama,
        FadeOut,
        CambioEscena
    }

    private EtapaGuion etapaActual = EtapaGuion.Inicio;
    private bool ventanaCerrada = false;
    private bool papelVisible = false;
    private bool personajeEnCama = false;
    private bool finEspera = false;
    private MovimientoPersonaje movimientoPersonaje;


    private void Awake()
    {
        personaje = GameObject.Find("Personaje");
        ventana = GameObject.Find("Ventana");
        abierta = GameObject.Find("Abierta");
        cerrada = GameObject.Find("Cerrada");
        ventana2 = GameObject.Find("Ventana2");
        cama = GameObject.Find("Cama");
        estrella = GameObject.Find("Estrella");
        papel = GameObject.Find("Papel");
        mensajes = GameObject.Find("Mensajes");
        
        InicializarValores();
    }

    void Start()
    {
        IniciarGuion();
    }
    private void Update()
    {
        Debug.Log(etapaActual.ToString());
        if (etapaActual == EtapaGuion.WaitEstrella)
        {
            if (finEspera)
            {
                textoGuion.text = "Ahora si debería cerrar la ventana.";
                textoAyuda.text = "Haz clic en la ventana.";
                ventana2.GetComponent<ZonaInteractuable>().SetInteraccionesActivas(true);
                etapaActual = EtapaGuion.WaitVentana2;
                finEspera = false;
                StopAllCoroutines();
            }
        }
        if (etapaActual == EtapaGuion.WaitVentana2)
        {
            if (finEspera)
            {
                papel.GetComponent<ZonaInteractuable>().SetInteraccionesActivas(true);
                textoGuion.text = "¿Qué es ese papel?.";
                textoAyuda.text = "Haz clic en el papel.";
                etapaActual = EtapaGuion.WaitPapel;
                StopAllCoroutines();
            }
        }
    }

    /// <summary>
    /// Inicializando valores generales
    /// </summary>
    private void InicializarValores()
    {
        // Inicializar estado
        if (camaraPrincipal == null)
            camaraPrincipal = Camera.main;

        if (panelFade != null)
        {
            panelFade.SetActive(false);
            CanvasGroup fadeGroup = panelFade.GetComponent<CanvasGroup>();
            if (fadeGroup == null)
            {
                fadeGroup = panelFade.AddComponent<CanvasGroup>();
            }
            fadeGroup.alpha = 0f;
        }

        // Ocultar objetos iniciales
        if (papel != null)
            papel.SetActive(false);
        papel.GetComponent<ZonaInteractuable>().SetInteraccionesActivas(false);
        if (mensajes != null)
            mensajes.SetActive(false);

        // Inicializar textos
        if (textoAyuda != null)
            textoAyuda.text = "";
        if (textoGuion != null)
            textoGuion.text = "";
        if (textoMensaje != null)
            textoMensaje.text = "";

        // Inicializar escenas
        if (escenaVentana != null)
            escenaVentana.SetActive(false);
        if (escenaHabitacion != null)
            escenaHabitacion.SetActive(true);
        if (abierta != null)
            abierta.SetActive(true);
        if (cerrada != null)
            cerrada.SetActive(false);
        StopAllCoroutines();
    }

    void IniciarGuion()
    {
        // Etapa 0
        etapaActual = EtapaGuion.WaitVentana1;
        textoGuion.text = "Hace frío";
        textoAyuda.text = "Cierra la Ventana";
        Debug.Log(textoAyuda.text);
    }

    public void OnClicVentana()
    {
        // Estapa 1
        if (etapaActual == EtapaGuion.WaitVentana1)
        {
            escenaHabitacion.SetActive(false);
            escenaVentana.SetActive(true);
            personaje.SetActive(false);
            textoGuion.text = "¡Una estrella fugaz!.";
            textoAyuda.text = "Pide un deseo.";
            etapaActual = EtapaGuion.WaitEstrella;
        }
        
        // Etapa 4
        if (etapaActual == EtapaGuion.WaitPapel)
        {
            textoGuion.text = "Debería revisar el papel.";
            textoAyuda.text = "Busca el papel y haz clic en el.";
        }
        // Etapa 5
        if (etapaActual == EtapaGuion.WaitCama)
        {
            textoGuion.text = "Está muy tarde";
            textoAyuda.text = "Haz clic en la cama";
        }
    }

    public void OnClicVentana2()
    {
        // Etapa 2
        if (etapaActual == EtapaGuion.WaitEstrella)
        {
            textoGuion.text = "Debería pedir un deseo.";
            textoAyuda.text = "Haz clic en la estrella fugaz.";
        }
        // Etapa 3
        if (etapaActual == EtapaGuion.WaitVentana2)
        {
            abierta.SetActive(false);
            cerrada.SetActive(true);
            escenaHabitacion.SetActive(true);
            escenaVentana.SetActive(false);
            personaje.SetActive(true);
            papel.SetActive(true);
            textoGuion.text = "¿Eh?, ¿Qué es eso?.";
            textoAyuda.text = "";
            StartCoroutine(EsperarYContinuar(5f));
            //animación papel  
        }
    }

    public void OnClicEstrella()
    {
        // Etapa 2
        if (etapaActual == EtapaGuion.WaitEstrella)
        {
            ventana2.GetComponent<ZonaInteractuable>().SetInteraccionesActivas(false);
            estrella.GetComponent<ZonaInteractuable>().SetInteraccionesActivas(false);
            textoGuion.text = "Deseo... Deseo...";
            textoAyuda.text = "";
            StartCoroutine(EsperarYContinuar(3f));
        }
    }

    // Método público para ser llamado desde ZonaSensible cuando se hace clic en papel
    public void OnClicPapel()
    {
        // Etapa 4
        if (etapaActual == EtapaGuion.WaitPapel)
        {
            etapaActual = EtapaGuion.WaitSalirPapel;
            mensajes.SetActive(true);
            textoMensaje.text = "Siempre estaré contigo.";
            textoGuion.text = "Dice...";
            textoAyuda.text = "";
        }
        
    }

    // Método público para ser llamado desde ZonaSensible cuando se hace clic en cama
    public void OnClicCama()
    {
        // Etapa 1
        if (etapaActual == EtapaGuion.WaitVentana1)
        {
            textoGuion.text = "Debería cerrar la ventana";
            textoAyuda.text = "Haz clic en la ventana";
        }
        // Etapa 5
        if (etapaActual == EtapaGuion.WaitCama)
        {
            textoGuion.text = "Hora de dormir";
            textoAyuda.text = "";
            IniciarFadeOut();
        }
    }

    // referencia en el botón salir de mensaje
    public void OnClicSalir()
    {
        if (etapaActual == EtapaGuion.WaitSalirPapel)
        {
            mensajes.SetActive(false);
            etapaActual = EtapaGuion.WaitCama;
            papel.SetActive(false);
            textoGuion.text = "Mejor debería descansar.";
            textoAyuda.text = "Haz clic en la cama";
            
        }
    }


    void IniciarFadeOut()
    {
        if (panelFade != null)
        {
            panelFade.SetActive(true);
            StopAllCoroutines();
            StartCoroutine(FadeOut(() =>
            {
                // Cambiar de escena
                etapaActual = EtapaGuion.CambioEscena;
                SceneManager.LoadScene(2);
            }));
        }
        else
        {
            // Si no hay panel de fade, cambiar escena directamente
            SceneManager.LoadScene(2);
        }
    }

    private IEnumerator FadeOut(System.Action onComplete = null)
    {
        CanvasGroup fadeGroup = panelFade.GetComponent<CanvasGroup>();
        if (fadeGroup == null)
            fadeGroup = panelFade.AddComponent<CanvasGroup>();

        float tiempo = 0f;
        while (tiempo < duracionFade)
        {
            tiempo += Time.deltaTime;
            fadeGroup.alpha = Mathf.Lerp(0f, 1f, tiempo / duracionFade);
            yield return null;
        }

        fadeGroup.alpha = 1f;
        onComplete?.Invoke();
    }

    private IEnumerator EsperarYContinuar(float tiempo)
    {
        yield return new WaitForSeconds(tiempo);
        finEspera = true;

    }

    private IEnumerator EsperarMovimiento()
    {
        while (movimientoPersonaje.estaMoviendose)
        {
            yield return null;
        }
    }
}
