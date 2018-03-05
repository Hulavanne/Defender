using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : ShipController
{
    [Header("Player Controller variables")]
    public Transform Ship;
    public Camera PlayerCamera;

    public float AccelerationSpeedX = 1.0f;
    public float DecelerationSpeedX = 1.0f;
    public float MaxSpeedX = 5.0f;
    public float CurrentMovementSpeedX = 0.0f;
    public float MovementSpeedY = 1.0f;
    public int FacingDirection = 1;

    public float MaxAccelerationEffectDistance = 1.0f;

    public float CameraDistance = 2.5f;
    public float CameraSpeed = 1.0f;

    public override void Initialize()
    {
        // Set camera starting position
        PlayerCamera.transform.localPosition = new Vector3(transform.position.x + FacingDirection * CameraDistance,
            PlayerCamera.transform.localPosition.y, PlayerCamera.transform.localPosition.z);
    }

    void Update()
    {
        setSpeed(PlayerInput.HorizontalInput);
        accelerationEffect();
        moveSectors();
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

            // Accelerate rightward
            CurrentMovementSpeedX = (CurrentMovementSpeedX < MaxSpeedX) ? CurrentMovementSpeedX + AccelerationSpeedX * Time.deltaTime : CurrentMovementSpeedX = MaxSpeedX;
        }
        else if (input < 0)
        {
            // Face the player to the left
            FacingDirection = -1;
            Ship.rotation = new Quaternion(0, 180, 0, 0);

            // Accelerate leftward
            CurrentMovementSpeedX = (CurrentMovementSpeedX > -MaxSpeedX) ? CurrentMovementSpeedX - AccelerationSpeedX * Time.deltaTime : CurrentMovementSpeedX = -MaxSpeedX;
        }
        // Decelerate if there is no input
        else
        {
            if (CurrentMovementSpeedX > 0) CurrentMovementSpeedX -= AccelerationSpeedX * Time.deltaTime;
            else if (CurrentMovementSpeedX < 0) CurrentMovementSpeedX += AccelerationSpeedX * Time.deltaTime;

            if (CurrentMovementSpeedX < 0.1f && CurrentMovementSpeedX > -0.1f) CurrentMovementSpeedX = 0;
        }
    }

    private void accelerationEffect()
    {
        float _percentageOfMaxSpeed = CurrentMovementSpeedX / MaxSpeedX;
        Ship.localPosition = new Vector2(_percentageOfMaxSpeed * MaxAccelerationEffectDistance, Ship.localPosition.y);
    }

    private void moveSectors()
    {
        // Move the sectors
        foreach (Transform sector in GameManager.Instance.Level)
        {
            sector.localPosition = new Vector2(sector.localPosition.x - CurrentMovementSpeedX * Time.deltaTime, sector.localPosition.y);
        }

        // Get the first the middle and the last sectors
        Transform _firstSector = GameManager.Instance.Level.GetChild(0);
        Transform _middleSector = GameManager.Instance.Level.GetChild(Mathf.CeilToInt(GameManager.Instance.Level.childCount / 2.0f) - 1);
        Transform _lastSector = GameManager.Instance.Level.GetChild(GameManager.Instance.Level.childCount - 1);

        // If the middle sector's position has moved more than its width in length, then
        // take the first/last sector and place it ahead of the other sector in relation
        // to the direction of movement, also shift the sectors in hierachy according to their positions
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
    }

    private void movePlayerVertically(int input)
    {
        // Move the ship vertically within the boundaries of the level
        Ship.localPosition = new Vector2(Ship.localPosition.x, Ship.localPosition.y + input * MovementSpeedY * Time.deltaTime);

        if (Ship.localPosition.y >= ProjectConstants.VERTICAL_BOUNDARY_TOP) Ship.localPosition = new Vector2(Ship.localPosition.x, ProjectConstants.VERTICAL_BOUNDARY_TOP);
        else if (Ship.localPosition.y <= ProjectConstants.VERTICAL_BOUNDARY_BOTTOM) Ship.localPosition = new Vector2(Ship.localPosition.x, ProjectConstants.VERTICAL_BOUNDARY_BOTTOM);
    }

    private void moveCamera()
    {
        // Move the camera smoothly so that it's centered ahead of the player
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
