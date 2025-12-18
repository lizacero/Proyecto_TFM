using UnityEngine;

public class ZonaInteractuable : MonoBehaviour
{
    [SerializeField] private ControlCursor controlCursor;
    [SerializeField] private PreGameManager preGameManager;
    [SerializeField] private string tipoObjeto = "";

    private bool interaccionesActivas = true;

    private void Awake()
    {
        controlCursor = FindAnyObjectByType<ControlCursor>();
        preGameManager = FindAnyObjectByType<PreGameManager>();
    }

    // Permite activar/desactivar interacciones desde fuera
    public void SetInteraccionesActivas(bool valor)
    {
        interaccionesActivas = valor;
    }

    private void OnMouseEnter()
    {
        if (!interaccionesActivas) return;
        if (controlCursor != null)
        {
            controlCursor.CambiarCursor("Mano");
        }
    }

    private void OnMouseExit()
    {
        if (!interaccionesActivas) return;
        if (controlCursor != null)
        {
            controlCursor.CambiarCursor("Normal");
        }
    }


    /// Se ejecuta cuando se hace clic en el objeto
    private void OnMouseDown()
    {
        if (!interaccionesActivas) return;
        if (controlCursor == null)
        {
            Debug.LogWarning($"ZonaSensible en '{gameObject.name}': No se ha asignado el ControlCursor.");
            return;
        }

        // Obtener el tipo de objeto del nombre del GameObject si no está asignado
        string objeto = tipoObjeto;
        if (string.IsNullOrEmpty(objeto))
        {
            objeto = gameObject.name.ToLower();
        }

        // Si hay un PreGameScene, notificarle el clic primero
        if (preGameManager != null)
        {
            if (objeto.Contains("ventana2"))
            {
                preGameManager.OnClicVentana2();
                return; // El director maneja todo
            }
            else if (objeto.Contains("ventana"))
            {
                preGameManager.OnClicVentana();
                return; // El director maneja todo
            }
            else if (objeto.Contains("estrella"))
            {
                preGameManager.OnClicEstrella();
                return; // El director maneja todo
            }
            else if (objeto.Contains("papel"))
            {
                preGameManager.OnClicPapel();
                return; // El director maneja todo
            }
            else if (objeto.Contains("cama"))
            {
                preGameManager.OnClicCama();
                return; // El director maneja todo
            }
            
        }

    }
}
