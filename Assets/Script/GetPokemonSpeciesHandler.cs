using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JwDeveloper;

public class GetPokemonSpeciesHandler : MonoBehaviour
{
    public UnityWebRequestHandler getPokemonSpeciesWebRequest;

    private string apiUrl = null;

    public void SendWebRequest(PokemonDetailsHandler pokemonDetailsHandler){
        if(apiUrl == null){
            apiUrl = getPokemonSpeciesWebRequest.GetApiUrl();
        }
        string temp = string.Format(apiUrl, pokemonDetailsHandler.GetPokemonId().ToString());
        getPokemonSpeciesWebRequest.SetApiUrl(temp);
        getPokemonSpeciesWebRequest.SendWebRequest();
    }
}
