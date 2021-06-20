using System;
using UnityEngine;
using UnityEngine.UIElements;
using System.Text.RegularExpressions;

public class UIScript : MonoBehaviour
{
    private Button button_next;
    private Button button_minimize;
    private VisualElement mainPane;
    public int currentPage = 1;
    public VisualElement root;
    public int numberOfSAM;
    public UQueryBuilder<VisualElement> test;
    public int[] valence;
    public int[] arousal;
    public int[] dominance;
    public RenderTexture textureVideo1;
    public UnityEngine.Video.VideoPlayer videoPlayer1;

    public UnityEngine.Video.VideoPlayer videoPlayer2;
    public RenderTexture textureVideo2;

    private void OnEnable()
    {

        valence = new int[numberOfSAM];
        arousal = new int[numberOfSAM];
        dominance = new int[numberOfSAM];

        Populate(valence, -1);
        Populate(arousal, -1);
        Populate(dominance, -1);
        root = GetComponent<UIDocument>().rootVisualElement;
        //var index = 0;
        /*
        button_next = root.Q<Button>("NextButton");
        button_next.RegisterCallback<ClickEvent>(ev => loadNextPage());
        */
        root.Query<Button>("NextButton").ForEach(Button =>
        {
            Button.RegisterCallback<ClickEvent>(ev => loadNextPage());
        }
        );

        /* Probably do not need minimize
        button_minimize = root.Q<Button>("MinimizeButton");
        button_minimize.RegisterCallback<ClickEvent>(ev => toggleMainPane());
        */
        var video = root.Q<Image>("Video1");
        video.image = textureVideo1;

        var video2 = root.Q<Image>("Video2");
        video2.image = textureVideo2;


        mainPane = root.Q("MainPane");
        for (int i = 1; i <= numberOfSAM; i++) {
            var index = 0;
            var currentI = i;
            Debug.Log(i);
            root.Query("Arousal"+ i + "TogglePane").Children<Toggle>().ForEach(toggle =>
            {
                var currentIndex = index;
                toggle.RegisterCallback<ClickEvent>(ev => setOnlyThisToggleActive2(toggle, "Arousal", currentI, currentIndex, arousal));
                index++;
            }
            );

            index = 0;
            root.Query("Valence" + i + "TogglePane").Children<Toggle>().ForEach(toggle =>
            {
                var currentIndex = index;
                toggle.RegisterCallback<ClickEvent>(ev => setOnlyThisToggleActive2(toggle, "Valence", currentI, currentIndex, valence));
                index++;
            }
            );

             index = 0;;
            root.Query("Dominance" + i + "TogglePane").Children<Toggle>().ForEach(toggle =>
            {
                var currentIndex = index;
                toggle.RegisterCallback<ClickEvent>(ev => setOnlyThisToggleActive2(toggle, "Dominance", currentI, currentIndex, dominance));
                index++;
            }
            );
        }

    }


    private void loadNextPage() {

        var curPage = root.Q("Page" + currentPage);
        currentPage++;
        curPage.style.display = DisplayStyle.None;
        var newPage = root.Q("Page" + currentPage);
        newPage.style.display = DisplayStyle.Flex;
        if (currentPage == 2) {
            disableAllBottomPane();
            videoPlayer1.Play();
            videoPlayer1.loopPointReached += EndReached;
        }

        if (currentPage == 6)
        {
            disableAllBottomPane();
            videoPlayer2.Play();
            videoPlayer2.loopPointReached += EndReached;
        }

        if (currentPage == 3 || currentPage == 5 || currentPage == 7 || currentPage == 9) {
            disableAllBottomPane();
        }
    }

    private void toggleMainPane() {
        VisualElement color = root.Q("Content");
        Color colorNew;
        if (mainPane.style.display == DisplayStyle.None)
        {
            colorNew = new Color32(255, 255, 255, 100);
            mainPane.style.display = DisplayStyle.Flex;
            color.style.backgroundColor = colorNew;
        }

        else {
            colorNew = new Color(255, 255, 255, 0);
            mainPane.style.display = DisplayStyle.None;
            color.style.backgroundColor = colorNew;
        }
    }
   
    private void setOnlyThisToggleActive2(Toggle currentToggle,String currentField, int currentI, int index, int[] toggleFieldName)
    {
        Debug.Log(currentI);
        root.Query(currentField+currentI+"TogglePane").Children<Toggle>().ForEach(toggle =>
        {
            toggle.value = false;
        }
);
        currentToggle.value = true;
        toggleFieldName[currentI-1] = index;

        if ((valence[currentI - 1] != -1) && (arousal[currentI - 1] != -1) && (dominance[currentI - 1] != -1)) { 
        enableAllBottomPane(); 
        }

    }

    void EndReached(UnityEngine.Video.VideoPlayer vp)
    {
        enableAllBottomPane();
        Debug.Log("End Reached");
    }

    public void disableAllBottomPane() {
        root.Query("BottomPane").Children<Button>().ForEach(button =>
        { button.style.display = DisplayStyle.None; });
    }

    public void enableAllBottomPane() {
        root.Query("BottomPane").Children<Button>().ForEach(button =>
        { button.style.display = DisplayStyle.Flex; });
    }

    public void Populate(int [] arr, int value)
    {
        for (int i = 0; i < arr.Length; i++)
        {
            arr[i] = value;
        }
    }
}
