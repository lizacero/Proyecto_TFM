using UnityEngine;

/// <summary>
/// Estado persistente simple para la escena del Baúl.
/// Se mantiene en memoria mientras la aplicación está abierta.
/// </summary>
public static class BaulSceneState
{
    // Indica si ya hay un estado guardado válido
    public static bool TieneEstado = false;

    // Posición del personaje en la escena del baúl
    public static Vector3 PosicionPersonaje;

    // Valor actual de la conversación (conversacionValor)
    public static int ConversacionValor;

    // Etapa actual del guion (guardada como int)
    public static int EtapaGuionInt;

    // Fragmentos depositados en el baúl (0 a 5)
    public static int FragmentosGuardadosEnBaul = 0;

    public static bool Puerta1abierta = false;
    public static bool Puerta2abierta = false;
    public static bool Puerta3abierta = false;
    public static bool Puerta4abierta = false;
    public static bool Puerta5abierta = false;
}
