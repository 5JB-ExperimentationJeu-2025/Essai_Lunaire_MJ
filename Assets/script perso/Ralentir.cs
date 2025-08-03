using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;



public class Ralentir : MonoBehaviour
{
    public InputActionProperty moveInput;
    public float speed = 2.0f;
    public float decelerationRate = 5.0f; // Plus élevé = arrêt plus rapide

    private CharacterController characterController;
    private Vector2 inputMove;
    private Vector3 currentVelocity;

    public TextMeshProUGUI trace;
    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        trace.text = characterController.velocity.ToString();

        inputMove = moveInput.action.ReadValue<Vector2>();

        Vector3 desiredDirection = new Vector3(inputMove.x, 0, inputMove.y);

        // Prendre en compte la direction de la tête (Camera)
        Transform headTransform = Camera.main.transform;
        Vector3 headForward = new Vector3(headTransform.forward.x, 0, headTransform.forward.z).normalized;
        Vector3 headRight = new Vector3(headTransform.right.x, 0, headTransform.right.z).normalized;
        Vector3 moveDirection = headForward * desiredDirection.z + headRight * desiredDirection.x;

        // Calcul de la vitesse avec inertie
        if (moveDirection.magnitude > 0.01f)
        {
            currentVelocity = moveDirection.normalized * speed;
        }
        else
        {
            currentVelocity = Vector3.Lerp(currentVelocity, Vector3.zero, Time.deltaTime * decelerationRate);
        }

        characterController.Move(currentVelocity * Time.deltaTime);
    }
}
