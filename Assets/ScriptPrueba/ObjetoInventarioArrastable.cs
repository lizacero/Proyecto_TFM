using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// Permite arrastrar un objeto desde el inventario.
/// Debe ir en cada Image que representa un slot.
/// </summary>
public class ObjetoInventarioArrastrable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public string nombreObjeto;  // "Fragmento1", "Fragmento2", etc.

    private RectTransform rectTransform;
    private Canvas canvas;
    private CanvasGroup canvasGroup;
    private Transform parentOriginal;

    //Propiedad pública para acceder al parent original
    public Transform ParentOriginal => parentOriginal;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        parentOriginal = transform.parent;
        transform.SetParent(canvas.transform);
        if (canvasGroup != null)
            canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (transform.parent != parentOriginal && parentOriginal != null)
        {
            transform.SetParent(parentOriginal);
        }
        if (canvasGroup != null)
            canvasGroup.blocksRaycasts = true;
    }

    public void RestaurarParent()
    {
        if (parentOriginal != null)
        {
            transform.SetParent(parentOriginal);
            if (canvasGroup != null)
                canvasGroup.blocksRaycasts = true;
        }
    }
}
