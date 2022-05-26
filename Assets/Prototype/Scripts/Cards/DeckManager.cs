using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DeckManager : MonoBehaviour
{
    [SerializeField] 
    private List<Card> deck = new List<Card>();
    [SerializeField] 
    private List<Card> discard_pile = new List<Card>();

    [SerializeField]
    private CardObject[] card_slots;
    [SerializeField]
    private bool[] available_card_slots;
    
    [SerializeField]
    private CardObject[] salvo_slots;
    [SerializeField]
    private bool[] available_salvo_slots;

    [SerializeField]
    private TextMeshProUGUI deck_size_text;
    [SerializeField]
    private TextMeshProUGUI discard_pile_size_text;

    private List<CardObject> current_hand = new List<CardObject>();

    //TODO: refactor code so it only used the card scriptable objects
    public void DrawCard()
    {
        if(deck.Count >= 1) 
        {
            Card random_card = deck[Random.Range(0, deck.Count)];

            for (int i = 0; i < available_card_slots.Length; i++)
            {
                if (available_card_slots[i])
                {
                    CardObject card_obj = card_slots[i];
                    card_obj.Card = random_card;
                    card_obj.DisplayCard();
                    card_obj.Index = i;

                    available_card_slots[i] = false;

                    deck.Remove(random_card);
                    current_hand.Add(card_obj);
                    return;
                }
            }
        }
    }

    public void UseCard()
    {
        if(current_hand.Count >= 1)
        {
            CardObject card_obj = current_hand[0];
            available_card_slots[card_obj.Index] = true;
            card_obj.HideCard();
            discard_pile.Add(card_obj.Card);
            current_hand.Remove(card_obj);
            DrawCard();
        }
    }
    
    public void ShuffleDeck()
    {
        if(discard_pile.Count >= 1)
        {
            if(current_hand.Count >= 1)
            {
                foreach (CardObject card_obj in current_hand)
                {
                    if (!available_card_slots[card_obj.Index])
                    {
                        available_card_slots[card_obj.Index] = true;
                        card_obj.HideCard();
                        discard_pile.Add(card_obj.Card);
                    }
                }
                current_hand.Clear();
            }

            foreach (Card card in discard_pile)
            {
                deck.Add(card);
            }
            discard_pile.Clear();
            DealNewHand();
        }
    }

    public void AddToSalvo()
    {
        if(current_hand.Count >= 1)
        {
            for (int i = 0; i < available_salvo_slots.Length; i++)
            {
                if (available_salvo_slots[i])
                {
                    CardObject card_obj = current_hand[i];
                    available_card_slots[card_obj.Index] = true;
                    card_obj.HideCard();

                    salvo_slots[i].Card = card_obj.Card;
                    salvo_slots[i].DisplayCard();
                    salvo_slots[i].Index = i;
                    available_salvo_slots[i] = false;

                    current_hand.Remove(card_obj);
                    DrawCard();
                    return;
                }
            }
        }
    }

    public void UseSalvo()
    {
        foreach (CardObject card_obj in salvo_slots)
        {
            if (!available_salvo_slots[card_obj.Index])
            {
                available_salvo_slots[card_obj.Index] = true;
                card_obj.HideCard();
                discard_pile.Add(card_obj.Card);
            }
        }
    }

    private void DealNewHand()
    {
        //Deal first set of cards when combat starts (currently done in start for prototype purposes)
        int i = 0;
        while (i < 3)
        {
            DrawCard();
            i++;
        }
    }

    private void Start()
    {
        DealNewHand();
    }

    private void Update()
    {
        deck_size_text.text = deck.Count.ToString();
        discard_pile_size_text.text = discard_pile.Count.ToString();
    }
}
