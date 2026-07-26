using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAim : MonoBehaviour
{
    [SerializeField] private GameObject playerModel;
    [SerializeField] private GameObject explosionEffectPrefab;
    private Camera mainCamera;
    
    private void Awake()
    {
        mainCamera = Camera.main;
    }

    void Start()
    {
        GameManager.Instance.OnStateChanged += HandleStateChanged;
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnStateChanged -= HandleStateChanged;
    }


    private void Update()
    {
        if(GameManager.Instance.CurrentState != GameManager.GameState.Playing) return;

        RotateTowardsMouse();
    }

    private void RotateTowardsMouse()
    {
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Ray ray = mainCamera.ScreenPointToRay(mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);

        if (groundPlane.Raycast(ray, out float rayLength))
        {
            Vector3 pointingDirection = ray.GetPoint(rayLength);

            // Usamos transform.position.y en vez de 0f fijo para evitar que el
            // objeto se incline hacia adelante/atrás si su pivote no está en y=0
            Vector3 lookTarget = new Vector3(pointingDirection.x, transform.position.y, pointingDirection.z);
            transform.LookAt(lookTarget);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, transform.forward * 5f);
    }

    private void HandleStateChanged(GameManager.GameState gameState)
    {
        playerModel.SetActive(gameState != GameManager.GameState.GameOver);

        if (gameState == GameManager.GameState.GameOver)
        {
            AudioManager.Instance.PlaySFXPlayerExplosion();

            if (explosionEffectPrefab != null)
            {
                Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
            }
        }
    }
}
