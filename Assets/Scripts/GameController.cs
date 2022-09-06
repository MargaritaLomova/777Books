using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [Header("Objects From Scene")]
    [SerializeField]
    private List<SpinHolderController> spinHolderControllers = new List<SpinHolderController>();

    [Header("Variables")]
    [SerializeField]
    private bool isGuaranteedVictory = false;

    [Space]
    [SerializeField]
    private List<Sprite> spinSprites = new List<Sprite>();
    [SerializeField]
    private int countOfSlotsInSpin = 9;

    [Header("Prefabs")]
    [SerializeField]
    private SlotController slotPrefab;

    [Header("Buttons")]
    [SerializeField]
    private Button startButton;
    [SerializeField]
    private Button privacyPolicyButton;
    [SerializeField]
    private Button exitStartButton;
    [SerializeField]
    private Button restartGameButton;
    [SerializeField]
    private Button spinButton;
    [SerializeField]
    private Button restartFinishButton;
    [SerializeField]
    private Button exitFinishButton;

    private int currentScore;

    private void Start()
    {
        startButton.onClick.AddListener(OnStartClicked);
        privacyPolicyButton.onClick.AddListener(OnPrivacyPolicyClicked);
        exitStartButton.onClick.AddListener(OnExitClicked);
        exitFinishButton.onClick.AddListener(OnExitClicked);
        restartGameButton.onClick.AddListener(OnRestartClicked);
        spinButton.onClick.AddListener(OnSpinClicked);
        restartFinishButton.onClick.AddListener(OnRestartClicked);
    }

    #region Clicked

    public void OnStartClicked()
    {
        GenerateStartSlots();

        UIController.Instance.ShowGame();

        restartGameButton.gameObject.SetActive(false);

        foreach(var spinHolder in spinHolderControllers)
        {
            spinHolder.ResetSelection();
        }

        currentScore = 1000;
        UIController.Instance.UpdateGameScore(currentScore);
    }

    public void OnPrivacyPolicyClicked()
    {
        Application.OpenURL("https://sites.google.com/view/777books/");
    }

    public void OnExitClicked()
    {
        Application.Quit();
    }

    public void OnSpinClicked()
    {
        StartCoroutine(Spin());
    }

    public void OnRestartClicked()
    {
        OnStartClicked();
    }

    #endregion

    private void GenerateSlotsToSpin(List<SpinHolderController> involvedSpinHolders)
    {
        foreach (var holder in involvedSpinHolders.Select(x => x.Content))
        {
            while(holder.transform.childCount < countOfSlotsInSpin)
            {
                var newSpinStuff = Instantiate(slotPrefab, holder);
                if (!isGuaranteedVictory)
                {
                    newSpinStuff.Set(spinSprites[Random.Range(0, spinSprites.Count)]);
                }
                else
                {
                    newSpinStuff.Set(spinSprites[0]);
                }
            }
        }
    }

    private void GenerateStartSlots()
    {
        foreach (var content in spinHolderControllers.Select(x => x.Content))
        {
            while(content.transform.childCount < 3)
            {
                var newSpinStuff = Instantiate(slotPrefab, content);
                if (!isGuaranteedVictory)
                {
                    newSpinStuff.Set(spinSprites[Random.Range(0, spinSprites.Count)]);
                }
                else
                {
                    newSpinStuff.Set(spinSprites[0]);
                }
            }
        }
    }

    private void CalculateScore(List<SpinHolderController> involvedSpinHolders)
    {
        if(CountOfCoincidences() > 0)
        {
            currentScore += 100 * CountOfCoincidences();
        }
        else
        {
            switch(involvedSpinHolders.Count())
            {
                case 1:
                    currentScore -= 200;
                    break;
                case 2:
                    currentScore -= 600;
                    break;
                case 3:
                    currentScore -= 800;
                    break;
            }

            if(currentScore < 0)
            {
                currentScore = 0;
            }
        }
    }

    private int CountOfCoincidences()
    {
        int count = 0;
        foreach(var content in spinHolderControllers.Where(x => x.IsSelected).Select(x => x.Content))
        {
            var slots = content.transform.GetComponentsInChildren<SlotController>().ToList();
            if(slots.TrueForAll(x => x.GetNameOfSprite() == slots.First().GetNameOfSprite()))
            {
                count++;
            }

        }
        return count;
    }

    private IEnumerator Spin()
    {
        if (currentScore > 0)
        {
            var involvedSpinHolders = spinHolderControllers.Where(x => x.IsSelected).ToList();

            GenerateSlotsToSpin(involvedSpinHolders);
            foreach (var spinHolderController in involvedSpinHolders)
            {
                StartCoroutine(UIController.Instance.SpinAnimation(spinHolderController, 3));
            }
            yield return new WaitUntil(() => involvedSpinHolders.TrueForAll(x => !x.IsSpin));
            CalculateScore(involvedSpinHolders);
            UIController.Instance.UpdateGameScore(currentScore);
            if (currentScore == 10000)
            {
                UIController.Instance.ShowFinish();
            }
            else if(currentScore == 0)
            {
                restartGameButton.gameObject.SetActive(true);
            }
        }
    }
}