using TMPro;
using UnityEngine;

public class Puerta1Manager : MonoBehaviour
{
    public enum EtapaPuzzle1
    {
        Inicio,
        Busqueda,
        Interruptores,
        Luz
    }

    [SerializeField] private GameObject personaje;
    [Header("Puzzle 1 - Estados")]
    //[SerializeField] private int clicsParaRevelarInterruptores = 5;  // Ajustable
    private int contadorClicsInicio = 0;
    private EtapaPuzzle1 etapaPuzzle1 = EtapaPuzzle1.Inicio;

    [Header("Puzzle 1 - Escenas")]
    [SerializeField] private GameObject escena1Habitacion;

    [Header("Puzzle 1 - Objetos")]
    [SerializeField] private GameObject[] interruptoresEnEscena;  // Los que aparecen en Busqueda
    [SerializeField] private GameObject interruptor;
    [SerializeField] private GameObject zonaInterruptor;
    //[SerializeField] private GameObject luz;                      // Se activa en Luz
    [SerializeField] private GameObject tablero;                 // Aparece cuando hay luz (puzzle 2)
    [SerializeField] private TextMeshProUGUI textoGuion;
    [SerializeField] private TextMeshProUGUI textoAyuda;
    [SerializeField] private int totalInterruptoresParaColocar = 3;  // Ajusta
    private int interruptoresColocados = 0;
    private bool luzEncendida = false;

    [Header("Puzzle 2 - Escenas")]
    [SerializeField] private GameObject escena2Tablero;
    [SerializeField] private GameObject puzzle2;

    [Header("Puzzle 2 - Rompecabezas")]
    [SerializeField] private int totalPiezas = 15;
    [SerializeField] private GameObject imagenConFrase;
    [SerializeField] private GameObject volver;
    private int piezasColocadasPuzzle2 = 0;

    [Header("Puzzle 3 - Escenas")]
    [SerializeField] private GameObject escena3salida;

    [Header("Puzzle 3 - Objetos")]
    [SerializeField] private GameObject fragmento1;
    [SerializeField] private GameObject puerta;

    void Start()
    {
        interruptor.SetActive(false);
        fragmento1.SetActive(true);
        puerta.GetComponent<ZonaInteractuable>().SetInteraccionesActivas(false);
        escena1Habitacion.SetActive(true);
        escena2Tablero.SetActive(false);
        escena3salida.SetActive(false);
        puzzle2.SetActive(false);
        volver.SetActive(false);
        textoGuion.text = "Entraste a la habitación de la insensibilidad";
        textoAyuda.text = "Interactúa con el entorno";

        //Revisar porque si no vuelvo a ingresar a la puerta no es necesario
        if (InventarioManager.instance.TieneObjeto("Fragmento1"))
            fragmento1.SetActive(false);
        //------------
    }

    /// <summary>
    /// Llamado desde ZonaInteractuable al hacer clic en muebles/objetos en etapa Inicio.
    /// </summary>
    public void OnClicMueble()
    {
        if (etapaPuzzle1 != EtapaPuzzle1.Inicio) return;

        contadorClicsInicio++;
        switch (contadorClicsInicio)
        {
            case 1:
                textoGuion.text = "Parece que no pasa nada";
                textoAyuda.text = "Interactúa con el entorno";
                break;
            case 2:
                textoGuion.text = "Parece que no pasa nada";
                textoAyuda.text = "Sigue interactuando";
                break;
            case 3:
                textoGuion.text = "¿Aún nada?, que extraño";
                textoAyuda.text = "Sigue interactuando";
                break;
            case 4:
                textoGuion.text = "Sigue interactuando";
                textoAyuda.text = "Sigue interactuando";
                break;
            case 5:
                textoGuion.text = "Quizá pase algo en algún momento";
                textoAyuda.text = "Sigue interactuando";
                break;
            case 6:
                textoGuion.text = "¡Oh!, ¿qué es eso?";
                textoAyuda.text = "Haz clic en los interruptores";
                etapaPuzzle1 = EtapaPuzzle1.Busqueda;
                interruptor.SetActive(true);
                break;
        }
    }

