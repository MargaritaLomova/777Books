using UnityEngine;
using UnityEngine.UI;

public class SpinHolderController : MonoBehaviour
{
    [Header("Components"), SerializeField]
    private GameObject line;
    [SerializeField]
    private RectTransform content;
    [SerializeField]
    private ScrollRect scrollRect;
    [SerializeField]
    private Button selectButton;

    public bool IsSelected => line.activeInHierarchy;
    public RectTransform Content => content;
    public ScrollRect ScrollRect => scrollRect;
    public bool IsSpin { get; private set; }

    private void Start()
    {
        selectButton.onClick.AddListener(() => line.SetActive(!line.activeInHierarchy));
    }

    public void SpinChanged(bool value)
    {
        IsSpin = value;
    }

    public void ResetSelection()
    {
        line.SetActive(false);
    }
}