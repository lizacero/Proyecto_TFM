using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AjustesDescriptionManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI descripcionText;
    
    // Texto por defecto que se muestra cuando no hay ninguna opción seleccionada
    [SerializeField] private string textoPorDefecto = "Pasa el cursor sobre las opciones.";
    
    // Diccionario que almacena todas las descripciones
    private Dictionary<string, string> descripciones;
    
    void Start()
    {
        InicializarDescripciones();
        
        // Establecer el texto por defecto al inicio
        if (descripcionText != null)
        {
            descripcionText.text = textoPorDefecto;
        }
    }
    
    // Inicializa el diccionario con todas las descripciones de las opciones de ajustes
    private void InicializarDescripciones()
    {
        descripciones = new Dictionary<string, string>();
        
        // Descripciones de Sonido
        descripciones["VolGeneral"] = "Controla el volumen general del juego. Ajusta este valor para subir o bajar el sonido de todos los elementos.";
        descripciones["VolMusica"] = "Controla el volumen de la música de fondo del juego.";
        descripciones["VolEfectos"] = "Controla el volumen de los efectos de sonido.";
        descripciones["VolCinematica"] = "Controla el volumen durante las secuencias cinematográficas.";
        
        // Descripción de Video
        descripciones["Resolucion"] = "Cambia la resolución de la pantalla. Resoluciones más altas ofrecen mejor calidad visual pero requieren más rendimiento.";
        descripciones["HUD"] = "Cambia el tamaño de los elementos de la interfaz (HUD).";
        
        // Descripción de Texto
        descripciones["Idioma"] = "Cambia el idioma.";
        descripciones["Texto"] = "Cambia tamaño del texto.";
        
        
        // Descripción del botón predeterminado
        descripciones["Aplicar"] = "Aplica los cambios de los ajustes realizados.";
        descripciones["Predeterminado"] = "Restaura todos los ajustes a sus valores predeterminados.";
    }
    
    /// Muestra la descripción correspondiente a la clave proporcionada
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
    
    /// Limpia la descripción y vuelve al texto por defecto
    public void LimpiarDescripcion()
    {
        if (descripcionText != null)
        {
            descripcionText.text = textoPorDefecto;
        }
    }
}



