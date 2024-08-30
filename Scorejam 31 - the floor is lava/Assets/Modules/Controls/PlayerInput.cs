using elZach.Common;
using Modules.CharacterController;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    public static PlayerInput Instance => _instance.OrSet(ref _instance, FindObjectOfType<PlayerInput>);
    private static PlayerInput _instance;
    
    [SerializeField] private InputActionReference movementReference;
    [SerializeField] private InputActionReference cameraReference;
    [SerializeField] private CameraController cameraController;
    public InputAction movementInputAction => movementReference.action;
    public InputAction cameraInputAction => cameraReference.action;
    private ILocomotionInputReceiver locomotionInputReceiver => _locomotionInputReceiver ??= GetComponent<ILocomotionInputReceiver>();
    private ILocomotionInputReceiver _locomotionInputReceiver;

    private static Transform cam => _cam.OrSet(ref _cam, () => Camera.main?.transform);
    private static Transform _cam;
    
    private void OnEnable()
    {
        movementReference.asset.Enable();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void FixedUpdate()
    {
        var locomotionInput = ProcessInput(movementInputAction.ReadValue<Vector2>());
        locomotionInputReceiver.SetInput(locomotionInput);
        if (!cameraReference) return;
        cameraController.SetInput(cameraInputAction.ReadValue<Vector2>(), Time.fixedDeltaTime);
        cameraController.Update(Time.fixedDeltaTime);
    }

    private Vector3 ProcessInput(Vector2 readValue)
    {
        return cam.FlattenInput(readValue);
    }

    public void FocusCameraInPlayerDirection()
    {
        cameraController.SetToDirection(transform.forward);
    }
}

public interface ILocomotionInputReceiver
{
    public void SetInput(Vector3 input);
}
