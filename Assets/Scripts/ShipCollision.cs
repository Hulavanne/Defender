using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipCollision : MonoBehaviour
{
    CharacterController m_characterController;

	void Awake()
    {
        m_characterController = transform.GetComponentInParent<CharacterController>();
	}

	void Update()
    {
		
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (tag == other.tag) return;

        if (tag == "Player")
        {
            if (other.tag == "Enemy")
            {
                m_characterController.Destroyed();
            }
            else if (other.tag == "EnemyProjectile")
            {
                Destroy(other.gameObject);
                m_characterController.Destroyed();
            }
        }
        else if (tag == "Enemy")
        {
            if (other.tag == "Player")
            {
                m_characterController.Destroyed();
            }
            else if (other.tag == "PlayerProjectile")
            {
                Destroy(other.gameObject);
                m_characterController.Destroyed();
            }
        }
    }
}
