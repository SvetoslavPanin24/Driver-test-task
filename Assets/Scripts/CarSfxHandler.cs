using UnityEngine;

public class CarSfxHandler : MonoBehaviour
{
    [Header("AudioSource")]
    [SerializeField] private AudioSource tiresScreechingAudioSource;
    [SerializeField] private AudioSource engineAudioSource;
    [SerializeField] private AudioSource hitAudioSource;

    //local variables
    private float desiredEnginePitch = 0.5f;
    private float tireSreechPitch = 0.5f;

    //components
    private CarMoveController carMoveController;

    private void Awake()
    {
        carMoveController = GetComponentInParent<CarMoveController>();
    }

    private void Update()
    {
        UpdateEngineSfx();
        UpdateTiresScreechingSFX();
    }

    private void UpdateEngineSfx()
    {
        float velocityMagnitude = carMoveController.GetVelocityMagnitude();
        float desiredEngineVolume = velocityMagnitude * 0.1f;

        desiredEngineVolume = Mathf.Clamp(desiredEngineVolume, 0.25f, 1);
        engineAudioSource.volume = Mathf.Lerp(engineAudioSource.volume, desiredEngineVolume, Time.deltaTime * 10);

        desiredEnginePitch = velocityMagnitude * 0.2f;
        desiredEnginePitch = Mathf.Clamp(desiredEnginePitch, 0.5f, 2f);
        engineAudioSource.pitch = Mathf.Lerp(engineAudioSource.pitch, desiredEnginePitch, Time.deltaTime * 1.5f);
    }

    private void UpdateTiresScreechingSFX()
    {
        if (carMoveController.IsTireScreeching(out float lateralVelocity, out bool isBraking))
        {
            if (isBraking)
            {
                tiresScreechingAudioSource.volume = Mathf.Lerp(tiresScreechingAudioSource.volume, 1.0f, Time.deltaTime * 10);
                tireSreechPitch = Mathf.Lerp(tireSreechPitch, 0.5f, Time.deltaTime * 10);
            }
            else
            {
                tiresScreechingAudioSource.volume = Mathf.Abs(lateralVelocity) * 0.05f;
                tireSreechPitch = Mathf.Abs(lateralVelocity) * 0.05f;
            }
        }
        else
        {
            tiresScreechingAudioSource.volume = Mathf.Lerp(tiresScreechingAudioSource.volume, 0, Time.deltaTime * 10);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision2D)
    {
        Debug.Log("sd");
        float relativeVelocity = collision2D.relativeVelocity.magnitude;

        float volume = relativeVelocity * 0.1f;

        hitAudioSource.pitch = UnityEngine.Random.Range(0.95f, 1.05f);
        hitAudioSource.volume = volume;

        if (!hitAudioSource.isPlaying)
        {
            hitAudioSource.Play();
        }
    }
}
