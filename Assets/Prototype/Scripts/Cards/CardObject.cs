using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardObject : MonoBehaviour
{
    private Card card;
    public Card Card { set { card = value; } get { return card; } }

    private Image image;
    private RectTransform rectTransform;

    private int hand_index;
    public int Index { set { hand_index = value; } get { return hand_index; } }

    public void DisplayCard()
    {
        rectTransform = gameObject.GetComponent<RectTransform>();
        //rectTransform.anchoredPosition = Vector3.zero;
        image = gameObject.GetComponent<Image>();
        image.sprite = card.card_art;
        gameObject.SetActive(true);
    }

    public void HideCard()
    {
        gameObject.SetActive(false);
    }
}
