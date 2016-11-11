using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WebScript : MonoBehaviour {

	// Use this for initialization
    public Transform statusImage;
    public Transform statusText;
    public Transform overlay;
    public Sprite notFoundIcon;
    public Sprite loadingIcon;

    public bool isLoading = false;

	void Start () {
        AnimatorScript.instance.OnPrepareExerciseStart += AnimatorScript_OnPrepareExerciseStart;
        AnimatorScript.instance.OnPrepareExerciseEnd += AnimatorScript_OnPrepareExerciseEnd;
        overlay.gameObject.SetActive(false);
        this.isLoading = false;
	}

    void AnimatorScript_OnPrepareExerciseEnd(object sender, PrepareEventArgs e)
    {
        Application.ExternalCall("CallAnimationLoadEnd", e.status, e.caller);
        this.isLoading = false;
        switch(e.status)
        {
            case PrepareStatus.Prepared:
                overlay.gameObject.SetActive(false);
                this.isLoading = false;
                break;
            
            case PrepareStatus.NotFound:
                this.statusImage.GetComponent<Image>().sprite = notFoundIcon;
                statusText.GetComponent<Text>().text = "No disponible";
                statusImage.transform.rotation = Quaternion.identity;
                break;
        }

    }

    void AnimatorScript_OnPrepareExerciseStart(object sender, PrepareEventArgs e)
    {
        Application.ExternalCall("CallAnimationLoadStart", e.caller);
        overlay.gameObject.SetActive(true);
        switch(e.status)
        { 
            case PrepareStatus.Preparing:
                statusImage.GetComponent<Image>().sprite = loadingIcon;
                statusText.GetComponent<Text>().text = "Preparando";
                break;
        }
        this.isLoading = true;
    }
	
    public void HideOverlay()
    {
        this.isLoading = false;
        overlay.gameObject.SetActive(false);
    }

    public void ShowOverlay(string s)
    {
        this.isLoading = false;
        overlay.gameObject.SetActive(true);
        statusImage.GetComponent<Image>().sprite = notFoundIcon;
        statusText.GetComponent<Text>().text = s;
        statusImage.transform.rotation = Quaternion.identity;
    }
	// Update is called once per frame
	void FixedUpdate () {      
	    if(this.isLoading)
        {
            statusImage.RotateAroundLocal(Vector3.forward, -Time.fixedDeltaTime);
        }
	}
    void OnDestroy()
    {
        if (AnimatorScript.instance != null)
        {
            AnimatorScript.instance.OnPrepareExerciseStart -= AnimatorScript_OnPrepareExerciseStart;
            AnimatorScript.instance.OnPrepareExerciseEnd -= AnimatorScript_OnPrepareExerciseEnd;
        }
    }
}
