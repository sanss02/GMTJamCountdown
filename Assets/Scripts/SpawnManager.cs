using UnityEngine;
using System;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject targetPrefab;
    [SerializeField] private float radiusMultiplier = 5f;
    [SerializeField] private float minSpawnDistance = 2f;
    [SerializeField] private Renderer mapFloorRenderer;
    private Vector3 startPosition = new Vector3(0f, 0f, 2.5f);
    private float maxRadius;

    public event Action<float, float> OnAreaGrown;

    void Awake()
    {
        maxRadius = mapFloorRenderer.bounds.extents.x;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameManager.Instance.OnGameStarted += HandleGameStarted;
        GameManager.Instance.OnTargetDestroyed += HandleTargetDestroyed;
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnGameStarted -= HandleGameStarted;
        GameManager.Instance.OnTargetDestroyed -= HandleTargetDestroyed;
    }

    private void HandleTargetDestroyed(int targetsDestroyed)
    {
        SpawnRandomTarget();

        if (targetsDestroyed % 5 == 0)
        {
            IncreaseMultiplier();
        }
    }

    private void HandleGameStarted()
    {
        Instantiate(targetPrefab, startPosition, targetPrefab.transform.rotation);
    }


    private void SpawnRandomTarget()
    {
        float angle = UnityEngine.Random.Range(0f, Mathf.PI * 2f);
        float distance = UnityEngine.Random.Range(minSpawnDistance, radiusMultiplier);

        float positionX = Mathf.Cos(angle) * distance;
        float positionZ = Mathf.Sin(angle) * distance;
        Vector3 spawnPosition = new Vector3(positionX, 0f, positionZ);

        Instantiate(targetPrefab, spawnPosition, targetPrefab.transform.rotation);
    }

    private void IncreaseMultiplier()
    {
        radiusMultiplier = Mathf.Min(radiusMultiplier += 5, maxRadius);
        OnAreaGrown?.Invoke(radiusMultiplier, maxRadius);
    }
}
