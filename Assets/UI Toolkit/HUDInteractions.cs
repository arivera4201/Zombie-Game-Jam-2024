using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Video;

public class uiInteractions : MonoBehaviour
{
    // GameObjects
    public GameObject arm;
    public GameObject grandma;
    public GameObject intro;

    public VideoPlayer theRock;
    public AudioSource heyBrah;

    public PlayerCamera playerCamera;

    // UI Elements
    public VisualElement ui;
    public VisualElement hud;
    public VisualElement mainmenu;
    public VisualElement brainRotMeter;
    public Label dogecoin;
    public Label ammo;
    public Label rounds;
    public VisualElement roundsimage;
    public VisualElement[] listOfMemes;
    public VisualElement gameOver;
    public VisualElement settings;
    
    public SliderInt sens;
    
    public Button hat;
    public Button settingsExit;

    public VisualElement pause;
    public VisualElement brain;

    public VisualElement buttons;
    public Button playButton;
    public Button settingsButton;
    public Button exitButton;
    public Button pauseRestart;
    public Button pauseQuit;
    public Button pauseSettings;
    
    private int numberOfActiveMemes;
    bool isPlaying = false;

    [SerializeField] Player player;
    private RoundHandler roundHandler;
    private SpreadOutPointGenerator generator = new SpreadOutPointGenerator();
    public GameObject brainRot;
    private AudioSource[] brainrotAudios;
    public VisualElement TV;

    private void Awake() {
        ui = GetComponent<UIDocument>().rootVisualElement;
    }

    // Start is called before the first frame update
    private void OnEnable()
    {
        // TV
        TV = ui.Q<VisualElement>("TV");
        TV.BringToFront();

        // Head Elements
        hud = ui.Q<VisualElement>("HUD");
        brain = ui.Q<VisualElement>("Brain");
        mainmenu = ui.Q<VisualElement>("MainMenu");
        settings = ui.Q<VisualElement>("Settings");
        pause = ui.Q<VisualElement>("Pause");
        gameOver = ui.Q<VisualElement>("GameOver");

        // MainMenu Elements
        playButton = ui.Q<Button>("Play");
        playButton.clicked += OnPlay;

        settingsButton = ui.Q<Button>("SettingsButton");
        settingsButton.clicked += OpenSettings;

        exitButton = ui.Q<Button>("Quit");
        exitButton.clicked += OnQuit;

        pauseRestart = ui.Q<Button>("PauseRestart");
        pauseSettings = ui.Q<Button>("PauseSettings");
        pauseQuit = ui.Q<Button>("PauseQuit");

        pauseRestart.clicked += RestartGame;
        pauseQuit.clicked += OnQuit;
        pauseSettings.clicked += OpenSettings;

        hat = ui.Q<Button>("Hat");
        hat.clicked += PlayHeyBrah;

        settingsExit = ui.Q<Button>("ExitSettings");
        settingsExit.clicked += CloseSettings; 

        sens = ui.Q<SliderInt>("Sensitivity");
        sens.value = 10;
        sens.RegisterCallback<ChangeEvent<int>>((evt) => {
            playerCamera.sensitivity = evt.newValue;
            sens.label = "SENS: " + evt.newValue;
            sens.value = evt.newValue;
        });

        buttons = ui.Q<VisualElement>("Buttons");
        

        // UI Element Initialization
        brainRotMeter = ui.Q<VisualElement>("BrainRotMeter");
        dogecoin = ui.Q<Label>("dogecoin");
        ammo = ui.Q<Label>("ammo");
        rounds = ui.Q<Label>("rounds");
        roundsimage = ui.Q<VisualElement>("roundsimage");
        listOfMemes = new VisualElement[]{
            ui.Q<VisualElement>("minecraft"),
            ui.Q<VisualElement>("satisfying1"),
            ui.Q<VisualElement>("satisfying2"),
            ui.Q<VisualElement>("satisfying3"),
            ui.Q<VisualElement>("subwaysurfers"),
            ui.Q<VisualElement>("csgosurfing"),
            ui.Q<VisualElement>("mlgteletubbies"),
            ui.Q<VisualElement>("mobilegamead1"),
            ui.Q<VisualElement>("mobilegamead2"),
            ui.Q<VisualElement>("soapcarving")
        };
        disableMemes();

        // In Game Scripts
        roundHandler = FindObjectOfType<RoundHandler>();
        brainrotAudios = brainRot.GetComponents<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && isPlaying) {
            if (!pause.visible) {
                Time.timeScale = 0;
                AudioListener.pause = true;
                arm.SetActive(false);
                player.GetComponent<PlayerCamera>().DisableCameraStuff();
                player.GetComponent<PlayerCamera>().enableCamera = false;
            } else {
                Time.timeScale = 1;
                AudioListener.pause = false;
                arm.SetActive(true);
                player.GetComponent<PlayerCamera>().EnableCameraStuff();
                player.GetComponent<PlayerCamera>().enableCamera = true;
            }

            pause.visible = !pause.visible;
        }

