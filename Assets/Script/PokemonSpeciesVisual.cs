using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PokemonSpeciesVisual : MonoBehaviour
{

    public TextMeshProUGUI descriptionText;

    private string defaultLoadingText = "Loading";
    private string loadingAnimeText = ".";
    private bool isLoading = false;

    public void LoadingDescriptionVisual(){
        isLoading = true;
        StartCoroutine(_LoadingDescriptionVisual());
    }

    public void SetDescriptionText(string text){
        descriptionText.text = text;
    }

    public void OnLoadDescriptionComplete(string text){
        isLoading = false;
        StopCoroutine(_LoadingDescriptionVisual());
        SetDescriptionText(text);
    }

    IEnumerator _LoadingDescriptionVisual(){
        string tempLoadingText = defaultLoadingText;
        var waitSec = new WaitForSeconds(0.2f);
        string refreshLoadingChecker = defaultLoadingText + loadingAnimeText + loadingAnimeText + loadingAnimeText;

        while (isLoading){
            SetDescriptionText(tempLoadingText);

            yield return waitSec;

            if(tempLoadingText == refreshLoadingChecker){
                tempLoadingText = defaultLoadingText;
            }else{
                tempLoadingText += loadingAnimeText;
            }
        }
    }
}
