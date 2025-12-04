using UnityEngine;

public class ZonaSensible : MonoBehaviour
{
    public ControlCursor controlCursor;

    //Tipo de objeto
    [SerializeField] private string tipoObjeto ="";

    private void OnMouseEnter()
    {
        if (controlCursor != null)
        {
            controlCursor.CambiarCursor("Mano");
        }
    }

    private void OnMouseExit()
    {
        if (controlCursor != null)
        {
            controlCursor.CambiarCursor("Normal");
        }
    }


    /// Se ejecuta cuando se hace clic en el objeto
    private void OnMouseDown()
    {
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

        // Llamar al método correspondiente según el tipo de objeto
        if (objeto.Contains("ventana"))
        {
            controlCursor.Ventana();
        }
        else if (objeto.Contains("cama"))
        {
            controlCursor.Cama();
        }
        else if (objeto.Contains("mesa"))
        {
            controlCursor.Mesa();
        }
        else
        {
            Debug.LogWarning($"ZonaSensible: No se reconoció el tipo de objeto '{objeto}'. Asegúrate de asignar el tipo en el Inspector.");
        }
    }
}
