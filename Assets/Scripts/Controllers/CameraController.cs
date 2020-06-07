using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform cameraPosition;

    public float cameraNormalSpeed = 10.0f;
    public float cameraHighSpeed = 25.0f;
    private float cameraMovementSpeed;
    public float cameraRotationSpeed = 5.0f;

    public float cameraMovementTime = 50.0f;

    public float zoomMinDistanceFromGround = 10.0f;
    public float zoomMaxDistanceFromGround = 100.0f;
    public Vector3 zoomAmount;

    /// <summary>
    /// The position of the camera in the world.
    /// </summary>
    public Vector3 currentPosition { get; private set; }
    /// <summary>
    /// The rotation of the camera in the world.
    /// </summary>
    public Quaternion currentRotation { get; private set; }
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
        if (Input.GetKey(KeyCode.E))
            this.CalculateCameraRotationHorizontalAxis();

        if (Input.GetKey(KeyCode.A))
            this.CalculateCameraRotationHorizontalAxis(false);
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

            this.currentRotation *= Quaternion.Euler(Vector3.up * (-difference.x / 5f));
        }
    }

    /// <summary>
    /// Applies rotation to the camera.
    /// </summary>
    /// <param name="positiveMovement">True if positive rotation angle. False if negative rotation angle.</param>
    private void CalculateCameraRotationHorizontalAxis(bool positiveMovement = true)
    {
        float rotationSpeed = positiveMovement ? this.cameraRotationSpeed : (-this.cameraRotationSpeed);
        this.currentRotation *= Quaternion.Euler(Vector3.up * rotationSpeed);
    }
    

    /// <summary>
    /// Applies the rotations to the camera based on the current rotation Vector.
    /// </summary>
    private void ApplyCameraRotations()
    {
        this.transform.rotation = Quaternion.Lerp(this.transform.rotation, this.currentRotation, (Time.deltaTime * this.cameraMovementTime));
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
