using UnityEngine;
using Ink.Runtime;

[CreateAssetMenu(fileName = "InkScript", menuName = "ScriptableObjects/Ink", order = 1)]
public class InkScript : ScriptableObject
{
    public TextAsset inkAsset;
    public AudioClip talkNoise;

    Story _inkStory;

    public void Start()
    {
        _inkStory = new Story(inkAsset.text);
    }

    public void End()
    {
        _inkStory = null;
    }

    public string AdvanceDialog()
    {
        string currentLine = _inkStory.Continue();
        Debug.Log(currentLine + " | " + _inkStory.canContinue + " | " + _inkStory.currentChoices.Count);
        return currentLine;
    }

    public AudioClip GetAudioClip()
    {
        return talkNoise;
    }

    public string[] GetChoices()
    {
        string[] choices = new string[_inkStory.currentChoices.Count];
        for (int i = 0; i < _inkStory.currentChoices.Count; i++)
        {
            choices[i] = _inkStory.currentChoices[i].text;
            Debug.Log("Choice " + (i + 1) + ". " + choices[i]);
        }
        return choices;
    }

    public void SetChoice(int index)
    {
        Debug.Log("Choice: " + index);
        _inkStory.ChooseChoiceIndex(index);
    }

    public string Save()
    {
        return _inkStory.state.ToJson();
    }

    public void Load(string savedJson)
    {
        _inkStory.state.LoadJson(savedJson);
    }

    public bool CheckContinue()
    {
        bool canContinue = _inkStory.canContinue;
        return canContinue;
    }

    public int CheckChoices()
    {
        return _inkStory.currentChoices.Count;
    }
}
