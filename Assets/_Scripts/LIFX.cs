using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LIFX : MonoBehaviour {

	public enum commands {lights=0, state, states, toggle}

	private List<string> mySelectors = new List<string>();

	public static LIFX instance;

	private string rootUrl = "https://api.lifx.com/v1beta1/lights/";
	private string myToken = "c63bfbeb83c9c6882badd3fe295693b47591ff36e1fa8fdfd684267522bcf2d3";


	private Dictionary<string, string> myAuthHeaders = new Dictionary<string, string> ();

	private WWWForm myForm;

	//public string listUrl = "https://api.lifx.com/v1beta1/lights/all/";

	// Use this for initialization
	void Awake () {
		if (instance == null || instance == this) {
			instance = this;
		} else {
			Destroy (instance);
		}

		//preliminary initialization
		myForm = new WWWForm();
		mySelectors.Add ("all/");
		myAuthHeaders.Add ("Authorization", "Bearer " + myToken);

		//myForm.headers

		//get list of selectors, save to list

		//StartCoroutine (GetLightStatus ());

		//StartCoroutine (ToggleLight(mySelectors[0], 1.0f));

	}

	//(GET) requests LIFX for our lights
	public IEnumerator GetLightStatus() {
		string url = rootUrl + mySelectors[0];	//all
		WWW www = new WWW(url, null, myAuthHeaders);

		CoroutineWithData cd = new CoroutineWithData (this, RequestDataWWW (www));

		//Debug.Log ("coroutine: " + cd.result);
		yield return cd.coroutine;
		Debug.Log ("value: " + cd.result);
		//StartCoroutine(WaitForRequest(www));
	}


	//saves the list of lights (JSON object) to our list of selectors
	public void SaveLightsList() {


	}

	//sets the state of the lights
	public IEnumerator SetStates(List<string> selectors, List<bool> states) {
		string url;

		//different url if multiple selectors
		if (selectors.Count > 1) {
			url = rootUrl + "states/";

		} else {
			url = rootUrl + selectors[0] + "state/";
		}

		//do things with myform



		byte[] rawFormData = myForm.data;
		WWW www = new WWW (url, rawFormData, myAuthHeaders);
		CoroutineWithData cd = new CoroutineWithData (this, RequestDataWWW (www));
		yield return cd.coroutine;
		Debug.Log ("set states value returned: " + cd.result);
	}

	//toggles lights and saves their on/off state
	public IEnumerator ToggleLight(string selector, float duration) {
		string url = rootUrl + selector + "toggle/";

		myForm.AddField ("duration", duration.ToString ());


		byte[] rawFormData = myForm.data;
		WWW www = new WWW (url, rawFormData, myAuthHeaders);
		CoroutineWithData cd = new CoroutineWithData (this, RequestDataWWW (www));
		yield return cd.coroutine;
		Debug.Log ("set states value returned: " + cd.result);
	}


	public void BreatheEffect(string selector, string color, string period, string cycles, bool persist, bool power_on = true) {


	}

	public void PulseEffect(string selector, string color, float period, float cycles, bool persist, bool power_on = true) {


	}

	public void TurnAllLightsOff() {
		//foreach light in list of selects, do powerlight(false);

	}

	public void PowerLight(string selector, bool powered) {


	}


	
	public void GetList () {
		WWWForm form = new WWWForm();
		form.AddField("selector", "all");
		form.AddField("duration", "5");

		form.headers["content-type"] = "application/json";


		byte[] rawData = form.data;

		var headers = form.headers;

		headers.Add("Authorization", "Bearer " + myToken);




		Debug.Log("going through headers: ");

		foreach (string key in headers.Keys) {
			Debug.Log ("found key: " + key);
		}

		// Post a request to an URL with our custom headers
		WWW www = new WWW(rootUrl, rawData, headers);
		StartCoroutine(WaitForRequest(www));
	}


	public void SetAllLights (bool active) {

		string url = "https://api.lifx.com/v1beta1/lights/all/toggle";


		WWWForm form = new WWWForm();
		form.AddField("selector", "all");
		form.AddField("duration", "5");

		form.headers["content-type"] = "application/json";



		string token = "c63bfbeb83c9c6882badd3fe295693b47591ff36e1fa8fdfd684267522bcf2d3";

		byte[] rawData = form.data;
		//Hashtable header = new Hashtable();
		var headers = form.headers;
		// Add a custom header to the request.
		// In this case a basic authentication to access a password protected resource.
		headers.Add("Authorization", "Bearer " + token);




		Debug.Log("going through headers: ");

		foreach (string key in headers.Keys) {
			Debug.Log ("found key: " + key);
		}

		// Post a request to an URL with our custom headers
		WWW www = new WWW(url, rawData, headers);
		StartCoroutine(WaitForRequest(www));
	}


	IEnumerator RequestDataWWW(WWW www){
		Debug.Log ("requestDataWWW");
		yield return www;
		// check for errors
		if (string.IsNullOrEmpty(www.error)){
			yield return "success! data is: " + www.text;
		} else {
			yield return "error! " + www.error;
		}    
	}

	IEnumerator WaitForRequest(WWW www){

		yield return www;
		Debug.Log(www.text);

		// check for errors
		if (www.error == null){
			Debug.LogWarning("WWW Ok!: " + www.text);
		} else {
			Debug.LogError("WWW Error: "+ www.error);
		}    
	}



	public void Example () {

		//DO NOT USE THIS URL
		//string url = "https://api.lifx.com/v1.0-beta1/lights/all/toggle";

		string url = "https://api.lifx.com/v1beta1/lights/all/";

		//string url = "https://api.lifx.com/v1/";


		WWWForm form = new WWWForm();
		//form.AddField("selector", "all");
		//form.AddField("duration", "5");
		form.AddField("color", "#ff0000");

		//form.headers["content-type"] = "application/json";



		string token = "c63bfbeb83c9c6882badd3fe295693b47591ff36e1fa8fdfd684267522bcf2d3";

		byte[] rawData = form.data;
		//Hashtable header = new Hashtable();
		var headers = form.headers;
		// Add a custom header to the request.
		// In this case a basic authentication to access a password protected resource.
		headers.Add("Authorization", "Bearer " + token);







		// Post a request to an URL with our custom headers
		WWW www = new WWW(url, null, headers);
		//WWW www = new WWW(url);
		StartCoroutine(WaitForRequest(www));
	}
		






}
