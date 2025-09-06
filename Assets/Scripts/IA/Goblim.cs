using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState
{
	PARADO,
	ALERTA,
	PATRULHA,
	ATK,
	RECUAR
}

public class Goblim : MonoBehaviour
{
	private GameController GC;
	private Player Player;
	private SpriteRenderer sRender;

	private Rigidbody2D rb;
	private Animator animator;

	public EnemyState currentEnemyState;
	public EnemyState stateInicial;

	public float velocidadeBase = 0.5f;
	public float velocidade = 0.5f;

	public float tempoEsperaIdle;
	public float tempoRecuo = 2;

	private Vector3 dir = Vector3.right;
	public float distanciaMudarRota = 0.5f;
	public LayerMask LayerObstaculos;

	public float distanciaVerPersonagem = 2.5f;
	public float distanciaAtaque = 0.8f;
	public float distanciaSairAlerta = 4f;
	public LayerMask LayerPersonagem;

	public GameObject alert;

	public bool lookLeft;

	private bool attaking;
	public int idArma;
	public int idClasseArma;
	public GameObject[] armas,
		arcos,
		staffs,
		flechaArco;

	public bool emAlertaHit,
		ambienteEscuro;

	void Start()
    {
		GC = FindObjectOfType(typeof(GameController)) as GameController;
		Player = FindObjectOfType(typeof(Player)) as Player;

		rb = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
		sRender = GetComponent<SpriteRenderer>();

		//velocidade = velocidadeBase;
		if (lookLeft == true) { Flip(); }

		ChangeState(stateInicial);
		TrocarArma(idArma);

		if(ambienteEscuro == true)
		{
			ChangeMaterial(GC.Luz2D);
		}
		else
		{
			ChangeMaterial(GC.padrao2D);
		}
	}

    void Update()
    {
		if(currentEnemyState != EnemyState.ATK && currentEnemyState != EnemyState.RECUAR)
		{
			Debug.DrawRay(transform.position, dir * distanciaVerPersonagem, Color.red);
			RaycastHit2D hitp = Physics2D.Raycast(transform.position, dir, distanciaVerPersonagem, LayerPersonagem);

			if (hitp == true)
			{
				ChangeState(EnemyState.ALERTA);
			}
		}

		if (currentEnemyState == EnemyState.PATRULHA)
		{
			//Debug.DrawRay(transform.position, dir * distanciaMudarRota, Color.blue);
			RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, distanciaMudarRota, LayerObstaculos);

			if (hit == true)
			{
				ChangeState(EnemyState.PARADO);
			}
		}

		if (currentEnemyState == EnemyState.RECUAR)
		{
			//Debug.DrawRay(transform.position, dir * distanciaMudarRota, Color.blue);
			RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, distanciaMudarRota, LayerObstaculos);

