using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class EnemyScore : MonoBehaviour
{
    public int _score;
    public int Score
    {
        get
        {
            return _score;
        }
    }

    public Animation scoreAnim;
    public Text scoreText;

    private void OnEnable()
    {
        scoreAnim.Stop();
        scoreText.text = "+" + Score.ToString();
        scoreText.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Transform playerScoreManager = null;


        if (player != null)
        {
            if (player.transform.childCount > 0)
            {
                Transform firstChild = player.transform.GetChild(0);

                if (firstChild.childCount >= 6)
                {
                    playerScoreManager = firstChild.GetChild(5);
                }
                else
                {
                    Debug.LogWarning("자식 6개 없음");
                }
            }
            else
            {
                Debug.LogWarning("플레이어 자식 없음");
            }
        }
        else
        {
            Debug.LogWarning("플레이어 태그 없음");
        }

        if (playerScoreManager != null)
        {
            playerScoreManager.gameObject.GetComponent<PlayerScoreManager>().PlayerScore += this.Score;
        }
        else
        {
            Debug.LogWarning("playerScoreManager가 NULL입니다.");
        }   
    }

    public void scoreVisualize()
    {
        scoreText.gameObject.SetActive(true);
        scoreText.transform.LookAt(Camera.main.transform.position);
        scoreText.transform.Rotate(0, 180, 0);

        scoreAnim.Play();
    }
}
