﻿using UnityEngine;
using System.Collections;
using Pathfinding;


public class EnemyController : MonoBehaviour {


	#region enemy movement and pathfinding
	public Transform targetTransform;

	private Seeker seeker;
	
	//The calculated path
	public Path path;
	
	//The AI's speed per second
	public float speed;

	//The AI's rotation to aim speed
	public float rotationSpeed;

	//Necessary distance to shoot at the player
	public float shootDistance;
	
	//The max distance from the AI to a waypoint for it to continue to the next waypoint
	public float nextWaypointDistance = 3;
	
	//The waypoint we are currently moving towards
	private int currentWaypoint = 0;

	#endregion

	#region shoot management
	public Enemy enemy;

	public Transform bulletSpawnPos;
	#endregion
	
	public void Start () {
		seeker = GetComponent<Seeker>();

		//Start a new path to the targetPosition, return the result to the OnPathComplete function
		seeker.StartPath (transform.position, targetTransform.position, OnPathComplete);
	}

	
	public void OnPathComplete (Path p) {
		//Debug.Log ("Yay, we got a path back. Did it have an error? "+p.error);
		if (!p.error) {
			path = p;
			//Reset the waypoint counter
			currentWaypoint = 0;
		}

		//Start a new path to the targetPosition, return the result to the OnPathComplete function
		seeker.StartPath (transform.position, targetTransform.position, OnPathComplete);
	}

	public void Update(){
		//Shoot every time the enemy can, to make player movement more difficult
		//also recharge, as with no recharge time, making enemies recharge will not slow down its shooting
		if(enemy.mainWeapon.CanShoot() && Vector2.Distance(transform.position, targetTransform.position) < shootDistance ){
			enemy.mainWeapon.Shoot(bulletSpawnPos.position, transform.rotation);
			enemy.mainWeapon.FullRecharge();
		}
	}
	
	public void FixedUpdate () {
		if (path == null) {
			//We have no path to move after yet
			return;
		}
		
		if (currentWaypoint >= path.vectorPath.Count) {
			Debug.Log ("End Of Path Reached");
			return;
		}
		
		//Direction to the next waypoint
		Vector3 dir = (path.vectorPath[currentWaypoint]-transform.position).normalized;
		dir *= speed * Time.fixedDeltaTime;

		//Rotate and move enemy smoothly
		transform.localRotation = Quaternion.Lerp(transform.rotation,
		                                          Quaternion.LookRotation(Vector3.forward, path.vectorPath[currentWaypoint]  - transform.position),
		                                          Time.fixedDeltaTime * rotationSpeed);
		transform.Translate (dir, Space.World);
		
		//Check if we are close enough to the next waypoint
		//If we are, proceed to follow the next waypoint
		if (Vector3.Distance (transform.position,path.vectorPath[currentWaypoint]) < nextWaypointDistance) {
			currentWaypoint++;
			return;
		}
	}
} 