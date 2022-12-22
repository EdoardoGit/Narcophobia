using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField] private Image uiFill;
    [SerializeField] private Text uiText;

    public int Duration;

    private int remainingDuration;
    private int total;

    public Animator transition;
    public float transitionTime = 1f;
    public GameObject panel;

    private void Start()
    {
        //RoomData.timeSet(60);
        if (RoomData.Instance.timer == 0)
        {
            remainingDuration = Duration;
        }
        else
            remainingDuration = RoomData.Instance.timer;
        uiText.text = $"{remainingDuration / 60:00} : {remainingDuration % 60:00}";
    }

    public void StartTimer()
    {
        Being(Duration);
        RoomData.Instance.timerReset();
    }

    private void Being(int Second)
    {
        if (RoomData.Instance.timer == 0)
        {
            remainingDuration = Second;
        }
        else
            remainingDuration = RoomData.Instance.timer;
        total = remainingDuration;
        StartCoroutine(UpdateTimer());
    }

    private IEnumerator UpdateTimer()
    {
        while(remainingDuration >= 0)
        {
            if (remainingDuration <= 7 && !RoomData.Instance.timerClose)
                RoomData.Instance.isClose();
            uiText.text = $"{remainingDuration / 60:00} : {remainingDuration % 60:00}";
            uiFill.fillAmount = Mathf.InverseLerp(0, total, remainingDuration);
            remainingDuration--;
            yield return new WaitForSeconds(1f);

        }
        onEnd();
    }

    private void onEnd()
    {
        RoomData.Instance.isEnded();
        panel.SetActive(true);
        StartCoroutine(rest());
        LoadNextScene();
    }

    private IEnumerator rest()
    {
        yield return new WaitForSeconds(3f);
    }

    public void LoadNextScene()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));

    }

    IEnumerator LoadLevel(int levelIndex)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(levelIndex);
    }
}
