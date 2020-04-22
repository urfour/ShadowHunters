using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using EventSystem;
using Scripts;
using Scripts.Settings;
using Scripts.event_in;
using Scripts.event_out;
using Assets.Noyau.Players.view;
using Assets.Noyau.Cards.controller;

namespace Scripts
{
    class KernelUI : MonoBehaviour, IListener<PlayerEvent>
    {
        public int m_player_turn;
        public int[] throw1;
        public int[] throw2;

        public void rollDices()
        {
            EventView.Manager.Emit(new NewTurnEvent()
            {
                PlayerId = m_player_turn
            });
        }

        public void attack()
        {
            EventView.Manager.Emit(new AttackEvent()
            {
                PlayerId = m_player_turn
            });
        }

        public void drawVisionCard()
        {
            EventView.Manager.Emit(new DrawCardEvent()
            {
                PlayerId = m_player_turn,
                SelectedCardType = CardType.Vision
            });
        }

        public void drawDarknessCard()
        {
            EventView.Manager.Emit(new DrawCardEvent()
            {
                PlayerId = m_player_turn,
                SelectedCardType = CardType.Darkness
            });
        }

        public void drawLightCard()
        {
            EventView.Manager.Emit(new DrawCardEvent()
            {
                PlayerId = m_player_turn,
                SelectedCardType = CardType.Light
            });
        }

        public void endOfTurn()
        {
            EventView.Manager.Emit(new EndTurnEvent()
            {
                PlayerId = m_player_turn
            });
        }

        public void revealCard()
        {
            Debug.Log("revealCard avant emit");
            EventView.Manager.Emit(new RevealCard()
            {
                PlayerId = m_player_turn
            });
            Debug.Log("revealCard apres emit");
        }

        public void usePower()
        {
            EventView.Manager.Emit(new PowerUsedEvent()
            {
                PlayerId = m_player_turn
            });
        }

        public void dontUsePower()
        {
            EventView.Manager.Emit(new TestEvent()
            {
                PlayerId = m_player_turn
            });
        }

        public void ForestHeal()
        {
            bool hurt = false;
            EventView.Manager.Emit(new ForestSelectTargetEvent()
            {
                PlayerId = m_player_turn,
                Hurt = hurt
            });
        }

        public void ForestWounds()
        {
            bool hurt = true;
            EventView.Manager.Emit(new ForestSelectTargetEvent()
            {
                PlayerId = m_player_turn,
                Hurt = hurt
            });
        }

        public void SelectDiceThrow()
        {
            int lancer = 5;
            int D6,D4;
            if(lancer == throw1[0]+throw1[1])
            {
                D6=throw1[0];
                D4=throw1[1];
            }
            else
            {
                D6=throw2[0];
                D4=throw2[1];
            }

            EventView.Manager.Emit(new SelectedDiceEvent()
            {
                PlayerId = m_player_turn,
                D6Dice = D6,
                D4Dice = D4
            });
        }

