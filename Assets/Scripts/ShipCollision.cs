using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipCollision : MonoBehaviour
{
    ShipController m_shipController;

	void Awake()
    {
        m_shipController = transform.GetComponentInParent<ShipController>();
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (tag == other.tag) return;

        if (tag == "Player")
        {
            if (other.tag == "Enemy")
            {
                m_shipController.Destroyed();
            }
            else if (other.tag == "EnemyProjectile")
            {
                Destroy(other.gameObject);
                m_shipController.Destroyed();
            }
        }
        else if (tag == "Enemy")
        {
            if (other.tag == "Player")
            {
                m_shipController.Destroyed();
            }
            else if (other.tag == "PlayerProjectile")
            {
                Destroy(other.gameObject);
                m_shipController.Destroyed();
            }
        }
    }
}
