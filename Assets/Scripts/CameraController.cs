using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] private Transform playerTransform;
    [SerializeField][Range(0.5f, 7.5f)] private float movingSpeed = 1.5f;

    private void Awake()
    {
        transform.position = new Vector3()
        {
            x = playerTransform.position.x,
            y = playerTransform.position.y,
            z = playerTransform.position.z - 10,
        };
    }

    private void Update()
    {
        if (this.playerTransform)
        {
            Vector3 target = new Vector3()
            {
                x = playerTransform.position.x,
                y = playerTransform.position.y,
                z = playerTransform.position.z - 10,
            };

            Vector3 pos = Vector3.Lerp(transform.position, target, movingSpeed * Time.deltaTime);
            transform.position = pos;
        }
    }
}

