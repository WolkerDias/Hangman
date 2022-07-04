using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Text timeText;
    private float time;

    public Text wordToFindText, wordTheme;
    
    private string[] words = File.ReadAllLines(@"Assets/palavras.txt");
    private string[] word;
    private string choosenWord;
    private string hiddenWord;

    public GameObject[] hangMan;
    private int fails;

    public GameObject winText;
    public GameObject looseText;

    private bool gameEnd = false;

    public Button replayButton;
    
    // Start is called before the first frame update
    void Start()
    {
        word = words[Random.Range(0, words.Length)].Split(';');
        choosenWord = word[0];

        wordTheme.text = word[1];

        for (int i = 0; i < choosenWord.Length; i++)
        {
            hiddenWord += "_";
        }    
        wordToFindText.text = hiddenWord;
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameEnd)
        {
            time += Time.deltaTime;
            timeText.text = time.ToString();
        }
    }


    private void OnGUI()
    {
        Event e = Event.current;
        if (e.type == EventType.KeyDown && e.keyCode.ToString().Length == 1 && !gameEnd)
        {
            string pressedLetter = e.keyCode.ToString();

            if (choosenWord.Contains(pressedLetter))
            {

                int index = choosenWord.IndexOf(pressedLetter);

                while (index != -1)
                {
                    hiddenWord = hiddenWord.Substring(0, index) + pressedLetter + hiddenWord.Substring(index + 1);

                    choosenWord = choosenWord.Substring(0, index) + "_" + choosenWord.Substring(index + 1);

                    index = choosenWord.IndexOf(pressedLetter);
                }
                wordToFindText.text = hiddenWord;
                
            }
            else
            {
                hangMan[fails].SetActive(true);
                fails++;
            }

            // Ganhou
            if (!hiddenWord.Contains("_"))
            {
                wordToFindText.color = Color.green;
                winText.SetActive(true);
                gameEnd = true;
                replayButton.gameObject.SetActive(true);
            }

            // Perdeu
            if (fails == hangMan.Length)
            {
                wordToFindText.color = Color.red;
                wordToFindText.text = word[0];
                looseText.SetActive(true);
                gameEnd = true;
                replayButton.gameObject.SetActive(true);
            }
        }
    }

    public void OnButtonClick()
    {
        SceneManager.LoadScene(0);
    }
}
