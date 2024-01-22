using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class LoadSceneButton : MonoBehaviour
    {
        [SerializeField] private int scene;

        public void Press()
        {
            SceneManager.LoadScene(scene);
        }
    }
}