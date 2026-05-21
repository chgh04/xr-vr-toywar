using UnityEngine;

public class PlayerScoreManager : MonoBehaviour
{
    private int overallScore;
    private int _playerScore;

    public int PlayerScore
    {
        get {  return _playerScore; }
        set 
        { 
            _playerScore = value; 
            if (value > 0)
            {
                overallScore += value;
            }
        }
    }

    public int OverallScore
    {
        get { return  overallScore; }
    }
}
