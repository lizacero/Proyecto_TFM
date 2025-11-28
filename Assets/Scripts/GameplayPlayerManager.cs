using UnityEngine;

public class GameplayPlayerManager : MonoBehaviour
{
    [SerializeField] private GameObject personaje1;
    [SerializeField] private GameObject personaje2;

    // Claves para PlayerPrefs (deben coincidir con las de NuevoJuegoManager)
    private const string KEY_PERSONAJE = "PersonajeSeleccionado";
    private const string KEY_NOMBRE = "NombreJugador";

    private int personajeSeleccionado = 0;
    private string nombreJugador = "";

    void Start()
    {
        // Asegurar que se ejecute después de que la escena esté completamente cargada
        CargarDatosJugador();
        ActivarPersonajeSeleccionado();
    }

    // Carga los datos del jugador desde PlayerPrefs
    private void CargarDatosJugador()
    {
        // Cargar personaje seleccionado (1 o 2)
        personajeSeleccionado = PlayerPrefs.GetInt(KEY_PERSONAJE, 0);

        // Cargar nombre del jugador
        nombreJugador = PlayerPrefs.GetString(KEY_NOMBRE, "Jugador");

        Debug.Log($"Datos cargados - Personaje: {personajeSeleccionado}, Nombre: {nombreJugador}");
    }

    // Activa el personaje correspondiente según la selección
    private void ActivarPersonajeSeleccionado()
    {
        // Primero, desactivar ambos personajes por seguridad
        if (personaje1 != null)
        {
            personaje1.SetActive(false);
        }

        if (personaje2 != null)
        {
            personaje2.SetActive(false);
        }

        // Activar el personaje seleccionado
        switch (personajeSeleccionado)
        {
            case 1:
                if (personaje1 != null)
                {
                    personaje1.SetActive(true);
                    Debug.Log("Personaje 1 activado");
                }
                else
                {
                    Debug.LogError("GameplayPlayerManager: No se encontró la referencia al Personaje1.");
                }
                break;

            case 2:
                if (personaje2 != null)
                {
                    personaje2.SetActive(true);
                    Debug.Log("Personaje 2 activado");
                }
                else
                {
                    Debug.LogError("GameplayPlayerManager: No se encontró la referencia al Personaje2.");
                }
                break;

            default:
                Debug.LogWarning($"GameplayPlayerManager: Personaje seleccionado inválido ({personajeSeleccionado}). No se activará ningún personaje.");
                // Opcional: activar personaje por defecto (Personaje1)
                if (personaje1 != null)
                {
                    personaje1.SetActive(true);
                    Debug.Log("Activado Personaje1 por defecto");
                }
                break;
        }
    }

    // Obtiene el personaje seleccionado actualmente
    public int GetPersonajeSeleccionado()
    {
        return personajeSeleccionado;
    }

    // Obtiene el nombre del jugador
    public string GetNombreJugador()
    {
        return nombreJugador;
    }
}
