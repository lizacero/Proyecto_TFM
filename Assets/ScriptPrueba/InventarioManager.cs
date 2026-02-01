using System.Collections.Generic;
using UnityEngine;

public class InventarioManager : MonoBehaviour
{
    // Singleton para acceso global
    public static InventarioManager instance;

    // Lista simple de objetos recolectados (por nombre)
    private HashSet<string> objetosRecolectados;

    private void Awake()
    {
        // Asegurar que solo hay una instancia
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Persistir entre escenas
            objetosRecolectados = new HashSet<string>();
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Agrega un objeto al inventario por su nombre
    /// </summary>
    public bool AgregarObjeto(string nombreObjeto)
    {
        if (objetosRecolectados == null)
        {
            objetosRecolectados = new HashSet<string>();
        }

        if (objetosRecolectados.Contains(nombreObjeto))
        {
            Debug.Log($"[Inventario] El objeto '{nombreObjeto}' ya está en el inventario.");
            return false;
        }

        objetosRecolectados.Add(nombreObjeto);
        Debug.Log($"[Inventario] Objeto '{nombreObjeto}' agregado al inventario.");
        return true;
    }

    /// <summary>
    /// Verifica si un objeto está en el inventario
    /// </summary>
    public bool TieneObjeto(string nombreObjeto)
    {
        if (objetosRecolectados == null)
        {
            return false;
        }
        return objetosRecolectados.Contains(nombreObjeto);
    }

    /// <summary>
    /// Obtiene todos los objetos del inventario
    /// </summary>
    public List<string> ObtenerObjetos()
    {
        if (objetosRecolectados == null)
        {
            return new List<string>();
        }
        return new List<string>(objetosRecolectados);
    }

    /// <summary>
    /// Limpia todo el inventario (útil para "Nuevo Juego")
    /// </summary>
    public void LimpiarInventario()
    {
        if (objetosRecolectados != null)
        {
            objetosRecolectados.Clear();
            Debug.Log("[Inventario] Inventario limpiado.");
        }
    }

    /// <summary>
    /// Obtiene el número de objetos en el inventario
    /// </summary>
    public int CantidadObjetos()
    {
        if (objetosRecolectados == null)
        {
            return 0;
        }
        return objetosRecolectados.Count;
    }
}