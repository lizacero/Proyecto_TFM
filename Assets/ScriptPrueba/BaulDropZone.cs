using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Zona de drop sobre el baúl. Solo funciona en la escena Baul.
/// </summary>
public class BaulDropZone : MonoBehaviour, IDropHandler
{
    [SerializeField] private TextMeshProUGUI textoGuion;
    [SerializeField] private BaulScene baulScene;

    private const int TOTAL_FRAGMENTOS = 5;

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null) return;

        var arrastrable = eventData.pointerDrag.GetComponent<ObjetoInventarioArrastrable>();
        if (arrastrable == null) return;

        string nombre = arrastrable.nombreObjeto;
        if (string.IsNullOrEmpty(nombre) || !nombre.StartsWith("Fragmento")) return;
        if (!InventarioManager.instance.TieneObjeto(nombre)) return;

        InventarioManager.instance.QuitarObjeto(nombre);
        BaulSceneState.FragmentosGuardadosEnBaul++;

        ActualizarTexto();
        MenuGameplay.instance.ActualizarInventario(); // Llamar al método público
        if (BaulSceneState.FragmentosGuardadosEnBaul >= TOTAL_FRAGMENTOS)
        {
            if (baulScene != null)
                baulScene.MostrarPantallaFinal();
        }
    }

    private void ActualizarTexto()
    {
        if (textoGuion == null) return;

        int guardados = BaulSceneState.FragmentosGuardadosEnBaul;
        int faltan = TOTAL_FRAGMENTOS - guardados;

        if (guardados >= TOTAL_FRAGMENTOS)
            textoGuion.text = "Todos los fragmentos han sido recolectados.";
        else
            textoGuion.text = $"{guardados} fragmento(s) guardado(s), faltan {faltan}.";
    }

}