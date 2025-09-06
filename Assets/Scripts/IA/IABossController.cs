using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IABossController : MonoBehaviour
{

	public enum Rotina
	{
		A, B, C, D
	}

	private Rigidbody2D rb;
	private Animator animator;

	public Rotina currentRotina;
	private int idEtapa;

	public float speed;
	//public float jumpForce;
	private int h;
	private bool isMove;

	private float tempTime;
	private float waitTime;

	public Transform[] wayPoints;
	private Transform target;
	public Transform groundCheck;
	public bool isGrounded;

	public bool isLookLeft;

    void Start()
    {
		rb = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();

		//
		currentRotina = Rotina.A;
		idEtapa = 0;
		tempTime = 0;
		waitTime = 3;
	}

    void Update()
    {
        
		switch(currentRotina)
		{
			case Rotina.A:
				switch(idEtapa)
				{
					case 0: // Espera 3 segundos e define o destino.
						tempTime += Time.deltaTime;
						if(tempTime >= waitTime)
						{
							idEtapa += 1;
							target = wayPoints[1];
							h = -1;
							isMove = true;
						}
						break;

					case 1: // Move até o destino.
						if(transform.position.x <= target.position.x)
						{
							idEtapa += 1;
							tempTime = 0;
							waitTime = 3;
							h = 0;
						}
						break;

					case 2:
						tempTime += Time.deltaTime;
						if (tempTime >= waitTime)
						{
							idEtapa += 1;
							target = wayPoints[0];
							h = 1;
						}
						break;

					case 3:
						if (transform.position.x >= target.position.x)
						{
							h = 0; // Fim rotina A

							currentRotina = Rotina.B;
							tempTime = 0;
							idEtapa = 0;
							waitTime = 3;
						} 
						break;
				}
				break;

			case Rotina.B:
				switch (idEtapa)
				{
					case 0: // Espera 3 segundos e define o destino.
						tempTime += Time.deltaTime;
						if (tempTime >= waitTime)
						{
							idEtapa += 1;
							target = wayPoints[1];
							h = -1;
							isMove = true;
						}
						break;

					case 1: // Move até o destino.
						if (transform.position.x <= target.position.x)
						{
							idEtapa += 1;
							tempTime = 0;
							waitTime = 3;
							h = 0;
						}
						break;

					case 2:
						tempTime += Time.deltaTime;
						if (tempTime >= waitTime)
						{
							idEtapa += 1;
							target = wayPoints[2];
							h = 1;
						}
						break;

					case 3:
						if (transform.position.x >= target.position.x)
						{
							h = 0;
							idEtapa += 1;
							rb.AddForce(new Vector2(0, 320));
							tempTime = 0;
						}
						break;

					case 4:
						tempTime += Time.deltaTime;
						if (tempTime >= waitTime)
						{
							idEtapa += 1;
						}
						break;

					case 5:
						tempTime = 0;
						waitTime = 5;
						idEtapa += 1;
						break;

					case 6:
						tempTime += Time.deltaTime;
						if (tempTime >= waitTime)
						{
							idEtapa += 1;
							isMove = false;
							rb.AddForce(new Vector2(165, 250));
							tempTime = 0;
							waitTime = 1;
						}
						break;

					case 7:
						tempTime += Time.deltaTime;
						if (tempTime >= waitTime)
						{
							if (isGrounded == true)
							{
								target = wayPoints[2];
								h = -1;
								idEtapa += 1;
								isMove = true;
							}
						}
						break;

					case 8:
						if (transform.position.x <= target.position.x)
						{
							int rand = Random.Range(0, 100);
							if(rand < 50)
							{
								target = wayPoints[0];
								h = 1;
								idEtapa = 9;
							}
							else
							{
								target = wayPoints[1];
								h = -1;
								idEtapa = 10;
							}
						}
						break;

					case 9:
						if (transform.position.x >= target.position.x)
						{
							idEtapa = 0;
							tempTime = 0;
							waitTime = 3;
							h = 0;
							currentRotina = Rotina.A;
						}
						break;

					case 10:
						if (transform.position.x <= target.position.x)
						{
							h = 0;
							idEtapa = 0;
							tempTime = 0;
							waitTime = 3;
							currentRotina = Rotina.B;
						}
						break;
				}
				break;

			case Rotina.C:

				break;

			case Rotina.D:

				break;
		}

		if(h > 0 && isLookLeft == true)
		{
			Flip();
		}
		else if(h < 0 && isLookLeft == false)
		{
			Flip();
		}

		if(isMove == true)
		{
			rb.velocity = new Vector2(h * speed, rb.velocity.y);
		}

		animator.SetInteger("idAnimation", 0);
	}

	private void FixedUpdate()
	{
		isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.02f);
	}

	void Flip()
	{
		isLookLeft = !isLookLeft;
		float x = transform.localScale.x * -1;
		transform.localScale = new Vector3(x, transform.localScale.y, transform.localScale.z);
	}
}
