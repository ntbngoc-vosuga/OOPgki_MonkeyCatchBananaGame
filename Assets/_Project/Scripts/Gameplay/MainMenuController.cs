using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [Header("How To Play Panel")]
    [Tooltip("Kéo Panel hướng dẫn chơi vào đây")]
    [SerializeField] private GameObject howToPlayPanel;

    private void Start()
    {
        // Đảm bảo ẩn bảng hướng dẫn khi mới vào menu
        if (howToPlayPanel != null)
        {
            howToPlayPanel.SetActive(false);
        }
    }

    // 1. Chuyển sang màn chơi chính (SampleScene)
    public void PlayGame()
    {
        Time.timeScale = 1f; // Khôi phục thời gian phòng hờ
        SceneManager.LoadScene("SampleScene");
    }

    // 2. Mở bảng hướng dẫn
    public void OpenHowToPlay()
    {
        if (howToPlayPanel != null)
        {
            howToPlayPanel.SetActive(true);
        }
    }

    // 3. Đóng bảng hướng dẫn
    public void CloseHowToPlay()
    {
        if (howToPlayPanel != null)
        {
            howToPlayPanel.SetActive(false);
        }
    }

    // 4. Thoát game (hoạt động khi xuất ra file .exe)
    public void QuitGame()
    {
        Debug.Log("Đã bấm Thoát game!");
        Application.Quit();
    }
}