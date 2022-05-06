using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

public class PokemonDetailsHandler : MonoBehaviour
{
    public GetPokemonSpeciesHandler getPokemonSpeciesHandler;
    public PokemonDetailsVisual pokemonDetailsVisual;
    private JSONNode pokemonDetailsJson;

    private string nameKey = "name";
    private string idKey = "id";
    private string typesKey = "types";
    private string heightKey = "height";
    private string weightKey = "weight";
    private string spritesKey = "sprites";
    private string spriteFrontVarationKey = "front_default";

    private void CheckFunction(){
        Debug.Log($"Pokemon name: {GetPokemonName()}");

        var pokemonTypes = GetPokemonTypes();
        int i = 1;
        foreach (var pokemonType in pokemonTypes){
            Debug.Log("Pokemon type " + i +  " is: " + pokemonType);
            i ++;
        }
        Debug.Log($"Height: {GetPokemonHeight_m()} m");
        Debug.Log($"Weight: {GetPokemonWeight_kg()} kg");
    }

    private void SetPokemonDetails(){
        if(pokemonDetailsVisual){
            pokemonDetailsVisual.SetVisual(this);
        }
    }

    public void SaveWebRequest(string webRespond){
        pokemonDetailsJson = JSONNode.Parse(webRespond);
        // CheckFunction();
        SetPokemonDetails();
        getPokemonSpeciesHandler.SendWebRequest(this); //get the description of the pokemon
    }

    public void DisplayRequestError(string errorText){
        
    }

    public string GetPokemonName(){
        string pokeName = default;
        pokeName = pokemonDetailsJson[nameKey];
        return pokeName;
    }

    public int GetPokemonId(){
        int id = default;
        id = pokemonDetailsJson[idKey].AsInt;
        return id;
    }

    public List<string> GetPokemonTypes(){
        List<string> pokemonTypes = new List<string>();
        JSONArray tempArray = pokemonDetailsJson[typesKey].AsArray;
        foreach (var temp in tempArray){
            // Debug.Log(temp.Value["type"]["name"]);
            pokemonTypes.Add(temp.Value["type"]["name"]);
        }

        return pokemonTypes;
    }

    public float GetPokemonWeight_kg(){
        float weightInKg = 0f;
        if(pokemonDetailsJson == null){return -1f;}
        weightInKg = pokemonDetailsJson[weightKey].AsFloat * 0.1f;
        return weightInKg;
    }

    public float GetPokemonHeight_m(){
        float heightInMetre = 0f; 
        if(pokemonDetailsJson == null){return -1f;}
        heightInMetre = pokemonDetailsJson[heightKey].AsFloat * 0.1f;
        return heightInMetre;
    }

    public string GetPokemonSpriteUrl(){
        string spriteUrl = pokemonDetailsJson[spritesKey][spriteFrontVarationKey];
        return spriteUrl;
    }
}