        public void OnEvent(PlayerEvent e, string[] tags = null)
        {
            if (e is SelectedNextPlayer snp)
            {
                int newPlayerId = (snp.PlayerId + 1) % 6;
                m_player_turn = newPlayerId;

                EventView.Manager.Emit(new NewTurnEvent()
                {
                    PlayerId = newPlayerId
                });
            }
            else if (e is SelectAttackTargetEvent sate)
            {
                int attacked;
                if (sate.TargetID == -1)
                {
                    attacked = sate.PossibleTargetId[UnityEngine.Random.Range(0, sate.PossibleTargetId.Length)];
                }
                else
                {
                    attacked = sate.TargetID;
                }

                EventView.Manager.Emit(new AttackPlayerEvent()
                {
                    PlayerId = sate.PlayerId,
                    PlayerAttackedId = attacked,
                    PowerFranklin = sate.PowerFranklin,
                    PowerGeorges = sate.PowerGeorges,
                    PowerLoup = sate.PowerLoup
                });
            }
            else if (e is SelectBobPowerEvent sbce)
            {
                bool up = false;
                if (UnityEngine.Random.Range(0, 1) == 0)
                    up = true;

                EventView.Manager.Emit(new BobPowerEvent()
                {
                    PlayerId = sbce.PlayerId,
                    UsePower = up
                });
            }
            else if (e is SelectGiveCardEvent sgce)
            {
                Player p = PlayerView.GetPlayer(sgce.PlayerId);

                bool isEquip = false;
                Card card = p.ListCard[UnityEngine.Random.Range(0, p.ListCard.Count)];
                if (card.isEquipement)
                    isEquip = true;

                while (!isEquip)
                {
                    card = p.ListCard[UnityEngine.Random.Range(0, p.ListCard.Count)];
                    if (card.isEquipement)
                        isEquip = true;
                }

                EventView.Manager.Emit(new GiveCardEvent()
                {
                    PlayerId = sgce.PlayerId,
                    PlayerGivedId = sgce.PossibleTargetId[UnityEngine.Random.Range(0, sgce.PossibleTargetId.Length)],
                    CardGivedName = card.cardLabel
                });
            }
            else if (e is SelectGiveOrWoundEvent sgowe)
            {
                bool up = false;
                if (UnityEngine.Random.Range(0, 1) == 0)
                    up = true;

                EventView.Manager.Emit(new GiveOrWoundEvent()
                {
                    PlayerId = sgowe.PlayerId,
                    Give = up
                });
            }
            else if (e is SelectLightCardTargetEvent slcdte)
            {
                EventView.Manager.Emit(new LightCardEffectEvent()
                {
                    PlayerId = slcdte.PlayerId,
                    PlayerChoosenId = slcdte.PossibleTargetId[UnityEngine.Random.Range(0, slcdte.PossibleTargetId.Length)],
                    LightCard = slcdte.LightCard
                });
            }
            else if (e is SelectDiceThrow sdt)
            {
                //choisit quel lancé de dés il veut
                //choiceDropdown.gameObject.SetActive(true);
                throw1[0]=sdt.D6Dice1;
                throw1[1]=sdt.D4Dice1;

                throw2[0]=sdt.D6Dice2;
                throw2[1]=sdt.D4Dice2;
                List<string> throws=new List<string>();
                throws.Add(throw1[0]+throw1[1].ToString());
                throws.Add(throw2[0]+throw2[1].ToString());
                //choiceDropdown.AddOptions(throws);
                //validateButton.gameObject.SetActive(true);

            }
            else if (e is SelectMovement sm)
            {
                EventView.Manager.Emit(new MoveOn()
                {
                    PlayerId = sm.PlayerId,
                    Location = sm.LocationAvailable[UnityEngine.Random.Range(0, sm.LocationAvailable.Length)]
                });
            }
            else if (e is SelectPlayerTakingWoundsEvent sptwe)
            {
                EventView.Manager.Emit(new TakingWoundsEffectEvent()
                {
                    PlayerId = sptwe.PlayerId,
                    PlayerAttackedId = sptwe.PossibleTargetId[UnityEngine.Random.Range(0, sptwe.PossibleTargetId.Length)],
                    IsPuppet = sptwe.IsPuppet,
                    NbWoundsTaken = sptwe.NbWoundsTaken,
                    NbWoundsSelfHealed = sptwe.NbWoundsSelfHealed
                });
            }
            else if (e is SelectRevealOrNotEvent srone)
            {
                bool up = false;
                if (UnityEngine.Random.Range(0, 1) == 0)
                    up = true;

                EventView.Manager.Emit(new RevealOrNotEvent()
                {
                    PlayerId = srone.PlayerId,
                    EffectCard = srone.EffectCard,
                    HasRevealed = up
                });
            }
            else if (e is SelectStealCardEvent ssce)
            {
                Player p = PlayerView.GetPlayer(ssce.PossiblePlayerTargetId[UnityEngine.Random.Range(0, ssce.PossiblePlayerTargetId.Length)]);

                EventView.Manager.Emit(new StealCardEvent()
                {
                    PlayerId = ssce.PlayerId,
                    PlayerStealedId = p.Id,
                    CardStealedName = p.ListCard[UnityEngine.Random.Range(0, p.ListCard.Count)].cardName
                });
            }
            else if (e is SelectStealCardFromPlayerEvent sscfpe)
            {
                Player p = PlayerView.GetPlayer(sscfpe.PlayerStealedId);

                bool isEquip = false;
                Card card = p.ListCard[UnityEngine.Random.Range(0, p.ListCard.Count-1)];
                if (card.isEquipement)
                    isEquip = true;

                while (!isEquip)
                {
                    card = p.ListCard[UnityEngine.Random.Range(0, p.ListCard.Count-1)];
                    if(card.isEquipement)
                        isEquip = true;
                }

                EventView.Manager.Emit(new StealCardEvent()
                {
                    PlayerId = sscfpe.PlayerId,
                    PlayerStealedId = sscfpe.PlayerStealedId,
                    CardStealedName = card.cardLabel
                });
            }
            else if (e is SelectVisionPowerEvent svpe)
            {

                bool up = false;
                int pc = svpe.PossiblePlayerTargetId[UnityEngine.Random.Range(0, svpe.PossiblePlayerTargetId.Length)];
                
                if (PlayerView.GetPlayer(pc).Character.characterName == "Metamorphe")
                {
                    if (UnityEngine.Random.Range(0, 1) == 0)
                        up = true;
                }
                

                EventView.Manager.Emit(new VisionCardEffectEvent()
                {
                    PlayerId = svpe.PlayerId,
                    TargetId = pc,
                    VisionCard = svpe.VisionCard,
                    MetamorphePower = up,
                });
            }
        }
    }
}
