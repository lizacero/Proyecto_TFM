using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BaulScene : MonoBehaviour
{
    [Header("Referencias UI")]
    [SerializeField] private TextMeshProUGUI textoAyuda;
    [SerializeField] private TextMeshProUGUI textoGuion;

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
    private int guionBaulValor = 0;

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
        
    }

    private void Update()
    {
        if (etapaActual == EtapaGuion.WaitPuerta1)
        {
            int fragmentosEnInventario = FragmentosEnInventario();
            int fragmentosGuardadosEnBaul = BaulSceneState.FragmentosGuardadosEnBaul;

            if (fragmentosGuardadosEnBaul >= 5)
            {
                textoGuion.text = "Todos los fragmentos han sido recolectados.";
                textoAyuda.text = "";
            }
            else if (fragmentosGuardadosEnBaul > 0)
            {
                textoGuion.text = $"{fragmentosGuardadosEnBaul} fragmento(s) guardado(s)";
                textoAyuda.text = "";
            }
            else if (fragmentosEnInventario >= 5)
            {
                textoGuion.text = "Guarda los fragmentos en el baúl";
                textoAyuda.text = "Abre el inventario y arrastra los fragmentos al baúl";
            }
            else if (fragmentosEnInventario > 0)
            {
                textoGuion.text = $"Muy bien lograste recolectar {fragmentosEnInventario} fragmento(s). ¿Cuál puerta quieres abrir ahora?";
                textoAyuda.text = "Haz clic sobre una puerta";
            }

            PuertasActivas();
        }
    }

    private int FragmentosEnInventario()
    {
        if (InventarioManager.instance == null) return 0;
        int count = 0;
        for (int i = 1; i <= 5; i++)
        {
            if (InventarioManager.instance.TieneObjeto($"Fragmento{i}"))
                count++;
        }
        return count;
    }

    private void PuertasActivas()
    {
        if (InventarioManager.instance == null) return;
        if (puerta1 != null && InventarioManager.instance.TieneObjeto("Fragmento1"))
            puerta1.GetComponent<ZonaInteractuable>().SetInteraccionesActivas(false);
        else if (puerta1 != null)
            puerta1.GetComponent<ZonaInteractuable>().SetInteraccionesActivas(true);

        if (puerta2 != null && InventarioManager.instance.TieneObjeto("Fragmento2"))
            puerta2.GetComponent<ZonaInteractuable>().SetInteraccionesActivas(false);
        else if (puerta2 != null)
            puerta2.GetComponent<ZonaInteractuable>().SetInteraccionesActivas(true);

        if (puerta3 != null && InventarioManager.instance.TieneObjeto("Fragmento3"))
            puerta3.GetComponent<ZonaInteractuable>().SetInteraccionesActivas(false);
        else if (puerta3 != null)
            puerta3.GetComponent<ZonaInteractuable>().SetInteraccionesActivas(true);

        if (puerta4 != null && InventarioManager.instance.TieneObjeto("Fragmento4"))
            puerta4.GetComponent<ZonaInteractuable>().SetInteraccionesActivas(false);
        else if (puerta4 != null)
            puerta4.GetComponent<ZonaInteractuable>().SetInteraccionesActivas(true);

        if (puerta5 != null && InventarioManager.instance.TieneObjeto("Fragmento5"))
            puerta5.GetComponent<ZonaInteractuable>().SetInteraccionesActivas(false);
        else if (puerta5 != null)
            puerta5.GetComponent<ZonaInteractuable>().SetInteraccionesActivas(true);
    }

    private void CargarDatosJugador()
    {
        personajeSeleccionado = PlayerPrefs.GetInt(KEY_PERSONAJE, 0);
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
        guionBaulValor = BaulSceneState.GuionBaulValor;

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
            PuertasActivas();
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
            conversacionValor = 0;
            guionBaulValor = 0;
            textoGuion.text = $"Hola {nombreJugador}. Soy Papilio. Te estaba esperando";
            textoAyuda.text = "Haz clic sobre Papilio para avanzar";
            oruga.GetComponent<ZonaInteractuable>().SetInteraccionesActivas(true);
        }
        

    }
    public void OnClicOruga()
    {
        if (etapaActual == EtapaGuion.WaitInstrucciones)
        {
            conversacionValor++;
            switch (conversacionValor)
            {
                case 1:
                    textoGuion.text = "¿Qué quién soy?";
                    break;
                case 2:
                    textoGuion.text = "Solo una simple oruga, estoy aquí para acompañarte";
                    break;
                case 3:
                    textoGuion.text = "¿Qué dónde estás?";
                    break;
                case 4:
                    textoGuion.text = "Bueno eso dímelo tú, este es tu sueño.";
                    break;
                case 5:
                    textoGuion.text = "Vamos a descubrirlo entonces.";
                    break;
                case 6:
                    textoGuion.text = "Observa el baúl";
                    textoAyuda.text = "Haz clic sobre el baúl";
                    baul.GetComponent<ZonaInteractuable>().SetInteraccionesActivas(true);
                    etapaActual = EtapaGuion.WaitBaul;
                    break;
            }
        }
    }
    public void OnClicBaul()
    {
        if (etapaActual == EtapaGuion.WaitBaul)
        {
            guionBaulValor++;
            switch (guionBaulValor)
            {
                case 1:
                    textoGuion.text = "Este es el baúl de los recuerdos.";
                    break;
                case 2:
                    textoGuion.text = "Tu misión es recolectar fragmentos de recuerdo";
                    break;
                case 3:
                    textoGuion.text = "Encontrarás uno detrás de cada puerta";
                    break;
                case 4:
                    textoGuion.text = "Cada puerta te llevará a una experiencia distinta";
                    break;
                case 5:
                    textoGuion.text = "¿Cuál quieres abrir primero?";
                    textoAyuda.text = "Haz clic sobre una puerta";
                    etapaActual = EtapaGuion.WaitPuerta1;
                    break;
            }
        }
    }
    public void OnClicPuerta1()
    {
        Debug.Log("Clic en puerta 1");
        Debug.Log(etapaActual);
        BaulSceneState.Puerta1abierta = true;
        if (etapaActual == EtapaGuion.WaitPuerta1)
        {
            GuardarEstado();
            if (SceneTransitionManager.instance != null)
                SceneTransitionManager.instance.LoadSceneConFade(3);
            else
                SceneManager.LoadScene(3);
        }
    }
    public void OnClicPuerta2()
    {
        Debug.Log("Clic en puerta 2");
        BaulSceneState.Puerta2abierta = true;
        if (etapaActual == EtapaGuion.WaitPuerta1)
        {
            GuardarEstado();
            if (SceneTransitionManager.instance != null)
                SceneTransitionManager.instance.LoadSceneConFade(4);
            else
                SceneManager.LoadScene(4);
        }
    }
    public void OnClicPuerta3()
    {
        Debug.Log("Clic en puerta 3");
        BaulSceneState.Puerta3abierta = true;
        if (etapaActual == EtapaGuion.WaitPuerta1)
        {
            GuardarEstado();
            if (SceneTransitionManager.instance != null)
                SceneTransitionManager.instance.LoadSceneConFade(5);
            else
                SceneManager.LoadScene(5);
        }
    }
    public void OnClicPuerta4()
    {
        Debug.Log("Clic en puerta 4");
        BaulSceneState.Puerta4abierta = true;
        if (etapaActual == EtapaGuion.WaitPuerta1)
        {
            GuardarEstado();
            if (SceneTransitionManager.instance != null)
                SceneTransitionManager.instance.LoadSceneConFade(6);
            else
                SceneManager.LoadScene(6);
        }
    }
    public void OnClicPuerta5()
    {
        Debug.Log("Clic en puerta 5");
        BaulSceneState.Puerta5abierta = true;
        if (etapaActual == EtapaGuion.WaitPuerta1)
        {
            GuardarEstado();
            if (SceneTransitionManager.instance != null)
                SceneTransitionManager.instance.LoadSceneConFade(7);
            else
                SceneManager.LoadScene(7);
        }
    }

    public void MostrarPantallaFinal()
    {
        //SceneManager.LoadScene(8);
        if (SceneTransitionManager.instance != null)
            SceneTransitionManager.instance.LoadSceneConFade(8);
        else
            SceneManager.LoadScene(8);
    }

    private void GuardarEstado()
    {
        if (personaje != null)
        {
            BaulSceneState.PosicionPersonaje = personaje.transform.position;
        }

        BaulSceneState.ConversacionValor = conversacionValor;
        BaulSceneState.EtapaGuionInt = (int)etapaActual;
        BaulSceneState.GuionBaulValor = guionBaulValor;
        BaulSceneState.TieneEstado = true;

        Debug.Log($"[BaulScene] Estado guardado. Etapa={etapaActual}, conversacionValor={conversacionValor}");
    }

}
