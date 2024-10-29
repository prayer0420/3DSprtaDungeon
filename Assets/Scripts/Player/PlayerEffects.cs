using UnityEngine;
using System.Collections;

public class PlayerEffects : MonoBehaviour
{
    private PlayerController playerController;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
    }

    public void ApplySpeedBoost(float multiplier, float duration)
    {
        StartCoroutine(SpeedBoostCoroutine(multiplier, duration));
    }

    private IEnumerator SpeedBoostCoroutine(float multiplier, float duration)
    {
        playerController.moveSpeed *= multiplier;
        yield return new WaitForSeconds(duration);
        playerController.moveSpeed /= multiplier;
    }
}
