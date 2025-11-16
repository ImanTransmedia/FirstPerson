using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void CargarEscena(string nombre)
    {
        SceneManager.LoadScene(nombre);
    }

    public void RecargarEscenaActual()
    {
        var actual = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(actual);
    }

    public void CargarSiguiente()
    {
        int index = SceneManager.GetActiveScene().buildIndex;
        if (index + 1 < SceneManager.sceneCountInBuildSettings)
            SceneManager.LoadScene(index + 1);
    }

    public void Salir()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
