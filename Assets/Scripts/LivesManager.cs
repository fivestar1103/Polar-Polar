using UnityEngine;
using UnityEngine.UI;

public class LivesManager : MonoBehaviour
{
    public Image[] hearts;

    public void LoseLife(int livesLeft)
    {
        for (int i = 0; i < hearts.Length; i++)
       
        {
            if (i < livesLeft)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].color = new Color(hearts[i].color.r, hearts[i].color.g, hearts[i].color.b, 0);
            }
        }
    }
    
    public void ResetLives()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].color = new Color(hearts[i].color.r, hearts[i].color.g, hearts[i].color.b, 1);
        }
    }
}