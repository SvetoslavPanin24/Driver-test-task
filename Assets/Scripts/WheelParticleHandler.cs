using UnityEngine;

public class WheelParticleHandler : MonoBehaviour
{
    //local variables
    private float emissionRate = 0;

    //components
    private CarMoveController carMoveController;

    private ParticleSystem particleSystem;
    private ParticleSystem.EmissionModule emissionModule;

    private void Awake()
    {
        carMoveController = GetComponentInParent<CarMoveController>();
        particleSystem = GetComponent<ParticleSystem>();
        emissionModule = particleSystem.emission;

        emissionModule.rateOverTime = 0;
    }

    private void Update()
    {
        emissionRate = Mathf.Lerp(emissionRate, 0, Time.deltaTime * 5);
        emissionModule.rateOverTime = emissionRate;

        if (carMoveController.IsTireScreeching(out float lateralVelocity, out bool isBraking))
        {
            if (isBraking)
            {
                emissionRate = 30;
            }
            else
            {
                emissionRate = Mathf.Abs(lateralVelocity) * 2;
            }
        }
    }
}