        if (isPlaying) {
            rounds.text = roundHandler.round.ToString();
            dogecoin.text = "$" + player.score.ToString();
        
            
            float brainRotPercentage = player.brainrot / player.maxBrainrot;
            brainRotMeter.style.width = Length.Percent(Mathf.Lerp(0f, 90f, brainRotPercentage));
            
        
            if (brainRotPercentage < 0.1f)
            {
                brainrotAudios[0].volume = 0;
                brainrotAudios[1].volume = 0;
                brainrotAudios[2].volume = 0;
                numberOfActiveMemes = 0;
            }
            else if (brainRotPercentage < 0.2f)
            {
                brainrotAudios[0].volume = 0.1f;
                brainrotAudios[1].volume = 0;
                brainrotAudios[2].volume = 0;
                numberOfActiveMemes = 1;
            }
            else if (brainRotPercentage < 0.3f)
            {
                brainrotAudios[0].volume = 0.25f;
                brainrotAudios[1].volume = 0;
                brainrotAudios[2].volume = 0;
                numberOfActiveMemes = 3;
            }
            else if (brainRotPercentage < 0.5f)
            {
                brainrotAudios[0].volume = 0.25f;
                brainrotAudios[1].volume = 0.1f;
                brainrotAudios[2].volume = 0;
                numberOfActiveMemes = 5;
            }
            else if (brainRotPercentage < 0.7f)
            {
                brainrotAudios[0].volume = 0.25f;
                brainrotAudios[1].volume = 0.25f;
                brainrotAudios[2].volume = 0.1f;
                numberOfActiveMemes = 8;
            }
            else
            {
                brainrotAudios[0].volume = 0.25f;
                brainrotAudios[1].volume = 0.25f;
                brainrotAudios[2].volume = 0.25f;
                numberOfActiveMemes = 10;
            }

            runMemes();
            
            if (player.weapon.isReloading)
                ammo.text = "Reloading...";
            else
                ammo.text = player.weapon.currentAmmo.ToString() + "|" + player.weapon.reservedAmmo.ToString();
        }
    }

    void OnKeyDown(KeyDownEvent ev) {
        Debug.Log(ev.keyCode);
        if (ev.keyCode == KeyCode.Escape && isPlaying) {
        }
    }

    void runMemes() {
        int currentActiveMemes = getMemesCurrentlyEnabled();
        if (numberOfActiveMemes == currentActiveMemes)
            return;
        else if (numberOfActiveMemes == 0)
            disableMemes();
        else if (numberOfActiveMemes > currentActiveMemes)
            enableMemes();
        return;
    }

    int getMemesCurrentlyEnabled() {
        int memeCount = 0;

        foreach (VisualElement meme in listOfMemes) {
            if (meme.visible)
                memeCount++;
        }

        return memeCount;
    }

    void enableMemes() {
        for (int i = 0; i < numberOfActiveMemes - 1; i++) {
            listOfMemes[i].visible = true;
        }
    }

     void disableMemes() {
        reshuffle();
        for (int i = 0; i < listOfMemes.Length; i++) {
            listOfMemes[i].visible = false;
        }
    }

    void reshuffle()
    {
        for (int t = 0; t < listOfMemes.Length; t++)
        {
            VisualElement tmp = listOfMemes[t];
            int r = Random.Range(t, listOfMemes.Length);
            listOfMemes[t] = listOfMemes[r];
            listOfMemes[r] = tmp;
        }

        List<Point> points = generator.GeneratePoints(11, 0, Screen.width + 400f, 0, Screen.height + 400f, Screen.width/5f);
        Debug.Log($"{listOfMemes.Length} Length OF Memes, {points.Count} points ocunt");
        for (int i = 0; i < listOfMemes.Length; i++)
        {
            listOfMemes[i].style.width = Random.Range(500f * i / 5f, 1000f * i / 5f);
            listOfMemes[i].style.height = Random.Range(500f * i / 5f, 1000f * i / 5f);

            listOfMemes[i].style.left = points[i].X - 200f;
            listOfMemes[i].style.top = points[i].Y - 200f;
            Debug.Log($"X: {points[i].X:F2}, Y: {points[i].Y:F2}");
        }
    }

    private void OnPlay()
    {
        Debug.Log("PLAYPLAY");
        if (buttons.visible == true) {
            buttons.visible = false;
            grandma.GetComponent<Grandma>().isActive = false;
            grandma.GetComponent<Grandma>().start = true;
            Invoke("StartGame", 3f);
        }
    }

    private void StartGame() {
        isPlaying = true;
        arm.SetActive(true);
        player.GetComponent<PlayerCamera>().EnableCameraStuff();
        player.GetComponent<PlayerCamera>().enableCamera = true;
        Destroy(intro);
        roundHandler.RoundStart();
        hud.visible = true;
        brain.visible = true;
        mainmenu.visible = false;
        player.GetComponent<AudioSource>().Play();
    }

    private void OpenSettings() {
        if (Time.timeScale == 0)
            pause.visible = false;
        settings.visible = true;
    }

    private void CloseSettings() {
        if (Time.timeScale == 0)
            pause.visible = true;
        settings.visible = false;
    }

    private void OnQuit()
    {
        Application.Quit();
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#endif
    }

    public void GameEnded() {
        AudioSource[] audios = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
        foreach(AudioSource audio in audios) {
            audio.volume = 0;
        }

        theRock.Play();

        hud.visible = false;
        brain.visible = false;
        gameOver.visible = true;
    }

    public void RestartGame() {
        player.Restart();
        Time.timeScale = 1;
        AudioListener.pause = false;
    }

    public void PlayHeyBrah() {
        heyBrah.Play();
    }
}
