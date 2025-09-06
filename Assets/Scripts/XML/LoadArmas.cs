using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Linq;

public class LoadArmas : MonoBehaviour
{
	private GameController GC;

	public string nomeArquivoXml;

	public List<string> nomeArma,
		nomeIconeArma;

	public List<Sprite> iconeArma;
	public List<string> categoriaArma;
	public List<int> idClasseArma;

	public List<int> danoMinArma,
		danoMaxArma,
		tipoDanoArma;

	public List<Sprite> spriteArmas1,
		spriteArmas2,
		spriteArmas3,
		spriteArmas4;

	// temporarios
	public List<Sprite> bancoDeSpritesArma;
	public Sprite[] SpriteSheetIconesArmas,
		espadas,
		machados,
		arcos,
		macas,
		martelos,
		staffs;

	private Dictionary<string, Sprite> SpriteSheetArmas;

	public Texture ssIcones,
		ssEspadas,
		ssMachados,
		ssArcos,
		ssMacas,
		ssMartelos,
		ssStaffs;

	void Start()
    {
		GC = FindObjectOfType(typeof(GameController)) as GameController;

		LoadData();

	}

	void LoadData()
	{
		SpriteSheetIconesArmas = Resources.LoadAll<Sprite>(ssIcones.name);

		espadas = Resources.LoadAll<Sprite>(ssEspadas.name);
		machados = Resources.LoadAll<Sprite>(ssMachados.name);
		arcos = Resources.LoadAll<Sprite>(ssArcos.name);
		macas = Resources.LoadAll<Sprite>(ssMacas.name);
		martelos = Resources.LoadAll<Sprite>(ssMartelos.name);
		staffs = Resources.LoadAll<Sprite>(ssStaffs.name);

		foreach (Sprite s in espadas)
		{
			bancoDeSpritesArma.Add(s);
		}

		foreach (Sprite s in machados)
		{
			bancoDeSpritesArma.Add(s);
		}

		foreach (Sprite s in arcos)
		{
			bancoDeSpritesArma.Add(s);
		}

		foreach (Sprite s in macas)
		{
			bancoDeSpritesArma.Add(s);
		}

		foreach (Sprite s in martelos)
		{
			bancoDeSpritesArma.Add(s);
		}

		foreach (Sprite s in staffs)
		{
			bancoDeSpritesArma.Add(s);
		}

		SpriteSheetArmas = bancoDeSpritesArma.ToDictionary(x => x.name, x => x);

		//XML
		TextAsset xmlData = (TextAsset)Resources.Load(GC.idiomaFolder[GC.idioma] + "/" + nomeArquivoXml);
		XmlDocument xmlDocument = new();

		xmlDocument.LoadXml(xmlData.text);

		foreach (XmlNode atributo in xmlDocument["Armas"].ChildNodes)
		{
			string att = atributo.Attributes["atributo"].Value;

			foreach (XmlNode a in atributo["armas"].ChildNodes)
			{
				switch(att)
				{
					case "nome":
						nomeArma.Add(a.InnerText);
						break;
					case "icone":
						nomeIconeArma.Add(a.InnerText);

						//
						for(int i = 0; i < SpriteSheetIconesArmas.Length; i++)
						{
							if(SpriteSheetIconesArmas[i].name == a.InnerText)
							{
								iconeArma.Add(SpriteSheetIconesArmas[i]);
								break;
							}
						}
						break;

					case "categoria":
						categoriaArma.Add(a.InnerText);

						if(a.InnerText == "Staff")
						{
							idClasseArma.Add(2);
						}
						else if (a.InnerText == "Arco")
						{
							idClasseArma.Add(1);
						}
						else
						{
							idClasseArma.Add(0);
						}

						break;

					case "danoMin":
						danoMinArma.Add(int.Parse(a.InnerText));
						break;

					case "danoMax":
						danoMaxArma.Add(int.Parse(a.InnerText));
						break;

					case "tipoDano":
						tipoDanoArma.Add(int.Parse(a.InnerText));
						break;
				}
			}
		}

		for(int i = 0; i < iconeArma.Count; i++)
		{
			spriteArmas1.Add(SpriteSheetArmas[nomeIconeArma[i] + "0"]);
			spriteArmas2.Add(SpriteSheetArmas[nomeIconeArma[i] + "1"]);
			spriteArmas3.Add(SpriteSheetArmas[nomeIconeArma[i] + "2"]);

			if (categoriaArma[i] != "Staff")
			{
				spriteArmas4.Add(null);
			}
			else
			{
				spriteArmas4.Add(SpriteSheetArmas[nomeIconeArma[i] + "3"]);
			}
		}
		atualizarGameController();
	}

	public void atualizarGameController()
	{
		GC.NomeArma = nomeArma;
		GC.idClasseArma = idClasseArma;

		GC.danoMinArma = danoMinArma;
		GC.danoMaxArma = danoMaxArma;
		GC.tipoDanoArma = tipoDanoArma;

		GC.imgInventario = iconeArma;

		GC.spriteArmas1 = spriteArmas1;
		GC.spriteArmas2 = spriteArmas2;
		GC.spriteArmas3 = spriteArmas3;
		GC.spriteArmas4 = spriteArmas4;
	}
}
