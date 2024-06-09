using UnityEngine;

public class WheelTrailRendererHandler : MonoBehaviour
{
    private CarMoveController carMoveController;
    private TrailRenderer trailRenderer;

    private void Awake()
    {
        carMoveController = GetComponentInParent<CarMoveController>();
        trailRenderer = GetComponent<TrailRenderer>();

        trailRenderer.emitting = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (carMoveController.IsTireScreeching(out float lateralVelocity, out bool isBraking))
        {
            trailRenderer.emitting = true;
        }
        else
        {
            trailRenderer.emitting = false;
        }
    }
}
