using UnityEngine;
using UnityEngine.UI;

public class GameSelection : MonoBehaviour
{
    private Button btn;
    private GameObject check;
    private bool selected = true;
    // Each button will represent a game
    public byte gameSceneIndex;

    // Invoked when the script gets enabled
    void Start()
    {
        btn = GetComponent<Button>();
        check = transform.GetChild(0).gameObject;

        btn.transition = Selectable.Transition.None;
        btn.onClick.AddListener(() => Click());

        // Will be checked or not if its game is in the game list
        if (!Common.common.gameScenesIndexes.Contains(gameSceneIndex)) {
            check.SetActive(false);
            selected = false;
        }
    }

    // Invoked every time button is clicked
    // Will check/uncheck the button and add/remove its game from the game list
    void Click()
    {
        selected = !selected;
        if (selected) {
            Common.common.gameScenesIndexes.Add(gameSceneIndex);
            check.SetActive(true);
        } else {
            Common.common.gameScenesIndexes.Remove(gameSceneIndex);
            check.SetActive(false);
        }
    }
}
