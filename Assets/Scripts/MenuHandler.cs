using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif


public class MenuHandler : MonoBehaviour
{
    public static MenuHandler startMenu;
    [SerializeField] TMP_InputField inputField;
    [SerializeField] TextMeshProUGUI bestScoreText;

    public string username;
    public string bestPlayerNameAndScore;
    private GameObject gameManager;
    private void Awake()
    {
        if(startMenu == null)
        {
            startMenu = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        LoadNameValueChange();
    }
    

    // Start is called before the first frame update
    void Start()
    {
        if (bestPlayerNameAndScore != null)
        {
            string path = Application.persistentDataPath + "/savejson.json";
            if (File.Exists(path))
            {
                Debug.Log("Load name");
                LoadName();
                bestScoreText.text = bestPlayerNameAndScore;
            }
            else
            {
                Debug.Log("No name");
               
                bestScoreText.text = "none";

            }
        }
    }
    public void FindGameManager()
    {
        gameManager = GameObject.Find("MainManager");
        if (gameManager != null)
        {
            MainManager gameManagerScript = gameManager.GetComponent<MainManager>();
            bestPlayerNameAndScore = gameManagerScript.BestScoreText();
            SaveName();
        }
    }

  
    public void StartNew()
    {
        username = inputField.text;
        SceneManager.LoadScene(1);
    }
    public void Exit()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
    void LoadNameValueChange()
    {
        LoadName();
        inputField.text = username;
        //bestScoreText.text = MainManager.mainManager.BestScoreText();
    }
    [System.Serializable]
    class SaveData
    {
        public string username;
        public string bestPlayerNameAndScore;
    }
    public void LoadName()
    {
        string path = Application.persistentDataPath + "/savejson.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            username = data.username;
            bestPlayerNameAndScore = data.bestPlayerNameAndScore;
        }
        
    }
    public void SaveName()
    {
        SaveData data = new SaveData();
        data.username = username;
        data.bestPlayerNameAndScore = bestPlayerNameAndScore;

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/savejson.json", json);
    }
}
