using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ReSkin : MonoBehaviour
{
	private GameController GameController;

	public bool isPlayer;

	private SpriteRenderer sRender;

	public Sprite[] sprites;
	public string spriteSheetName, LoadedSpriteSheetName; //novo e atual

	private Dictionary<string, Sprite> spriteSheet;

	void Awake()
    {
		sRender = GetComponent<SpriteRenderer>();
	}

	void Start()
	{
		GameController = FindObjectOfType(typeof(GameController)) as GameController;

		if(isPlayer)
		{
			spriteSheetName = GameController.spriteSheetName[GameController.idPersonagem].name;
		}

		LoadSpriteSheet();
	}

	void LateUpdate()
    {
		if(isPlayer)
		{
			if(GameController.idPersonagem != GameController.idPersonagemAtual)
			{
				spriteSheetName = GameController.spriteSheetName[GameController.idPersonagem].name;
				GameController.idPersonagemAtual = GameController.idPersonagem;
			}

			GameController.ValidarArma();
		}

        if(LoadedSpriteSheetName != spriteSheetName)
		{
			LoadSpriteSheet();
		}
		sRender.sprite = spriteSheet[sRender.sprite.name];
    }

	private void LoadSpriteSheet()
	{
		sprites = Resources.LoadAll<Sprite>(spriteSheetName);
		spriteSheet = sprites.ToDictionary(x => x.name, x => x);
		LoadedSpriteSheetName = spriteSheetName;
	}
}

