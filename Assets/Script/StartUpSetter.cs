using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartUpSetter : MonoBehaviour
{
    public CanvasGroup pokedexView;

    private void Start() {
        SetCanvasGroupAlpha(0f);
    }

    public void SetCanvasGroupAlpha(float alpha){
        pokedexView.alpha = alpha;
    }
}
