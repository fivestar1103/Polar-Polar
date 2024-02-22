using UnityEngine;
using UnityEngine.UI;

public class LivesManager : MonoBehaviour
{
    public Image[] hearts;

    // Call this method to update the UI when a life is lost
    public void LoseLife(int livesLeft)
    {
        // Disable one heart icon for each lost life
        for (int i = 0; i < hearts.Length; i++)
       
        {
            if (i < livesLeft)
            {
                hearts[i].enabled = true; // Heart should be visible
            }
            else
            {
                hearts[i].color = new Color(hearts[i].color.r, hearts[i].color.g, hearts[i].color.b, 0); // Heart should be hidden
            }
        }
    }
    
    public void ResetLives()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].color = new Color(hearts[i].color.r, hearts[i].color.g, hearts[i].color.b, 1); // Make all hearts visible
        }
    }
}