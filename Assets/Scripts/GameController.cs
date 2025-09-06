using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum GameState
{
	GAMEPLAY,
	PAUSE,
	ITENS
}

public class GameController : MonoBehaviour
{
	public GameState currentState;
	//private Fade fade;
	private Player Player;
	private Inventario inventario;

	public string[] tiposDano;
	public GameObject[] fxDano;
	public GameObject fxMorte;

	public int gold;

	public TextMeshProUGUI goldTxt;

	//[Header("Player")]
	public int
		idPersonagem, idPersonagemAtual,
		idArma, idArmaAtual,
		vidaMax = 100,
		vidaAtual,
		manaMax = 10,
		manaAtual;

	//[Header("Personagens")]
	public string[] nomePersonagem; 
	public Texture[] spriteSheetName;
	public int[] idClasse;
	public int
		idArmaInicial,
		idFlechaEquipada;
	public GameObject[] ArmaInicial;

	//[Header("Armas")]
	public string[] NomeArma;

	public Sprite[]
		imgInventario,
		spriteArmas1, spriteArmas2, spriteArmas3, spriteArmas4,
		icoFlecha,
		imgFlecha;
	public GameObject[] flechaPrefab;
	public float[] velocidadeFlecha;
	public int[]
		qtdFlechas, // 0: Normal 1: Prata 2: Ouro
		qtdPocoes, // 0: Cura 1: Mana
		custoArma,
		idClasseArma, // 0: de perto 1: arco 2: staff
		danoMinArma, danoMaxArma,
		tipoDanoArma,
		aprimoramentoArma;

	public Material Luz2D, padrao2D;

	//[Header("Paineis")]
	public GameObject painelPause, painelItens, painelItemInfo;

	//[Header("fistPainel")]
	public Button fistPainelPause, fistPainelItens, fistPainelItemInfo;

	void Start()
    {
		//fade = FindObjectOfType(typeof(Fade)) as Fade;
		//fade.FadeOut();
		DontDestroyOnLoad(gameObject);
		Player = FindObjectOfType(typeof(Player)) as Player;

		inventario = FindObjectOfType(typeof(Inventario)) as Inventario;

		painelPause.SetActive(false);
		painelItens.SetActive(false);
		painelItemInfo.SetActive(false);

		idPersonagem = PlayerPrefs.GetInt("idPersonagem");

		inventario.itemInventario.Add(ArmaInicial[idPersonagem]);

		GameObject tempArma = Instantiate(ArmaInicial[idPersonagem]);
		inventario.itensCarregados.Add(tempArma);

		idArmaInicial = tempArma.GetComponent<Item>().idItem;

		vidaAtual = vidaMax;
		manaAtual = manaMax;
    }

    void Update()
    {
		if(Player == null) { Player = FindObjectOfType(typeof(Player)) as Player; }

		string s = gold.ToString("N0");

		goldTxt.text = s; //.Replace(",", ".")

		//ValidarArma();

		if(Input.GetButtonDown("Cancel") && currentState != GameState.ITENS)
		{
			PauseGame();
		}
	}

	public void ValidarArma()
	{
		if(idClasseArma[idArma] != idClasse[idPersonagem])
		{
			idArma = idArmaInicial; //[idPersonagem]
			Player.TrocarArma(idArma);
		}
	}

	public void PauseGame()
	{
		bool pauseState = painelPause.activeSelf;
		pauseState = !pauseState;

		painelPause.SetActive(pauseState);

		switch(pauseState)
		{
			case true:
				//Time.timeScale = 0;
				ChangeState(GameState.PAUSE);
				fistPainelPause.Select();
				break;
			case false:
				//Time.timeScale = 1;
				ChangeState(GameState.GAMEPLAY);
				break;
		}
	}

	public void ChangeState(GameState newState)
	{
		currentState = newState;
		switch(newState)
		{
			case GameState.GAMEPLAY:
				Time.timeScale = 1;
				break;

			case GameState.ITENS:
				Time.timeScale = 0;
				break;

			case GameState.PAUSE:
				Time.timeScale = 0;
				break;
		}
	}

	public void BtnItensDown()
	{
		painelPause.SetActive(false);
		painelItens.SetActive(true);
		fistPainelItens.Select();
		inventario.CarregarInventario();
		ChangeState(GameState.ITENS);
	}

	public void FecharPainel()
	{
		painelItens.SetActive(false);
		painelPause.SetActive(true);

		inventario.LimparItensCarregados();

		fistPainelPause.Select();
		ChangeState(GameState.PAUSE);
	}

	public void UsarItemArma(int idArma)
	{
		Player.TrocarArma(idArma);
	}

	public void OpenItemInfo()
	{
		painelItemInfo.SetActive(true);
		fistPainelItemInfo.Select();
	}

	public void FecharItemInfo()
	{
		painelItemInfo.SetActive(false);
	}

	public void VoltarGamePlay()
	{
		painelItens.SetActive(false);
		painelPause.SetActive(false);
		painelItemInfo.SetActive(false);
		ChangeState(GameState.GAMEPLAY);
	}

	public void ExcluirItem(int idSlot)
	{
		inventario.itemInventario.RemoveAt(idSlot);
		inventario.CarregarInventario();
		painelItemInfo.SetActive(false);
		fistPainelItens.Select();
	}

	public void AprimorarArma(int idArma)
	{
		int ap = aprimoramentoArma[idArma];
		if(ap < 10)
		{
			ap += 1;
			aprimoramentoArma[idArma] = ap;
		}
	}

	public void Swap(int idSlot)
	{
		GameObject t1 = inventario.itemInventario[0];
		GameObject t2 = inventario.itemInventario[idSlot];

		inventario.itemInventario[0] = t2;
		inventario.itemInventario[idSlot] = t1;

		VoltarGamePlay();
	}
	public void ColetarArma(GameObject objetoColetado)
	{
		inventario.itemInventario.Add(objetoColetado);
	}

	public void UsarPocao(int idPocao)
	{ // 0: Cura 1: Mana

		if(qtdPocoes[idPocao] > 0)
		{
			qtdPocoes[idPocao] -= 1;

			switch (idPocao)
			{
				case 0:
					vidaAtual += 3;
					if(vidaAtual > vidaMax) { vidaAtual = vidaMax; }

					break;
				case 1:
					manaAtual += 3;
					if(manaAtual > manaMax) { manaAtual = manaMax; }

					break;
			}
		}

	}
}
