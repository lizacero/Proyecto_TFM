using UnityEngine;

public class Puerta3Manager : MonoBehaviour
{
    [Header("Referencias Objetos")]
    [SerializeField] private GameObject fragmento3;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Si ya tenemos el Fragmento en el inventario, ocultarlo
        if (InventarioManager.instance.TieneObjeto("Fragmento3"))
        {
            if (fragmento3 != null)
            {
                fragmento3.SetActive(false);
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
        // Verificar si ya está en el inventario
        if (InventarioManager.instance.TieneObjeto("Fragmento3"))
        {
            Debug.Log("[Puerta1] Ya tienes el Fragmento en el inventario.");
            return;
        }

        // Agregar al inventario
        else if (InventarioManager.instance.AgregarObjeto("Fragmento3"))
        {
            Debug.Log("[Puerta1] Fragmento recolectado y agregado al inventario.");

            // Ocultar el objeto en la escena
            if (fragmento3 != null)
            {
                fragmento3.SetActive(false);
            }
        }
    }
}
