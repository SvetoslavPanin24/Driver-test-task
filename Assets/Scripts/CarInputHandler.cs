using UnityEngine;

public class CarInputHandler : MonoBehaviour
{
    private CarMoveController carMoveController;

    private void Awake()
    {
        carMoveController = GetComponent<CarMoveController>();
    }

    private void Update()
    {
        Vector2 input = Vector2.zero;

        input.x = Input.GetAxis("Horizontal");
        input.y = Input.GetAxis("Vertical");

        carMoveController.SetInputVector(input);
    }
}
