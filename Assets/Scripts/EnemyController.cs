using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : ShipController
{
    [Header("Enemy Controller variables")]
    public float MovementSpeed = 1.0f;
    public float ShootingRange = 5.0f;
    public int DestroyPoints = 100;

    private Transform m_currentSector;
    private Vector3 m_targetPosition;

    private float m_minRoamingDistance = 1.0f;
    private float m_maxRoamingDistance = 2.0f;

    public override void Initialize()
    {
        // Get the sector the enemy is currently in and get a random position for the enemy to roam to
        m_currentSector = getCurrentSector();
        m_targetPosition = ProjectConstants.PickRandomPositionNearby(transform.localPosition, m_minRoamingDistance, m_maxRoamingDistance);
    }

    void Update()
    {
        // Move the enemy towards its current target position and pick a new target when reaching it
        transform.localPosition = Vector2.MoveTowards(transform.localPosition, m_targetPosition, MovementSpeed * Time.deltaTime);
        if (transform.localPosition == m_targetPosition) m_targetPosition = ProjectConstants.PickRandomPositionNearby(transform.localPosition, m_minRoamingDistance, m_maxRoamingDistance);

        // Get the sector the enemy is currently in and adjust the enemy's position and target according to it if needed
        m_currentSector = getCurrentSector();

        // Shoot at the player when close enough
        if (GameManager.Instance.PlayerShip != null && Mathf.Abs(GameManager.Instance.PlayerShip.position.x - transform.position.x) <= ShootingRange)
            shoot(transform.position, (GameManager.Instance.PlayerShip.position - transform.position).normalized, ProjectileSpeed, transform.parent);
    }

    private Transform getCurrentSector()
    {
        Transform _sector = transform.parent;
        
        if (transform.localPosition.x > ProjectConstants.SECTOR_WIDTH / 2)
        {
            _sector = (_sector.GetSiblingIndex() < GameManager.Instance.Level.childCount - 1) ? GameManager.Instance.Level.GetChild(_sector.GetSiblingIndex() + 1) : GameManager.Instance.Level.GetChild(0);
            transform.SetParent(_sector);
            transform.localPosition = new Vector2(-ProjectConstants.SECTOR_WIDTH / 2, transform.localPosition.y);
            m_targetPosition -= new Vector3(ProjectConstants.SECTOR_WIDTH / 2, 0, 0);
        }
        else if (transform.localPosition.x < -ProjectConstants.SECTOR_WIDTH / 2)
        {
            _sector = (_sector.GetSiblingIndex() > 0) ? GameManager.Instance.Level.GetChild(_sector.GetSiblingIndex() - 1) : GameManager.Instance.Level.GetChild(GameManager.Instance.Level.childCount - 1);
            transform.SetParent(_sector);
            transform.localPosition = new Vector2(ProjectConstants.SECTOR_WIDTH / 2, transform.localPosition.y);
            m_targetPosition += new Vector3(ProjectConstants.SECTOR_WIDTH / 2, 0, 0);
        }

        return _sector;
    }

    public override void Destroyed()
    {
        GameManager.Instance.EnemyDestroyed(DestroyPoints);
        base.Destroyed();
    }
}
