using UnityEngine;

public class Puerta5Manager : MonoBehaviour
{
    [Header("Referencias Objetos")]
    [SerializeField] private GameObject fragmento5;
    [SerializeField] private GameObject puerta;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        puerta.GetComponent<ZonaInteractuable>().SetInteraccionesActivas(false);
        // Si ya tenemos el Fragmento en el inventario, ocultarlo
        if (InventarioManager.instance.TieneObjeto("Fragmento5"))
        {
            if (fragmento5 != null)
            {
                fragmento5.SetActive(false);
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
        if (InventarioManager.instance.TieneObjeto("Fragmento5"))
        {
            Debug.Log("[Puerta1] Ya tienes el Fragmento en el inventario.");
            return;
        }

        // Agregar al inventario
        else if (InventarioManager.instance.AgregarObjeto("Fragmento5"))
        {
            Debug.Log("[Puerta1] Fragmento recolectado y agregado al inventario.");

            // Ocultar el objeto en la escena
            if (fragmento5 != null)
            {
                fragmento5.SetActive(false);
            }
        }
    }
}
