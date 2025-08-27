using UnityEngine;

public class UIManager : MonoBehaviour
{

    public GameObject menuPanel;      // Menü panelin
    public GameObject settingsPanel;  // Settings panelin

    public void OpenMenu()
    {
        CloseAll();
        menuPanel.SetActive(true);
        menuPanel.transform.SetAsLastSibling();  
    }

    public void OpenSettings()
    {
        CloseAll();
        settingsPanel.SetActive(true);
        settingsPanel.transform.SetAsLastSibling();
    }

    public void ClosePanels()
    {
        menuPanel.SetActive(false);
        settingsPanel.SetActive(false);
                   
    }

    void CloseAll()
    {
                  
        menuPanel.SetActive(false);
        settingsPanel.SetActive(false);
    }
}
