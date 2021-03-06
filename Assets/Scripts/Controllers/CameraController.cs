using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Start is called before the first frame update
    Camera mainCamera;
    private bool rotating = false;
    public float startFacingDirection;
    public float currentFacingDirection;
    public bool facingOposingDirection;
    public Vector3 startPosition;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (rotating)
        {
            var rotateSpeed = -25f;
            mainCamera.transform.RotateAround(Vector3.zero, Vector3.up, rotateSpeed * Time.deltaTime);
            currentFacingDirection = GetCameraFacingDirection();
            facingOposingDirection = (startFacingDirection > 0 && currentFacingDirection < 0) || (startFacingDirection < 0 && currentFacingDirection > 0);
            if (Mathf.Abs(mainCamera.transform.position.x) <= 0.2f && facingOposingDirection)
            {
                rotating = false;
                var faceDirection = startFacingDirection > 0 ? -1 : 1;
                var position = new Vector3(0, startPosition.y, startPosition.z * faceDirection);
                var yRotation = faceDirection > 0 ? 0 : 180;
                mainCamera.transform.eulerAngles = new Vector3(30, yRotation, 0);
            }
        }

    }

    public void ChangeSides()
    {
        startPosition = mainCamera.transform.position;
        startFacingDirection = GetCameraFacingDirection();
        rotating = true;
    }

    private float GetCameraFacingDirection()
    {
        return mainCamera.transform.forward.z;
    }
}
