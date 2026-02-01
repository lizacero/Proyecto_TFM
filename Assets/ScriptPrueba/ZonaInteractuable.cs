using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ZonaInteractuable : MonoBehaviour
{
    [SerializeField] private ControlCursor controlCursor;
    [SerializeField] private PreGameManager preGameManager;
    [SerializeField] private BaulScene baulScene;
    [SerializeField] private Puerta1Manager puerta1Manager;
    [SerializeField] private Puerta2Manager puerta2Manager;
    [SerializeField] private Puerta3Manager puerta3Manager;
    [SerializeField] private Puerta4Manager puerta4Manager;
    [SerializeField] private Puerta5Manager puerta5Manager;
    [SerializeField] private string tipoObjeto = "";
    [SerializeField] private MovimientoPersonaje movimientoPersonaje;

    private bool interaccionesActivas = true;

    private void Awake()
    {
        controlCursor = FindAnyObjectByType<ControlCursor>();
        preGameManager = FindAnyObjectByType<PreGameManager>();
        baulScene = FindAnyObjectByType<BaulScene>();
        movimientoPersonaje = FindAnyObjectByType<MovimientoPersonaje>();
        puerta1Manager = FindAnyObjectByType<Puerta1Manager>();
        puerta2Manager = FindAnyObjectByType<Puerta2Manager>();
        puerta3Manager = FindAnyObjectByType<Puerta3Manager>();
        puerta4Manager = FindAnyObjectByType<Puerta4Manager>();
        puerta5Manager = FindAnyObjectByType<Puerta5Manager>();
    }

    // Permite activar/desactivar interacciones desde fuera
    public void SetInteraccionesActivas(bool valor)
    {
        interaccionesActivas = valor;
    }

    private void OnMouseEnter()
    {
        if (!interaccionesActivas) return;
        if (controlCursor != null)
        {
            controlCursor.CambiarCursor("Mano");
        }
    }

    private void OnMouseExit()
    {
        if (!interaccionesActivas) return;
        if (controlCursor != null)
        {
            controlCursor.CambiarCursor("Normal");
        }
    }

    /// Se ejecuta cuando se hace clic en el objeto
    private void OnMouseDown()
    {
        if (!interaccionesActivas) return;
        if (controlCursor == null)
        {
            Debug.LogWarning($"ZonaSensible en '{gameObject.name}': No se ha asignado el ControlCursor.");
            return;
        }

        // Obtener el tipo de objeto del nombre del GameObject si no está asignado
        string objeto = tipoObjeto;
        if (string.IsNullOrEmpty(objeto))
        {
            objeto = gameObject.name.ToLower();
        }

        // Si hay un PreGameScene, notificarle el clic primero
        if (preGameManager != null)
        {
            if (objeto.Contains("ventana2"))
            {
                preGameManager.OnClicVentana2();
                return; // El director maneja todo
            }
            else if (objeto.Contains("ventana"))
            {
                preGameManager.OnClicVentana();
                return; // El director maneja todo
            }
            else if (objeto.Contains("estrella"))
            {
                preGameManager.OnClicEstrella();
                return; // El director maneja todo
            }
            else if (objeto.Contains("papel"))
            {
                preGameManager.OnClicPapel();
                return; // El director maneja todo
            }
            else if (objeto.Contains("cama"))
            {
                preGameManager.OnClicCama();
                return; // El director maneja todo
            }
        }

        // Si hay un baulScene, notificarle el clic primero
        else if (baulScene != null)
        {
            if (objeto.Contains("baul"))
            {
                baulScene.OnClicBaul();
                return;
            }
            else if (objeto.Contains("puerta1"))
            {
                baulScene.OnClicPuerta1();
                return;
            }
            else if (objeto.Contains("puerta2"))
            {
                baulScene.OnClicPuerta2();
                return;
            }
            else if (objeto.Contains("puerta3"))
            {
                baulScene.OnClicPuerta3();
                return;
            }
            else if (objeto.Contains("puerta4"))
            {
                baulScene.OnClicPuerta4();
                return;
            }
            else if (objeto.Contains("puerta5"))
            {
                baulScene.OnClicPuerta5();
                return;
            }
            else if (objeto.Contains("oruga"))
            {
                baulScene.OnClicOruga();
                MovimientoPersonaje movimientoPersonaje = FindAnyObjectByType<MovimientoPersonaje>();
                if (movimientoPersonaje != null)
                {
                    movimientoPersonaje.SetMovimientoHabilitado(false);
                    // Rehabilitar el movimiento al final del frame (después de que se procese el clic)
                    StartCoroutine(movimientoPersonaje.RehabilitarMovimiento());
                }
                Debug.Log("Clic en la oruga");
                return;
            }
        }
        else if (puerta1Manager != null)
        {
            if (objeto.Contains("puerta"))
            {
                SceneManager.LoadScene(2);
            }
            if (objeto.Contains("fragmento1"))
            {
                puerta1Manager.OnClicFragmento();
                return;
            }
        }
        else if (puerta2Manager != null)
        {
            if (objeto.Contains("puerta"))
            {
                SceneManager.LoadScene(2);
            }
            if (objeto.Contains("fragmento2"))
            {
                puerta2Manager.OnClicFragmento();
                return;
            }
        }
        else if (puerta3Manager != null)
        {
            if (objeto.Contains("puerta"))
            {
                SceneManager.LoadScene(2);
            }
            if (objeto.Contains("fragmento3"))
            {
                puerta3Manager.OnClicFragmento();
                return;
            }
        }
        else if (puerta4Manager != null)
        {
            if (objeto.Contains("puerta"))
            {
                SceneManager.LoadScene(2);
            }
            if ( objeto.Contains("fragmento4"))
            {
                puerta4Manager.OnClicFragmento();
                return;
            }
        }
        else if (puerta5Manager != null)
        {
            if (objeto.Contains("puerta"))
            {
                SceneManager.LoadScene(2);
            }
            if (objeto.Contains("fragmento5"))
            {
                puerta5Manager.OnClicFragmento();
                return;
            }
        }
    }

}
