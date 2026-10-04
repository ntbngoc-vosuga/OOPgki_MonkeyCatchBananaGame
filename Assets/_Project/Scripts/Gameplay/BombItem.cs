using UnityEngine;

public class BombItem : FallingItem
{
    public override void OnCollected(GameObject player)
    {
        Debug.Log("Dính bom! BÙM! Thua game!");
        CatchGameManager.Instance.GameOver(player.transform.position, player);
    }
}