using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class GrappleGunner : MonoBehaviour
{
  
    public TextMeshProUGUI afficheTrace;
    [Header("Références principales")]
    public Transform rayOrigin;
    public Transform xrRig;
    public LineRenderer lineRenderer;

    [Header("Rayon")]
    public float maxDistance = 20f;

    [Header("Déplacement")]
    public float grappleSpeed = 10f;

    [Header("Input Actions")]
    public InputActionProperty aimAction;    // Trigger
    public InputActionProperty gripAction;   // Grip

    [Header("Conditions")]
    public string grappleTag = "GrapplePoint";

    private bool isGrappling = false;
    private bool hasArrived = false;

    private Vector3 targetPoint;
    private Vector3 lockedRayOrigin;

    private CharacterController charController;

    void Start()
    {
        if (lineRenderer && rayOrigin)
            lineRenderer.transform.SetParent(rayOrigin, false);

        lineRenderer.enabled = false;

        if (xrRig != null)
            charController = xrRig.GetComponent<CharacterController>();
    }

    void Update()
    {
        bool triggerHeld = aimAction.action?.ReadValue<float>() > 0.1f;
        bool gripHeld = gripAction.action?.ReadValue<float>() > 0.1f;

        if (!isGrappling && triggerHeld)
        {
            lineRenderer.enabled = true;

            // Mise à jour continue du rayon
            if (Physics.Raycast(rayOrigin.position, rayOrigin.forward, out var hit, maxDistance))
            {
                lineRenderer.SetPositions(new Vector3[] { rayOrigin.position, hit.point });

                if (gripHeld && hit.collider.CompareTag(grappleTag))
                {
                    // Lancer le grappin
                    isGrappling = true;
                    hasArrived = false;
                    targetPoint = hit.point;
                    lockedRayOrigin = rayOrigin.position;

                    lineRenderer.transform.SetParent(null, true);

                    if (charController != null && charController.enabled)
                        charController.enabled = false;
                }
            }
            else
            {
                Vector3 far = rayOrigin.position + rayOrigin.forward * maxDistance;
                lineRenderer.SetPositions(new Vector3[] { rayOrigin.position, far });
            }
        }
        else if (isGrappling)
        {
            lineRenderer.enabled = true;
            lineRenderer.SetPositions(new Vector3[] { lockedRayOrigin, targetPoint });

            if (!hasArrived)
            {
                float distance = Vector3.Distance(xrRig.position, targetPoint);
                if (distance > 0.01f)
                {
                    Vector3 direction = (targetPoint - xrRig.position).normalized;
                    xrRig.position += direction * grappleSpeed * Time.deltaTime;
                }
                else
                {
                    xrRig.position = targetPoint;
                    hasArrived = true;
                }
            }

            // Si on relâche une des deux touches, on annule
            if (!triggerHeld || !gripHeld)
                StopGrapple();
        }
        else
        {
            lineRenderer.enabled = false;
            if (lineRenderer.transform.parent != rayOrigin)
                lineRenderer.transform.SetParent(rayOrigin, false);

            if (charController != null && !charController.enabled)
                charController.enabled = true;
        }
    }

    void StopGrapple()
    {
        isGrappling = false;
        hasArrived = false;
        lineRenderer.transform.SetParent(rayOrigin, false);
        lineRenderer.enabled = false;

        if (charController != null && !charController.enabled)
            charController.enabled = true;
    }

}
