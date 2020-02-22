using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoard
{
	// Decks des différents types de cartes
	private List<Card> m_visionDeck;
	public List<Card> VisionDeck
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
	private List<Card> m_hermit;
	public List<Card> Hermit
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
	public GameBoard(List<Card> l_areas, List<Card> l_hermit, List<Card> l_black, List<Card> l_white, int nbPlayers)
	{

		m_visionDeck = l_hermit;
		m_darknessDeck = l_black;
		m_lightDeck = l_white;

		m_hermit = new List<Card>();
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
}
