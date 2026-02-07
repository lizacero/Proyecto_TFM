using TMPro;
using UnityEngine;

public class Puerta2Manager : MonoBehaviour
{
    private enum Etapa { WaitFragmento, WaitPuerta }

    [Header("Referencias UI")]
    [SerializeField] private TextMeshProUGUI textoGuion;
    [SerializeField] private TextMeshProUGUI textoAyuda;

    [Header("Referencias Objetos")]
    [SerializeField] private GameObject fragmento2;
    [SerializeField] private GameObject puerta;
    [SerializeField] private GameObject oruga;

    private Etapa etapaActual = Etapa.WaitFragmento;

    void Start()
    {
        puerta.GetComponent<ZonaInteractuable>().SetInteraccionesActivas(false);

        if (InventarioManager.instance.TieneObjeto("Fragmento2"))
            fragmento2.SetActive(false);

        textoGuion.text = "Entraste a la habitación de la tristeza";
        textoAyuda.text = "Habitación en construcción";

        if (oruga != null)
            oruga.GetComponent<ZonaInteractuable>().SetInteraccionesActivas(true);
    }

    public void OnClicOruga()
    {
        if (etapaActual != Etapa.WaitFragmento) return;

        textoGuion.text = "Recolecta el fragmento";
        textoAyuda.text = "Haz clic en el fragmento";
    }

    /// <summary>
    /// Se llama cuando se hace clic en el Fragmento
    /// </summary>
    public void OnClicFragmento()
    {
        if (InventarioManager.instance.TieneObjeto("Fragmento2"))
        {
            puerta.GetComponent<ZonaInteractuable>().SetInteraccionesActivas(true);
            textoGuion.text = "Has guardado el fragmento en el inventario";
            textoAyuda.text = "Sal de la habitación";
            return;
        }

        if (InventarioManager.instance.AgregarObjeto("Fragmento2"))
        {
            fragmento2.SetActive(false);
            puerta.GetComponent<ZonaInteractuable>().SetInteraccionesActivas(true);
            textoGuion.text = "Has guardado el fragmento en el inventario";
            textoAyuda.text = "Sal de la habitación";
            etapaActual = Etapa.WaitPuerta;
        }
    }
}
