using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Puzzle2Pieza : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public string idPieza = "Pieza1";           // En piezas: su id. En slots: la id que aceptan.
    public bool esSlot = false;                  // true = zona de drop, false = pieza arrastrable

    [SerializeField] private Puerta1Manager puerta1Manager;
    [SerializeField] private GameObject visualColocado;  // Solo en slots: imagen al colocar bien

    private RectTransform rectTransform;
    private Canvas canvas;
    private CanvasGroup canvasGroup;
    private Transform parentOriginal;
    private bool colocada = false;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null) canvasGroup = gameObject.AddComponent<CanvasGroup>();
        if (puerta1Manager == null) puerta1Manager = FindAnyObjectByType<Puerta1Manager>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (esSlot || colocada) return;
        parentOriginal = transform.parent;
        transform.SetParent(canvas.transform);
        if (canvasGroup != null) canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (esSlot || colocada) return;
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (esSlot || colocada) return;
        transform.SetParent(parentOriginal);
        if (canvasGroup != null) canvasGroup.blocksRaycasts = true;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (!esSlot || eventData.pointerDrag == null) return;

        var pieza = eventData.pointerDrag.GetComponent<Puzzle2Pieza>();
        if (pieza == null || pieza.esSlot || pieza.colocada) return;
        if (pieza.idPieza != idPieza) return;  // idPieza en slot = id que acepta

        pieza.Colocar(transform);
        if (visualColocado != null) visualColocado.SetActive(true);
        if (puerta1Manager != null) puerta1Manager.OnPiezaPuzzle2Colocada();
    }

    private void Colocar(Transform nuevoParent)
    {
        colocada = true;
        if (canvasGroup != null) canvasGroup.blocksRaycasts = true;
        transform.SetParent(nuevoParent);
        rectTransform.anchoredPosition = Vector2.zero;
        rectTransform.localScale = Vector3.one;
    }
}
