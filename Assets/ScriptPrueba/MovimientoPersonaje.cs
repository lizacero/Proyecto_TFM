using System.Collections;
using UnityEngine;

public class MovimientoPersonaje : MonoBehaviour
{
    [Header("Configuración de Movimiento")]
    [SerializeField] private float velocidadMovimiento = 3f;
    [SerializeField] private float distanciaMinima = 0.01f;

    [Header("Zona Caminable")]
    [SerializeField] private bool usarZonaCaminable = true;
    [SerializeField] private GameObject objetoSuelo;

    [Header("Referencias")]
    [SerializeField] private Camera camaraPrincipal;

    private Vector3 destino;
    public bool estaMoviendose = false;
    private Coroutine movimientoCorrutina;
    private Collider2D colliderSuelo;
    private bool movimientoHabilitado = true;

    void Start()
    {
        if (camaraPrincipal == null)
            camaraPrincipal = Camera.main;

        if (usarZonaCaminable && objetoSuelo != null)
        {
            colliderSuelo = objetoSuelo.GetComponent<Collider2D>();
            if (colliderSuelo == null)
            {
                Debug.LogError($"MovimientoPersonaje: '{objetoSuelo.name}' no tiene Collider2D.");
                usarZonaCaminable = false;
            }
        }

        destino = transform.position;
    }

    void Update()
    {
        if (movimientoHabilitado && Input.GetMouseButtonDown(0) && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
        {
            MoverAlClic();
        }
    }

    private void MoverAlClic()
    {
        if (camaraPrincipal == null) return;

        Vector3 posicionMouse = Input.mousePosition;
        posicionMouse.z = camaraPrincipal.nearClipPlane + 1f;
        Vector3 destinoClic = camaraPrincipal.ScreenToWorldPoint(posicionMouse);
        destinoClic.z = transform.position.z;

        // Si hay zona caminable, ajustar el destino al punto más cercano dentro del collider
        if (usarZonaCaminable && colliderSuelo != null)
        {
            Vector2 punto2D = new Vector2(destinoClic.x, destinoClic.y);
            destino = colliderSuelo.ClosestPoint(punto2D);
            destino.z = transform.position.z;
        }
        else
        {
            destino = destinoClic;
        }

        IniciarMovimiento();
    }

    private void IniciarMovimiento()
    {
        if (movimientoCorrutina != null)
            StopCoroutine(movimientoCorrutina);

        movimientoCorrutina = StartCoroutine(MoverSuavemente());
    }

    private IEnumerator MoverSuavemente()
    {
        estaMoviendose = true;

        while (Vector3.Distance(transform.position, destino) > distanciaMinima)
        {
            Vector3 nuevaPosicion = Vector3.MoveTowards(transform.position, destino, velocidadMovimiento * Time.deltaTime);

            // Si hay zona caminable, asegurar que la posición esté dentro del collider
            if (usarZonaCaminable && colliderSuelo != null)
            {
                Vector2 punto2D = new Vector2(nuevaPosicion.x, nuevaPosicion.y);
                Vector2 puntoAjustado = colliderSuelo.ClosestPoint(punto2D);
                nuevaPosicion = new Vector3(puntoAjustado.x, puntoAjustado.y, nuevaPosicion.z);
            }

            transform.position = nuevaPosicion;
            yield return null;
        }

        transform.position = destino;
        estaMoviendose = false;
        movimientoCorrutina = null;
    }

    public void SetMovimientoHabilitado(bool habilitado)
    {
        movimientoHabilitado = habilitado;

        if (!habilitado && estaMoviendose)
        {
            if (movimientoCorrutina != null)
            {
                StopCoroutine(movimientoCorrutina);
                movimientoCorrutina = null;
            }
            estaMoviendose = false;
        }
    }
    /// <summary>
    /// Rehabilita el movimiento al final del frame actual
    /// Esto asegura que el clic ya fue procesado antes de rehabilitar
    /// </summary>
    public IEnumerator RehabilitarMovimiento()
    {
        // Esperar hasta el final del frame actual
        yield return new WaitForEndOfFrame();
        // Rehabilitar el movimiento
        SetMovimientoHabilitado(true);
    }
}
