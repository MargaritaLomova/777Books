using UnityEngine;
using UnityEngine.UI;

public class SlotController : MonoBehaviour
{
    [Header("Components"), SerializeField]
    private Image image;

    public void Set(Sprite sprite)
    {
        image.sprite = sprite;
        image.preserveAspect = true;
    }

    public string GetNameOfSprite()
    {
        return image.sprite.name;
    }
}