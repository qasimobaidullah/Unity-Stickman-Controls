using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

	public GameObject Player;
	// Update is called once per frame
	void Update()
	{
		transform.position = new Vector3(Player.transform.position.x + 3, transform.position.y, -10);
	}
}
