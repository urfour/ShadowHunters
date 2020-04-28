using UnityEngine;
using System.Collections;
using Assets.Noyau.Cards.controller;
using UnityEngine.UI;
using Assets.Scripts.GameUI;
using Assets.Noyau.Cards.model;
using System.Collections.Generic;
using Assets.Noyau.Players.view;
using Assets.Noyau.Manager.view;

public class CardDisplayer : MonoBehaviour
{
    public Image cardImage;
    public RectTransform ChoiceContent;
    public Button DismissButton;

    private List<(CardEffect, Player)> choices = new List<(CardEffect, Player)>();
    private Card card;

    public void Display(Card card)
    {
        choices.Clear();
        this.card = card;
        cardImage.sprite = ResourceLoader.CardSprites[card.cardLabel];
        DismissButton.gameObject.SetActive(true);
    }


    public void DisplayUsableCard(UsableCard card)
    {
        choices.Clear();
        if (card.canDismiss || GameManager.PlayerTurn.Value != GameManager.LocalPlayer.Value)
        if (card.cardType == CardType.Vision)
        {
            cardImage.sprite = ResourceLoader.CardSprites["card.vision"];
        }
        else
        {
            this.card = card;
            cardImage.sprite = ResourceLoader.CardSprites[card.cardLabel];
            for (int i = 0; i < card.cardEffect.Length; i++)
            {
                CardEffect ce = card.cardEffect[i];
                foreach (Player p in PlayerView.GetPlayers())
                {

                }
            }
        }
    }
}
