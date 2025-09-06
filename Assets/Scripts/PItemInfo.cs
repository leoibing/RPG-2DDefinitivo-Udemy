using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PItemInfo : MonoBehaviour
{
	private GameController GameController;

	public int idSlot;
	public GameObject objetoSlot;

	//[Header("hud")]
	public Image imgItem;
	public TMP_Text nomeItem,
					danoArma;

	public GameObject[] aprimoramentos;

	//[Header("Btn")]
	public Button btnAprimorar,
		btnEquipar,
		btnExcluir;

	private int idArma,
		aprimoramento;

	void Start()
    {
		GameController = FindObjectOfType(typeof(GameController)) as GameController;
	}

    public void CarregarInfoItem()
    {
		Item itemInfo = objetoSlot.GetComponent<Item>();
		idArma = itemInfo.idItem;

		imgItem.sprite = GameController.imgInventario[idArma];
		nomeItem.text = GameController.NomeArma[idArma];

		string tipoDano = GameController.tiposDano[GameController.tipoDanoArma[idArma]];

		CarregarAprimoramento();

		int danoMin = GameController.danoMinArma[idArma]; // + aprimoramento
		int danoMax = GameController.danoMaxArma[idArma]; // + aprimoramento

		danoArma.text = "Dano: " + danoMin.ToString() + "-" + danoMax.ToString() + " / " + tipoDano;
	
		if(idSlot == 0)
		{
			btnEquipar.interactable = false;
			btnExcluir.interactable = false;
		}
		else
		{
			int idClasseArma = GameController.idClasseArma[idArma];
			int idClassePersonagem = GameController.idClasse[GameController.idPersonagem];

			if(idClasseArma == idClassePersonagem)
			{
				btnEquipar.interactable = true;
			}
			else
			{
				btnEquipar.interactable = false;
			}

			

			btnExcluir.interactable = true;
		}
	}

	public void BAprimorar()
	{
		GameController.AprimorarArma(idArma);
		CarregarAprimoramento();
	}

	public void BEquipar()
	{
		objetoSlot.SendMessage("UsarItem", SendMessageOptions.DontRequireReceiver);
		GameController.Swap(idSlot);
	}

	public void BExcluir()
	{
		GameController.ExcluirItem(idSlot);
	}

	void CarregarAprimoramento()
	{
		aprimoramento = GameController.aprimoramentoArma[idArma];

		if(aprimoramento >= 10) 
		{
			btnAprimorar.interactable = false;
		}
		else
		{
			btnAprimorar.interactable = true;
		}

		foreach (GameObject a in aprimoramentos)
		{
			a.SetActive(false);
		}

		for (int i = 0; i < aprimoramento; i++)
		{
			aprimoramentos[i].SetActive(true);
		}
	}
}
