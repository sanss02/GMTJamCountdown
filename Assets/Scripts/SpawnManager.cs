using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject targetPrefab;
    [SerializeField] private int radiusMultiplier;
    private Vector3 startPosition = new Vector3(0f, 0f, 2.5f);
    private int maxRadius = 10;
    
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
        Vector2 randomPosition = Random.insideUnitCircle;
        float positionX = randomPosition.x * radiusMultiplier;
        float positionZ = randomPosition.y * radiusMultiplier;
        Vector3 spawnPosition = new Vector3(positionX, 0f, positionZ);

        Instantiate(targetPrefab, spawnPosition, targetPrefab.transform.rotation);
    }

    private void IncreaseMultiplier()
    {

        radiusMultiplier = Mathf.Min(radiusMultiplier += 5, maxRadius);
        
    }
}
