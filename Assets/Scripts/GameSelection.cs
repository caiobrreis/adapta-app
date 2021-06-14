using UnityEngine;
using UnityEngine.UI;

public class GameSelection : MonoBehaviour
{
    private Button btn;
    private GameObject check;
    private bool selected = true;
    public byte gameSceneIndex;

    void Start()
    {
        btn = GetComponent<Button>();
        check = transform.GetChild(0).gameObject;

        btn.transition = Selectable.Transition.None;
        btn.onClick.AddListener(() => Click());

        if (!Common.common.gameScenesIndexes.Contains(gameSceneIndex)) {
            check.SetActive(false);
            selected = false;
        }
    }

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
