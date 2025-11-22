using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuInicio : MonoBehaviour
{
    [SerializeField] private Animator anim;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
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
        SceneManager.LoadScene(1);
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
