using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BaulScene : MonoBehaviour
{
    [Header("Referencias UI")]
    [SerializeField] private TextMeshProUGUI textoAyuda;
    [SerializeField] private TextMeshProUGUI textoGuion;

    [Header("Referencias Fade")]
    [SerializeField] private GameObject panelFade;
    [SerializeField] private float duracionFade = 5f;

    [Header("Referencias Objetos")]
    [SerializeField] private GameObject personaje;
    [SerializeField] private GameObject oruga;
    [SerializeField] private GameObject baul;
    [SerializeField] private GameObject puerta1;
    [SerializeField] private GameObject puerta2;
    [SerializeField] private GameObject puerta3;
    [SerializeField] private GameObject puerta4;
    [SerializeField] private GameObject puerta5;

    [Header("Referencias Cámara")]
    [SerializeField] private Camera camaraPrincipal;

    [Header("Referencias personaje")]
    // Claves para PlayerPrefs (deben coincidir con las de NuevoJuegoManager)
    private const string KEY_PERSONAJE = "PersonajeSeleccionado";
    private const string KEY_NOMBRE = "NombreJugador";
    private int personajeSeleccionado = 0;
    private string nombreJugador = "";
    private int conversacionValor=0;
    private string[] conversacionTexto = new string[10];

    private enum EtapaGuion
    {
        Inicio,
        WaitInstrucciones,
        WaitBaul,
        WaitPuerta1
    }


    private void Awake()
    {
        personaje = GameObject.Find("Personaje");
        oruga = GameObject.Find("Oruga");
        baul = GameObject.Find("Baul");
        puerta1 = GameObject.Find("Puerta1");
        puerta2 = GameObject.Find("Puerta2");
        puerta3 = GameObject.Find("Puerta3");
        puerta4 = GameObject.Find("Puerta4");
        puerta5 = GameObject.Find("Puerta5");
        CargarDatosJugador();
        InicializarValores();
    }

    private EtapaGuion etapaActual = EtapaGuion.Inicio;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        IniciarFadeIn();
    }

    // Update is called once per frame
    void Update()
    {
        
        ListaAyuda();
    }

    private void CargarDatosJugador()
    {
        // Cargar personaje seleccionado (1 o 2)
        personajeSeleccionado = PlayerPrefs.GetInt(KEY_PERSONAJE, 0);

        // Cargar nombre del jugador
        nombreJugador = PlayerPrefs.GetString(KEY_NOMBRE, "Jugador");

        Debug.Log($"Datos cargados - Personaje: {personajeSeleccionado}, Nombre: {nombreJugador}");
    }

    private void InicializarValores()
    {
        // Inicializar estado
        if (camaraPrincipal == null)
            camaraPrincipal = Camera.main;

        // Inicializar textos
        if (textoAyuda != null)
            textoAyuda.text = "";
        if (textoGuion != null)
            textoGuion.text = "";

        oruga.GetComponent<ZonaInteractuable>().SetInteraccionesActivas(false);
        baul.GetComponent<ZonaInteractuable>().SetInteraccionesActivas(false);
        puerta1.GetComponent<ZonaInteractuable>().SetInteraccionesActivas(false);
        puerta2.GetComponent<ZonaInteractuable>().SetInteraccionesActivas(false);
        puerta3.GetComponent<ZonaInteractuable>().SetInteraccionesActivas(false);
        puerta4.GetComponent<ZonaInteractuable>().SetInteraccionesActivas(false);
        puerta5.GetComponent<ZonaInteractuable>().SetInteraccionesActivas(false);
    }

    // Referencia animación orugaManager.
    public void IniciarGuion()
    {
        // Etapa 0
        etapaActual = EtapaGuion.WaitInstrucciones;
        textoGuion.text = "";
        textoAyuda.text = conversacionTexto[0];
        oruga.GetComponent<ZonaInteractuable>().SetInteraccionesActivas(true);
    }

    public void OnClicOruga()
    {
        if (etapaActual == EtapaGuion.WaitInstrucciones)
        {
            conversacionValor++;
            
            
            if (conversacionValor == 2)
            {
                baul.GetComponent<ZonaInteractuable>().SetInteraccionesActivas(true);
                conversacionValor = 2;
                etapaActual = EtapaGuion.WaitBaul;
            }
        }
        if (etapaActual == EtapaGuion.WaitPuerta1 || conversacionValor ==3)
        {
            conversacionValor++;
        }
    }

    public void OnClicBaul()
    {
        Debug.Log("Clic en baul");
        if(etapaActual == EtapaGuion.WaitBaul)
        {
            conversacionValor++;
            //cambia animación baul abierto
            etapaActual = EtapaGuion.WaitPuerta1;
        }
        if (etapaActual == EtapaGuion.WaitPuerta1)
        {
            conversacionValor++;
        }
    }

    public void OnClicPuerta1()
    {
        Debug.Log("Clic en puerta 1");
    }
    public void OnClicPuerta2()
    {
        Debug.Log("Clic en puerta 2");
    }
    public void OnClicPuerta3()
    {
        Debug.Log("Clic en puerta 3");
    }
    public void OnClicPuerta4()
    {
        Debug.Log("Clic en puerta 4");
    }
    public void OnClicPuerta5()
    {
        Debug.Log("Clic en puerta 5");
    }

    private void ListaAyuda()
    {
        textoAyuda.text = conversacionTexto[conversacionValor];
        conversacionTexto[0] = "Hola " + nombreJugador + ". Soy Polipol y estoy aquí para guiarte. Haz clic sobre mi";
        conversacionTexto[1] = "De esta manera siempre que te sientas sin rumbo, yo te ayudaré.";
        conversacionTexto[2] = "Dale clic al baúl";
        conversacionTexto[3] = "Este es el baul de los recuerdos.";
        conversacionTexto[4] = "Detrás de cada puerta encontrarás una emoción diferente para enfrentar.";

    }
    void IniciarFadeIn()
    {
        if (panelFade != null)
        {
            panelFade.SetActive(true);
            StopAllCoroutines();
            StartCoroutine(FadeIn(() =>
            {
                // Cambiar de escena
                etapaActual = EtapaGuion.WaitBaul;
                panelFade.SetActive(false);
            }));
        }
        else
        {
            // Si no hay panel de fade, cambiar escena directamente
            SceneManager.LoadScene(2);
        }
    }

    private IEnumerator FadeIn(System.Action onComplete = null)
    {
        CanvasGroup fadeGroup = panelFade.GetComponent<CanvasGroup>();
        if (fadeGroup == null)
            fadeGroup = panelFade.AddComponent<CanvasGroup>();

        float tiempo = 0f;
        while (tiempo < duracionFade)
        {
            tiempo += Time.deltaTime;
            fadeGroup.alpha = Mathf.Lerp(1f, 0f, tiempo / duracionFade);
            yield return null;
        }

        fadeGroup.alpha = 0f;
        onComplete?.Invoke();
    }
}
