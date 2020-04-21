using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Noyau.Players.controller
{
    class GPlayer
    {
        public Player[] Players { get; private set; }

        public GPlayer(int nbPlayers)
        {

            GCharacter characters = new GCharacter(nbPlayers);

            Players = new Player[nbPlayers];
            for (int i = 0; i < nbPlayers; i++)
            {
                Players[i] = new Player(i, characters.PickCharacter());
            }
        }

        /// <summary>
        /// Fonction permettant de préparer un deck de cartes en fonction du nombre de joueurs
        /// </summary>
        /// <param name="deck">Deck final construit</param>
        /// <param name="deckCharacter">Deck comportant les cartes personnages du même type</param>
        /// <param name="nb">Nombre de cartes à ajouter dans la pile finale</param>
        /// <param name="addBob">Booléen représentant la nécessité d'ajouter Bob ou non</param>
        void AddCharacterCards(List<Character> deck, List<Character> deckCharacter, int nb, bool addBob)
        {
            for (int i = 0; i < nb; i++)
            {
                if (nb >= 7 && !addBob)
                    i--;
                else
                    deck.Add(deckCharacter[i]);
            }
        }
    }
}
