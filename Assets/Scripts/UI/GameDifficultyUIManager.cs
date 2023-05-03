using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameDifficultyUIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject previousButton;

    [SerializeField]
    private GameObject nextButton;

    [SerializeField]
    private TextMeshProUGUI valueText;

    [SerializeField]
    private TextMeshProUGUI descriptionText;

    private List<string> availableDifficulties;
    private string currentDifficulty;
    private string selectedDifficulty;

    private void Start()
    {
        InitDifficulties();

        UIUtil.InitButton(previousButton, HandlePreviousButtonClick);
        UIUtil.InitButton(nextButton, HandleNextButtonClick);
    }

    private void InitDifficulties()
    {
        availableDifficulties = new List<string>();

        availableDifficulties.Add("noob");
        availableDifficulties.Add("easy");
        availableDifficulties.Add("normal");
        availableDifficulties.Add("hard");

        currentDifficulty = PlayerPrefsController.GetDifficulty();
        selectedDifficulty = currentDifficulty;

        SetDifficultyValueText(selectedDifficulty);
    }

    private void HandlePreviousButtonClick()
    {
        selectedDifficulty = WindowUIUtil.GetNextInList<string>(
            availableDifficulties,
            selectedDifficulty,
            false
        );

        SetDifficultyValueText(selectedDifficulty);
        ApplyDifficulty(selectedDifficulty);
    }

    private void HandleNextButtonClick()
    {
        selectedDifficulty = WindowUIUtil.GetNextInList<string>(
            availableDifficulties,
            selectedDifficulty
        );

        SetDifficultyValueText(selectedDifficulty);
        ApplyDifficulty(selectedDifficulty);
    }

    private void SetDifficultyValueText(string value)
    {
        valueText.text = value;

        descriptionText.text = GenerateDescriptionText(value);
    }

    private void ApplyDifficulty(string difficulty)
    {
        PlayerPrefsController.SetDifficulty(difficulty);
    }

    private string GenerateDescriptionText(string difficulty)
    {
        if (difficulty == "noob")
        {
            return "Enemies are siginificantly slower and weaker. Enemies will spawn less frequent. This is for noobs.";
        }

        if (difficulty == "easy")
        {
            return "Enemies are slightly slower and weaker. This is for new or casual players.";
        }

        if (difficulty == "hard")
        {
            return "Enemies are slighly faster and stronger. This is for players who are looking for a challenge.";
        }

        return "Enemies are at their normal strength. This is how Knight's Quest is intended to be played.";
    }
}
