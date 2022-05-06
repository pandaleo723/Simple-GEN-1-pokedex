using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ClampInputFieldInterger : MonoBehaviour
{
    public InputField inputField;
    public TMP_InputField tmpInputField;
    public int min = 0;
    public int max = 1;

    private void Awake() {
        if(!inputField) inputField = GetComponent<InputField>();
        if (!tmpInputField)  tmpInputField = GetComponent<TMP_InputField>();
    }

    private void Start() {
        if(inputField) {
            inputField.contentType = InputField.ContentType.IntegerNumber;
            inputField.onEndEdit.AddListener(OnEndEdit);
            }
        if(tmpInputField) {
            tmpInputField.contentType = TMP_InputField.ContentType.IntegerNumber;
            tmpInputField.onEndEdit.AddListener(OnEndEdit);
            }
    }

    private void OnDestroy() {
        if(inputField){
            inputField.onEndEdit.RemoveListener(OnEndEdit);
        }
        if(tmpInputField){
            tmpInputField.onEndEdit.RemoveListener(OnEndEdit);
        }
    }

    public string ClampInt(int value){
        return Mathf.Clamp(value, min, max).ToString();
    }

    public void OnEndEdit(string value){
        if(inputField) inputField.text = ClampInt(int.Parse(value));
        if(tmpInputField) tmpInputField.text = ClampInt(int.Parse(value));
    }
}
