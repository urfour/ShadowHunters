/*using Assets.Noyau.Cards.controller;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Plateau de jeu comportant les différentes cartes
/// </summary>
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

	public LocationCard GetAreaAt(int index)
	{
		return m_areas[index];
	}

	public int GetIndexOfPosition(Position position)
	{
		int index = -1;
		for (int i = 0 ; i < m_areas.Length ; i++)
			if (m_areas[i].area == position)
				index = i;

		return index;
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

	public int GetDamageOf(int index)
	{
		return m_damage[index];
	}

	public void IncreaseDamageOfBy(int player, int damage)
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
		Debug.Log("################## CARTES ##################");
		Debug.Log("--------------------------------------------");
		Debug.Log("Cartes Ténèbre : " + m_darknessDeck.Count);
		for (int i = 0; i < m_darknessDeck.Count; i++)
			Debug.Log("Carte n°" + i + " : " + m_darknessDeck[i].cardName);
		Debug.Log("--------------------------------------------");
		Debug.Log("Cartes Vision : " + m_visionDeck.Count);
		for (int i = 0; i < m_visionDeck.Count; i++)
			Debug.Log("Carte n°" + i + " : " + m_visionDeck[i].cardName);
		Debug.Log("--------------------------------------------");
		Debug.Log("Cartes Lumière : " + m_lightDeck.Count);
		for (int i = 0; i < m_lightDeck.Count; i++)
			Debug.Log("Carte n°" + i + " : " + m_lightDeck[i].cardName);
		Debug.Log("--------------------------------------------");
		Debug.Log("");
		Debug.Log("################# DÉFAUSSE #################");
		Debug.Log("--------------------------------------------");
		Debug.Log("Cartes Ténèbre : " + m_black.Count);
		for (int i = 0; i < m_black.Count; i++)
			Debug.Log("Carte n°" + i + " : " + m_black[i].cardName);
		Debug.Log("--------------------------------------------");
		Debug.Log("Cartes Vision : " + m_hermit.Count);
		for (int i = 0; i < m_hermit.Count; i++)
			Debug.Log("Carte n°" + i + " : " + m_hermit[i].cardName);
		Debug.Log("--------------------------------------------");
		Debug.Log("Cartes Lumière : " + m_white.Count);
		for (int i = 0; i < m_white.Count; i++)
			Debug.Log("Carte n°" + i + " : " + m_white[i].cardName);
		Debug.Log("--------------------------------------------");
	}

	public Card DrawCard(CardType cardType)
	{
		Card pickedCard = ScriptableObject.CreateInstance("Card") as Card;
		switch (cardType)
		{
			case CardType.Vision:
				if (m_visionDeck.Count == 0)
				{
					for (int i = 0; i < m_hermit.Count; i++)
					{
						m_visionDeck.Add(m_hermit[i]);
						m_hermit.RemoveAt(0);
					}
					m_visionDeck.Shuffle<VisionCard>();
					Debug.Log("Redistribution du deck Vision");
				}
				pickedCard = m_visionDeck[0];
				pickedCard.isHidden = false;
				m_visionDeck.RemoveAt(0);
				break;
			case CardType.Darkness:
				if (m_darknessDeck.Count == 0)
				{
					for (int i = 0; i < m_black.Count; i++)
					{
						m_darknessDeck.Add(m_black[i]);
						m_black.RemoveAt(0);
					}
					m_darknessDeck.Shuffle<DarknessCard>();
					Debug.Log("Redistribution du deck Ténèbre");
				}
				pickedCard = m_darknessDeck[0];
				pickedCard.isHidden = false;
				m_darknessDeck.RemoveAt(0);
				break;
			case CardType.Light:
				if (m_lightDeck.Count == 0)
				{
					for (int i = 0; i < m_white.Count; i++)
					{
						m_lightDeck.Add(m_white[i]);
						m_white.RemoveAt(0);
					}
					m_lightDeck.Shuffle<LightCard>();
					Debug.Log("Redistribution du deck Lumière");
				}
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
		Debug.Log("La carte " + type + " a été retirée de la défausse.");
	}
}*/