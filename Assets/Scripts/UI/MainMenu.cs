using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

	public GameObject newButton;
	public GameObject loadButton;
	public GameObject settingsButton;
	public GameObject exitButton;
	public GameObject rtToMainButton;
	public GameObject fullscreenButton;
	public GameObject windowedButton;
	public GameObject resDrop;
	public Blackout bout;
	private int currentX;
	private int currentY;
	private bool fullscreen = false;
	private Dictionary<string, Resolution> possibleRes;
	private List<string> options;
	private Dropdown drop;

	// Use this for initialization
	void Start () {
		StartCoroutine(bout.FadeInBlack());
		Screen.fullScreen = false;
		Resolution[] all = Screen.resolutions;
		possibleRes = new Dictionary<string, Resolution> ();
		currentX = Screen.currentResolution.width;
		currentY = Screen.currentResolution.height;
		newButton.SetActive (true);
		loadButton.SetActive (StateSaver.Load ());
		settingsButton.SetActive (true);
		exitButton.SetActive (true);
		windowedButton.SetActive (false);
		rtToMainButton.SetActive (false);
		resDrop.SetActive (false);
		fullscreenButton.SetActive (false);
		drop = resDrop.GetComponent<Dropdown> ();
		drop.ClearOptions ();
		options = new List<string>();
		int curVal = 0;
		foreach(Resolution r in all){
			string tmp = r.width + " x " + r.height;
			if(!possibleRes.ContainsKey(tmp)){
				if (r.width == Screen.width && r.height == Screen.height) {
					curVal = options.Count;
				}
				options.Add (tmp);
				possibleRes.Add (tmp, r);
			}
		}
		drop.AddOptions (options);
		drop.value = curVal;
		drop.onValueChanged.AddListener(delegate { dropHandle(drop); });
		dropHandle (drop);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void NewGameClicked(){
		StateSaver.Reset ();
		StartCoroutine(bout.FadeInBlack());
		StateSaver.Save();
		SceneManager.LoadSceneAsync(StateSaver.areaNames[(int)StateSaver.Area.Base]);
	}

	public void LoadGameClicked(){
		StartCoroutine(bout.FadeInBlack());
		StateSaver.Save();
		SceneManager.LoadSceneAsync(StateSaver.areaNames[(int)StateSaver.Area.Overworld]);
	}

	public void SettingsClicked(){
		newButton.SetActive (false);
		loadButton.SetActive (false);
		settingsButton.SetActive (false);
		exitButton.SetActive (false);
		windowedButton.SetActive (false);
		rtToMainButton.SetActive (true);
		resDrop.SetActive (true);
		fullscreenButton.SetActive (true);

	}

	public void ExitGameClicked(){
		Application.Quit ();
	}

	public void ReturnToMainClicked(){
		newButton.SetActive (true);
		loadButton.SetActive (true);
		settingsButton.SetActive (true);
		exitButton.SetActive (true);
		windowedButton.SetActive (false);
		rtToMainButton.SetActive (false);
		resDrop.SetActive (false);
		fullscreenButton.SetActive (false);
	}

	public void FullscreenClicked(){
		fullscreenButton.SetActive (false);
		windowedButton.SetActive (true);
		if (!fullscreen) {
			fullscreen = true;
			updateRes ();
		}
	}

	public void WindowedClicked(){
		fullscreenButton.SetActive (true);
		windowedButton.SetActive (false);
		if (fullscreen) {
			fullscreen = false;
			updateRes ();
		}
	}

	private void updateRes(){
		Screen.SetResolution (currentX, currentY, fullscreen);
	}

	private void dropHandle(Dropdown toBeHandled){
		currentX = possibleRes[toBeHandled.options[toBeHandled.value].text].width;
		currentY = possibleRes[toBeHandled.options[toBeHandled.value].text].height;
		updateRes ();
	}
}
