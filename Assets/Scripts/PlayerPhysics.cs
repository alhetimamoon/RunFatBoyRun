using UnityEngine;
using System.Collections;


[RequireComponent (typeof(BoxCollider))]
public class PlayerPhysics : MonoBehaviour {
	
	public LayerMask collisionMask;
	
	private BoxCollider collider;
	private Vector3 colliderSize;
	private Vector3 colliderCentre;

	private Vector3 originalSize;
	private Vector3 originalCentre;
	private float scale;

	//We need these variables to dtermine how many rays are we sending from the sides and the bottom 
	//so that we wont have huge gaps between an ray and another 

	private int collisionRayX = 3; 
	private int collisionRayY = 10;
	
	private float skin = .005f;
	
	[HideInInspector]
	public bool grounded;
	[HideInInspector]
	public bool collided;
	
	Ray ray;
	RaycastHit hit;
	
	void Start() {

		collider = GetComponent<BoxCollider>();
		scale = transform.localScale.x;
		originalSize = collider.size;
		originalCentre = collider.center;
		SetCollider (originalCentre, originalSize);
	}
	
	public void Move(Vector2 moveAmount) {
		
		float deltaY = moveAmount.y;
		float deltaX = moveAmount.x;
		Vector2 p = transform.position;
		
		// Check collisions above and below
		grounded = false;
		
		for (int i = 0; i<collisionRayX; i ++) {
			float dir = Mathf.Sign(deltaY);
			float x = (p.x + colliderCentre.x - colliderSize.x/2) + colliderSize.x/ (collisionRayX-1) * i; // Left, centre and then rightmost point of collider
			float y = p.y + colliderCentre.y + colliderSize.y/2 * dir; // Bottom of collider
			
			ray = new Ray(new Vector2(x,y), new Vector2(0,dir));
			Debug.DrawRay(ray.origin,ray.direction);
			
			if (Physics.Raycast(ray,out hit,Mathf.Abs(deltaY) + skin,collisionMask)) {
				// Get Distance between player and ground
				float dst = Vector3.Distance (ray.origin, hit.point);
				
				// Stop player's downwards movement after coming within skin width of a collider
				if (dst > skin) {
					deltaY = dst * dir - skin * dir;
				}
				else {
					deltaY = 0;
				}
				grounded = true;
				break;
				
			}
		}
		// Check collisions left and right
		collided = false;
		for (int i = 0; i<collisionRayY; i ++) {
			float dir = Mathf.Sign(deltaX);
			float x = p.x + colliderCentre.x + colliderSize.x/2 * dir;
			float y = p.y + colliderCentre.y - colliderSize.y/2 + colliderSize.y/ (collisionRayY-1) * i;
			
			ray = new Ray(new Vector2(x,y), new Vector2(dir,0));
			Debug.DrawRay(ray.origin,ray.direction);
			
			if (Physics.Raycast(ray,out hit,Mathf.Abs(deltaX) + skin,collisionMask)) {
				// Get Distance between player and ground
				float dst = Vector3.Distance (ray.origin, hit.point);
				
				// Stop player's downwards movement after coming within skin width of a collider
				if (dst > skin) {
					deltaX = dst * dir - skin * dir;
				}
				else {
					deltaX = 0;
				}
				collided = true;
				break;	
			}
		}
		//we check only if none of the horizontal and vertical collision was not detected
		if (!grounded && !collided)
		{
			//check collision in an angle 
			Vector2 playerDirection = new Vector2(deltaX, deltaY);

			//set the origin point 
			Vector2 origin = new Vector2(p.x + colliderCentre.x + colliderSize.x/2 * Mathf.Sign(deltaX), p.y + colliderCentre.y + colliderSize.y/2 * Mathf.Sign (deltaY));


			//create a ray in the direction of the player movement, in this case the direction is angled 
			Ray ray = new Ray(origin, playerDirection.normalized);

			//check for collision
			//physics.raycast(origin, direction, distance, mask)
			//origin: the starting point of the ray
			//direction: the direction of the ray 
			//distance: the distance of the ray. In angle case it's pythagoras
			//colision mask: the mask layer of the object thst we want to check collision with
			if (Physics.Raycast(ray, Mathf.Sqrt(deltaX * deltaX + deltaY * deltaY), collisionMask))
			{
				grounded = true;
				deltaY = 0;
			}
		}
		//for debugging purposes, uncomment the following line to see the ray
		//Debug.DrawRay(origin, playerDirection.normalized);
		Vector2 finalTransform = new Vector2(deltaX,deltaY);
		transform.Translate(finalTransform);
	}
	public void SetCollider(Vector2 center, Vector2 size)
	{
		collider.center = center;
		collider.size = size; 

		//we also have to update colliderSize and colliderCenter which they're being used in the move method 
		colliderCentre = center * scale;
		colliderSize = size * scale;
	}
	
}
