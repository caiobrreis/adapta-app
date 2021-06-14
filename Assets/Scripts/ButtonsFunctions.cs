using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class ButtonsFunctions : MonoBehaviour
{
    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void SetPrevFrame(GameObject frame)
    {
        Common.common.prevFrames.Add(frame);
    }

    public void GotoPrevFrame()
    {
        var pf = Common.common.prevFrames;
        pf[pf.Count - 1].SetActive(true);
        pf.RemoveAt(pf.Count - 1);
    }

    public void StartGame()
    {
        Common cmm = Common.common;

        cmm.playedJgCores = false;
        cmm.correctJgCores = 0;
        cmm.incorrectJgCores = 0;
        cmm.timeJgCores = 0;

        cmm.playedJgMat = false;
        cmm.correctJgMat = 0;
        cmm.incorrectJgMat = 0;
        cmm.timeJgMat = 0;
        
        cmm.playedJgSilh = false;
        cmm.correctJgSilh = 0;
        cmm.incorrectJgSilh = 0;
        cmm.timeJgSilh = 0;

        cmm.gameScenesIndexesCopy = cmm.gameScenesIndexes.ToList();
        cmm.NextGame();
    }

    public void EndGame()
    {
        Common.common.gameScenesIndexesCopy.Clear();
        Common.common.NextGame();
    }

    public void ShowOverlay(Transform parent)
    {
        foreach (Transform c in parent.GetComponentsInChildren<Transform>(true))
        {
            if (c.CompareTag("Stable") == false) {
                if (c.CompareTag("Overlay")) {
                    c.gameObject.SetActive(true);
                    return;
                } else {
                    if(c.TryGetComponent<TextMeshProUGUI>(out var txt)) {
                        txt.alpha = 0.5f;
                    } else if (c.TryGetComponent<Image>(out var img)) {
                        Color clr = img.color;
                        clr.a = 0.5f;
                        img.color = clr;
                    }
                }
            }
        }
    }

    public void HideOverlay(Transform parent)
    {
        foreach (Transform c in parent.GetComponentsInChildren<Transform>(true))
        {
            if (c.CompareTag("Stable") == false) {
                if (c.CompareTag("Overlay")) {
                    c.gameObject.SetActive(false);
                    return;
                } else {
                    if(c.TryGetComponent<TextMeshProUGUI>(out var txt)) {
                        txt.alpha = 1f;
                    } else if (c.TryGetComponent<Image>(out var img)) {
                        Color clr = img.color;
                        clr.a = 1f;
                        img.color = clr;
                    }
                }
            }
        }
    }
}
