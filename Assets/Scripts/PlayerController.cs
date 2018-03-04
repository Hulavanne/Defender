using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : CharacterController
{
    [Header("Player Controller variables")]
    public Transform Ship;
    public Camera PlayerCamera;

    public float AccelerationSpeedX = 1.0f;
    public float DecelerationSpeedX = 1.0f;
    public float MaxSpeedX = 5.0f;
    public float MovementSpeedX = 0.0f;
    public float MovementSpeedY = 1.0f;
    public int FacingDirection = 1;

    public float MaxDistance = 1.0f;

    public float CameraDistance = 2.5f;
    public float CameraSpeed = 1.0f;

    public override void Initialize()
    {
        // Set camera position
        PlayerCamera.transform.localPosition = new Vector3(transform.position.x + FacingDirection * CameraDistance,
            PlayerCamera.transform.localPosition.y, PlayerCamera.transform.localPosition.z);
    }

    void Update()
    {
        setSpeed(PlayerInput.HorizontalInput);
        speedEffect();
        moveLevel();
        movePlayerVertically(PlayerInput.VerticalInput);
        moveCamera();

        if (PlayerInput.ShootInput) shoot(Ship.position, new Vector2(FacingDirection, 0), ProjectileSpeed,
            GameManager.Instance.Level.GetChild(Mathf.CeilToInt(GameManager.Instance.Level.childCount / 2.0f) - 1));
    }

    private void setSpeed(int input)
    {
        if (input > 0)
        {
            // Face the player to the right
            FacingDirection = 1;
            Ship.rotation = new Quaternion(0, 0, 0, 0);

            // Accelerate right
            MovementSpeedX = (MovementSpeedX < MaxSpeedX) ? MovementSpeedX + AccelerationSpeedX * Time.deltaTime : MovementSpeedX = MaxSpeedX;
        }
        else if (input < 0)
        {
            // Face the player to the left
            FacingDirection = -1;
            Ship.rotation = new Quaternion(0, 180, 0, 0);

            // Accelerate left
            MovementSpeedX = (MovementSpeedX > -MaxSpeedX) ? MovementSpeedX - AccelerationSpeedX * Time.deltaTime : MovementSpeedX = -MaxSpeedX;
        }
        // Decelerate if there is no input
        else
        {
            if (MovementSpeedX > 0) MovementSpeedX -= AccelerationSpeedX * Time.deltaTime;
            else if (MovementSpeedX < 0) MovementSpeedX += AccelerationSpeedX * Time.deltaTime;

            if (MovementSpeedX < 0.1f && MovementSpeedX > -0.1f) MovementSpeedX = 0;
        }
    }

    private void speedEffect()
    {
        // Speed effect
        float _percentage = MovementSpeedX / MaxSpeedX;
        Ship.localPosition = new Vector2(_percentage * MaxDistance, Ship.localPosition.y);
    }

    private void moveLevel()
    {
        // Move the sectors
        foreach (Transform sector in GameManager.Instance.Level)
        {
            sector.localPosition = new Vector2(sector.localPosition.x - MovementSpeedX * Time.deltaTime, sector.localPosition.y);
        }

        #region Seamless level effect
        // Get sectors
        Transform _firstSector = GameManager.Instance.Level.GetChild(0);
        Transform _middleSector = GameManager.Instance.Level.GetChild(Mathf.CeilToInt(GameManager.Instance.Level.childCount / 2.0f) - 1);
        Transform _lastSector = GameManager.Instance.Level.GetChild(GameManager.Instance.Level.childCount - 1);

        if (_middleSector.position.x >= ProjectConstants.SECTOR_WIDTH)
        {
            _lastSector.position = new Vector2(_firstSector.localPosition.x - 8, 0);
            _lastSector.SetAsFirstSibling();
        }
        if (_middleSector.position.x <= -ProjectConstants.SECTOR_WIDTH)
        {
            _firstSector.position = new Vector2(_lastSector.localPosition.x + 8, 0);
            _firstSector.SetAsLastSibling();
        }
        #endregion
    }

    private void movePlayerVertically(int input)
    {
        // Vertical movement
        Ship.localPosition = new Vector2(Ship.localPosition.x, Ship.localPosition.y + input * MovementSpeedY * Time.deltaTime);

        if (Ship.localPosition.y >= ProjectConstants.VERTICAL_BOUNDARY_TOP) Ship.localPosition = new Vector2(Ship.localPosition.x, ProjectConstants.VERTICAL_BOUNDARY_TOP);
        else if (Ship.localPosition.y <= ProjectConstants.VERTICAL_BOUNDARY_BOTTOM) Ship.localPosition = new Vector2(Ship.localPosition.x, ProjectConstants.VERTICAL_BOUNDARY_BOTTOM);
    }

    private void moveCamera()
    {
        Vector3 _targetPosition = new Vector3(transform.position.x + FacingDirection * CameraDistance, 0, 0);
        Vector3 _directionToTarget = (_targetPosition - PlayerCamera.transform.position).normalized;
        PlayerCamera.transform.localPosition = new Vector3(PlayerCamera.transform.localPosition.x + _directionToTarget.x * CameraSpeed * Time.deltaTime,
            PlayerCamera.transform.localPosition.y, PlayerCamera.transform.localPosition.z);
    }

    public override void Destroyed()
    {
        GameManager.Instance.RestartWave();
    }
}
