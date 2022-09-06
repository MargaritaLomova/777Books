using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [Header("Blocks")]
    [SerializeField]
    private CanvasGroup startBlock;
    [SerializeField]
    private CanvasGroup gameBlock;
    [SerializeField]
    private CanvasGroup finishBlock;

    [Header("Texts")]
    [SerializeField]
    private TMP_Text gameScore;

    [Header("Components")]
    [SerializeField]
    private GameObject gameScoreHolder;

    public static UIController Instance;

    private void Start()
    {
        Screen.orientation = ScreenOrientation.Landscape;

        ShowStart();

        if (Instance == null)
        {
            Instance = this;
            return;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void ShowStart()
    {
        startBlock.alpha = 1;
        startBlock.blocksRaycasts = true;

        gameBlock.alpha = 0;
        gameBlock.blocksRaycasts = false;
        finishBlock.alpha = 0;
        finishBlock.blocksRaycasts = false;
    }

    public void ShowGame()
    {
        gameBlock.alpha = 1;
        gameBlock.blocksRaycasts = true;

        startBlock.alpha = 0;
        startBlock.blocksRaycasts = false;
        finishBlock.alpha = 0;
        finishBlock.blocksRaycasts = false;
    }

    public void ShowFinish()
    {
        finishBlock.alpha = 1;
        finishBlock.blocksRaycasts = true;

        gameBlock.alpha = 0;
        gameBlock.blocksRaycasts = false;
        startBlock.alpha = 0;
        startBlock.blocksRaycasts = false;
    }

    public void UpdateGameScore(int currentScore)
    {
        gameScore.text = $"{currentScore}";
        gameScoreHolder.SetActive(currentScore > 0);
    }

    public IEnumerator SpinAnimation(SpinHolderController controller, int countOfFinalSlots)
    {
        controller.SpinChanged(true);

        var scrollRect = controller.ScrollRect;
        var content = controller.Content;

        scrollRect.enabled = true;

        scrollRect.horizontalNormalizedPosition = 0;

        while (scrollRect.horizontalNormalizedPosition < 1)
        {
            scrollRect.horizontalNormalizedPosition += Time.deltaTime;
            yield return null;
        }

        var slots = content.GetComponentsInChildren<SlotController>().ToList();

        for (int i = 0; i < slots.Count() - countOfFinalSlots; i++)
        {
            Destroy(slots[i].gameObject);
        }

        content.localPosition = new Vector2(0, 0);

        scrollRect.enabled = false;

        controller.SpinChanged(false);
    }
}