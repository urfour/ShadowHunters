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

	private List<DarknessCard> m_darknessDeck;
	public List<DarknessCard> DarknessDeck
	{
		get { return m_darknessDeck; }
	}

	private List<LightCard> m_lightDeck;
	public List<LightCard> LightDeck
	{
		get { return m_lightDeck; }
	}

	// Défausses des différents types de cartes
	private List<VisionCard> m_hermit;
	public List<VisionCard> Hermit
	{
		get { return m_hermit; }
	}

	private List<DarknessCard> m_black;
	public List<DarknessCard> Black 
	{
		get { return m_black; }
	}

	private List<LightCard> m_white;
	public List<LightCard> White
	{
		get { return m_white; }
	}

	// Lieux du plateau
	private LocationCard[] m_areas;
	public LocationCard[] Areas
	{
		get { return m_areas; }
	}

	public LocationCard getAreaAt(int index)
	{
		return m_areas[index];
	}

	public LocationCard getAreaByPosition(Position position)
	{
		foreach(LocationCard location in m_areas)
			if(location.area = position)
				return location;
		return null;
	}

	// Position de chaque joueur (index = nbPlayers)
	private Position[] m_position;

	public Position getPositionOf(int index)
	{
		return m_position[index];
	}

	public string GetAreaNameByPosition(Position position)
	{
		foreach (LocationCard location in m_areas)
			if (location.area == position)
				return location.cardName;
		return null;
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
	public GameBoard(List<LocationCard> l_areas, List<VisionCard> l_hermit, List<DarknessCard> l_black, List<LightCard> l_white, int nbPlayers)
	{

		m_visionDeck = l_hermit;
		m_darknessDeck = l_black;
		m_lightDeck = l_white;

		m_hermit = new List<VisionCard>();
		m_black = new List<DarknessCard>();
		m_white = new List<LightCard>();

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

				if (m_visionDeck.Count == 0)
				{
					foreach (VisionCard card in m_hermit)
					{
						m_visionDeck.Add(card);
						m_hermit.RemoveAt(0);
					}
				}
				m_visionDeck.Shuffle<VisionCard>();
				break;
			case CardType.Darkness:
				pickedCard = m_darknessDeck[0];
				pickedCard.isHidden = false;
				m_darknessDeck.RemoveAt(0);

				if (m_darknessDeck.Count == 0)
				{
					foreach (DarknessCard card in m_black)
					{
						m_darknessDeck.Add(card);
						m_black.RemoveAt(0);
					}
				}
				m_darknessDeck.Shuffle<DarknessCard>();
				break;
			case CardType.Light:
				pickedCard = m_lightDeck[0];
				pickedCard.isHidden = false;
				m_lightDeck.RemoveAt(0);

				if (m_lightDeck.Count == 0)
				{
					foreach (LightCard card in m_white)
					{
						m_lightDeck.Add(card);
						m_white.RemoveAt(0);
					}
				}
				m_lightDeck.Shuffle<LightCard>();
				break;
		}
		if (cardType != CardType.Vision)
		{
			Debug.Log("Carte piochée : " + pickedCard.cardName);
			Debug.Log("Effet : " + pickedCard.description);
		}
		return pickedCard;
	}

	public void AddDiscard(Card card, CardType cardType)
	{
		string type = "";
		switch (cardType)
		{
			case CardType.Vision:
				m_hermit.Add(card as VisionCard);
				type += "Vision";
				break;
			case CardType.Darkness:
				m_black.Add(card as DarknessCard);
				type += "Ténèbres";
				break;
			case CardType.Light:
				m_white.Add(card as LightCard);
				type += "Lumière";
				break;
		}
		Debug.Log("La carte " + type + " a été ajoutée à la défausse.");
	}

	public void RemoveDiscard(Card card, CardType cardType)
	{
		string type = "";
		switch (cardType)
		{
			case CardType.Vision:
				m_hermit.Remove(card as VisionCard);
				type += "Vision";
				break;
			case CardType.Darkness:
				m_black.Remove(card as DarknessCard);
				type += "Ténèbres";
				break;
			case CardType.Light:
				m_white.Remove(card as LightCard);
				type += "Lumière";
				break;
		}
		Debug.Log("La carte " + type + " a été retirée à la défausse.");
	}
}