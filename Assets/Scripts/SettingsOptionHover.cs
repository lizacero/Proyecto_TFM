using UnityEngine;
using UnityEngine.EventSystems;

public class SettingsOptionHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private AjustesDescriptionManager descriptionManager;
    [SerializeField] private string descripcionKey;
    
    // Se ejecuta cuando el cursor entra en el área del componente
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (descriptionManager != null && !string.IsNullOrEmpty(descripcionKey))
        {
            descriptionManager.MostrarDescripcion(descripcionKey);
        }
        else
        {
            if (descriptionManager == null)
            {
                Debug.LogWarning($"SettingsOptionHover en '{gameObject.name}': No se ha asignado el AjustesDescriptionManager.");
            }
            if (string.IsNullOrEmpty(descripcionKey))
            {
                Debug.LogWarning($"SettingsOptionHover en '{gameObject.name}': No se ha asignado una clave de descripción.");
            }
        }
    }
    
    // Se ejecuta cuando el cursor sale del área del componente
    public void OnPointerExit(PointerEventData eventData)
    {
        if (descriptionManager != null)
        {
            descriptionManager.LimpiarDescripcion();
        }
    }
}



