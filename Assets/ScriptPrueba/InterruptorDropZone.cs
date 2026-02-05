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

        var arrastrable = eventData.pointerDrag.GetComponent<ObjetoInventarioArrastrable>();
        if (arrastrable == null) return;

        string nombre = arrastrable.nombreObjeto;
        if (!InventarioManager.instance.TieneObjeto(nombre)) return;

        InventarioManager.instance.QuitarObjeto(nombre);
        arrastrable.nombreObjeto = "";

        arrastrable.RestaurarParent();
        arrastrable.gameObject.SetActive(false);

        //Destroy(arrastrable.gameObject);
        if (interruptorColocadoVisual != null) interruptorColocadoVisual.SetActive(true);

        var menu = MenuGameplay.instance ?? Object.FindAnyObjectByType<MenuGameplay>();
        if (menu != null) menu.ActualizarInventario();

        if (puerta1Manager != null) puerta1Manager.OnInterruptorColocado();
    }
}
