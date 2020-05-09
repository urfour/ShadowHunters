using Assets.Noyau.Manager.view;
using Assets.Noyau.Players.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Noyau.Players.controller
{
    /// <summary>
    /// Classe qui va instancier tout les personnages du jeu.
    /// </summary>
    class GCharacter
    {
        public List<Character> characters = new List<Character>();

        /// <summary>
        /// Fonction qui va préparer les decks des différentes équipes
        /// en fonction du nombre de joueurs
        /// </summary>
        /// <param name="nbPlayers">Le nombre de joueurs de la partie</param>
        public GCharacter(int nbPlayers, bool withExtension)
        {
            List<Character> Neutral = new List<Character>() {
                new Character("character.name.allie", CharacterTeam.Neutral, 8, GGoal.AllieGoal, GPower.Allie),
                new Character("character.name.bob", CharacterTeam.Neutral, 10, GGoal.BobGoal, GPower.Bob),
                new Character("character.name.charles", CharacterTeam.Neutral, 11, GGoal.CharlesGoal, GPower.Charles),
                new Character("character.name.daniel", CharacterTeam.Neutral, 13, GGoal.DanielGoal, GPower.Daniel)
            };

            List<Character> Hunter = new List<Character>()
            {
                new Character("character.name.emi", CharacterTeam.Hunter, 10, GGoal.HunterGoal, null),
                new Character("character.name.georges", CharacterTeam.Hunter, 14, GGoal.HunterGoal, GPower.George),
                new Character("character.name.franklin", CharacterTeam.Hunter, 12, GGoal.HunterGoal, GPower.Franklin)
            };
            List<Character> Shadow = new List<Character>()
            {
                new Character("character.name.loup_garou", CharacterTeam.Shadow, 14, GGoal.ShadowGoal, GPower.Loup),
                new Character("character.name.vampire", CharacterTeam.Shadow, 13, GGoal.ShadowGoal, GPower.Vampire),
                new Character("character.name.metamorphe", CharacterTeam.Shadow, 11, GGoal.ShadowGoal, null)
            };

            List<Character> NeutralExtension = new List<Character>()
            {
                new Character("character.name.bryan", CharacterTeam.Neutral, 10, GGoal.BryanGoal, GPower.Bryan),
                new Character("character.name.catherine", CharacterTeam.Neutral, 11, GGoal.CatherineGoal, GPower.Catherine),
                new Character("character.name.agnes", CharacterTeam.Neutral, 8, GGoal.AgnesGoal, GPower.Agnes),
                new Character("character.name.bob_extension", CharacterTeam.Neutral, 10, GGoal.BobGoal, GPower.BobExtension),
                new Character("character.name.david", CharacterTeam.Neutral, 13, GGoal.DavidGoal, GPower.David)
            };

            List<Character> HunterExtension = new List<Character>()
            {
                new Character("character.name.ellen", CharacterTeam.Hunter, 10, GGoal.HunterGoal, GPower.Ellen),
                new Character("character.name.fuka", CharacterTeam.Hunter, 12, GGoal.HunterGoal, GPower.Fuka),
                new Character("character.name.gregor", CharacterTeam.Hunter, 14, GGoal.HunterGoal, null)
            };

            List<Character> ShadowExtension = new List<Character>()
            {
                new Character("character.name.liche", CharacterTeam.Shadow, 14, GGoal.ShadowGoal, GPower.Liche),
                new Character("character.name.momie", CharacterTeam.Shadow, 11, GGoal.ShadowGoal, GPower.Momie),
                new Character("character.name.valkyrie", CharacterTeam.Shadow, 13, GGoal.ShadowGoal, null)
            };

            if (nbPlayers >= 7)
            {
                Neutral.Remove(Neutral.Find((item) => { return item.characterName.Equals("character.name.bob"); }));
            }

            int nbshadow = 0;
            int nbhunter = 0;
            int nbneutral = 0;
            
            switch (nbPlayers)
            {
                case 4:
                    {
                        nbhunter = 2;
                        nbshadow = 2;
                        nbneutral = 0;
                    }
                    break;
                case 5:
                    {
                        nbhunter = 2;
                        nbshadow = 2;
                        nbneutral = 1;
                    }
                    break;
                case 6:
                    {
                        nbhunter = 2;
                        nbshadow = 2;
                        nbneutral = 2;
                    }
                    break;
                case 7:
                    {
                        nbhunter = 2;
                        nbshadow = 2;
                        nbneutral = 3;
                    }
                    break;
                case 8:
                    {
                        nbhunter = 3;
                        nbshadow = 3;
                        nbneutral = 2;
                    }
                    break;
                default:
                    throw new ArgumentException("Invalid number of players : " + nbPlayers);
            }
            if (withExtension)
            {
                Shadow.AddRange(ShadowExtension);
                Hunter.AddRange(HunterExtension);
                Neutral.AddRange(NeutralExtension);
                if (nbPlayers < 7)
                {
                    Neutral.Remove(Neutral.Find((item) => { return item.characterName.Equals("character.name.bob_extension"); }));
                }
            }
            for (int i = 0; i < nbhunter; i++)
            {
                int r = GameManager.rand.Next(0, Hunter.Count);
                characters.Add(Hunter[r]);
                Hunter.RemoveAt(r);
            }
            for (int i = 0; i < nbshadow; i++)
            {
                int r = GameManager.rand.Next(0, Shadow.Count);
                characters.Add(Shadow[r]);
                Shadow.RemoveAt(r);
            }
            for (int i = 0; i < nbneutral; i++)
            {
                int r = GameManager.rand.Next(0, Neutral.Count);
                characters.Add(Neutral[r]);
                Neutral.RemoveAt(r);
            }
        }

        /// <summary>
        /// Fonction qui attribue un personnage à un joueur.
        /// </summary>
        public Character PickCharacter()
        {
            int r = GameManager.rand.Next(0, characters.Count);
            Character c = characters[r];
            characters.RemoveAt(r);
            return c;
        }
    }
}
