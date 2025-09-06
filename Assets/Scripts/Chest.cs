using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
	//private GameController GC;
	private SpriteRenderer spriteRenderer;
	public Sprite[] imagemObjeto;
	public bool open;
	public GameObject[] loots;
	private bool gerouLoot;
	public int qtdMinItens, qtdMaxItens;

	void Start()
    {
		//GC = FindObjectOfType(typeof(GameController)) as GameController;
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

    public void Interacao()
    {
        if(open == false)
		{
			open = true;
			spriteRenderer.sprite = imagemObjeto[1];

			if(gerouLoot == false)
			{
				StartCoroutine("GerarLoot");
			}

			GetComponent<Collider2D>().enabled = false;
			print(transform.name);
		}
    }

	IEnumerator GerarLoot()
	{
		gerouLoot = true;
		int qtdMoedas = Random.Range(qtdMinItens, qtdMaxItens);
		for (int i = 0; i < qtdMoedas; i++)
		{
			//int rand = 0;
			int idLot = 0;

			/*int rand = Random.Range(0, 100);

			if(rand >= 75)
			{
				idLot = 1;
			}
			*/
			GameObject lootTemp = Instantiate(loots[idLot], transform.position, transform.localRotation);
			lootTemp.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-10, 10), 50));
			yield return new WaitForSeconds(0.1f);
		}
	}
}