    /// <summary>
    /// Llamado al hacer clic en un interruptor en la escena (etapa Busqueda).
    /// nombreObjeto: "Interruptor1", "Interruptor2", ...
    /// </summary>
    public void OnClicInterruptor(string nombreObjeto, GameObject interruptorEnEscena)
    {
        if (etapaPuzzle1 != EtapaPuzzle1.Busqueda) return;
        if (InventarioManager.instance.TieneObjeto(nombreObjeto)) return;

        if (InventarioManager.instance.AgregarObjeto(nombreObjeto))
        {
            if (interruptorEnEscena != null) interruptorEnEscena.SetActive(false);
            textoGuion.text = "Recolectaste un interruptor, revisa tu inventario";
            textoAyuda.text = "Haz clic en el inventario";
            var menu = MenuGameplay.instance ?? Object.FindAnyObjectByType<MenuGameplay>();
            if (menu != null) menu.ActualizarInventario();
        }
    }

    public void OnInventarioAbierto()
    {
        if (etapaPuzzle1 == EtapaPuzzle1.Busqueda || etapaPuzzle1 == EtapaPuzzle1.Interruptores)
        {
            textoGuion.text = "Coloca los interruptores";
            textoAyuda.text = "Selecciona y arrastra los interruptores";
        }
    }

    public void OnInventarioCerrado()
    {
        if (etapaPuzzle1 == EtapaPuzzle1.Luz)
        {
            textoGuion.text = "Enciende la luz";
            textoAyuda.text = "Haz clic en los interruptores";
        }
    }

    public void OnInterruptorColocado()
    {
        interruptoresColocados++;
        if (textoGuion != null) textoGuion.text = $"{interruptoresColocados} interruptor(es) colocado(s).";
        if (interruptoresColocados >= totalInterruptoresParaColocar)
        {
            etapaPuzzle1 = EtapaPuzzle1.Luz;
            Destroy(zonaInterruptor);
        }
    }
    public void OnClicInterruptorColocado()
    {
        if (etapaPuzzle1 != EtapaPuzzle1.Luz || luzEncendida) return;

        if (etapaPuzzle1 != EtapaPuzzle1.Luz || luzEncendida) return;

        luzEncendida = true;
        if (tablero != null) tablero.SetActive(true);
        textoGuion.text = "¿Y ese tablero?";
        textoAyuda.text = "Haz clic en el tablero";
    }
    public void EntrarPuzzle2()
    {
        escena1Habitacion.SetActive(false);
        personaje.SetActive(false);
        escena2Tablero.SetActive(true);
        puzzle2.SetActive(true);
        textoGuion.text = "Es un rompecabezas, ¿Podrás resolverlo?";
        textoAyuda.text = "Selecciona y arrastra las piezas";
    }

    public void SalirPuzzle2()
    {
        escena1Habitacion.SetActive(false);
        personaje.SetActive(true);
        escena2Tablero.SetActive(false);
        puzzle2.SetActive(false);
        escena3salida.SetActive(true);
        textoGuion.text = "Felicidades, Lograste completar el puzle";
        textoAyuda.text = "Haz clic en el fragmento";
    }

    /// <summary>
    /// Llamado por las drop zones cuando se coloca una pieza correcta.
    /// </summary>
    public void OnPiezaPuzzle2Colocada()
    {
        piezasColocadasPuzzle2++;
        if (piezasColocadasPuzzle2 >= totalPiezas)
        {
            imagenConFrase.SetActive(true);
            volver.SetActive(true);
        }
    }

    public void OnClicOruga()
    {
        textoGuion.text = "Recolecta el fragmento";
        textoAyuda.text = "";
    }

    /// <summary>
    /// Se llama cuando se hace clic en el Fragmento
    /// </summary>
    public void OnClicFragmento()
    {
        puerta.GetComponent<ZonaInteractuable>().SetInteraccionesActivas(true);
        // Verificar si ya está en el inventario
        if (InventarioManager.instance.TieneObjeto("Fragmento1"))
        {
            puerta.GetComponent<ZonaInteractuable>().SetInteraccionesActivas(true);
            textoGuion.text = "Has guardado el fragmento en el inventario";
            textoAyuda.text = "Sal de la habitación";
            return;
        }

        // Agregar al inventario
        if (InventarioManager.instance.AgregarObjeto("Fragmento1"))
        {
            fragmento1.SetActive(false);
            puerta.GetComponent<ZonaInteractuable>().SetInteraccionesActivas(true);
            textoGuion.text = "Has guardado el fragmento en el inventario";
            textoAyuda.text = "Sal de la habitación";
        }
    }
}
