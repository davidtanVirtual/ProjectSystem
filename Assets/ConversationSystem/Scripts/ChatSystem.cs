using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ChatSystem : MonoBehaviour
{
    [SerializeField]
    public ChatData[] chatData;
    [SerializeField]
    public Sprite[] bgData;
    [SerializeField]
    public TextMeshProUGUI[] nameText;

    [Space(5)]
    [SerializeField]
    private TextMeshProUGUI chatTexts;
    [SerializeField]
    private Image chatBackground;
    [SerializeField]
    private float displayTextDelay;
    [SerializeField]
    private Button input;

    private int conversionIndex;
    private string displayChar;
    private bool press;

    public Action chatCompleteCallback;

    //[SerializeField]
    //private PlayerInput input;

    private void Awake()
    {
        //input.SwitchCurrentActionMap("UI");
        //input.actions["Click"].started += ctx =>
        //{
        //    press = true;
        //};
    }

    private void OnEnable()
    {
        input.onClick.AddListener(() => press = true);

        chatCompleteCallback += CompleteChat;
    }

    private void OnDisable()
    {
        input.onClick.RemoveAllListeners();

        chatCompleteCallback -= CompleteChat;
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(StartConversation());
    }

    public IEnumerator StartConversation()
    {
        while(conversionIndex < chatData.Length)
        {
            ResetData();

            ChatData data = chatData[conversionIndex];

            chatBackground.sprite = bgData[data.charIndex];
            nameText[data.charIndex].gameObject.SetActive(true);
            nameText[data.charIndex].text = data.name;

            yield return StartCoroutine(DisplayText(conversionIndex));

            press = false;

            while (!press)
            {
                yield return new WaitForEndOfFrame();
            }
            conversionIndex++;
        }

        if (chatCompleteCallback != null)
            chatCompleteCallback();
    }

    public IEnumerator DisplayText(int chatIndex)
    {
        string text = chatData[chatIndex].chat;
        press = false;
        int index = 0;

        while (index < text.Length && press == false)
        {
            //get one letter
            char charText = text[index];

            //Actualize on screen
            chatTexts.text = Write(charText);

            //set to go to the next
            index ++;

            yield return new WaitForSeconds(displayTextDelay);
        }

        if (press)
        {
            chatTexts.text = text;
        }
    }

    private string Write(char letter)
    {
        displayChar += letter;
        return displayChar;
    }

    private void ResetData()
    {
        chatTexts.text = "";
        displayChar = "";

        for (int i = 0; i < nameText.Length; i++)
        {
            nameText[i].gameObject.SetActive(false);
        }
    }

    public void CompleteChat()
    {
        //input.SwitchCurrentActionMap("Player");
        gameObject.SetActive(false);
        chatCompleteCallback -= CompleteChat;
    }

    [System.Serializable]
    public class ChatData
    {
        [SerializeField]
        public string name;
        [SerializeField]
        public int charIndex;
        [TextArea(2, 3)]
        [SerializeField]
        public string chat;
    }
}
