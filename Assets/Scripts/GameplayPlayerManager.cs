using UnityEngine;

public class GameplayPlayerManager : MonoBehaviour
{
    // Va en las escenas de juego.
    // Lee de PlayerPrefs el personaje que eligió el jugador en el menú y activa solo ese.
    [SerializeField] private GameObject[] personajes; // [0] = personaje1, [1] = personaje2

    // Claves para PlayerPrefs (deben coincidir con las de NuevoJuegoManager)
    private const string KEY_PERSONAJE = "PersonajeSeleccionado";
    private const string KEY_NOMBRE = "NombreJugador";

    private int personajeSeleccionado = 0;
    private string nombreJugador = "";

    void Start()
    {
        personajeSeleccionado = PlayerPrefs.GetInt(KEY_PERSONAJE, 0);
        nombreJugador = PlayerPrefs.GetString(KEY_NOMBRE, "Jugador");
        ActivarPersonajeSeleccionado();
    }

    // Desactiva todos y activa solo el elegido
    private void ActivarPersonajeSeleccionado()
    {
        if (personajes == null || personajes.Length == 0) return;

        for (int i = 0; i < personajes.Length; i++)
        {
            if (personajes[i] != null)
                personajes[i].SetActive(i == personajeSeleccionado - 1);
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
