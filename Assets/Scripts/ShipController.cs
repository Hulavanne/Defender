using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ShipController : MonoBehaviour
{
    [Header("Ship Controller variables")]
    public GameObject ProjectilePrefab;
    public float ProjectileSpeed = 1.0f;
    public float ReloadTime = 1.0f;

    private bool m_readyToShoot = true;

    public abstract void Initialize();

    protected void shoot(Vector2 position, Vector3 direction, float projectileSpeed, Transform projectileParent)
    {
        if (m_readyToShoot)
        {
            Transform _projectile = Instantiate(ProjectilePrefab, position, new Quaternion(0, 0, 0, 0)).transform;
            _projectile.SetParent(projectileParent);
            _projectile.GetComponent<Projectile>().Speed = projectileSpeed;
            _projectile.GetComponent<Projectile>().Direction = direction;
            StartCoroutine(reloadTimer());
        }
    }

    private IEnumerator reloadTimer()
    {
        m_readyToShoot = false;
        yield return new WaitForSeconds(ReloadTime);
        m_readyToShoot = true;
    }

    public virtual void Destroyed()
    {
        GameManager.Instance.Ships.Remove(this);
        Destroy(gameObject);
    }
}
