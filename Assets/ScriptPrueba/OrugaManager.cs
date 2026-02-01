using UnityEngine;

public class OrugaManager : MonoBehaviour
{
    [SerializeField] private BaulScene baulScene;
    [SerializeField] private Animator anim;

    private void Awake()
    {
        baulScene = FindAnyObjectByType<BaulScene>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (BaulSceneState.TieneEstado)
        {
            anim.SetBool("Quieto",true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    // Se llama en el evento de la animación
    private void Saludo()
    {
        if (baulScene != null)
            baulScene.IniciarGuion();
    }
}
