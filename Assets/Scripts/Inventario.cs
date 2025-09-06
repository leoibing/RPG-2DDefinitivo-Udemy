using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Inventario : MonoBehaviour
{
	private GameController GC;

	public Button[] slot;
	public Image[] iconItem;

	public TextMeshProUGUI qtdPocao, qtdMana, qtdFlechaA, qtdFlechaB, qtdFlechaC;

	public int qPocao, qMana, qFlechaA, qFlechaB, qFlechaC;

	public List<GameObject> itemInventario, itensCarregados;

	void Start()
    {
		GC = FindObjectOfType(typeof(GameController)) as GameController;

	}

    public void CarregarInventario()
    {
		LimparItensCarregados();

		foreach (Button b in slot)
		{
			b.interactable = false;
		}

		foreach (Image i in iconItem)
		{
			i.sprite = null;
			i.gameObject.SetActive(false);
		}

		qtdPocao.text = "x " + GC.qtdPocoes[0].ToString();
		qtdMana.text = "x " + GC.qtdPocoes[1].ToString();
		qtdFlechaA.text = "x " + GC.qtdFlechas[0].ToString();
		qtdFlechaB.text = "x " + GC.qtdFlechas[1].ToString();
		qtdFlechaC.text = "x " + GC.qtdFlechas[2].ToString();

		int s = 0;

		foreach(GameObject i in itemInventario)
		{
			GameObject temp = Instantiate(i);

			Item itemInfo = temp.GetComponent<Item>();

			itensCarregados.Add(temp);

			slot[s].GetComponent<SlotInventario>().objetoSlot = temp;
			slot[s].interactable = true;

			iconItem[s].sprite = GC.imgInventario[itemInfo.idItem];
			iconItem[s].gameObject.SetActive(true);

			s++;
		}
	}

	public void LimparItensCarregados()
	{
		foreach (GameObject ic in itensCarregados)
		{
			Destroy(ic);
		}

		itensCarregados.Clear();
	}
}
