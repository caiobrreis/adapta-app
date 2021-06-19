using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

// All methods of this class are invoked by buttons inside the game
public class ButtonsFunctions : MonoBehaviour
{
    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void AddToScreenHistory(GameObject frame)
    {
        Common.common.framesHistory.Add(frame);
    }

    public void GotoPrevScreen()
    {
        var fh = Common.common.framesHistory;

        fh[fh.Count - 1].SetActive(true);
        fh.RemoveAt(fh.Count - 1);
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
}
