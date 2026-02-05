using TMPro;
using UnityEngine;

public class Puerta1Manager : MonoBehaviour
{
    public enum EtapaPuzzle1
    {
        Inicio,        // Interactuar con muebles, no pasa nada; tras N clics → Busqueda
        Busqueda,       // Aparecen interruptores; se recogen al inventario
        Interruptores,  // Drag & drop de interruptores a zonas
        Luz             // Clic en interruptores colocados → luz y tablero
    }

    [Header("Puzzle 1 - Estados")]
    [SerializeField] private int clicsParaRevelarInterruptores = 5;  // Ajustable
    private int contadorClicsInicio = 0;
    private EtapaPuzzle1 etapaPuzzle1 = EtapaPuzzle1.Inicio;

    [Header("Puzzle 1 - Escenas")]
    [SerializeField] private GameObject escena1Habitacion;

    [Header("Puzzle 1 - Objetos")]
    [SerializeField] private GameObject[] interruptoresEnEscena;  // Los que aparecen en Busqueda
    [SerializeField] private GameObject interruptor;
    [SerializeField] private GameObject zonaInterruptor;
    [SerializeField] private GameObject luz;                      // Se activa en Luz
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
    [SerializeField] private int totalPiezas = 4;
    [SerializeField] private GameObject imagenConFrase;
    [SerializeField] private GameObject volver;
    private int piezasColocadasPuzzle2 = 0;

    [Header("Puzzle 3 - Escenas")]
    [SerializeField] private GameObject escena3salida;

    [Header("Puzzle 3 - Objetos")]
    [SerializeField] private GameObject fragmento1;
    [SerializeField] private GameObject puerta;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
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
        // Si ya tenemos el Fragmento en el inventario, ocultarlo
        if (InventarioManager.instance.TieneObjeto("Fragmento1"))
        {
            if (fragmento1 != null)
            {
                fragmento1.SetActive(false);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Llamado desde ZonaInteractuable al hacer clic en muebles/objetos en etapa Inicio.
    /// </summary>
    public void OnClicMueble()
    {
        if (etapaPuzzle1 != EtapaPuzzle1.Inicio) return;

        if (textoGuion != null) textoGuion.text = "No ocurre nada.";
        if (textoAyuda != null) textoAyuda.text = "Sigue explorando...";

        contadorClicsInicio++;
        if (contadorClicsInicio >= clicsParaRevelarInterruptores)
        {
            etapaPuzzle1 = EtapaPuzzle1.Busqueda;
            RevelarInterruptores();
        }
    }

    private void RevelarInterruptores()
    {
        interruptor.SetActive(true);
        //if (interruptoresEnEscena == null) return;
        //foreach (var o in interruptoresEnEscena)
        //    if (o != null) o.SetActive(true);
        if (textoGuion != null) textoGuion.text = "Han aparecido interruptores. Recógelos.";
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
            var menu = MenuGameplay.instance ?? Object.FindAnyObjectByType<MenuGameplay>();
            if (menu != null) menu.ActualizarInventario();
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

        luzEncendida = true;
        //if (luz != null) luz.SetActive(true);
        if (tablero != null) tablero.SetActive(true);
        if (textoGuion != null) textoGuion.text = "Se enciende la luz. El tablero queda a la vista.";
    }
    public void EntrarPuzzle2()
    {
        if (escena1Habitacion != null) escena1Habitacion.SetActive(false);
        if (escena2Tablero != null) escena2Tablero.SetActive(true);
        if (puzzle2 != null) puzzle2.SetActive(true);
    }

    public void SalirPuzzle2()
    {
        if (escena1Habitacion != null) escena1Habitacion.SetActive(false);
        if (escena2Tablero != null) escena2Tablero.SetActive(false);
        if (puzzle2 != null) puzzle2.SetActive(false);
        if (escena3salida!=null) escena3salida.SetActive(true);
    }

    /// <summary>
    /// Llamado por las drop zones cuando se coloca una pieza correcta.
    /// </summary>
    public void OnPiezaPuzzle2Colocada()
    {
        piezasColocadasPuzzle2++;
        if (piezasColocadasPuzzle2 >= totalPiezas && imagenConFrase != null)
        {
            imagenConFrase.SetActive(true);
            volver.SetActive(true);
        }

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
            Debug.Log("[Puerta1] Ya tienes el Fragmento en el inventario.");
            return;
        }

        // Agregar al inventario
        else if (InventarioManager.instance.AgregarObjeto("Fragmento1"))
        {
            Debug.Log("[Puerta1] Fragmento recolectado y agregado al inventario.");

            // Ocultar el objeto en la escena
            if (fragmento1 != null)
            {
                fragmento1.SetActive(false);
            }
        }
    }
}
