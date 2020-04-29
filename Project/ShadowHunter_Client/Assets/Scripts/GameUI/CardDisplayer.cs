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

    public CardChoiceDisplayer choicePrefab;

    private List<(int ceIndex, Player player)> choices = new List<(int, Player)>();
    private Card card;

    public void Display(Card card, Player player)
    {
        Clear();
        this.card = card;
        playerDisplayer.text = player.Name;
        cardImage.sprite = ResourceLoader.CardSprites[card.cardLabel];
        DismissButton.gameObject.SetActive(true);
        gameObject.SetActive(true);
    }


    public void ChoiceSelected(int index)
    {
        CardEffect ce = ((UsableCard)card).cardEffect[choices[index].ceIndex];
        Player p = choices[index].player;
        if (ce.targetableCondition(p))
        {
            EventView.Manager.Emit(new UsableCardUseEvent(card.Id, choices[index].ceIndex, p.Id));
        }
        gameObject.SetActive(false);
    }


    public void Dismiss()
    {
        gameObject.SetActive(false);
    }

    public void DisplayUsableCard(UsableCard card, Player player)
    {
        Clear();
        playerDisplayer.text = player.Name;
        DismissButton.gameObject.SetActive(card.canDismiss || GameManager.PlayerTurn.Value != player);

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
                        if (ce.targetableCondition(p))
                        {
                            choices.Add((i, p));
                            GameObject choice = Instantiate(choicePrefab.gameObject, ChoiceContent);
                            CardChoiceDisplayer ccd = choice.GetComponent<CardChoiceDisplayer>();
                            ccd.Display(p, ce);
                            int index = choices.Count - 1;
                            ccd.button.onClick.AddListener(delegate () { ChoiceSelected(index); });
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

    public void Clear()
    {
        for (int i = ChoiceContent.childCount - 1; i >=0; i--)
        {
            Destroy(ChoiceContent.GetChild(i).gameObject);
        }
        choices.Clear();
    }
}
