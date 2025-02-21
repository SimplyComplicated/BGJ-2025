using UnityEngine;

public sealed class PlaygroundBehaviour : MonoBehaviour
{
    public PlayerBrain FindPlayer()
    {
        return FindFirstObjectByType<PlayerBrain>();
    }

    public void KillPlayer()
    {
        var player = FindPlayer();

        player.Rip();
    }
}
