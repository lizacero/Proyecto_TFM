using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuInicio : MonoBehaviour
{
    [SerializeField] private Animator anim;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    // Referencia al manager de nuevo juego para validar antes de jugar
    [SerializeField] private NuevoJuegoManager nuevoJuegoManager;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NuevoJuego()
    {
        anim.SetBool("activarJ", true);
        anim.SetFloat("juego", 1);
    }
    public void VolverJ()
    {
        anim.SetFloat("juego", -1);
        StartCoroutine(Delay());
        anim.SetBool("activarJ", false);
    }
    public void Jugar()
    {
        // Validar que se hayan guardado los datos antes de cargar la escena
        if (nuevoJuegoManager != null)
        {
            // Guardar datos y validar
            if (nuevoJuegoManager.GuardarDatosYValidar())
            {
                // Si todo está bien, cargar la escena de gameplay
                //SceneManager.LoadScene(1);
                if (SceneTransitionManager.instance != null)
                    SceneTransitionManager.instance.LoadSceneConFade(1);
                else
                    SceneManager.LoadScene(1);
            }
            else
            {
                // Si no se validó (por ejemplo, no se seleccionó personaje), no cargar
                Debug.LogWarning("No se puede iniciar el juego. Por favor, completa la selección de personaje.");
                // Aquí podrías mostrar un mensaje en la UI si lo deseas
            }
        }
        else
        {
            // Si no hay manager, cargar directamente (comportamiento antiguo)
            Debug.LogWarning("MenuInicio: No se ha asignado el NuevoJuegoManager. Cargando escena sin validación.");
            SceneManager.LoadScene(1);
        }
    }

    public void Continuar()
    {
        anim.SetBool("activarC", true);
        anim.SetFloat("continuar", 1);
    }
    public void VolverC()
    {
        anim.SetFloat("continuar", -1);
        StartCoroutine(Delay());
        anim.SetBool("activarC", false);
    }

    public void Ajustes()
    {
        anim.SetBool("activarA", true);
        anim.SetFloat("ajustes", 1);
    }
    public void VolverA()
    {
        anim.SetFloat("ajustes", -1);
        StartCoroutine(Delay());
        anim.SetBool("activarA", false);
    }

    public void Instrucciones()
    {
        anim.SetBool("activarI", true);
        anim.SetFloat("instrucciones", 1);
    }
    public void VolverI()
    {
        anim.SetFloat("instrucciones", -1);
        StartCoroutine(Delay());
        anim.SetBool("activarI", false);
    }

    public void Salir()
    {
        Debug.Log("Saliendo");
        Application.Quit();
    }

    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(1);
    }
}
