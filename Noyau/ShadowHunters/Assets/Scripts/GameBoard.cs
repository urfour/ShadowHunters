using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoard
{
	// Decks des différents types de cartes
	private List<VisionCard> m_visionDeck;
	public List<VisionCard> VisionDeck
	{
		get { return m_visionDeck; }
	}

	private List<Card> m_darknessDeck;
	public List<Card> DarknessDeck
	{
		get { return m_darknessDeck; }
	}

	private List<Card> m_lightDeck;
	public List<Card> LightDeck
	{
		get { return m_lightDeck; }
	}

	// Défausses des différents types de cartes
	private List<VisionCard> m_hermit;
	public List<VisionCard> Hermit
	{
		get { return m_hermit; }
	}

	private List<Card> m_black;
	public List<Card> Black 
	{
		get { return m_black; }
	}

	private List<Card> m_white;
	public List<Card> White
	{
		get { return m_white; }
	}

	// Lieux du plateau
	private Card[] m_areas;

	public Card getAreaAt(int index)
	{
		return m_areas[index];
	}

	// Position de chaque joueur (index = nbPlayers)
	private Position[] m_position;

	public Position getPositionOf(int index)
	{
		return m_position[index];
	}

	public int GetCardIndexByName(string name)
	{
		for (int i = 0; i < m_areas.Length; i++)
			if (m_areas[i].cardName == name)
			{
				Debug.Log("Carte trouvée : " + m_areas[i].cardName);
				return i;
			}

		return -1;
	}

	public void setPositionOfAt(int player, Position position)
	{
		m_position[player] = position;
	}

	// Dégâts de chaque joueur (index = nbPlayers)
	private int[] m_damage;

	public int getDamageOf(int index)
	{
		return m_damage[index];
	}

	public void increaseDamageOfBy(int player, int damage)
	{
		m_damage[player] += damage;
	}

	//Constructeur (l_areas = liste mélangée des 6 lieux utilisés)
	public GameBoard(List<Card> l_areas, List<VisionCard> l_hermit, List<Card> l_black, List<Card> l_white, int nbPlayers)
	{

		m_visionDeck = l_hermit;
		m_darknessDeck = l_black;
		m_lightDeck = l_white;

		m_hermit = new List<VisionCard>();
		m_black = new List<Card>();
		m_white = new List<Card>();

		m_areas = l_areas.ToArray();

		m_position = new Position[nbPlayers];
		m_damage = new int[nbPlayers];
	}

	public void PrintLog()
    {
		Debug.Log("Ténèbre : " + m_darknessDeck.Count);
		Debug.Log("Vision : " + m_visionDeck.Count);
		Debug.Log("Lumière : " + m_lightDeck.Count);
    }

	public Card DrawCard(CardType cardType)
	{
		Card pickedCard = ScriptableObject.CreateInstance("Card") as Card;
		switch (cardType)
		{
			case CardType.Vision:
				pickedCard = m_visionDeck[0];
				pickedCard.isHidden = false;
				m_visionDeck.RemoveAt(0);
				break;
			case CardType.Darkness:
				pickedCard = m_darknessDeck[0];
				pickedCard.isHidden = false;
				m_darknessDeck.RemoveAt(0);
				break;
			case CardType.Light:
				pickedCard = m_lightDeck[0];
				pickedCard.isHidden = false;
				m_lightDeck.RemoveAt(0);
				break;
		}
		if (cardType != CardType.Vision)
		{
			Debug.Log("Carte piochée : " + pickedCard.cardName);
			Debug.Log("Effet : " + pickedCard.description);
		}
		return pickedCard;
	}
}
