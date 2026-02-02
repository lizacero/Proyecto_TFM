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
    private int conversacionValor = 0;
    private string[] conversacionTexto = new string[10];
    private int guionValor = 0;
    private string[] guionTexto = new string[10];

    private enum EtapaGuion
    {
        Inicio,
        WaitInstrucciones,
        WaitBaul,
        WaitPuerta1
    }

    private EtapaGuion etapaActual = EtapaGuion.Inicio;

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
        RestaurarEstado();
    }

    private void Start()
    {
        IniciarFadeOut();
    }

    private void Update()
    {
        Debug.Log("ConversacionValor: "+conversacionValor);
        Debug.Log(etapaActual + ": " + (int)etapaActual);
        ListaAyuda();
        ListaGuion();
        if (etapaActual == EtapaGuion.WaitPuerta1)
        {
            puerta1.GetComponent<ZonaInteractuable>().SetInteraccionesActivas(true);
            puerta2.GetComponent<ZonaInteractuable>().SetInteraccionesActivas(true);
            puerta3.GetComponent<ZonaInteractuable>().SetInteraccionesActivas(true);
            puerta4.GetComponent<ZonaInteractuable>().SetInteraccionesActivas(true);
            puerta5.GetComponent<ZonaInteractuable>().SetInteraccionesActivas(true);
        }
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

    private void RestaurarEstado()
    {
        if (!BaulSceneState.TieneEstado)
        {
            // Primera vez que entramos al baúl: estado inicial
            etapaActual = EtapaGuion.Inicio;
            conversacionValor = 0;
            return;
        }

        // Restaurar valores básicos
        conversacionValor = BaulSceneState.ConversacionValor;
        etapaActual = (EtapaGuion)BaulSceneState.EtapaGuionInt;

        if (personaje != null)
        {
            personaje.transform.position = BaulSceneState.PosicionPersonaje;
        }

        // Reconfigurar interacciones según la etapa
        if (etapaActual == EtapaGuion.WaitInstrucciones)
        {
            oruga.GetComponent<ZonaInteractuable>().SetInteraccionesActivas(true);
        }
        else if (etapaActual == EtapaGuion.WaitBaul)
        {
            oruga.GetComponent<ZonaInteractuable>().SetInteraccionesActivas(true);
            baul.GetComponent<ZonaInteractuable>().SetInteraccionesActivas(true);
        }
        else if (etapaActual == EtapaGuion.WaitPuerta1)
        {
            oruga.GetComponent<ZonaInteractuable>().SetInteraccionesActivas(true);
            baul.GetComponent<ZonaInteractuable>().SetInteraccionesActivas(true);
            puerta1.GetComponent<ZonaInteractuable>().SetInteraccionesActivas(true);
            puerta2.GetComponent<ZonaInteractuable>().SetInteraccionesActivas(true);
            puerta3.GetComponent<ZonaInteractuable>().SetInteraccionesActivas(true);
            puerta4.GetComponent<ZonaInteractuable>().SetInteraccionesActivas(true);
            puerta5.GetComponent<ZonaInteractuable>().SetInteraccionesActivas(true);
        }

        Debug.Log($"[BaulScene] Estado restaurado. Etapa={etapaActual}, conversacionValor={conversacionValor}");

    }
    /// <summary>
    /// Métodos que se llaman y mantienen el flujo de los estados
    /// </summary>
    public void IniciarGuion()
    {
        if (!BaulSceneState.TieneEstado)
        {
            // Etapa 0
            etapaActual = EtapaGuion.WaitInstrucciones;
            conversacionValor = 1;
            guionValor = 0;
            oruga.GetComponent<ZonaInteractuable>().SetInteraccionesActivas(true);
        }
        

    }
    public void OnClicOruga()
    {
        if (etapaActual == EtapaGuion.WaitInstrucciones)
        {
            conversacionValor++;


            if (conversacionValor == 3)
            {
                baul.GetComponent<ZonaInteractuable>().SetInteraccionesActivas(true);
                conversacionValor = 3;
                etapaActual = EtapaGuion.WaitBaul;
            }
        }
        if (etapaActual == EtapaGuion.WaitPuerta1 || conversacionValor == 4)
        {
            conversacionValor++;
        }
    }
    public void OnClicBaul()
    {
        if (etapaActual == EtapaGuion.WaitBaul)
        {
            conversacionValor++;
            //cambia animación baul abierto
            etapaActual = EtapaGuion.WaitPuerta1;
            Debug.Log("Clic en baul");
        }
        else if (etapaActual == EtapaGuion.WaitPuerta1)
        {
            conversacionValor++;
            Debug.Log("Clic en baul2");
        }
    }
    public void OnClicPuerta1()
    {
        Debug.Log("Clic en puerta 1");
        Debug.Log(etapaActual);
        if (etapaActual == EtapaGuion.WaitPuerta1)
        {
            GuardarEstado();
            SceneManager.LoadScene(3);
        }
    }
    public void OnClicPuerta2()
    {
        Debug.Log("Clic en puerta 2");
        if (etapaActual == EtapaGuion.WaitPuerta1)
        {
            GuardarEstado();
            SceneManager.LoadScene(4);
        }
    }
    public void OnClicPuerta3()
    {
        Debug.Log("Clic en puerta 3");
        if (etapaActual == EtapaGuion.WaitPuerta1)
        {
            GuardarEstado();
            SceneManager.LoadScene(5);
        }
    }
    public void OnClicPuerta4()
    {
        Debug.Log("Clic en puerta 4");
        if (etapaActual == EtapaGuion.WaitPuerta1)
        {
            GuardarEstado();
            SceneManager.LoadScene(6);
        }
    }
    public void OnClicPuerta5()
    {
        Debug.Log("Clic en puerta 5");
        if (etapaActual == EtapaGuion.WaitPuerta1)
        {
            GuardarEstado();
            SceneManager.LoadScene(7);
        }
    }

    private void ListaAyuda()
    {
        textoAyuda.text = conversacionTexto[conversacionValor];
        conversacionTexto[0] = "";
        conversacionTexto[1] = "Hola " + nombreJugador + ". Soy Papilio y estoy aquí para guiarte. Haz clic sobre mi";
        conversacionTexto[2] = "De esta manera siempre que te sientas sin rumbo, yo te ayudaré.";
        conversacionTexto[3] = "Dale clic al baúl";
        conversacionTexto[4] = "Este es el baul de los recuerdos.";
        conversacionTexto[5] = "Detrás de cada puerta encontrarás una emoción diferente.";
        conversacionTexto[6] = "¿Cuál puerta quieres abrir?.";
    }
    private void ListaGuion()
    {
        // Si hay fragmentos depositados, priorizar ese mensaje
        if (BaulSceneState.FragmentosGuardadosEnBaul > 0)
        {
            int g = BaulSceneState.FragmentosGuardadosEnBaul;
            int f = 5 - g;
            if (g >= 5)
                textoGuion.text = "Todos los fragmentos han sido recolectados.";
            else
                textoGuion.text = $"{g} fragmento(s) guardado(s), faltan {f}.";
            return;
        }
        textoGuion.text = guionTexto[guionValor];
        guionTexto[0] = "";
    }

    private void GuardarEstado()
    {
        if (personaje != null)
        {
            BaulSceneState.PosicionPersonaje = personaje.transform.position;
        }

        BaulSceneState.ConversacionValor = conversacionValor;
        BaulSceneState.EtapaGuionInt = (int)etapaActual;
        BaulSceneState.TieneEstado = true;

        Debug.Log($"[BaulScene] Estado guardado. Etapa={etapaActual}, conversacionValor={conversacionValor}");
    }

    void IniciarFadeOut()
    {
        if (panelFade != null)
        {
            panelFade.SetActive(true);
            StopAllCoroutines();
            StartCoroutine(FadeOut(() =>
            {
                if (!BaulSceneState.TieneEstado)
                {
                    etapaActual = EtapaGuion.Inicio;
                }
                panelFade.SetActive(false);
            }));
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
            fadeGroup.alpha = Mathf.Lerp(1f, 0f, tiempo / duracionFade);
            yield return null;
        }

        fadeGroup.alpha = 0f;
        onComplete?.Invoke();
    }
}
