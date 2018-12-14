using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace CompleteProject
{
    public class ScoreManager : MonoBehaviour
    {
        public static int score;        // The player's score.
        int highscore;

        Text text;                      // Reference to the Text component.
        public Text highscoreText;

        void Awake ()
        {
            // Set up the reference.
            text = GetComponent <Text> ();

            // Reset the score.
            highscore = PlayerPrefs.GetInt("highscore", highscore);
            highscoreText.text = "Highscore: " + highscore;
            score = 0;
        }

        void Update ()
        {
            // Set the displayed text to be the word "Score" followed by the score value.
            text.text = "Kills: " + score;

            if (score > highscore)
            {
                highscore = score;
                highscoreText.text = "Highscore: " + score;

                PlayerPrefs.SetInt("highscore", highscore);
            }
        }
    }
}