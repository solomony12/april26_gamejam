using UnityEngine;
using UnityEngine.SceneManagement;
using static Unity.VisualScripting.Member;

public class MenuManager : MonoBehaviour
{
    [SerializeField] AudioSource[] menu_audio;
    Animator animator;
    int selectedScene = -1; // 0 is start, 1 is credits

    private void Start()
    {
        animator = GetComponent<Animator>();
        menu_audio[1].time = Random.Range(0f, menu_audio[1].clip.length);
        MailManager.ResetMail();
    }

    public void OnStartButtonClicked()
    {
        animator.SetTrigger("fade_screen");
        selectedScene = 0;
    }

    public void OnCreditsButtonClicked()
    {
        animator.SetTrigger("fade_screen");
        selectedScene = 1;
    }

    public void OnExitButtonClicked()
    {
        Application.Quit();
    }

    private void Update()
    {
        if ( selectedScene != -1)
        {
            menu_audio[0].volume = Mathf.Lerp(menu_audio[0].volume, 0, 0.01f);
            menu_audio[1].volume = Mathf.Lerp(menu_audio[1].volume, 0, 0.01f);
        }
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("MenuFade") &&
            animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f)
        {
            if (selectedScene == 1) SceneManager.LoadScene("Credits");
            else SceneManager.LoadScene("Game New");
        }
    }
}
