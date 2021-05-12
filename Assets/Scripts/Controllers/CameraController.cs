using UnityEngine;
using AndorinhaEsporte.Domain;

public class CameraController : MonoBehaviour
{
    // Start is called before the first frame update
    Camera mainCamera;
    private bool rotating = false;
    public bool InTransition = false;
    public float startFacingDirection;
    public float currentFacingDirection;
    public bool facingOposingDirection;
    private Vector3 startPosition;
    private Vector3 mainCameraPosition = new Vector3(0, 4, -12f);

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

    public void MoveToServeAngle(Vector3 playerPosition)
    {
        Debug.Log("??");
        var target = new Vector3(playerPosition.x, 3, -14 * (GetCameraFacingDirection()));
        MoveToTarget(target);
    }

    public void MoveToMainAngle()
    {
        MoveToTarget(mainCameraPosition);
    }

    private void MoveToTarget(Vector3 target)
    {
        if (transform.position.Distance(target) > 0.02f)
        {
            transform.position = Vector3.Lerp(transform.position, target, Time.deltaTime * 2);
            InTransition = true;
        }
        else
        {
            InTransition = false;
        }
    }



    private float GetCameraFacingDirection()
    {
        return mainCamera.transform.forward.z;
    }
}
