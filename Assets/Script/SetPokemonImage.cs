using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class SetPokemonImage : MonoBehaviour
{
    public Image image;
    public Sprite loadingSprite;
    public Vector3 loadingScale;
    public float rotateDegree;

    private bool isLoadingImage;

    public void SendImageRequest(string url){
        StartCoroutine(_SendImageRequest(url));
    }

    IEnumerator _SendImageRequest(string url){
        isLoadingImage = true;
        StartCoroutine(StartLoadingImage());
        UnityWebRequest uwr = UnityWebRequest.Get(url);
        yield return uwr.SendWebRequest();

        isLoadingImage = false;
        if(uwr.result == UnityWebRequest.Result.Success){
            SetImage(uwr.downloadHandler.data);
        }else{
            Debug.Log(uwr.error);
        }

    }

    private void SetImage(byte[] spriteBytes){
        Texture2D tex = new Texture2D(1,1);
        tex.LoadImage(spriteBytes);
        image.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(tex.width/2, tex.height/2));
    }

    IEnumerator StartLoadingImage(){
        var waitSec = new WaitForSeconds(.1f);
        //cache default transform;
        Vector3 defaultScale = image.transform.localScale;
        Vector3 defaultEulerRotate = image.transform.localEulerAngles;

        image.sprite = loadingSprite;
        image.transform.localScale = loadingScale;
        Vector3 rotateDegreeVec3 = new Vector3 (0, 0, rotateDegree);

        while(isLoadingImage){
            image.transform.localEulerAngles += rotateDegreeVec3;
            yield return waitSec;
        }

        //return to default scale
        image.transform.localScale = defaultScale;
        image.transform.localEulerAngles = defaultEulerRotate;
    }
}
