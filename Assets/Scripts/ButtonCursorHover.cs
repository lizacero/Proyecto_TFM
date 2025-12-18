using UnityEngine;
using UnityEngine.EventSystems;

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
                    "Asegúrate de que existe un GameObject con el script ControlCursor.");
            }
        }
    }

    /// <summary>
    /// Se ejecuta cuando el cursor entra en el botón
    /// </summary>
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (controlCursor != null)
        {
            controlCursor.CambiarCursor("Mano");
        }
    }

    /// <summary>
    /// Se ejecuta cuando el cursor sale del botón
    /// </summary>
    public void OnPointerExit(PointerEventData eventData)
    {
        if (controlCursor != null)
        {
            controlCursor.CambiarCursor("Normal");
        }
    }
}
