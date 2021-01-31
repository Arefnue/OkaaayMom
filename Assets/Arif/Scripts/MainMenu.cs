using UnityEngine;
using UnityEngine.SceneManagement;

namespace Arif.Scripts
{
    public class MainMenu : MonoBehaviour
    {
        public void OpenPanel(GameObject panel)
        {
            panel.SetActive(true);
        }
        
        public void ClosePanel(GameObject panel)
        {
            panel.SetActive(false);
        }

        public void NextScene()
        {
            SceneManager.LoadScene(1);
        }

        public void PreScene()
        {
            SceneManager.LoadScene(0);
        }
        
        public void CloseGame()
        {
            Application.Quit();
        }
        
    }
}
