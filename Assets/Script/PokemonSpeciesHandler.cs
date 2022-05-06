using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using System;

public class PokemonSpeciesHandler : MonoBehaviour
{
    public PokemonSpeciesVisual pokemonSpeciesVisual;

    Action<string> OnGetDescriptionComplete;

    private JSONNode pokemonSpeciesJson;

    private string descriptionArrayKey = "flavor_text_entries";
    private string descriptionKey = "flavor_text";
    private string languageKey = "language";
    private string gameVersionKey = "version";
    private string nameKey = "name";

    private void Awake() {
        OnGetDescriptionComplete += pokemonSpeciesVisual.OnLoadDescriptionComplete;
    }

    private void OnDestroy() {
        OnGetDescriptionComplete -= pokemonSpeciesVisual.OnLoadDescriptionComplete;
    }

    //clean up the string get from api
    private string CleanUpString(string text){
        return text.Replace("-\n", " ").Replace("\f", " ").Replace("\n", " ");
    }

    public void SaveWebRequest(string webRespond)
    {
        pokemonSpeciesJson = JSONNode.Parse(webRespond);
        StartCoroutine(_GetPokemonDescription_Englsih_VerionRed());
    } 

    IEnumerator _GetPokemonDescription_Englsih_VerionRed(){
        string selectedDescription = default;
        string selectedLanguage = "en";
        string selectedVersion = "red";
        JSONNode descriptions = pokemonSpeciesJson[descriptionArrayKey];

        int tracker = 0;
        foreach (var item in descriptions)
        {
            string languageCheck = item.Value[languageKey][nameKey];
            string versionCheck = item.Value[gameVersionKey][nameKey];
            if(languageCheck == selectedLanguage && versionCheck == selectedVersion){
                selectedDescription = item.Value[descriptionKey];
                break;
            }else{
                tracker++;
                continue;
            }
        }

        yield return 0;
        selectedDescription = CleanUpString(selectedDescription);
        OnGetDescriptionComplete?.Invoke(selectedDescription);
    }
    
}
