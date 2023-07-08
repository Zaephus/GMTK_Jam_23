
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EntityCard : MonoBehaviour {

    public static System.Action<int> CardClicked;

    [HideInInspector]
    public int index;

    [SerializeField]
    private Image thumbnailImage;
    
    public void SetThumbnail(Sprite _sprite) {
        thumbnailImage.sprite = _sprite;
    }

    public void ClickCard() {
        CardClicked?.Invoke(index);
    }
    
}