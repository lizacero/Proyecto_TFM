using System.Collections.Generic;
using UnityEngine;
using TMPro;

// Gestiona el texto de descripción del menú de Ajustes.
// Cuando se pasa el cursor por una opción se pone el texto correspondiente en el panel de descripción.
public class AjustesDescriptionManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI descripcionText;
    [SerializeField] private string textoPorDefecto = "Pasa el cursor sobre las opciones.";
    
    // Diccionario que almacena todas las descripciones
    private Dictionary<string, string> descripciones;
    
    void Start()
    {
        InicializarDescripciones();
        if (descripcionText != null) descripcionText.text = textoPorDefecto;
    }

    // Llena el diccionario con todas las claves y textos de las opciones
    private void InicializarDescripciones()
    {
        descripciones = new Dictionary<string, string>();
        
        // Descripciones de Sonido
        descripciones["VolGeneral"] = "Controla el volumen general del juego. Ajusta este valor para subir o bajar el sonido de todos los elementos. \n(No implementado)";
        descripciones["VolMusica"] = "Controla el volumen de la música de fondo del juego.";
        descripciones["VolEfectos"] = "Controla el volumen de los efectos de sonido. \n(No implementado)";
        descripciones["VolCinematica"] = "Controla el volumen durante las secuencias cinematográficas. \n(No implementado)";
        
        // Descripción de Video
        descripciones["Resolucion"] = "Cambia la resolución de la pantalla. Resoluciones más altas ofrecen mejor calidad visual pero requieren más rendimiento. \n(No implementado)";
        descripciones["HUD"] = "Cambia el tamaño de los elementos de la interfaz (HUD).\n(No implementado)";
        
        // Descripción de Texto
        descripciones["Idioma"] = "Cambia el idioma.\n(No implementado)";
        descripciones["Texto"] = "Cambia tamaño del texto.\n(No implementado)";
        
        
        // Descripción del botón predeterminado
        descripciones["Aplicar"] = "Aplica los cambios de los ajustes realizados.\n(No implementado)";
        descripciones["Predeterminado"] = "Restaura todos los ajustes a sus valores predeterminados.\n(No implementado)";
    }

    // Recibe una clave y pone en descripcionText el texto asociado.
    // Si no existe la clave, pone el texto por defecto.
    public void MostrarDescripcion(string clave)
    {
        if (descripcionText == null)
        {
            Debug.LogWarning("AjustesDescriptionManager: No se ha asignado el componente TextMeshProUGUI de descripción.");
            return;
        }
        
        if (descripciones != null && descripciones.ContainsKey(clave))
        {
            descripcionText.text = descripciones[clave];
        }
        else
        {
            Debug.LogWarning($"AjustesDescriptionManager: No se encontró descripción para la clave '{clave}'.");
            descripcionText.text = textoPorDefecto;
        }
    }

    // Vuelve a mostrar el texto por defecto cuando el cursor sale de la opción.
    public void LimpiarDescripcion()
    {
        if (descripcionText != null)
        {
            descripcionText.text = textoPorDefecto;
        }
    }
}



