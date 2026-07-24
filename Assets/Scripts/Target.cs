using UnityEngine;

public class Target : MonoBehaviour
{
    public void Hit()
    {
        GameManager.Instance.RegisterTargetDestroyed();

        Destroy(gameObject);
    }
}
