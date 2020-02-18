using System.Collections;
using System.Collections.Generic;

public class GameBoard {
	
	// Défausses des différents types de cartes
	/* Précision : ici public List<Card> Hermit {get;} ne fonctionnera
	 * pas, en effet cette méthode get pointerai sur Hermit et non pas 
	 * sur m_hermit.
	*/
	private List<Card> m_hermit;
	public List<Card> Hermit {
		get {
			return m_hermit;
		}
	}
	
	private List<Card> m_black;
	public List<Card> Black {
		get {
			return m_black;
		}
	}
	
	private List<Card> m_white;
	public List<Card> White {
		get {
			return m_white;
		}
	}

	
	// Lieux du plateau
	private Card[] m_areas;
	
	public Card getAreaAt (int index) {
		return m_areas[index];
	}
	
	// Position de chaque joueur (index = nbPlayers)
	private int[] m_position;
	
	public int getPositionOf (int index) {
		return m_position[index];
	}
	
	public void setPositionOfAt (int player, int position) {
		m_position[player] = position;
	}
	
	// Dégâts de chaque joueur (index = nbPlayers)
	private int[] m_damage;
	
	public int getDamageOf (int index) {
		return m_damage[index];
	}
	
	public void increaseDamageOfBy (int player, int damage) {
		m_damage[player] += damage;
	}
	
	//Constructeur (l_areas = liste mélangée des 6 lieux utilisés)
	public GameBoard(List<Card> l_areas, int nbPlayer) {
		
		m_hermit = new List<Card>();
		m_black = new List<Card>();
		m_white = new List<Card>();
		
		m_areas = l_areas.ToArray();
		
		m_position = new int[nbPlayer];
		m_damage = new int[nbPlayer];
	}	
}
