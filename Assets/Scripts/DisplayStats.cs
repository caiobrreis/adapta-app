using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayStats : MonoBehaviour
{
    public TextMeshProUGUI cores_vzs_jogadas;
    public TextMeshProUGUI cores_acertos;
    public TextMeshProUGUI cores_erros;
    public TextMeshProUGUI cores_tempo_medio;
    public TextMeshProUGUI cores_melhor_tempo;
    
    [Space]
    public TextMeshProUGUI mat_vzs_jogadas;
    public TextMeshProUGUI mat_acertos;
    public TextMeshProUGUI mat_erros;
    public TextMeshProUGUI mat_tempo_medio;
    public TextMeshProUGUI mat_melhor_tempo;

    [Space]
    public TextMeshProUGUI silhu_vzs_jogadas;
    public TextMeshProUGUI silhu_acertos;
    public TextMeshProUGUI silhu_erros;
    public TextMeshProUGUI silhu_tempo_medio;
    public TextMeshProUGUI silhu_melhor_tempo;

    // Invoked with the stats screen
    // Get player data from the database and write to the user screen
    IEnumerator Start()
    {
        WWWForm form = new WWWForm();
        form.AddField("email", Common.common.email);
        WWW request = new WWW("http://localhost/sqlconnect/returnstats.php", form);
        yield return request;

        string[] results = request.text.Split('\t');
        cores_vzs_jogadas.text = results[0];
        cores_acertos.text = results[1];
        cores_erros.text = results[2];
        cores_tempo_medio.text = results[3] + " Seg.";
        cores_melhor_tempo.text = results[4] + " Seg.";
        mat_vzs_jogadas.text = results[5];
        mat_acertos.text = results[6];
        mat_erros.text = results[7];
        mat_tempo_medio.text = results[8] + " Seg.";
        mat_melhor_tempo.text = results[9] + " Seg.";
        silhu_vzs_jogadas.text = results[10];
        silhu_acertos.text = results[11];
        silhu_erros.text = results[12];
        silhu_tempo_medio.text = results[13] + " Seg.";
        silhu_melhor_tempo.text = results[14] + " Seg.";
    }
}
