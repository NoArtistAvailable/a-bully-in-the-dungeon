using System.Collections;
using System.Collections.Generic;
using elZach.Common;
using TMPro;
using UnityEngine;

public class LeaderboardUI : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<LeaderboardManager>().onGotLeaderBoard += CreateLeaderBoard;
        template.gameObject.SetActive(false);
        panel.SetActive(false);
        GameManager.onRestart += () =>
        {
            panel.SetActive(false);
        };
    }

    public GameObject panel;
    public TextMeshProUGUI template;
    private List<TextMeshProUGUI> created = new List<TextMeshProUGUI>();
    private void CreateLeaderBoard(List<LeaderboardManager.ScoreData> obj)
    {
        panel.SetActive(true);
        for (int i = created.Count - 1; i >= 0; i--)
        {
            Destroy(created[i].gameObject);
        }
        created.Clear();
        for (var i = 0; i < obj.Count; i++)
        {
            var data = obj[i];
            var clone = Instantiate(template, template.transform.parent);
            clone.transform.SetSiblingIndex(template.transform.GetSiblingIndex() + i);
            clone.text = $"{data.score} - {data.name}";
            clone.gameObject.SetActive(true);
            created.Add(clone);
        }
    }
}
