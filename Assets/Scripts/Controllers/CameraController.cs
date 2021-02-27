using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Start is called before the first frame update
    Camera mainCamera;
    GameObject ball;
    void Start()
    {
        mainCamera = Camera.main;
        ball = GameObject.FindGameObjectWithTag("Ball");
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (ball == null || mainCamera == null) return;
        var cameraAngle = transform.rotation.eulerAngles;
        // if (cameraAngle.y > 280)
        // {
        //     transform.Translate(new Vector3(0, 0, 3) * Time.deltaTime, Space.World);
        // }
        // if (cameraAngle.y < 260)
        // {
        //     transform.Translate(new Vector3(0, 0, -3) * Time.deltaTime, Space.World);
        // }
        mainCamera.transform.LookAt(ball.transform);
    }
}
