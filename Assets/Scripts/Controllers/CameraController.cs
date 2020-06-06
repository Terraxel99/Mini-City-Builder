using System;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float cameraNormalSpeed = 10.0f;
    public float cameraHighSpeed = 25.0f;
    private float cameraMovementSpeed;
    public float cameraMovementTime = 50.0f;

    /// <summary>
    /// The position of the camera in the world.
    /// </summary>
    public Vector3 currentPosition { get; private set; }


    private void Start()
    {
        this.cameraMovementSpeed = this.cameraNormalSpeed;
        this.currentPosition = this.transform.position;
       // TODO : Positionner la caméra au point de départ de la map. 
    }

    private void Update()
    {
        this.CalculateCameraTranslation();
        this.ApplyCameraTranslation();
    }

    #region Camera Translation

    /// <summary>
    /// Calculates the movement of the camera at the next frame.
    /// </summary>
    private void CalculateCameraTranslation()
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
    private void ApplyCameraTranslation()
    {
        this.transform.position = Vector3.Lerp(this.transform.position, this.currentPosition, (Time.deltaTime * this.cameraMovementTime));
    }

    #endregion

    #region Camera Rotation



    #endregion

    #region Camera Zoom



    #endregion
}
