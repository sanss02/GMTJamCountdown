using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    [SerializeField] private SpawnManager spawnManager;
    [SerializeField] private float minDistance = 8f;
    [SerializeField] private float maxDistance = 20f;
    [SerializeField] private float zoomSpeed = 2f;

    private float targetDistance;

    private void Start()
    {
        targetDistance = minDistance;
        spawnManager.OnAreaGrown += HandleAreaGrown;
    }

    private void OnDestroy()
    {
        spawnManager.OnAreaGrown -= HandleAreaGrown;
    }

    private void HandleAreaGrown(float currentRadius, float maxRadius)
    {
        Debug.Log($"Evento recibido. Radio actual: {currentRadius}, Radio máximo: {maxRadius}");
        float t = currentRadius / maxRadius;
        targetDistance = Mathf.Lerp(minDistance, maxDistance, t);
        Debug.Log($"Nuevo targetDistance: {targetDistance}");
    }

    private void Update()
    {
        float currentDistance = transform.position.magnitude;
        float newDistance = Mathf.Lerp(currentDistance, targetDistance, Time.deltaTime * zoomSpeed);
        transform.position = transform.position.normalized * newDistance;
    }
}