using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PokemonDetailsVisual : MonoBehaviour
{
    // public PokemonDetailsHandler pokemonDetailsHandler;
    public SetPokemonType setPokemonType;
    public SetPokemonImage setPokemonImage;
    public TextMeshProUGUI pokemonNameText;
    public TextMeshProUGUI heightText;
    public TextMeshProUGUI weightText;

    private string defaultHeightText = "Height: {0} m";
    private string defaultWeightText = "Weight: {0} kg";

    public void SetVisual(PokemonDetailsHandler pokemonDetailsHandler){
        SetPokemonName(pokemonDetailsHandler);

        setPokemonType.SetTypeImage(pokemonDetailsHandler.GetPokemonTypes());

        setPokemonImage.SendImageRequest(pokemonDetailsHandler.GetPokemonSpriteUrl());

        float height = pokemonDetailsHandler.GetPokemonHeight_m();
        if(height > 0f){
            SetPokemonHeight(height.ToString("F2"));
        }else{
            Debug.Log("Error Height Value: " + height);
        }

        float weight = pokemonDetailsHandler.GetPokemonWeight_kg();
        if(weight > 0f){
            SetPokemonWeight(weight.ToString("F2"));
        }else{
            Debug.Log("Error Weight Value: " + weight);
        }

    }

    private void SetPokemonName(PokemonDetailsHandler pokemonDetailsHandler){
        int pokeId = pokemonDetailsHandler.GetPokemonId();
        string pokeName = JwDeveloper.UtilityFunction.FirstLetterUppercase(pokemonDetailsHandler.GetPokemonName());
        string text = $"{pokeId} : {pokeName}";
        pokemonNameText.text = text;
    }

    private void SetPokemonHeight(string height){
        heightText.text = string.Format(defaultHeightText, height);
    }

    private void SetPokemonWeight(string weight){
        weightText.text = string.Format(defaultWeightText, weight);
    }

}