using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField] private GameObject hitEffectPrefab;
    public void Hit()
    {
        GameManager.Instance.RegisterTargetDestroyed();
        AudioManager.Instance.PlaySFXTargetDestroyed();

        if (hitEffectPrefab != null)
        {
            Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}
