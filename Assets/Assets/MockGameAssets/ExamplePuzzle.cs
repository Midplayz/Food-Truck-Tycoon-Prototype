using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExamplePuzzle : MonoBehaviour
{
    public Button[] buttons;
    public int totalMoves = 5;
    public float displayInterval = 1f;
    public float errorDisplayTime = 1f;
    public float successDisplayTime = 1f;
    public GameObject puzzleUI;
    public TextMeshProUGUI instructionText;

    private List<int> pattern = new List<int>();
    private int currentMoveIndex = 0;
    private bool isDisplayingPattern = false;

    public ButtonInteraction ButtonInteraction;

    public void StartPuzzle()
    {
        instructionText.text = "Observe Pattern";
        puzzleUI.SetActive(true);
        InitializeButtons();
        GeneratePattern();
        StartCoroutine(DisplayPattern());
        Cursor.lockState = CursorLockMode.None;
    }

    void InitializeButtons()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            int index = i;
            buttons[i].onClick.AddListener(() => OnButtonClick(index));
        }
        MovementValues.Instance.ToggleMovementCompletely(false);
    }

    void GeneratePattern()
    {
        pattern.Clear();
        int previousIndex = -1;
        for (int i = 0; i < totalMoves; i++)
        {
            int newIndex;
            do
            {
                newIndex = Random.Range(0, buttons.Length);
            } while (newIndex == previousIndex);
            pattern.Add(newIndex);
            previousIndex = newIndex;
        }
    }

    IEnumerator DisplayPattern()
    {
        yield return new WaitForSeconds(1f);

        instructionText.text = "Observe Pattern";
        isDisplayingPattern = true;
        foreach (Button button in buttons)
        {
            button.interactable = false;
        }

        foreach (int index in pattern)
        {
            buttons[index].image.color = Color.yellow;
            yield return new WaitForSeconds(displayInterval);
            buttons[index].image.color = Color.white;
        }

        isDisplayingPattern = false;
        instructionText.text = "Enter Pattern!";
        foreach (Button button in buttons)
        {
            button.interactable = true;
        }
    }

    void OnButtonClick(int index)
    {
        if (isDisplayingPattern) return;

        if (currentMoveIndex > 0)
        {
            int previousIndex = pattern[currentMoveIndex - 1];
            buttons[previousIndex].image.color = Color.white;
        }

        if (index == pattern[currentMoveIndex])
        {
            buttons[index].image.color = Color.green;
            currentMoveIndex++;
            if (currentMoveIndex >= pattern.Count)
            {
                StartCoroutine(DisplaySuccess());
            }
        }
        else
        {
            StartCoroutine(DisplayError());
        }
    }


    IEnumerator DisplayError()
    {
        instructionText.text = "Incorrect Pattern!";
        foreach (Button button in buttons)
        {
            button.interactable = false;
        }

        foreach (Button button in buttons)
        {
            button.image.color = Color.red;
        }

        yield return new WaitForSeconds(errorDisplayTime);

        foreach (Button button in buttons)
        {
            button.image.color = Color.white;
        }

        currentMoveIndex = 0;
        StartCoroutine(DisplayPattern());
    }

    IEnumerator DisplaySuccess()
    {
        foreach (Button button in buttons)
        {
            button.interactable = false;
        }

        instructionText.text = "Unlocked!";
        foreach (Button button in buttons)
        {
            button.image.color = Color.green;
        }

        yield return new WaitForSeconds(successDisplayTime);

        OnPuzzleSolved();
    }

    void OnPuzzleSolved()
    {
        Debug.Log("Puzzle Solved!");
        puzzleUI.SetActive(false);
        MovementValues.Instance.ToggleMovementCompletely(true);
        Cursor.lockState = CursorLockMode.Locked;
        ButtonInteraction.OpenAndCloseDoor();
    }
}
