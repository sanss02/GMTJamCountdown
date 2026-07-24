using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private InputSystem_Actions controls;

    void OnEnable()
    {
        controls = new InputSystem_Actions();
    }
}
