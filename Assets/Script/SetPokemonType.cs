using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SetPokemonType : MonoBehaviour
{
    public Image imageType1;
    public Image imageType2;
    public TextMeshProUGUI tmpType1;
    public TextMeshProUGUI tmpType2;
    [Space(10)]
    public List<PokemonTypeColor> pokemonTypeColorsMatching;

    public void SetTypeImage(List<string> types){
        if(types.Count == 1){
            SetType2DisplayActive(false);
        }else if(types.Count == 2){
            SetType2DisplayActive(true);
        }

        for(int i = 0; i < types.Count; i++){
            string typeName = types[i];
            
            var tempTypeColorMatch = pokemonTypeColorsMatching.Find(x => x.typeName == typeName);
            if(tempTypeColorMatch == null) {
                Debug.Log($"Can't find {typeName}"); 
                tempTypeColorMatch = new PokemonTypeColor();
            }

            Color typeColor = tempTypeColorMatch.color;

            if(i == 0){
                SetImageAndText(imageType1, typeColor, tmpType1, typeName);
            }else if(i == 1){
                SetImageAndText(imageType2, typeColor, tmpType2, typeName);
            }
        }
    }

    private void SetImageAndText(Image image, Color color, TextMeshProUGUI tmpText, string text){
        image.color = color;
        tmpText.text = text;
    }


    private void SetType2DisplayActive(bool active){
        imageType2.gameObject.SetActive(active);
        tmpType2.gameObject.SetActive(active);
    }

    [System.Serializable]
    public class PokemonTypeColor{
        public string typeName;
        public Color color = Color.red;
    }
}
