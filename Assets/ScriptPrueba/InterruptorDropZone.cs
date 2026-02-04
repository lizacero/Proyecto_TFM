using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InterruptorDropZone : MonoBehaviour, IDropHandler
{
    //[SerializeField] private string interruptorEsperado = "Interruptor1";  // Opcional: validar cuál va aquí
    [SerializeField] private Puerta1Manager puerta1Manager;
    [SerializeField] private GameObject interruptorColocadoVisual;  // Sprite/objeto que se muestra al colocar

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null) return;

        var arrastrable = eventData.pointerDrag.GetComponent<FragmentoArrastrable>();
        if (arrastrable == null) return;

        string nombre = arrastrable.nombreObjeto;
        //if (string.IsNullOrEmpty(nombre) || !nombre.StartsWith("Interruptor")) return;
        if (!InventarioManager.instance.TieneObjeto(nombre)) return;

        // Opcional: si quieres que solo el Interruptor1 vaya en esta ranura, descomenta:
        // if (nombre != interruptorEsperado) return;

        InventarioManager.instance.QuitarObjeto(nombre);
        if (interruptorColocadoVisual != null) interruptorColocadoVisual.SetActive(true);

        var menu = MenuGameplay.instance ?? Object.FindAnyObjectByType<MenuGameplay>();
        if (menu != null) menu.ActualizarInventario();

        if (puerta1Manager != null) puerta1Manager.OnInterruptorColocado();
    }
}
