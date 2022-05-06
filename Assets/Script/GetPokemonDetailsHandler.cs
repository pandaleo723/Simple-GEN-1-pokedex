using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using JwDeveloper;

public class GetPokemonDetailsHandler : MonoBehaviour
{
    public UnityWebRequestHandler getPokemonIdWebRequest; 
    public CanvasGroup playerUiCanvasGroup;
    public TMP_InputField idInputFied;
    public Slider idSlider;

    private int pokemonId = 1;
    private string apiUrl = null;
    private bool isOnHold = false;
    private bool isHolded = false;
    private Coroutine OnPointerDownCor;

    public void SetPokemonId_Slider(float value){
        pokemonId = (int)value;
        ChangePokemonIdDisplay();
    }

    public void SetPokemonId_InputField(string value){
        if(string.IsNullOrEmpty(value)){return;}
        pokemonId = int.Parse(value);
        ChangePokemonIdDisplay();
    }

    public void IncreasePokemonId(bool increase){
        if(isHolded) {
            isHolded = false;
            return;
        }
        
        IncreasePokemonId(increase, 1f);
    }

    private void IncreasePokemonId(bool increase, float value){
        if(increase){
            idSlider.value += value;
        }else{
            idSlider.value -= value;
        }
    }

    public void OnPointerDownIncreasePokemonId(bool increase){
        isOnHold = true;
        OnPointerDownCor = StartCoroutine(HoldIncreasePokemonId(increase));
    }

    public void OnPointerUpIncreasePokemonId(){
        isOnHold = false;
        StopCoroutine(OnPointerDownCor);
    }

    public void SetPlayerUiInteractable(bool interactable){
        playerUiCanvasGroup.interactable = interactable;
    }

    public void SendWebRequest(){
        if(apiUrl == null){
            apiUrl = getPokemonIdWebRequest.GetApiUrl();
        }
        string temp = string.Format(apiUrl, pokemonId.ToString());
        getPokemonIdWebRequest.SetApiUrl(temp);
        getPokemonIdWebRequest.SendWebRequest();
    }

    private IEnumerator HoldIncreasePokemonId(bool increase){
        //only start increase after 1sec
        yield return new WaitForSeconds(1f);
        
        isHolded = true;
        float waitTime = 0.2f;
        var waitSec = new WaitForSeconds(waitTime);
        float elapsedTime = 0f;


        int valueToChange = 1;
        while(isOnHold){  

            valueToChange = valueToChange * (1 + Mathf.RoundToInt(elapsedTime/2f));


            IncreasePokemonId(increase, valueToChange);

            elapsedTime += waitTime;
            yield return waitSec;
        }

    }

    private void ChangePokemonIdDisplay(){
        if (idInputFied.text != pokemonId.ToString()){
            idInputFied.text = pokemonId.ToString();
        }
        if(idSlider.value != pokemonId){
            idSlider.value = pokemonId;
        }
    }
}
