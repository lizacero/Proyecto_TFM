using UnityEngine;

public class Puerta4Manager : MonoBehaviour
{
    [Header("Referencias Objetos")]
    [SerializeField] private GameObject fragmento4;
    [SerializeField] private GameObject puerta;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        puerta.GetComponent<ZonaInteractuable>().SetInteraccionesActivas(false);
        // Si ya tenemos el Fragmento en el inventario, ocultarlo
        if (InventarioManager.instance.TieneObjeto("Fragmento4"))
        {
            if (fragmento4 != null)
            {
                fragmento4.SetActive(false);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Se llama cuando se hace clic en el Fragmento
    /// </summary>
    public void OnClicFragmento()
    {
        puerta.GetComponent<ZonaInteractuable>().SetInteraccionesActivas(true);
        // Verificar si ya está en el inventario
        if (InventarioManager.instance.TieneObjeto("Fragmento4"))
        {
            Debug.Log("[Puerta1] Ya tienes el Fragmento en el inventario.");
            return;
        }

        // Agregar al inventario
        else if (InventarioManager.instance.AgregarObjeto("Fragmento4"))
        {
            Debug.Log("[Puerta1] Fragmento recolectado y agregado al inventario.");

            // Ocultar el objeto en la escena
            if (fragmento4 != null)
            {
                fragmento4.SetActive(false);
            }
        }
    }
}
