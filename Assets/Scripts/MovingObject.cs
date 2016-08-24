using UnityEngine;
using System.Collections;

public abstract class MovingObject : MonoBehaviour {

	public float moveTime = .1f;
	public LayerMask blockingLayer;
	private BoxCollider2D boxCollider;
	private Rigidbody2D rb2d;

	private float inverseMoveTime;

	// Use this for initialization
	protected virtual void Start () {
		boxCollider = GetComponent<BoxCollider2D> ();
		rb2d = GetComponent<Rigidbody2D> ();
		inverseMoveTime = 1f / moveTime;

	}

	protected bool Move (int xDir, int yDir, out RaycastHit2D hit)
	{
		Vector2 start = transform.position;
		Vector2 end = start + new Vector2 (xDir, yDir);
		boxCollider.enabled = false;
		hit = Physics2D.Linecast (start, end);
		boxCollider.enabled = true;
		if (hit.transform == null) {
			StartCoroutine (SmoothMovement (end));
			return true;
		}
		return false;
	}

	protected virtual void AttemptMove<T> (int xDir, int yDir) where T:Component {
		RaycastHit2D hit;
		bool canMove = Move (xDir, yDir, out hit);
		if (hit.transform == null) {
			return;
		}
		T hitComponen = hit.transform.GetComponent<T> ();
		if (!canMove && hitComponen != null) {
			OnCanMove (hitComponen);
		}

	}

	protected IEnumerator SmoothMovement(Vector3 end) {
		float sqrMagnitude = (transform.position - end).sqrMagnitude;
		while (sqrMagnitude > float.Epsilon) {
			Vector3 newPosition = Vector3.MoveTowards (rb2d.position, end, inverseMoveTime * Time.deltaTime);
			rb2d.MovePosition (newPosition);
			sqrMagnitude = (transform.position - end).sqrMagnitude;
			yield return null;
		}
	}


	public abstract void OnCanMove <T> (T componend)
		where T : Component;
}
