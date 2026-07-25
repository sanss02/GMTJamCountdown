using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShoot : MonoBehaviour
{
    [UnitHeaderInspectable("Shoot Settings")]
    [Tooltip("Fire Point, if it is not assigned use this transform.")]
    [SerializeField] private Transform firePoint;
    [SerializeField] private float maxDistance = 100f;

    // Update is called once per frame
    void Update()
    {
        if(GameManager.Instance.CurrentState != GameManager.GameState.Playing) return;

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        Vector3 origin = firePoint != null ? firePoint.position : transform.position;
        Vector3 direction = transform.forward;

        if (Physics.Raycast(origin, direction, out RaycastHit hit, maxDistance))
        {
            Debug.DrawLine(origin, hit.point, Color.red, 0.5f);

            if(hit.collider.TryGetComponent(out Target target))
            {
                target.Hit();
            }
        }
        else
        {
            Debug.DrawRay(origin, direction * maxDistance, Color.gray, 0.5f);
        }
    }
}
