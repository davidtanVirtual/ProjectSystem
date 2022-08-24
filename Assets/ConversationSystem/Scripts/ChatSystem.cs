using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
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

    private int conversionIndex;
    private string displayChar;

    bool press;

    private void Awake()
    {
        InputSystem.onAnyButtonPress.Call(ctrl => press = true);
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
