using UnityEngine;

public class ItemObject : MonoBehaviour, IInteractable
{
    public ItemData data;

    public string GetInteractPrompt()
    {
        string str = $"{data.displayName}\n{data.description}";
        return str;
    }

    public void OnInteract()
    {
        CharacterManager.Instance.Player.itemData = data;
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerEffects playerEffects = other.GetComponent<PlayerEffects>();
            if (playerEffects != null)
            {
                if (data.displayName == "Speed Boost")
                {
                    playerEffects.ApplySpeedBoost(2f, 5f);
                    Debug.Log("Speed Up");
                }
                Destroy(gameObject);
            }
        }
    }
}