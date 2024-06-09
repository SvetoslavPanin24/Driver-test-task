using UnityEngine;

public class CarMoveController : MonoBehaviour
{
    [Header("Car settings")]
    [SerializeField] private float driftFactor = 0.95f;
    [SerializeField] private float acceleratorFactor = 30f;
    [SerializeField] private float turnFactor = 3.5f;
    [SerializeField] private float maxSpeed = 20f;

    //local variables
    private float accelerationInput = 0;
    private float steeringInput = 0;

    private float rotationAngle = 0;

    private float velocityVsUp = 0;

    //Components
    private Rigidbody2D carRigidbody;

    private void Awake()
    {
        carRigidbody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        ApplyEngineForce();
        KillOrthonalVelocity();
        ApplySteering();
    }

    private void ApplySteering()
    {
        float minSpeedBeforeAllowTurningFactor = carRigidbody.velocity.magnitude / 8;
        minSpeedBeforeAllowTurningFactor = Mathf.Clamp01(minSpeedBeforeAllowTurningFactor);

        rotationAngle -= steeringInput * turnFactor * minSpeedBeforeAllowTurningFactor;
        carRigidbody.MoveRotation(rotationAngle);
    }

    private void ApplyEngineForce()
    {
        velocityVsUp = Vector2.Dot(transform.up, carRigidbody.velocity);

        if (velocityVsUp >= maxSpeed && accelerationInput > 0)
        {
            return;
        }

        if (velocityVsUp <= -maxSpeed * 0.5f && accelerationInput < 0)
        {
            return;
        }

        if (carRigidbody.velocity.sqrMagnitude > maxSpeed * maxSpeed && accelerationInput > 0)
        {
            return;
        }

        if (accelerationInput == 0)
        {
            carRigidbody.drag = Mathf.Lerp(carRigidbody.drag, 3.0f, Time.fixedDeltaTime * 3);
        }
        else
        {
            carRigidbody.drag = 0;
        }

        Vector2 engineForceVector = transform.up * accelerationInput * acceleratorFactor;
        carRigidbody.AddForce(engineForceVector, ForceMode2D.Force);
    }

    private void KillOrthonalVelocity()
    {
        Vector2 forwardVelocity = transform.up * Vector2.Dot(carRigidbody.velocity, transform.up);
        Vector2 rightVelocity = transform.right * Vector2.Dot(carRigidbody.velocity, transform.right);

        carRigidbody.velocity = forwardVelocity + rightVelocity * driftFactor;
    }

    public void SetInputVector(Vector2 InputVector)
    {
        steeringInput = InputVector.x;
        accelerationInput = InputVector.y;
    }

    public bool IsTireScreeching(out float lateralVelocity, out bool isBraking)
    {
        lateralVelocity = GetLateralVelocity();
        isBraking = false;

        if (accelerationInput < 0 && velocityVsUp > 0)
        {
            isBraking = true;
            return true;
        }

        if (Mathf.Abs(GetLateralVelocity()) > 3.0)
        {
            return true;
        }

        return false;
    }

    private float GetLateralVelocity()
    {
        return Vector2.Dot(transform.right, carRigidbody.velocity);
    }

    public float GetVelocityMagnitude()
    {
        return carRigidbody.velocity.magnitude;
    }
}
