using UnityEngine;

public class OrugaManager : MonoBehaviour
{
    [SerializeField] private BaulScene baulScene;

    private void Awake()
    {
        baulScene = FindAnyObjectByType<BaulScene>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Saludo()
    {
        if (baulScene != null)
            baulScene.IniciarGuion();
    }
}
