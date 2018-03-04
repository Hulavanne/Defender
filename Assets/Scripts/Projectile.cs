using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float Speed = 1.0f;
    public Vector3 Direction;

	void Update()
    {
        // If the projectile is in view of the main camera, update its position, otherwise destroy it
        if (Camera.main != null && Camera.main.WorldToViewportPoint(transform.position).x >= 0.0f && Camera.main.WorldToViewportPoint(transform.position).x <= 1.0f &&
            Camera.main.WorldToViewportPoint(transform.position).y >= 0.0f && Camera.main.WorldToViewportPoint(transform.position).y <= 1.0f)
        {
            transform.position += Direction * Speed * Time.deltaTime;
        }
        else Destroy(this.gameObject);
	}
}
