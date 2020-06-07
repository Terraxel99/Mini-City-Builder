using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Camera")]
    [Space(10)]
    public Transform cameraPosition;

    [Header("Camera Movement Speeds")]
    [Space(10)]
    public float cameraNormalSpeed = 10.0f;
    public float cameraHighSpeed = 25.0f;
    private float cameraMovementSpeed;

    [Header("Camera Rotation Speeds")]
    [Space(10)]
    public float cameraRotationSpeed = 5.0f;
    public float cameraHorizontalGrabRotationSpeedReducer = 5.0f;

    [Header("Camera Rotation Angles")]
    [Space(10)]
    public float cameraVerticalRotationMinAngle = 15.0f;
    public float cameraVerticalRotationMaxAngle = 75.0f;

    [Header("Camera Times")]
    [Space(10)]
    [Tooltip("The time for the camera to move from A to B")]
    public float cameraMovementTime = 50.0f;

    [Header("Camera Zoom")]
    [Space(10)]
    public float zoomMinDistanceFromGround = 10.0f;
    public float zoomMaxDistanceFromGround = 100.0f;
    public Vector3 zoomAmount;

    /// <summary>
    /// The position of the camera in the world.
    /// </summary>
    public Vector3 currentPosition { get; private set; }
    /// <summary>
    /// The rotation of the camera's parent object in the world.
    /// </summary>
    public Quaternion currentRotation { get; private set; }
    /// <summary>
    /// The rotation of the camera depending on its parent (local rotation).
    /// </summary>
    public Quaternion currentCameraLocalRotation { get; private set; }
    /// <summary>
    /// The zoom of the camera in the world.
    /// </summary>
    public Vector3 currentZoom { get; private set; }
    /// <summary>
    /// The start position of the drag camera rotation.
    /// </summary>
    public Vector3 dragRotateStartPosition { get; private set; }
    /// <summary>
    /// The current position of the drag camera rotation.
    /// </summary>
    public Vector3 dragRotateCurrentPosition { get; private set; }


    private void Start()
    {
        this.cameraMovementSpeed = this.cameraNormalSpeed;
        this.currentPosition = this.transform.position;
        this.currentRotation = this.transform.rotation;
        this.currentCameraLocalRotation = this.cameraPosition.localRotation;
        this.ResetCurrentZoomToCameraPosition();
    }

    private void Update()
    {
        this.CalculateCameraTranslations();
        this.ApplyCameraTranslations();

        this.CalculateCameraRotation();
        this.CalculateCameraGrabRotation();
        this.ApplyCameraRotations();

        this.ResetCurrentZoomToCameraPosition();
        this.CalculateCameraZoom();
    }

    #region Camera Translation

    /// <summary>
    /// Calculates the movement of the camera at the next frame.
    /// </summary>
    private void CalculateCameraTranslations()
    {
        this.cameraMovementSpeed = Input.GetKey(KeyCode.LeftShift) ? this.cameraHighSpeed : this.cameraNormalSpeed;

        if (Input.GetKey(KeyCode.Z))
            this.CalculateMoveCameraForwardAxis();

        if (Input.GetKey(KeyCode.S))
            this.CalculateMoveCameraForwardAxis(false);

        if (Input.GetKey(KeyCode.Q))
            this.CalculateMoveCameraRightAxis(false);

        if (Input.GetKey(KeyCode.D))
            this.CalculateMoveCameraRightAxis();

        if (Input.GetKey(KeyCode.C))
            this.CalculateMoveCameraUpAxis(false);

        if (Input.GetKey(KeyCode.Space))
            this.CalculateMoveCameraUpAxis();
    }

    /// <summary>
    /// Calculates the movement on the blue axis before moving.
    /// </summary>
    /// <param name="positiveMovement">Whether the movement's direction is positive or negative</param>
    private void CalculateMoveCameraForwardAxis(bool positiveMovement = true)
    {
        Vector3 movement = this.transform.forward * this.cameraMovementSpeed * Time.deltaTime;

        if (!positiveMovement)
            movement *= -1f;

        this.currentPosition += movement;
    }

    /// <summary>
    /// Calculates the movement on the green axis before moving.
    /// </summary>
    /// <param name="positiveMovement">Whether the movement's direction is positive or negative</param>
    private void CalculateMoveCameraUpAxis(bool positiveMovement = true)
    {
        // Block vertical movement if the position begins to be negative and we want to go lower.
        if (!positiveMovement && this.transform.position.y <= 0)
        {
            return;
        }

        Vector3 movement = this.transform.up * this.cameraMovementSpeed * Time.deltaTime;

        if (!positiveMovement)
            movement *= -1f;

        this.currentPosition += movement;
    }

    /// <summary>
    /// Calculates the movement on the red axis before moving.
    /// </summary>
    /// <param name="positiveMovement">Whether the movement's direction is positive or negative</param>
    private void CalculateMoveCameraRightAxis(bool positiveMovement = true)
    {
        Vector3 movement = this.transform.right * this.cameraMovementSpeed * Time.deltaTime;

        if (!positiveMovement)
            movement *= -1f;

        this.currentPosition += movement;
    }

    /// <summary>
    /// Applies the translation to the camera.
    /// </summary>
    private void ApplyCameraTranslations()
    {
        this.transform.position = Vector3.Lerp(this.transform.position, this.currentPosition, (Time.deltaTime * this.cameraMovementTime));
    }

    #endregion

    #region Camera Rotation

    /// <summary>
    /// Calculates when to rotate the camera and in which direction.
    /// </summary>
    private void CalculateCameraRotation()
    {
        if (Input.GetKey(KeyCode.E) || Input.GetKey(KeyCode.RightArrow))
            this.CalculateCameraRotationHorizontalAxis();

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            this.CalculateCameraRotationHorizontalAxis(false);

        if (Input.GetKey(KeyCode.UpArrow))
            this.CalculateCameraRotationVerticalAxis();

        if (Input.GetKey(KeyCode.DownArrow))
            this.CalculateCameraRotationVerticalAxis(false);
    }

    /// <summary>
    /// Makes the calculations to rotate the camera on world grabbing.
    /// </summary>
    private void CalculateCameraGrabRotation()
    {
        if (Input.GetMouseButtonDown(2))
        {
            this.dragRotateStartPosition = Input.mousePosition;
        }

        if (Input.GetMouseButton(2))
        {
            this.dragRotateCurrentPosition = Input.mousePosition;
            Vector3 difference = this.dragRotateStartPosition - this.dragRotateCurrentPosition;

            this.dragRotateStartPosition = this.dragRotateCurrentPosition;

            this.currentRotation *= Quaternion.Euler(Vector3.up * (-difference.x / this.cameraHorizontalGrabRotationSpeedReducer));
        }
    }

    /// <summary>
    /// Calculates the horizontal rotation of the world.
    /// </summary>
    /// <param name="positiveMovement">True if positive rotation angle. False if negative rotation angle.</param>
    private void CalculateCameraRotationHorizontalAxis(bool positiveMovement = true)
    {
        float rotationSpeed = positiveMovement ? this.cameraRotationSpeed : (-this.cameraRotationSpeed);
        this.currentRotation *= Quaternion.Euler(Vector3.up * rotationSpeed);
    }
    
    /// <summary>
    /// Calculates the rotation on the vertical axis.
    /// </summary>
    /// <param name="positiveMovement">True if the movement is a positive angle. False for a negative angle.</param>
    private void CalculateCameraRotationVerticalAxis(bool positiveMovement = true)
    {
        if (!canRotateVertically(positiveMovement))
            return;

        float rotationSpeed = positiveMovement ? this.cameraRotationSpeed : (-this.cameraRotationSpeed);
        this.currentCameraLocalRotation *= Quaternion.Euler(Vector3.right * rotationSpeed);
    }

    /// <summary>
    /// Checks if the user can rotate the camera vertically or not.
    /// </summary>
    /// <param name="positiveMovement">True if the movement is increasing the camera angle. False if the movement is decreasing the camera angle.</param>
    /// <returns>True if the user can rotate the camera.</returns>
    private bool canRotateVertically(bool positiveMovement)
    {
        // If the movement is increasing the angle, the rotation has to be under the maximum angle
        // If the movement is decreasing the angle, the rotation has to be above the minimum angle
        return (positiveMovement && this.cameraPosition.localRotation.eulerAngles.x <= this.cameraVerticalRotationMaxAngle)
            || (!positiveMovement && this.cameraPosition.localRotation.eulerAngles.x >= this.cameraVerticalRotationMinAngle);
    }

    /// <summary>
    /// Applies the rotations to the camera based on the current rotation Vector.
    /// </summary>
    private void ApplyCameraRotations()
    {
        this.transform.rotation = Quaternion.Lerp(this.transform.rotation, this.currentRotation, (Time.deltaTime * this.cameraMovementTime));
        this.cameraPosition.localRotation = Quaternion.Lerp(this.cameraPosition.localRotation, this.currentCameraLocalRotation, (Time.deltaTime * this.cameraMovementTime));
    }

    #endregion

    #region Camera Zoom

    /// <summary>
    /// Calculates whether the camera has to zoom in or out.
    /// </summary>
    private void CalculateCameraZoom()
    {
        if (Input.GetKeyDown(KeyCode.KeypadPlus) || Input.GetAxis("Mouse ScrollWheel") > 0f)
            this.ApplyZoom();

        if (Input.GetKeyDown(KeyCode.KeypadMinus) || Input.GetAxis("Mouse ScrollWheel") < 0f)
            this.ApplyZoom(false);
    }

    /// <summary>
    /// Sets the current zoom to the current local position of the camera.
    /// </summary>
    private void ResetCurrentZoomToCameraPosition()
    {
        this.currentZoom = this.cameraPosition.localPosition;
    }

    /// <summary>
    /// Applies a zoom to the camera.
    /// </summary>
    /// <param name="zoomIn">True to zoom in, false to zoom out.</param>
    private void ApplyZoom(bool zoomIn = true)
    {
        // If we try to zoom and camera is positioned too near from the ground, zoom is cancelled.
        // Same happens when user tries to zoom out while being too far away from the ground.
        if ((zoomIn && this.cameraPosition.localPosition.y <= this.zoomMinDistanceFromGround)
            || (!zoomIn && this.cameraPosition.localPosition.y >= this.zoomMaxDistanceFromGround))
        {
            return;
        }

        // Calculating and applying zoom :

        if (zoomIn)
            this.currentZoom += this.zoomAmount;
        else
            this.currentZoom -= this.zoomAmount;

        this.cameraPosition.localPosition = Vector3.Lerp(this.cameraPosition.localPosition, this.currentZoom, Time.deltaTime * this.cameraMovementTime);
    }

    #endregion
}
