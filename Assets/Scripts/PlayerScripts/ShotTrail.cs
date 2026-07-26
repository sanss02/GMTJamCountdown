using UnityEngine;

public class ShotTrail : MonoBehaviour
{
    [SerializeField] private float lifetime = 0.1f;
    private LineRenderer lineRenderer;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    public void Show(Vector3 origin, Vector3 endPoint)
    {
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;

        lineRenderer.useWorldSpace = true;
        lineRenderer.SetPosition(0, origin);
        lineRenderer.SetPosition(1, endPoint);
        Destroy(gameObject, lifetime);
    }
}