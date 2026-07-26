using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    [SerializeField] private SpawnManager spawnManager;

    [SerializeField] private float minDistanceScale = 1f;   // tu posición actual = escala 1
    [SerializeField] private float maxDistanceScale = 9f;   // ajusta jugando, hasta cubrir tu radio máximo
    [SerializeField] private float zoomSpeed = 2f;
    
    private Vector3 baseDirection; // dirección normalizada de tu posición inicial
    private float targetScale;
    private float baseMagnitude;

    private void Start()
    {
        baseDirection = transform.position.normalized;
        baseMagnitude = transform.position.magnitude;
        ResetCamera();

        spawnManager.OnAreaGrown += HandleAreaGrown;
        GameManager.Instance.OnGameStarted += HandleGameStarted;
    }

    private void OnDestroy()
    {
        spawnManager.OnAreaGrown -= HandleAreaGrown;
        GameManager.Instance.OnGameStarted -= HandleGameStarted;
    }

    private void HandleGameStarted()
    {
        ResetCamera();
    }

    private void HandleAreaGrown(float currentRadius, float maxRadius)
    {
        if (maxRadius <= 0f) return; // evita división entre cero

        float t = currentRadius / maxRadius;
        targetScale = Mathf.Lerp(minDistanceScale, maxDistanceScale, t);
    }

    private void Update()
    {
        float baseMagnitude = 6.5f; 
        float currentMagnitude = transform.position.magnitude;
        float targetMagnitude = baseMagnitude * targetScale;

        float newMagnitude = Mathf.Lerp(currentMagnitude, targetMagnitude, Time.deltaTime * zoomSpeed);
        transform.position = baseDirection * newMagnitude;
    }

    private void ResetCamera()
    {
        targetScale = minDistanceScale;
        transform.position = baseDirection * (baseMagnitude * minDistanceScale);
    }
}