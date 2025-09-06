using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public enum GameState
{
	GAMEPLAY,
	PAUSE,
	ITENS,
	DIALOGO,
	FIMDIALOGO,
	LOADGAME
}

public class GameController : MonoBehaviour
{
	private AudioController audioController;

	public int idioma;
	public string[] idiomaFolder;

	public GameState currentState;
	//private Fade fade;
	private Player Player;
	private Inventario inventario;
	private HUD hud;

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
	public ItemModelo[] armaInicialPersonagem;

	//[Header("Armas")]
	public List<string> NomeArma;
	public List<Sprite> imgInventario;

	public List<int>
		custoArma,
		idClasseArma, // 0: de perto 1: arco 2: staff
		danoMinArma, danoMaxArma,
	tipoDanoArma;

	public List<Sprite> spriteArmas1, spriteArmas2, spriteArmas3, spriteArmas4;

	public GameObject[] flechaPrefab;
	public float[] velocidadeFlecha;

	public int[]
		qtdFlechas, // 0: Normal 1: Prata 2: Ouro
		qtdPocoes; // 0: Cura 1: Mana
	public List<int> aprimoramentoArma;

	public Sprite[]
	icoFlecha,
	imgFlecha;

	public Material Luz2D, padrao2D;

	//[Header("Paineis")]
	public GameObject painelPause, painelItens, painelItemInfo;

	//[Header("fistPainel")]
	public Button fistPainelPause, fistPainelItens, fistPainelItemInfo;

	public bool missao1;

	public List<String> itensInventario;

	void Start()
    {
		//fade = FindObjectOfType(typeof(Fade)) as Fade;
		//fade.FadeOut();
		DontDestroyOnLoad(gameObject);

		audioController = FindObjectOfType(typeof(AudioController)) as AudioController;

		Player = FindObjectOfType(typeof(Player)) as Player;

		hud = FindObjectOfType(typeof(HUD)) as HUD;
		inventario = FindObjectOfType(typeof(Inventario)) as Inventario;

		painelPause.SetActive(false);
		painelItens.SetActive(false);
		painelItemInfo.SetActive(false);

		Load(PlayerPrefs.GetString("slot"));
	}

    void Update()
    {
		if(currentState == GameState.GAMEPLAY)
		{
			if (Player == null) { Player = FindObjectOfType(typeof(Player)) as Player; }

			string s = gold.ToString("N0");

			goldTxt.text = s; //.Replace(",", ".")

			//ValidarArma();

			if (Input.GetButtonDown("Cancel") && currentState != GameState.ITENS)
			{
				audioController.TocarFx(audioController.fxClick, 1);
				PauseGame();
			}
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
				audioController.TrocarMusica(audioController.musicaTitulo, "", false);
				//Time.timeScale = 0;
				ChangeState(GameState.PAUSE);
				fistPainelPause.Select();
				break;
			case false:
				audioController.TrocarMusica(audioController.musicaFase1, "", false);
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

			case GameState.FIMDIALOGO:
				StartCoroutine("FimConversa");
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

	IEnumerator FimConversa()
	{
		yield return new WaitForEndOfFrame();
		ChangeState(GameState.GAMEPLAY);
	}

	public string TextoFormatado(string frase)
	{
		string temp = frase;

		temp = temp.Replace("|cor=yellow|", "<color=#FFFF00FF>");
		temp = temp.Replace("|cor=red|", "<color=#FF0000FF>");

		temp = temp.Replace("|/fimcor|", "</color>");

		return temp;
	}

	public void Save()
	{
		string nomeArquivoSave = PlayerPrefs.GetString("slot");

		BinaryFormatter bf = new();
		FileStream file = File.Create(Application.persistentDataPath + "/" + nomeArquivoSave);
		PlayerData data = new();
		data.idioma = idioma;
		//data.idPersonagem = idPersonagem;
		data.gold = gold;
		data.idArma = idArma;

		data.idFlechaEquipada = idFlechaEquipada;
		data.qtdFlechas = qtdFlechas;
		data.qtdPocoes = qtdPocoes;
		data.aprimoramentoArma = aprimoramentoArma;

		//
		itensInventario.Clear();
		foreach(GameObject i in inventario.itemInventario)
		{
			itensInventario.Add(i.name);
		}

		data.itensInventario = itensInventario;

		bf.Serialize(file, data);
		file.Close();

	}

	public void Load(string slot)
	{
		if(File.Exists(Application.persistentDataPath + "/playerdata.dat"))
		{
			BinaryFormatter bf = new();
			FileStream file = File.Open(Application.persistentDataPath + "/playerdata.dat", FileMode.Open);
			
			PlayerData data = (PlayerData)bf.Deserialize(file);
			file.Close();

			idioma = data.idioma;
			gold = data.gold;
			idPersonagem = data.idPersonagem;
			idFlechaEquipada = data.idFlechaEquipada;
			qtdFlechas = data.qtdFlechas;
			//qtdPocoes = data.qtdPocoes;
			itensInventario = data.itensInventario;
			aprimoramentoArma = data.aprimoramentoArma;

			idArma = data.idArma;
			idArmaAtual = data.idArma;
			//idArmaInicial = data.idArma;

			inventario.itemInventario.Clear();

			foreach(string i in itensInventario)
			{
				inventario.itemInventario.Add(Resources.Load<GameObject>("Armas/" + i));
			}

			inventario.itemInventario.Add(ArmaInicial[idPersonagem]);
			GameObject tempArma = Instantiate(ArmaInicial[idPersonagem]);
			inventario.itensCarregados.Add(tempArma);
			idArmaInicial = tempArma.GetComponent<Item>().idItem;

			vidaAtual = vidaMax;
			manaAtual = manaMax;

			file.Close();
			ChangeState(GameState.GAMEPLAY);
			hud.VerificarHudPersonagem();
			//SceneManager.LoadScene("Cena1");
			string nomeCena = "Cena1";

			audioController.TrocarMusica(audioController.musicaFase1, nomeCena, true);

		}
		else
		{
			NewGame();
		}
	}

	public void Click()
	{
		audioController.TocarFx(audioController.fxClick, 1);
	}

	void NewGame()
	{
		gold = 0;
		idPersonagem = PlayerPrefs.GetInt("idPersonagem");
		idArma = armaInicialPersonagem[idPersonagem].idArma;

		idFlechaEquipada = 0;
		qtdFlechas[0] = 23;
		qtdFlechas[1] = 1;
		qtdFlechas[2] = 1;

		qtdPocoes[0] = 3;
		qtdPocoes[1] = 3;
		Save();
		Load(PlayerPrefs.GetString("slot"));
	}
}

[Serializable]
class PlayerData
{
	public int idioma,
		gold,
		idPersonagem,
		idArma,
		idFlechaEquipada;
	public int[] qtdFlechas,
		qtdPocoes;
	public List<String> itensInventario;
	public List<int> aprimoramentoArma;

}