			if (hit == true)
			{
				Flip();
			}
		}

		if (currentEnemyState == EnemyState.ALERTA)
		{
			float dist = Vector3.Distance(transform.position, Player.transform.position);

			if(dist <= distanciaAtaque)
			{
				ChangeState(EnemyState.ATK);
			}
			else if(dist >= distanciaSairAlerta && emAlertaHit == false)
			{
				ChangeState(EnemyState.PARADO);
			}
		}

		if(currentEnemyState != EnemyState.ALERTA)
		{
			alert.SetActive(false);
		}

		rb.velocity = new Vector2(velocidade, rb.velocity.y);

		if (velocidade == 0) { animator.SetInteger("idAnimation", 0); }
		else if (velocidade != 0) { animator.SetInteger("idAnimation", 1); }

		animator.SetFloat("idClasseArma", idClasseArma);
	}

	void Flip()
	{
		lookLeft = !lookLeft;
		float x = transform.localScale.x;
		x *= -1;
		transform.localScale = new Vector3(x, transform.localScale.y, transform.localScale.z);
		dir.x = x;
		velocidadeBase *= -1;
		float vAtual = velocidadeBase *= -1;
		velocidade = vAtual;
	}

	IEnumerator Idle()
	{
		yield return new WaitForSeconds(tempoEsperaIdle);
		Flip();
		ChangeState(EnemyState.PATRULHA);
	}

	IEnumerator Recuar()
	{
		yield return new WaitForSeconds(tempoRecuo);
		Flip();
		ChangeState(EnemyState.ALERTA);
	}

	void ChangeState(EnemyState newState)
	{
		currentEnemyState = newState;
		switch(newState)
		{
			case EnemyState.PARADO:
				velocidade = 0;
				StartCoroutine("Idle");
				break;
			case EnemyState.PATRULHA:
				velocidade = velocidadeBase;
				break;
			case EnemyState.ALERTA:
				velocidade = 0;
				alert.SetActive(true);
				break;
			case EnemyState.ATK:
				animator.SetTrigger("atk");
				break;
			case EnemyState.RECUAR:
				Flip();
				velocidade = velocidadeBase *2;
				StartCoroutine("Recuar");
				break;
		}
	}

	//Eventos na Animação
	void Atk(int atk)
	{
		switch (atk)
		{
			case 0:
				attaking = false;
				armas[2].SetActive(false);
				ChangeState(EnemyState.RECUAR);
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
				break;
			case 1:
				attaking = true;
				break;
			case 2:
				if (GC.qtdFlechas[GC.idFlechaEquipada] > 0)
				{
					/*GC.qtdFlechas[GC.idFlechaEquipada] -= 1;
					GameObject tempPrefab = Instantiate(GC.flechaPrefab[GC.idFlechaEquipada], spawnFlecha.position, spawnFlecha.localRotation);
					tempPrefab.transform.localScale = new Vector3(tempPrefab.transform.localScale.x * dir.x, tempPrefab.transform.localScale.y, tempPrefab.transform.localScale.z);
					tempPrefab.GetComponent<Rigidbody2D>().velocity = new Vector2(GC.velocidadeFlecha[GC.idFlechaEquipada] * dir.x, 0);
					Destroy(tempPrefab, 2);
				*/}

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
				break;
			case 1:
				attaking = true;
				break;
			case 2:
				if (GC.manaAtual > 0)
				{
					/*GC.manaAtual -= 1;
					GameObject tempPrefab = Instantiate(magiaPrefab, spawnMagia.position, spawnMagia.localRotation);
					tempPrefab.GetComponent<Rigidbody2D>().velocity = new Vector2(3 * dir.x, 0);
					Destroy(tempPrefab, 1);
				*/}

				break;
		}
	}

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

	public void TrocarArma(int id)
	{
		//GC.idArma = id;
		// idClasseArma 0: de perto 1: arco 2: staff
		switch (id) // GC.idClasseArma[id]
		{
			case 0:
				armas[0].GetComponent<SpriteRenderer>().sprite = GC.spriteArmas1[idArma];

				ArmaInfo tempInfoArma = armas[0].GetComponent<ArmaInfo>();
				tempInfoArma.danoMin = GC.danoMinArma[idArma];
				tempInfoArma.danoMax = GC.danoMaxArma[idArma];
				tempInfoArma.tipoDano = GC.tipoDanoArma[idArma];

				armas[1].GetComponent<SpriteRenderer>().sprite = GC.spriteArmas2[idArma];

				tempInfoArma = armas[1].GetComponent<ArmaInfo>();
				tempInfoArma.danoMin = GC.danoMinArma[idArma];
				tempInfoArma.danoMax = GC.danoMaxArma[idArma];
				tempInfoArma.tipoDano = GC.tipoDanoArma[idArma];

				armas[2].GetComponent<SpriteRenderer>().sprite = GC.spriteArmas3[idArma];

				tempInfoArma = armas[2].GetComponent<ArmaInfo>();
				tempInfoArma.danoMin = GC.danoMinArma[idArma];
				tempInfoArma.danoMax = GC.danoMaxArma[idArma];
				tempInfoArma.tipoDano = GC.tipoDanoArma[idArma];
				break;

			case 1:
				arcos[0].GetComponent<SpriteRenderer>().sprite = GC.spriteArmas1[idArma];
				arcos[1].GetComponent<SpriteRenderer>().sprite = GC.spriteArmas2[idArma];
				arcos[2].GetComponent<SpriteRenderer>().sprite = GC.spriteArmas3[idArma];
				break;

			case 2:
				staffs[0].GetComponent<SpriteRenderer>().sprite = GC.spriteArmas1[idArma];
				staffs[1].GetComponent<SpriteRenderer>().sprite = GC.spriteArmas2[idArma];
				staffs[2].GetComponent<SpriteRenderer>().sprite = GC.spriteArmas3[idArma];
				staffs[3].GetComponent<SpriteRenderer>().sprite = GC.spriteArmas4[idArma];
				break;
		}
		//GC.idArmaAtual = GC.idArma;
	}

	public void tomeiHit()
	{
		emAlertaHit = true;
		StartCoroutine("HitAlerta");
		ChangeState(EnemyState.ALERTA);
	}

	IEnumerator HitAlerta()
	{
		yield return new WaitForSeconds(1);
		emAlertaHit = false;
	}

	public void ChangeMaterial(Material novoMaterial)
	{
		sRender.material = novoMaterial;

		foreach (GameObject o in armas)
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
}
