using UnityEngine;

public class RottenBanana : FallingItem
{
    [SerializeField] private int penaltyValue = 4; // Số chuối bị trừ

    public override void OnCollected(GameObject player)
    {
        CatchGameManager.Instance.SubtractScore(penaltyValue);
        Debug.Log("Dính chuối hư! Bị trừ " + penaltyValue + " quả chuối!");
    }
}