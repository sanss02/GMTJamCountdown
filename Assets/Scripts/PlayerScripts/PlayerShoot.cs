using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShoot : MonoBehaviour
{
    [UnitHeaderInspectable("Shoot Settings")]
    [Tooltip("Fire Point, if it is not assigned use this transform.")]
    [SerializeField] private Transform firePoint;
    [SerializeField] private float maxDistance = 100f;

    [SerializeField] private GameObject shotTrailPrefab;

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
        AudioManager.Instance.PlaySFXShoot();      

        Vector3 origin = firePoint != null ? firePoint.position : transform.position;
        Vector3 direction = transform.forward;

        Vector3 endPoint;

        if (Physics.Raycast(origin, direction, out RaycastHit hit, maxDistance))
        {
            endPoint = hit.point;

            if (hit.collider.TryGetComponent(out Target target))
            {
                target.Hit();
            }
        }
        else
        {
            endPoint = origin + direction * maxDistance;
        }

        if (shotTrailPrefab != null)
        {
            GameObject trailObj = Instantiate(shotTrailPrefab);
            trailObj.GetComponent<ShotTrail>().Show(origin, endPoint);
        }
    }
}
