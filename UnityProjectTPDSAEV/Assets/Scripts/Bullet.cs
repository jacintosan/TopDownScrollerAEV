﻿using UnityEngine;
using System.Collections;

//Bullets will travel in an straigh line hitting an enemy
//or an obstacle/level bundary
public class Bullet : MonoBehaviour {

	//[HideInInspector]
	public int bulletDamage;

	//[HideInInspector]
	public float bulletSpeed;

	public void Update()
	{
		transform.Translate (transform.up * bulletSpeed * Time.deltaTime, Space.World);
	}


	//If bullet collides with a level boundary destroy bullet
	public void OnTriggerEnter2D(Collider2D other)
	{
		Destroy (gameObject);
	}
}
