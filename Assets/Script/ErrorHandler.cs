using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class ErrorHandler : MonoBehaviour
{
    public TextMeshProUGUI errorText;
    
    public UnityEvent OnDelayFinish;

    private string defaulttext;

    private void Awake() {
        defaulttext = errorText.text;
    }

    public void SetErrorText(string text){
        errorText.text = defaulttext + "\n" + text;
    }

    public void DelayCallback(float delaySeconds){
        StartCoroutine(_DelayCallback(delaySeconds));
    }

    IEnumerator _DelayCallback(float delaySeconds){
        var waitSec = new WaitForSeconds(delaySeconds);
        yield return waitSec;
        OnDelayFinish?.Invoke();
    }

}
