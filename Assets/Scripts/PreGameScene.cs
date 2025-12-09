using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PreGameScene : MonoBehaviour
{
    [Header("Referencias UI")]
    [SerializeField] private TextMeshProUGUI textoUI; // El texto de "Leyendas" del Canvas

    [Header("Referencias Objetos")]
    [SerializeField] private GameObject ventana;
    [SerializeField] private GameObject estrella; // Debe crearse/activarse en el momento correcto
    [SerializeField] private GameObject mesa;
    [SerializeField] private GameObject papel; // Debe aparecer sobre la mesa
    [SerializeField] private GameObject cama;
    [SerializeField] private GameObject personaje;

    [Header("Referencias Cámara")]
    [SerializeField] private Camera camaraPrincipal;

    [Header("Configuración Cámara")]
    [SerializeField] private float tamanoZoomIn = 2.5f; // Tamaño de cámara cuando hace zoom a la ventana
    [SerializeField] private float tamanoZoomOut = 5f; // Tamaño de cámara normal
    [SerializeField] private float velocidadZoom = 2f;
    [SerializeField] private Vector3 posicionCamaraVentana = new Vector3(-0.11f, 0.77f, -10f); // Posición de cámara para ver la ventana

    [Header("Configuración Fade")]
    [SerializeField] private GameObject panelFade; // Panel UI para el fade out (Image con color negro)
    [SerializeField] private float duracionFade = 5f;


    // Estado del guión
    private enum EtapaGuion
    {
        Inicio,
        EsperandoClicVentana1,
        AcercandoCamara,
        EsperandoClicEstrella,
        EsperandoClicVentana2,
        AlejandoCamara,
        EsperandoClicPapel,
        Esperando10Segundos1,
        Esperando10Segundos2,
        EsperandoClicCama,
        FadeOut,
        CambioEscena
    }

    private EtapaGuion etapaActual = EtapaGuion.Inicio;
    private bool ventanaCerrada = false;
    private bool papelVisible = false;
    private bool personajeEnCama = false;
    private Coroutine corrutinaZoom;
    private Coroutine corrutinaEspera;

    void Start()
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
        if (estrella != null)
            estrella.SetActive(false);

        if (papel != null)
            papel.SetActive(false);

        // Iniciar el guión
        IniciarGuion();
    }

    void IniciarGuion()
    {
        // Etapa 0: Mostrar texto inicial
        MostrarTexto("hace frío (cerrar ventana)");
        etapaActual = EtapaGuion.EsperandoClicVentana1;
    }

    // Método público para ser llamado desde ZonaSensible cuando se hace clic en ventana
    public void OnClicVentana()
    {
        if (etapaActual == EtapaGuion.EsperandoClicVentana1)
        {
            // Etapa 2: Acercar cámara a la ventana
            etapaActual = EtapaGuion.AcercandoCamara;
            AcercarCamaraAVentana();
        }
        else if (etapaActual == EtapaGuion.EsperandoClicVentana2)
        {
            // Etapa 5: Alejando cámara
            etapaActual = EtapaGuion.AlejandoCamara;
            AlejandoCamara();
        }
    }

    // Método público para ser llamado desde ZonaSensible cuando se hace clic en estrella
    public void OnClicEstrella()
    {
        if (etapaActual == EtapaGuion.EsperandoClicEstrella)
        {
            // Etapa 4: Mostrar texto "deseo, deseo" y ocultar estrella
            MostrarTexto("deseo, deseo");
            if (estrella != null)
                estrella.SetActive(false);

            // Esperar un poco y continuar
            StartCoroutine(EsperarYContinuar(1f, () =>
            {
                MostrarTexto("cerrar ventana");
                etapaActual = EtapaGuion.EsperandoClicVentana2;
            }));
        }
    }

    // Método público para ser llamado desde ZonaSensible cuando se hace clic en papel
    public void OnClicPapel()
    {
        if (etapaActual == EtapaGuion.EsperandoClicPapel)
        {
            MostrarTexto("Dice: Siempre estaré contigo");

            // Esperar 10 segundos
            etapaActual = EtapaGuion.Esperando10Segundos1;
            if (corrutinaEspera != null)
                StopCoroutine(corrutinaEspera);
            corrutinaEspera = StartCoroutine(EsperarYContinuar(10f, () =>
            {
                MostrarTexto("Mejor intento descansar");
                etapaActual = EtapaGuion.Esperando10Segundos2;
                corrutinaEspera = StartCoroutine(EsperarYContinuar(10f, () =>
                {
                    MostrarTexto("ir a la cama");
                    etapaActual = EtapaGuion.EsperandoClicCama;
                }));
            }));
        }
    }

    // Método público para ser llamado desde ZonaSensible cuando se hace clic en cama
    public void OnClicCama()
    {
        if (etapaActual == EtapaGuion.EsperandoClicCama)
        {
            // Estado personaje: en cama
            personajeEnCama = true;
            // Aquí puedes cambiar el sprite del personaje o animación si lo necesitas

            // Iniciar fade out
            etapaActual = EtapaGuion.FadeOut;
            IniciarFadeOut();
        }
    }

    void AcercarCamaraAVentana()
    {
        if (camaraPrincipal != null)
        {
            if (corrutinaZoom != null)
                StopCoroutine(corrutinaZoom);

            corrutinaZoom = StartCoroutine(ZoomCamara(tamanoZoomIn, posicionCamaraVentana, () =>
            {
                // Después del zoom, mostrar estrella y texto
                if (estrella != null)
                    estrella.SetActive(true);

                MostrarTexto("una estrella fugaz");
                etapaActual = EtapaGuion.EsperandoClicEstrella;
            }));
        }
    }

    void AlejandoCamara()
    {
        if (camaraPrincipal != null)
        {
            if (corrutinaZoom != null)
                StopCoroutine(corrutinaZoom);

            Vector3 posicionOriginal = new Vector3(0, 0, -10); // Posición original de la cámara
            corrutinaZoom = StartCoroutine(ZoomCamara(tamanoZoomOut, posicionOriginal, () =>
            {
                // Ventana cerrada
                ventanaCerrada = true;
                // Aquí puedes cambiar el sprite de la ventana si lo necesitas

                MostrarTexto("Ventana cerrada");

                // Aparece papel sobre la mesa
                if (papel != null)
                {
                    papel.SetActive(true);
                    papelVisible = true;
                }

                StartCoroutine(EsperarYContinuar(1f, () =>
                {
                    MostrarTexto("y eso sobre la mesa?");
                    etapaActual = EtapaGuion.EsperandoClicPapel;
                }));
            }));
        }
    }

    private IEnumerator ZoomCamara(float tamanoObjetivo, Vector3 posicionObjetivo, System.Action onComplete = null)
    {
        float tamanoInicial = camaraPrincipal.orthographicSize;
        Vector3 posicionInicial = camaraPrincipal.transform.position;
        float tiempo = 0f;
        float duracion = Vector3.Distance(posicionInicial, posicionObjetivo) / velocidadZoom;

        while (tiempo < duracion)
        {
            tiempo += Time.deltaTime;
            float t = tiempo / duracion;
            t = Mathf.SmoothStep(0f, 1f, t); // Suavizar la transición

            camaraPrincipal.orthographicSize = Mathf.Lerp(tamanoInicial, tamanoObjetivo, t);
            camaraPrincipal.transform.position = Vector3.Lerp(posicionInicial, posicionObjetivo, t);

            yield return null;
        }

        // Asegurar valores finales
        camaraPrincipal.orthographicSize = tamanoObjetivo;
        camaraPrincipal.transform.position = posicionObjetivo;

        onComplete?.Invoke();
    }

    void IniciarFadeOut()
    {
        if (panelFade != null)
        {
            panelFade.SetActive(true);
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

    void MostrarTexto(string texto)
    {
        if (textoUI != null)
        {
            textoUI.text = texto;
        }
        Debug.Log($"[PreGameDirector] {texto}");
    }

    private IEnumerator EsperarYContinuar(float tiempo, System.Action onComplete)
    {
        yield return new WaitForSeconds(tiempo);
        onComplete?.Invoke();
    }
}
