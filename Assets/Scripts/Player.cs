using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	private GameController GC;

	private Animator animator;
	private Rigidbody2D rb;
	private SpriteRenderer sRender;

	public Transform groundCheck, hand, spawnFlecha, spawnMagia;
	public LayerMask whatIsGround, interacao;

	public float speed, jumpForce, h, v;

	public bool grounded, attaking, lookLeft, naoPodeAtacar;
	public int idAnimation, vidaMax = 100, vidaAtual;
	public Collider2D stading, crounching;

	private Vector3 dir = Vector3.right;
	public GameObject objetoInteracao, balaoAlerta, magiaPrefab;

	public int idArma, idArmaAtual;
	public GameObject[] armas, arcos, flechaArco, staffs;



	void Start()
    {
		GC = FindObjectOfType(typeof(GameController)) as GameController;

		vidaMax = GC.vidaMax;
		idArma = GC.idArma;

		GC.manaAtual = GC.manaMax;

		rb = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
		sRender = GetComponent<SpriteRenderer>();

		vidaAtual = vidaMax;

		foreach(GameObject a in armas)
		{
			a.SetActive(false);
		}

		foreach (GameObject a in arcos)
		{
			a.SetActive(false);
		}

		foreach (GameObject a in staffs)
		{
			a.SetActive(false);
		}

		TrocarArma(idArma);
    }

	void FixedUpdate()
	{
		if(GC.currentState != GameState.GAMEPLAY)
		{ 
			return; 
		}

		grounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, whatIsGround);
		rb.velocity = new Vector2(h * speed, rb.velocity.y);

		Interagir();
	}

	void Update()
    {
		if (GC.currentState != GameState.GAMEPLAY)
		{
			return;
		}

		h = Input.GetAxisRaw("Horizontal");
		v = Input.GetAxisRaw("Vertical");

		if((h > 0 && lookLeft == true && attaking == false)
			|| (h < 0 && lookLeft == false && attaking == false))
		{
			Flip();
		}

		if(v < 0)
		{
			idAnimation = 2;
			if(grounded == true)
			{
				h = 0;
			}
		}
		else if(h!= 0)
		{
			idAnimation = 1;
		}
		else
		{
			idAnimation = 0;
		}

		if (Input.GetButtonDown("Fire1") && v >= 0 && attaking == false && objetoInteracao == null && naoPodeAtacar == false)
		{
			naoPodeAtacar = true;
			animator.SetTrigger("atk");
		}

		if (Input.GetButtonDown("Fire1") && v >= 0 && attaking == false && objetoInteracao != null)
		{
			/*if(objetoInteracao.tag == "Door")
			{
				objetoInteracao.GetComponent<Door>().tPlayer = transform;
			}*/

			objetoInteracao.SendMessage("Interacao", SendMessageOptions.DontRequireReceiver);
		}

		if(Input.GetButtonDown("Jump") && grounded == true && attaking == false)
		{
			rb.AddForce(new Vector2(0, jumpForce));
			CrounchingFalse();
		}

		if(Input.GetKeyDown(KeyCode.Alpha1) && attaking == false)
		{
			TrocarArma(0);
		}
		if (Input.GetKeyDown(KeyCode.Alpha2) && attaking == false)
		{
			TrocarArma(4);
		}
		if (Input.GetKeyDown(KeyCode.Alpha3) && attaking == false)
		{
			TrocarArma(5);
		}

		if (attaking == true && grounded == true)
		{
			h = 0;
		}

		if(v < 0 && grounded == true)
		{
			crounching.enabled = true;
			stading.enabled = false;
		}
		else if(v >= 0 && grounded == true)
		{
			CrounchingFalse();
		}
		else if (v != 0 && grounded == false)
		{
			CrounchingFalse();
		}

		void CrounchingFalse()
		{
			crounching.enabled = false;
			stading.enabled = true;
		}

		animator.SetBool("grounded", grounded);
		animator.SetInteger("idAnimation", idAnimation);
		animator.SetFloat("speedY", rb.velocity.y);
		animator.SetFloat("idClasseArma", GC.idClasseArma[GC.idArmaAtual]);

		if(GC.qtdFlechas[GC.idFlechaEquipada] > 0)
		{
			foreach(GameObject f in flechaArco)
			{
				f.SetActive(true);
			}
		}
		else
		{
			foreach (GameObject f in flechaArco)
			{
				f.SetActive(false);
			}
		}

	}

	private void LateUpdate()
	{
		if(GC.idArma != GC.idArmaAtual)
		{
			TrocarArma(GC.idArma);
		}
	}

	void Flip()
	{
		lookLeft = !lookLeft;
		float x = transform.localScale.x;
		x *= -1;
		transform.localScale = new Vector3(x, transform.localScale.y, transform.localScale.z);
		
		dir.x = x;
	}

	//Eventos na Animação
	void Atk(int atk)
	{
		switch(atk)
		{
			case 0:
				attaking = false;
				armas[2].SetActive(false);
				StartCoroutine("EsperarNovoAtaque");
				break;
			case 1:
				attaking = true;
				break;
		}
	}
	void AtkFlecha(int atk)
	{
		switch (atk)
		{
			case 0:
				attaking = false;
				arcos[2].SetActive(false);
				StartCoroutine("EsperarNovoAtaque");
				break;
			case 1:
				attaking = true;
				break;
			case 2:
				if(GC.qtdFlechas[GC.idFlechaEquipada] > 0)
				{
					GC.qtdFlechas[GC.idFlechaEquipada] -= 1;
					GameObject tempPrefab = Instantiate(GC.flechaPrefab[GC.idFlechaEquipada], spawnFlecha.position, spawnFlecha.localRotation);
					tempPrefab.transform.localScale = new Vector3(tempPrefab.transform.localScale.x * dir.x, tempPrefab.transform.localScale.y, tempPrefab.transform.localScale.z);
					tempPrefab.GetComponent<Rigidbody2D>().velocity = new Vector2(GC.velocidadeFlecha[GC.idFlechaEquipada] * dir.x, 0);
					Destroy(tempPrefab, 2);
				}

				break;
		}
	}
	void AtkStaff(int atk)
	{
		switch (atk)
		{
			case 0:
				attaking = false;
				staffs[3].SetActive(false);
				StartCoroutine("EsperarNovoAtaque");
				break;
			case 1:
				attaking = true;
				break;
			case 2:
				if(GC.manaAtual > 0)
				{
					GC.manaAtual -= 1;
					GameObject tempPrefab = Instantiate(magiaPrefab, spawnMagia.position, spawnMagia.localRotation);
					tempPrefab.GetComponent<Rigidbody2D>().velocity = new Vector2(3 * dir.x, 0);
					Destroy(tempPrefab, 1);
				}

				break;
		}
	}

	void Interagir()
	{
		Debug.DrawRay(hand.position, dir * 0.2f, Color.red);
		RaycastHit2D hit = Physics2D.Raycast(hand.position, dir, 0.2f, interacao);

		if(hit == true)
		{
			objetoInteracao = hit.collider.gameObject;
			balaoAlerta.SetActive(true);
		}
		else
		{
			objetoInteracao = null;
			balaoAlerta.SetActive(false);
		}
		
	}

	//Eventos na Animação
	void ControleArma(int id)
	{
		foreach (GameObject a in armas)
		{
			a.SetActive(false);
		}

		armas[id].SetActive(true);
	}

	void ControleArco(int id)
	{
		foreach (GameObject a in arcos)
		{
			a.SetActive(false);
		}

		arcos[id].SetActive(true);
	}

	void ControleStaff(int id)
	{
		foreach (GameObject a in staffs)
		{
			a.SetActive(false);
		}

		staffs[id].SetActive(true);
	}

	private void OnTriggerEnter2D(Collider2D col)
	{
		switch(col.gameObject.tag)
		{
			case "Coletavel":
				col.gameObject.SendMessage("Coletar", SendMessageOptions.DontRequireReceiver);
				
				break;
			case "Inimigo":
				GC.vidaAtual -= 1;
				break;
		}
	}

	public void ChangeMaterial(Material novoMaterial)
	{
		sRender.material = novoMaterial;

		foreach(GameObject o in armas)
		{
			o.GetComponent<SpriteRenderer>().material = novoMaterial;
		}

		foreach (GameObject o in arcos)
		{
			o.GetComponent<SpriteRenderer>().material = novoMaterial;
		}

		foreach (GameObject o in flechaArco)
		{
			o.GetComponent<SpriteRenderer>().material = novoMaterial;
		}

		foreach (GameObject o in staffs)
		{
			o.GetComponent<SpriteRenderer>().material = novoMaterial;
		}
	}

	public void TrocarArma(int id)
	{
		GC.idArma = id;

		// idClasseArma 0: de perto 1: arco 2: staff
		switch (GC.idClasseArma[id])
		{
			case 0:
				armas[0].GetComponent<SpriteRenderer>().sprite = GC.spriteArmas1[id];

				ArmaInfo tempInfoArma = armas[0].GetComponent<ArmaInfo>();
				tempInfoArma.danoMin = GC.danoMinArma[id];
				tempInfoArma.danoMax = GC.danoMaxArma[id];
				tempInfoArma.tipoDano = GC.tipoDanoArma[id];

				armas[1].GetComponent<SpriteRenderer>().sprite = GC.spriteArmas2[id];

				tempInfoArma = armas[1].GetComponent<ArmaInfo>();
				tempInfoArma.danoMin = GC.danoMinArma[id];
				tempInfoArma.danoMax = GC.danoMaxArma[id];
				tempInfoArma.tipoDano = GC.tipoDanoArma[id];

				armas[2].GetComponent<SpriteRenderer>().sprite = GC.spriteArmas3[id];

				tempInfoArma = armas[2].GetComponent<ArmaInfo>();
				tempInfoArma.danoMin = GC.danoMinArma[id];
				tempInfoArma.danoMax = GC.danoMaxArma[id];
				tempInfoArma.tipoDano = GC.tipoDanoArma[id];
				break;

			case 1:
				arcos[0].GetComponent<SpriteRenderer>().sprite = GC.spriteArmas1[id];
				arcos[1].GetComponent<SpriteRenderer>().sprite = GC.spriteArmas2[id];
				arcos[2].GetComponent<SpriteRenderer>().sprite = GC.spriteArmas3[id];
				break;

			case 2:
				staffs[0].GetComponent<SpriteRenderer>().sprite = GC.spriteArmas1[id];
				staffs[1].GetComponent<SpriteRenderer>().sprite = GC.spriteArmas2[id];
				staffs[2].GetComponent<SpriteRenderer>().sprite = GC.spriteArmas3[id];
				staffs[3].GetComponent<SpriteRenderer>().sprite = GC.spriteArmas4[id];
				break;
		}

		GC.idArmaAtual = GC.idArma;
	}

	IEnumerator EsperarNovoAtaque()
	{
		yield return new WaitForSeconds(0.2f);
		naoPodeAtacar = false;
	}
}
