using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DeckManager : MonoBehaviour
{
    [SerializeField] 
    private List<Card> deck = new List<Card>();
    private List<Card> discard_pile = new List<Card>();
    private List<Card> syngergized_cards = new List<Card>();

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
    private PlayerShooting p_shooting;

    private void Awake()
    {
        p_shooting = GetComponent<PlayerShooting>();
    }

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
            CardBasedBehavior(card_obj.Card);

            available_card_slots[card_obj.Index] = true;
            card_obj.HideCard();
            discard_pile.Add(card_obj.Card);
            current_hand.Remove(card_obj);
            DrawCard();
        }
    }

    private void CardBasedBehavior(Card card)
    {
        switch (card.type)
        {
            case Card_Types.hollow_point:
                p_shooting.Shoot(card);
                break;
            case Card_Types.laser_shot:
                p_shooting.Shoot(card);
                break;
            case Card_Types.energy_blast:
                p_shooting.Shoot(card);
                break;
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
            StartCoroutine(DealNewHand());
        }
    }

    public void ReloadDeck()
    {
        if(syngergized_cards.Count >= 1)
        {
            foreach (Card card in syngergized_cards)
            {
                deck.Add(card);
            }
            syngergized_cards.Clear();
            StartCoroutine(DealNewHand());
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
                    CardObject card_obj = current_hand[0];
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
        Synergy possible_synergy = null;
        if (salvo_slots[0].gameObject.activeSelf && salvo_slots[1].gameObject.activeSelf && salvo_slots[2].gameObject.activeSelf)
        {
            foreach (Synergy synergy in salvo_slots[0].Card.synergies)
            {
                possible_synergy = synergy;
                for (int i = 0; i < salvo_slots.Length; i++)
                {
                    if (salvo_slots[i].Card != possible_synergy.required_cards[i])
                    {
                        Debug.Log(salvo_slots[i].Card);
                        Debug.Log(possible_synergy.required_cards[i]);

                        possible_synergy = null;
                        break;
                    }
                }
            }

            foreach (CardObject salvo_card in salvo_slots)
            {
                if (!available_salvo_slots[salvo_card.Index])
                {
                    //check each card type and count how many of each are being used
                    //use count of each type and compare to syngery roster
                    //use appropriate syngergy

                    if (possible_synergy == null)
                    {
                        CardBasedBehavior(salvo_card.Card);
                    }

                    available_salvo_slots[salvo_card.Index] = true;
                    salvo_card.HideCard();
                    syngergized_cards.Add(salvo_card.Card);
                }
            }
        }

        if (possible_synergy != null)
        {
            Debug.Log(possible_synergy.synergy_behaviour);
            p_shooting.Shoot(possible_synergy.salvo);
        }
    }

    IEnumerator DealNewHand()
    {
        //Deal first set of cards when combat starts (currently done in start for prototype purposes)
        int i = 0;
        while (i < 3)
        {
            DrawCard();
            i++;
            yield return new WaitForSeconds(.3f);
        }
    }

    private void HighlightFirstCard()
    {
        if(current_hand.Count > 0)
        {
            if (current_hand[0].isActiveAndEnabled)
            {
                Image img = current_hand[0].gameObject.GetComponent<Image>();
                StartCoroutine(FlashCard(img));
            }
        }
    }

    IEnumerator FlashCard(Image image)
    {
        Color color = image.color;
        color.a = .7f;
        image.color = color;
        yield return new WaitForSeconds(.5f);
        color.a = 1f;
        image.color = color;
    }

    private void Start()
    {
        StartCoroutine(DealNewHand());
        InvokeRepeating("HighlightFirstCard", 1f, 1f);
    }

    private void Update()
    {
        deck_size_text.text = deck.Count.ToString();
        discard_pile_size_text.text = discard_pile.Count.ToString();
    }
}
