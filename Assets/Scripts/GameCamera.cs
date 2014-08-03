using UnityEngine;
using System.Collections;

public class GameCamera : MonoBehaviour {

	private Transform target;
	public float camSpeed = 10;

	public void SetTarget(Transform transform)
	{
		target = transform;
	}
	void LateUpdate()
	{
		if (target)
		{
			float x = IncrementTowards(target.position.x, target.position.x, camSpeed);
			float y = IncrementTowards(target.position.y, target.position.y, camSpeed);
			transform.position = new Vector3 (x, y, transform.position.z);
		}

	}
	private float IncrementTowards(float n, float target, float a)
	{
		if (n == target)
			return n;
		else{
			float dir = Mathf.Sign(target - n); //to get the sign of the acceleration
			n += a * Time.deltaTime * dir;
			
			// if n has passed the target speed then reset it to the target speed otherwise return n 
			return (dir == Mathf.Sign(target-n))? n:target; 
		}
	}
}
