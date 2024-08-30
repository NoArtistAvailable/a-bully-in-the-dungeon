using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LeaderboardUI : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<LeaderboardManager>().onGotLeaderBoard += CreateLeaderBoard;
        template.gameObject.SetActive(false);
    }

    public TextMeshProUGUI template;
    private List<TextMeshProUGUI> created = new List<TextMeshProUGUI>();
    private void CreateLeaderBoard(List<LeaderboardManager.ScoreData> obj)
    {
        for (int i = created.Count - 1; i >= 0; i--)
        {
            Destroy(created[i].gameObject);
        }
        created.Clear();
        foreach (var data in obj)
        {
            var clone = Instantiate(template, template.transform.parent);
            clone.text = $"{data.score} - {data.name}";
            clone.gameObject.SetActive(true);
            created.Add(clone);
        }
    }
}
