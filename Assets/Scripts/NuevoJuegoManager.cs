using UnityEngine;
using TMPro;

public class NuevoJuegoManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputFieldNombre;
    //[SerializeField] private Button btnPersonaje1;
    //[SerializeField] private Button btnPersonaje2;

    // Personaje seleccionado: 1 o 2
    private int personajeSeleccionado = 0;
    private string nombreJugador = "";

    // Claves para PlayerPrefs
    private const string KEY_PERSONAJE = "PersonajeSeleccionado";
    private const string KEY_NOMBRE = "NombreJugador";

    void Start()
    {
        // Limpiar selección anterior al iniciar
        personajeSeleccionado = 0;
        nombreJugador = "";

    }

    //Botón. Se llama cuando se selecciona el Personaje 1
    public void SeleccionarPersonaje1()
    {
        personajeSeleccionado = 1;
        Debug.Log("Personaje 1 seleccionado");
    }

    //Botón Se llama cuando se selecciona el Personaje 2
    public void SeleccionarPersonaje2()
    {
        personajeSeleccionado = 2;
        Debug.Log("Personaje 2 seleccionado");
    }

    // Obtiene el nombre del campo de texto y lo guarda
    public void GuardarNombre()
    {
        if (inputFieldNombre != null)
        {
            nombreJugador = inputFieldNombre.text;

            // Si el nombre está vacío, asignar un nombre por defecto
            if (string.IsNullOrEmpty(nombreJugador.Trim()))
            {
                nombreJugador = "Jugador";
                if (inputFieldNombre != null)
                {
                    inputFieldNombre.text = nombreJugador;
                }
            }
        }
        else
        {
            nombreJugador = "Jugador"; // Nombre por defecto si no hay input field
        }
    }

    /// Guarda todos los datos en PlayerPrefs y retorna true si todo está listo
    public bool GuardarDatosYValidar()
    {
        // Obtener el nombre del input field
        GuardarNombre();

        // Validar que se haya seleccionado un personaje
        if (personajeSeleccionado == 0)
        {
            Debug.LogWarning("Por favor, selecciona un personaje antes de jugar.");
            return false;
        }

        // Validar que haya un nombre (aunque ya tiene por defecto)
        if (string.IsNullOrEmpty(nombreJugador))
        {
            nombreJugador = "Jugador";
        }

        // Guardar en PlayerPrefs
        PlayerPrefs.SetInt(KEY_PERSONAJE, personajeSeleccionado);
        PlayerPrefs.SetString(KEY_NOMBRE, nombreJugador);
        PlayerPrefs.Save(); // Guardar inmediatamente

        if (InventarioManager.instance != null)
            InventarioManager.instance.LimpiarInventario();
        BaulSceneState.ResetearTodo();


        Debug.Log($"Datos guardados - Personaje: {personajeSeleccionado}, Nombre: {nombreJugador}");

        return true;
    }

    // Obtiene el personaje seleccionado actualmente (sin guardar)
    public int GetPersonajeSeleccionado()
    {
        return personajeSeleccionado;
    }

    // Obtiene el nombre ingresado actualmente (sin guardar)
    public string GetNombreJugador()
    {
        if (inputFieldNombre != null && !string.IsNullOrEmpty(inputFieldNombre.text))
        {
            return inputFieldNombre.text;
        }
        return nombreJugador;
    }
}

