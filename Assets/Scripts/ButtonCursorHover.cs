using UnityEngine;
using UnityEngine.EventSystems;

// Se pone en botones o UI que tengan collider/raycast.
// Al pasar el cursor por encima cambia a la mano, al salir, vuelve al cursor normal.
// Usa ControlCursor para eso.
public class ButtonCursorHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Referencia al ControlCursor")]
    [Tooltip("Si está vacío, buscará automáticamente en la escena")]
    [SerializeField] private ControlCursor controlCursor;

    private void Awake()
    {
        // Si no se asignó manualmente, buscar en la escena
        if (controlCursor == null)
        {
            controlCursor = FindAnyObjectByType<ControlCursor>();

            if (controlCursor == null)
            {
                Debug.LogWarning($"ButtonCursorHover en '{gameObject.name}': No se encontró ControlCursor en la escena. " +
                    "Debe existir un GameObject con el script ControlCursor.");
            }
        }
    }

    // Cuando el cursor entra en el área del botón se cambia a cursor mano
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (controlCursor != null) controlCursor.CambiarCursor("Mano");
    }

    // Se ejecuta cuando el cursor sale del botón, vuelve a cursor normal
    public void OnPointerExit(PointerEventData eventData)
    {
        if (controlCursor != null) controlCursor.CambiarCursor("Normal");
    }
}
