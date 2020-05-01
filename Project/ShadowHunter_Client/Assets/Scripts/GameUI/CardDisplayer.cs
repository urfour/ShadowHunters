using UnityEngine;
using System.Collections;
using Assets.Noyau.Cards.controller;
using UnityEngine.UI;
using Assets.Scripts.GameUI;
using Assets.Noyau.Cards.model;
using System.Collections.Generic;
using Assets.Noyau.Players.view;
using Assets.Noyau.Manager.view;
using EventSystem;
using Scripts.event_in;

public class CardDisplayer : MonoBehaviour
{
    public Text playerDisplayer;
    public Image cardImage;
    public RectTransform ChoiceContent;
    public Button DismissButton;

    public CharacterEquipmentDisplayer characterEquipmentDisplayerPrefab;
    public CardChoiceDisplayer choicePrefab;

    private List<(int ceIndex, Player player)> choices = new List<(int, Player)>();
    private Card card;
    private Player player;

    public void Display(Card card, Player player)
    {
        Clear();
        this.player = player;
        this.card = card;
        playerDisplayer.text = player.Name;
        if (ResourceLoader.CardSprites.ContainsKey(card.cardLabel))
        {
            cardImage.sprite = ResourceLoader.CardSprites[card.cardLabel];
        }
        else
        {
            cardImage.sprite = null;
            Debug.LogWarning("Unknown card label : " + card.cardLabel);
        }
        DismissButton.gameObject.SetActive(true);
        gameObject.SetActive(true);
    }


    public void ChoiceSelected(int index)
    {
        CardEffect ce = ((UsableCard)card).cardEffect[choices[index].ceIndex];
        Player p = choices[index].player;
        if (ce.targetableCondition == null || ce.targetableCondition(p, player) && GameManager.LocalPlayer.Value == player)
        {
            EventView.Manager.Emit(new UsableCardUseEvent(card.Id, choices[index].ceIndex, p.Id, player.Id));
            gameObject.SetActive(false);
        }
    }


    public void Dismiss()
    {
        gameObject.SetActive(false);
    }

    public void DisplayUsableCard(UsableCard card, Player player)
    {
        Clear();
        this.player = player;
        playerDisplayer.text = player.Name;
        DismissButton.gameObject.SetActive(card.canDismiss || GameManager.LocalPlayer.Value != player);

        if (card.cardType == CardType.Vision && GameManager.LocalPlayer.Value != player)
        {
            cardImage.sprite = ResourceLoader.CardSprites["card.vision"];
        }
        else
        {
            this.card = card;
            if (ResourceLoader.CardSprites.ContainsKey(card.cardLabel))
            {
                cardImage.sprite = ResourceLoader.CardSprites[card.cardLabel];
            }
            else
            {
                cardImage.sprite = null;
                Debug.LogWarning("Unknown card label : " + card.cardLabel);
            }
            if (!card.hiddenChoices || GameManager.LocalPlayer.Value == player)
            {
                for (int i = 0; i < card.cardEffect.Length; i++)
                {
                    CardEffect ce = card.cardEffect[i];
                    if (ce.targetableCondition == null)
                    {
                        choices.Add((i, player));
                        GameObject choice = Instantiate(choicePrefab.gameObject, ChoiceContent);
                        CardChoiceDisplayer ccd = choice.GetComponent<CardChoiceDisplayer>();
                        ccd.Display(player, ce);
                        int index = choices.Count - 1;
                        ccd.button.onClick.AddListener(delegate () { ChoiceSelected(index); });
                        ccd.button.interactable = GameManager.LocalPlayer.Value == player;
                    }
                    else
                    {
                        foreach (Player p in PlayerView.GetPlayers())
                        {
                            if (ce.targetableCondition(p, player))
                            {
                                choices.Add((i, p));
                                GameObject choice = Instantiate(choicePrefab.gameObject, ChoiceContent);
                                CardChoiceDisplayer ccd = choice.GetComponent<CardChoiceDisplayer>();
                                ccd.Display(p, ce);
                                int index = choices.Count - 1;
                                ccd.button.onClick.AddListener(delegate () { ChoiceSelected(index); });
                                ccd.button.interactable = GameManager.LocalPlayer.Value == player;
                            }
                        }
                    }
                }
            }
        }

        float choicesheigh = 0;
        for (int i = 0; i < ChoiceContent.childCount; i++)
        {
            RectTransform c = ChoiceContent.GetChild(i) as RectTransform;
            choicesheigh += c.sizeDelta.y;
        }
        choicesheigh += (ChoiceContent.childCount - 1) * ChoiceContent.GetComponent<VerticalLayoutGroup>().spacing;
        ChoiceContent.sizeDelta = new Vector2(ChoiceContent.sizeDelta.x, choicesheigh);
        gameObject.SetActive(true);
    }

    public void DisplayPlayer(Player player)
    {
        Clear();
        this.player = player;
        playerDisplayer.text = player.Name;
        DismissButton.gameObject.SetActive(true);

        if (player.Revealed.Value || player == GameManager.LocalPlayer.Value)
        {
            if (ResourceLoader.CardSprites.ContainsKey(player.Character.characterName))
            {
                cardImage.sprite = ResourceLoader.CardSprites[player.Character.characterName];
            }
            else
            {
                cardImage.sprite = null;
                Debug.LogWarning("Unknown card label : " + player.Character.characterName);
            }

            foreach (Card c in player.ListCard)
            {
                GameObject equipment = Instantiate(characterEquipmentDisplayerPrefab.gameObject, ChoiceContent);
                CharacterEquipmentDisplayer ccd = equipment.GetComponent<CharacterEquipmentDisplayer>();
                ccd.Display((EquipmentCard)c);
                ccd.button.onClick.AddListener(delegate () { this.Display(c, player); });
                ccd.button.interactable = true;
            }
        }
    }

    public void Clear()
    {
        for (int i = ChoiceContent.childCount - 1; i >=0; i--)
        {
            Destroy(ChoiceContent.GetChild(i).gameObject);
        }
        choices.Clear();
    }
}
