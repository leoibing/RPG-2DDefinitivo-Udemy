using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Inimigo : MonoBehaviour
{
	private GameController GameController;
	private Player Player;
	private SpriteRenderer sRender;
	private Animator animator;

	public int vidaInimigo, vidaAtual;
	public Color[] characterColor;

	public float[] ajusteDano;

	public bool olhandoEsquerda, playerEsquerda;

	public GameObject barrasVida, knockForcePrefab, danoTxtPrefab, loots;
	//public GameObject[] loots;
	public Transform hpBar, knockPosition, groundCheck;
	public float knockX = 0.3f;
	private float kx, percVida;
	public LayerMask whatIsGround;
	private bool getHit, died;

    void Start()
    {
		GameController = FindObjectOfType(typeof(GameController)) as GameController;
		Player = FindObjectOfType(typeof(Player)) as Player;
		sRender = GetComponent<SpriteRenderer>();
		animator = GetComponent<Animator>();

		sRender.color = characterColor[0];
		barrasVida.SetActive(false);
		vidaAtual = vidaInimigo;
		hpBar.localScale = new Vector3(1, 1, 1);

		if(olhandoEsquerda == true)
		{
			float x = transform.localScale.x;
			x *= -1;
			transform.localScale = new Vector3(x, transform.localScale.y, transform.localScale.z);
			barrasVida.transform.localScale = new Vector3(x, barrasVida.transform.localScale.x, barrasVida.transform.localScale.z);
		}
	}

    void Update()
    {
		float xPlayer = Player.transform.position.x;

		if(xPlayer < transform.position.x)
		{
			playerEsquerda = true;
		}
		if(xPlayer > transform.position.x)
		{
			playerEsquerda = false;
		}

		if (olhandoEsquerda == true && playerEsquerda == true)//?
		{
			kx = knockX;
		}
		if (olhandoEsquerda == false && playerEsquerda == true)
		{
			kx = knockX *-1;
		}
		if (olhandoEsquerda == true && playerEsquerda == false)
		{
			kx = knockX *-1;
		}
		if (olhandoEsquerda == false && playerEsquerda == false)//?
		{
			kx = knockX;
		}

		knockPosition.localPosition = new Vector3(kx, knockPosition.localPosition.y, 0);

		animator.SetBool("grounded", true);
	}

	private void OnTriggerEnter2D(Collider2D col)
	{
		if(died == true) { return; }

		switch(col.gameObject.tag)
		{
			case "Arma":

				if(getHit == false)
				{
					getHit = true;
					barrasVida.SetActive(true);
					ArmaInfo infoArma = col.gameObject.GetComponent<ArmaInfo>();

					animator.SetTrigger("hit");

					float danoArma = Random.Range(infoArma.danoMin, infoArma.danoMax); //infoArma.dano;
					int tipoDano = infoArma.tipoDano;

					float danoTomado = danoArma + (danoArma * (ajusteDano[tipoDano] / 100));

					vidaAtual -= Mathf.RoundToInt(danoTomado);

					percVida = (float)vidaAtual / (float)vidaInimigo;
					if(percVida < 0) { percVida = 0; }

					hpBar.localScale = new Vector3(percVida, 1, 1);

					if (vidaAtual <= 0)
					{
						died = true;
						animator.SetInteger("idAnimation", 3);
						StartCoroutine("Loot");
					}

					GameObject danoTemp = Instantiate(danoTxtPrefab, transform.position, transform.localRotation);
					danoTemp.GetComponent<TextMeshPro>().text = Mathf.RoundToInt(danoTomado).ToString();
					danoTemp.GetComponent<MeshRenderer>().sortingLayerName = "HUD";

					GameObject fxTemp = Instantiate(GameController.fxDano[tipoDano], transform.position, transform.localRotation);
					Destroy(fxTemp, 1);

					int forcaX = 50;
					if(playerEsquerda == false){ forcaX *= -1; }
					danoTemp.GetComponent<Rigidbody2D>().AddForce(new Vector2(forcaX, 250));
					Destroy(danoTemp, 2f);
					
					//print(danoTomado + " dano do tipo " + GameController.tiposDano[tipoDano]);

					GameObject knockTemp = Instantiate(knockForcePrefab, knockPosition.position, knockPosition.localRotation);
					Destroy(knockTemp, 0.02f);

					StartCoroutine("Invulneravel");

					gameObject.SendMessage("TomeiHit", SendMessageOptions.DontRequireReceiver);
				}
			break;
		}
	}

	void Flip() //repetido
	{
		olhandoEsquerda = !olhandoEsquerda;
		float x = transform.localScale.x;
		x *= -1;
		transform.localScale = new Vector3(x, transform.localScale.y, transform.localScale.z);
		barrasVida.transform.localScale = new Vector3(x, barrasVida.transform.localScale.x, barrasVida.transform.localScale.z);
	}

	IEnumerator Loot()
	{
		yield return new WaitForSeconds(1);
		GameObject fxMorte = Instantiate(GameController.fxMorte, groundCheck.position, transform.localRotation);
		yield return new WaitForSeconds(0.5f);
		sRender.enabled = false;

		int qtdMoedas = Random.Range(1, 5);
		for(int i = 0; i < qtdMoedas; i++)
		{
			GameObject lootTemp = Instantiate(loots, transform.position, transform.localRotation);
			lootTemp.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-10, 10), 50));
			yield return new WaitForSeconds(0.1f);
		}

		yield return new WaitForSeconds(1);
		Destroy(fxMorte);
		Destroy(gameObject);
	}

	IEnumerator Invulneravel()
	{
		sRender.color = characterColor[1];
		yield return new WaitForSeconds(0.1f);
		sRender.color = characterColor[0];
		yield return new WaitForSeconds(0.1f);

		sRender.color = characterColor[1];
		yield return new WaitForSeconds(0.1f);
		sRender.color = characterColor[0];
		yield return new WaitForSeconds(0.1f);

		sRender.color = characterColor[1];
		yield return new WaitForSeconds(0.1f);
		sRender.color = characterColor[0];
		getHit = false;
		barrasVida.SetActive(false);
	}

}
