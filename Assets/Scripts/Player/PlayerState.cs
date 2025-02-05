using UnityEngine;

public static class PlayerState
{
    public static PlayerStates currentState = PlayerStates.Playing;

    public enum PlayerStates
    {
        Playing,
        Reading
    }
}
