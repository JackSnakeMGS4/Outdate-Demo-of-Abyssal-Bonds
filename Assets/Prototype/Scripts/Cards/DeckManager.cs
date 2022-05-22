using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DeckManager : MonoBehaviour
{
    [SerializeField] 
    private List<CardObject> deck = new List<CardObject>();
    [SerializeField] 
    private List<CardObject> discard_pile = new List<CardObject>();
    [SerializeField]
    private Transform[] card_slots;
    [SerializeField]
    private bool[] available_card_slots;

    [SerializeField]
    private TextMeshProUGUI deck_size_text;
    [SerializeField]
    private TextMeshProUGUI discard_pile_size_text;

    public void DrawCard()
    {
        if(deck.Count >= 1) 
        {
            CardObject random_card = deck[Random.Range(0, deck.Count)];

            for (int i = 0; i < available_card_slots.Length; i++)
            {
                if (available_card_slots[i])
                {
                    random_card.gameObject.SetActive(true);
                    random_card.transform.position = card_slots[i].position;
                    available_card_slots[i] = false;
                    deck.Remove(random_card);
                    return;
                }
            }
        }
    }

    private void Update()
    {
        deck_size_text.text = deck.Count.ToString();
    }
}
