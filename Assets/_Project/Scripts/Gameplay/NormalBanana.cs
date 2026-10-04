using UnityEngine;

public class NormalBanana : FallingItem
{
    [SerializeField] private int scoreValue = 1;

    public override void OnCollected(GameObject player)
    {
        CatchGameManager.Instance.AddScore(scoreValue);
        Debug.Log("Hứng được chuối ngon! + " + scoreValue);
    }
}