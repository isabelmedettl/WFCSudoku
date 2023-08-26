using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CellClickButtonHandler : MonoBehaviour
{
    public Vector2 position;
    public GameObject managerObject;
    public Manager manager;
    public Image image;
    public TextMeshProUGUI text;

    void Start()
    {
        image = GetComponent<Image>();
        manager = managerObject.GetComponent<Manager>();
        text = GetComponentInChildren<TextMeshProUGUI>();
        //SetText("hej" + 1);
    }

    public void OnClickEvent()
    {
        manager.CollapseSequence(position);
    }

    public void SetSprite(Sprite sprite)
    {
        image.sprite = sprite;
    }

    public void SetText(string textToSet)
    {
        
        GetComponentInChildren<TextMeshProUGUI>().text = textToSet;
    }
}
