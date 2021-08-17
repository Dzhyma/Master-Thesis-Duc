using System;
using UnityEngine;
using UnityEngine.UIElements;
using System.IO;
using System.Text;
using System.Collections;

public class UIScript : MonoBehaviour
{
    private Button button_next;
    private Button button_minimize;
    private VisualElement mainPane;
    public int currentPage = 1;
    public VisualElement root;
    private int numberOfSAM = 5;
    private int numberOfPresence = 19;
    private int numberOfPages = 51;
    public int[] presenceResult;
    public UQueryBuilder<VisualElement> test;
    public int[] arousal;
    public int[] dominance;
    public int[] valence;

    public int[] VEQ_AC1;
    public int[] VEQ_CO1;
    public int[] VEQ_CH1;

    public int[] VEQ_AC2;
    public int[] VEQ_CO2;
    public int[] VEQ_CH2;

    public int[] VEQ_AC3;
    public int[] VEQ_CO3;
    public int[] VEQ_CH3;

    public int[] VEQ_AC4;
    public int[] VEQ_CO4;
    public int[] VEQ_CH4;
    public String[] colorsPicked;
    public String[] dates;

    public GameObject arrow1;
    public GameObject arrow2;
    public GameObject arrow3;


    //Video Stuff

    public UnityEngine.Video.VideoPlayer videoPlayer1;
    public UnityEngine.Video.VideoPlayer videoPlayer2;
    public UnityEngine.Video.VideoPlayer videoPlayer3;
    public UnityEngine.Video.VideoPlayer videoPlayer4;

    public RenderTexture textureVideo1;
    public RenderTexture textureVideo2;
    public RenderTexture textureVideo3;
    public RenderTexture textureVideo4;

    public GameObject firstAvatar;
    public GameObject secondAvatar;
    public GameObject thirdAvatar;
    public GameObject fourthAvatar;

    public GameObject maleChair;
    public GameObject femaleChair;


    //Path where to save file to
    private String path = @"C:\Users\unibwlab\Desktop\Master Thesis Duc\Result\";

    //private String path = @"C:\Users\Nguyen\Desktop\Master\Result\";

    public void changeAvatarSameGenderFirstSecond() {
        /*
        Debug.Log("changing avatar");
        firstAvatar.SetActive(false);
        //String[] bodyPartsToChange = { "Wolf3D_Hair", "EyeLeft", "EyeRight", "Wolf3D_Body", "Wolf3D_Head", "Wolf3D_Outfit_Bottom", "Wolf3D_Outfit_Footwear", "Wolf3D_Outfit_Top", "Wolf3D_Teeth" };
        String[] bodyPartsToChange = { "Wolf3D_Hair","Wolf3D_Body", "Wolf3D_Head", "Wolf3D_Outfit_Bottom", "Wolf3D_Outfit_Footwear", "Wolf3D_Outfit_Top", "Wolf3D_Teeth" };
        foreach (String body in bodyPartsToChange) {
            firstAvatar.transform.Find(body).GetComponent<SkinnedMeshRenderer>().materials = secondAvatar.transform.Find(body).GetComponent<SkinnedMeshRenderer>().materials;
            firstAvatar.transform.Find(body).GetComponent<SkinnedMeshRenderer>().sharedMesh = secondAvatar.transform.Find(body).GetComponent<SkinnedMeshRenderer>().sharedMesh;
        }

        firstAvatar.SetActive(true);
                */
        firstAvatar.SetActive(false);
        firstAvatar.GetComponent<VRRig>().chair.SetActive(false);
        secondAvatar.GetComponent<VRRig>().chair.SetActive(true);
        secondAvatar.SetActive(true);
    }

    public void changeAvatarSameGenderThirdFourth()
    {
        Debug.Log("changing avatar");
        /*
        String[] bodyPartsToChange = { "Wolf3D_Hair", "EyeLeft", "EyeRight", "Wolf3D_Body", "Wolf3D_Head", "Wolf3D_Outfit_Bottom", "Wolf3D_Outfit_Footwear", "Wolf3D_Outfit_Top", "Wolf3D_Teeth" };
        foreach (String body in bodyPartsToChange)
        {
            thirdAvatar.transform.Find(body).GetComponent<SkinnedMeshRenderer>().materials = fourthAvatar.transform.Find(body).GetComponent<SkinnedMeshRenderer>().materials;
            thirdAvatar.transform.Find(body).GetComponent<SkinnedMeshRenderer>().sharedMesh = fourthAvatar.transform.Find(body).GetComponent<SkinnedMeshRenderer>().sharedMesh;
        }*/
        thirdAvatar.SetActive(false);
        thirdAvatar.GetComponent<VRRig>().chair.SetActive(false);
        fourthAvatar.GetComponent<VRRig>().chair.SetActive(true);
        fourthAvatar.SetActive(true);
    }

    //Change von Female to Male
    public void changeAvatarDifferentGender()
    {
        secondAvatar.SetActive(false);
        secondAvatar.GetComponent<VRRig>().chair.SetActive(false);
        thirdAvatar.GetComponent<VRRig>().chair.SetActive(true);
        thirdAvatar.SetActive(true);


    }

    GameObject GetChildWithName(GameObject obj, string name)
    {
        Transform trans = obj.transform;
        Transform childTrans = trans.Find(name);
        if (childTrans != null)
        {
            return childTrans.gameObject;
        }
        else
        {
            return null;
        }

    }
    private void OnEnable()
    {

        valence = new int[numberOfSAM];
        arousal = new int[numberOfSAM];
        dominance = new int[numberOfSAM];
        colorsPicked = new string[numberOfSAM];


        Populate(valence, -1);
        Populate(arousal, -1);
        Populate(dominance, -1);

        VEQ_AC1 = new int[4];
        VEQ_CO1 = new int[4];
        VEQ_CH1 = new int[4];
        Populate(VEQ_AC1, -1);
        Populate(VEQ_CO1, -1);
        Populate(VEQ_CH1, -1);


        VEQ_AC2 = new int[4];
        VEQ_CO2 = new int[4];
        VEQ_CH2 = new int[4];
        Populate(VEQ_AC2, -1);
        Populate(VEQ_CO2, -1);
        Populate(VEQ_CH2, -1);

        VEQ_AC3 = new int[4];
        VEQ_CO3 = new int[4];
        VEQ_CH3 = new int[4];
        Populate(VEQ_AC3, -1);
        Populate(VEQ_CO3, -1);
        Populate(VEQ_CH3, -1);

        VEQ_AC4 = new int[4];
        VEQ_CO4 = new int[4];
        VEQ_CH4 = new int[4];
        Populate(VEQ_AC4, -1);
        Populate(VEQ_CO4, -1);
        Populate(VEQ_CH4, -1);

        presenceResult = new int[numberOfPresence];
        Populate(presenceResult,-1) ;



        dates = new String[numberOfPages];
        dates[0] = DateTime.Now.ToString("HH:mm:ss");


        root = GetComponent<UIDocument>().rootVisualElement;
        //var index = 0;
        /*
        button_next = root.Q<Button>("NextButton");
        button_next.RegisterCallback<ClickEvent>(ev => loadNextPage());
        */

        //button_next = root.Q<Button>("Change");
        //button_next.RegisterCallback<ClickEvent>(ev => changeAvatar());

        root.Query<Button>("NextButton").ForEach(Button =>
        {
            Button.RegisterCallback<ClickEvent>(ev => LoadNextPage());
        }
        );

        root.Query<Button>("HelpButton").ForEach(Button =>
        {
            Button.RegisterCallback<ClickEvent>(ev => ShowHelpPage());
        }
        );

        root.Query<Button>("HideButton").ForEach(Button =>
        {
            Button.RegisterCallback<ClickEvent>(ev => HideHelpPage());
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

        var video3 = root.Q<Image>("Video3");
        video3.image = textureVideo3;

        var video4 = root.Q<Image>("Video4");
        video4.image = textureVideo4;


        mainPane = root.Q("MainPane");
        for (int i = 1; i <= numberOfSAM; i++)
        {
            var value = 1;
            root.Query("Arousal" + i + "TogglePane").Children<Toggle>().ForEach(toggle =>
             {
                 var currentIndex = i;
                 var currentValue = value;
                 toggle.RegisterCallback<ClickEvent>(ev => SetOnlyThisToggleActiveSAM(toggle, "Arousal", currentIndex, currentValue, arousal));
                 value++;
             }
            );

            value = 1; ;
            root.Query("Dominance" + i + "TogglePane").Children<Toggle>().ForEach(toggle =>
            {
                var currentIndex = i;
                var currentValue = value;
                toggle.RegisterCallback<ClickEvent>(ev => SetOnlyThisToggleActiveSAM(toggle, "Dominance", currentIndex, currentValue, dominance));
                value++;
            }
            );


            value = 1;
            root.Query("Valence" + i + "TogglePane").Children<Toggle>().ForEach(toggle =>
            {
                var currentIndex = i;
                var currentValue = value;
                toggle.RegisterCallback<ClickEvent>(ev => SetOnlyThisToggleActiveSAM(toggle, "Valence", currentIndex, currentValue, valence));
                value++;
            }
            );


        }



        //Set all 4 VEQ

        for(int j = 1; j <= 4; j++) {
        for (int i = 1; i <= 4; i++)
        {
            var index = 1;
            var currentI = i;
            root.Query("EmbodimentTogglePaneAC"+j + i).Children<Toggle>().ForEach(toggle =>
              {
                  var currentIndex = index;
                  var currentJ = j;
                  switch (currentJ) {
                      case 1: toggle.RegisterCallback<ClickEvent>(ev => setThisToggleActiveVEQ(toggle, "EmbodimentTogglePaneAC" + currentJ, currentI, currentIndex, VEQ_AC1)); break;
                      case 2: toggle.RegisterCallback<ClickEvent>(ev => setThisToggleActiveVEQ(toggle, "EmbodimentTogglePaneAC" + currentJ, currentI, currentIndex, VEQ_AC2)); break;
                      case 3: toggle.RegisterCallback<ClickEvent>(ev => setThisToggleActiveVEQ(toggle, "EmbodimentTogglePaneAC" + currentJ, currentI, currentIndex, VEQ_AC3)); break;
                      case 4: toggle.RegisterCallback<ClickEvent>(ev => setThisToggleActiveVEQ(toggle, "EmbodimentTogglePaneAC" + currentJ, currentI, currentIndex, VEQ_AC4)); break;
                      default: Debug.Log("There was an unknown currentJ" + currentJ);break;

                  }
                  index++;
              }
            );

            index = 1;
            root.Query("EmbodimentTogglePaneCO"+j + i).Children<Toggle>().ForEach(toggle =>
            {
                var currentIndex = index;
                var currentJ = j;
                switch (currentJ)
                {
                    case 1: toggle.RegisterCallback<ClickEvent>(ev => setThisToggleActiveVEQ(toggle, "EmbodimentTogglePaneCO" + currentJ, currentI, currentIndex, VEQ_CO1)); break;
                    case 2: toggle.RegisterCallback<ClickEvent>(ev => setThisToggleActiveVEQ(toggle, "EmbodimentTogglePaneCO" + currentJ, currentI, currentIndex, VEQ_CO2)); break;
                    case 3: toggle.RegisterCallback<ClickEvent>(ev => setThisToggleActiveVEQ(toggle, "EmbodimentTogglePaneCO" + currentJ, currentI, currentIndex, VEQ_CO3)); break;
                    case 4: toggle.RegisterCallback<ClickEvent>(ev => setThisToggleActiveVEQ(toggle, "EmbodimentTogglePaneCO" + currentJ, currentI, currentIndex, VEQ_CO4)); break;
                    default: Debug.Log("There was an unknown currentJ" + currentJ); break;

                }
                index++;
            }
            );

            index = 1; ;
            root.Query("EmbodimentTogglePaneCH"+j + i).Children<Toggle>().ForEach(toggle =>
            {
                var currentIndex = index;
                var currentJ = j;

                switch (currentJ)
                {
                    case 1: toggle.RegisterCallback<ClickEvent>(ev => setThisToggleActiveVEQ(toggle, "EmbodimentTogglePaneCH" + currentJ, currentI, currentIndex, VEQ_CH1)); break;
                    case 2: toggle.RegisterCallback<ClickEvent>(ev => setThisToggleActiveVEQ(toggle, "EmbodimentTogglePaneCH" + currentJ, currentI, currentIndex, VEQ_CH2)); break;
                    case 3: toggle.RegisterCallback<ClickEvent>(ev => setThisToggleActiveVEQ(toggle, "EmbodimentTogglePaneCH" + currentJ, currentI, currentIndex, VEQ_CH3)); break;
                    case 4: toggle.RegisterCallback<ClickEvent>(ev => setThisToggleActiveVEQ(toggle, "EmbodimentTogglePaneCH" + currentJ, currentI, currentIndex, VEQ_CH4)); break;
                    default: Debug.Log("There was an unknown currentJ" + currentJ); break;

                }
                index++;
            }
            );

        }
        }


        //Set Color picker
        for (int i = 1; i <= numberOfSAM; i++)
        {
            var currentI = i;
            root.Query("ColorTogglePane" + i).Children<Toggle>().ForEach(toggle =>
          {
              toggle.RegisterCallback<ClickEvent>(ev => pickColor(toggle.name, currentI));
          });
        }

        for (int i = 1; i <= numberOfPresence; i++)
        {
            var value = 1;
            root.Query("PresenceTogglePane"+i).Children<Toggle>().ForEach(toggle =>
            {
                var currentIndex = i;
                var currentValue = value;
                toggle.RegisterCallback<ClickEvent>(ev => SetOnlyThisToggleActivePresence(toggle, currentIndex, currentValue));
                value++;
            }
            );

        }

    }

    private void ShowHelpPage()
    {
        var curPage = root.Q("Page" + currentPage);
        curPage.style.display = DisplayStyle.None;
        var newPage = root.Q("PageHelp");
        newPage.style.display = DisplayStyle.Flex;

    }

    private void HideHelpPage()
    {

        var curPage = root.Q("PageHelp");
        curPage.style.display = DisplayStyle.None;
        var newPage = root.Q("Page" + currentPage);
        newPage.style.display = DisplayStyle.Flex;

    }


    private void LoadNextPage() {

 
        var curPage = root.Q("Page" + currentPage);
        currentPage++;
        curPage.style.display = DisplayStyle.None;
        //Play Video first to avoid seeing last image first
        TriggerVideoIfOnPage();
        ChangeAvatarIfOnPage();
        WriteDateOnPage();
        //TODO: Set final page
        if (currentPage == numberOfPages) {
            CreateAndWriteFile();
        }

        var newPage = root.Q("Page" + currentPage);
        newPage.style.display = DisplayStyle.Flex;
        if (currentPage != 1) {
            disableAllBottomPane();
        }

        if (currentPage == 5 || currentPage == 15 || currentPage == 25|| currentPage == 35) {
            enableAllArrow();
            StartCoroutine(EnableBottomPaneAfterSeconds(30));

        }
    }

    private void WriteDateOnPage() {

        dates[currentPage-1] = DateTime.Now.ToString("HH:mm:ss");
    }
    private void ChangeAvatarIfOnPage() {
//For now added in TriggerVideo method
    }

    private void TriggerVideoIfOnPage() {
        if (currentPage == 2)
        {
            videoPlayer1.Play();
            videoPlayer1.loopPointReached += EndReached;
        }

        if (currentPage == 14)
        {
            videoPlayer2.Play();
            videoPlayer2.loopPointReached += EndReached;
            changeAvatarSameGenderFirstSecond();
        }

        if (currentPage == 24)
        {
            videoPlayer3.Play();
            videoPlayer3.loopPointReached += EndReached;
            changeAvatarDifferentGender();
        }

        if (currentPage == 34)
        {
            videoPlayer4.Play();
            videoPlayer4.loopPointReached += EndReached;
            changeAvatarSameGenderThirdFourth();
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

    private void SetOnlyThisToggleActiveSAM(Toggle currentToggle, String currentField, int currentI, int value, int[] toggleFieldName)
    {
        Debug.Log("Current Panel: " + currentI);
        Debug.Log("Current value chosen: " + value);
        root.Query(currentField + currentI + "TogglePane").Children<Toggle>().ForEach(toggle =>
            {
                toggle.value = false;
            }
);
        currentToggle.value = true;
        toggleFieldName[currentI - 1] = value;

        if ((valence[currentI - 1] != -1) && (arousal[currentI - 1] != -1) && (dominance[currentI - 1] != -1))
        {

            enableAllBottomPane();
        }

    }


    private void SetOnlyThisToggleActivePresence(Toggle currentToggle, int currentI, int value)
    {
        Debug.Log("PresenceTogglePane" + currentI);
        Debug.Log("Current value chosen: " + value);
        root.Query("PresenceTogglePane" + currentI).Children<Toggle>().ForEach(toggle =>
        {
            toggle.value = false;
        }
);
        currentToggle.value = true;
        presenceResult[currentI - 1] = value;

        for (int i = 1; i <= 6; i++) {
            if (currentI == 1*i || currentI == 2*i || currentI == 3*i)
            {
                if ((presenceResult[1 * i - 1] != -1) && (presenceResult[2 * i - 1] != -1) && (presenceResult[3 * i - 1] != -1))
                {
                    enableAllBottomPane();
                }
            }
        }

        if (currentI == 19)
        {
                enableAllBottomPane();
        }


    }


    private void setThisToggleActiveVEQ(Toggle currentToggle, String currentField, int currentI, int index, int[] toggleFieldName) {
        Debug.Log("Current Index: "+index+" chosen for "+ currentField + " : " + currentI);
        root.Query(currentField + currentI).Children<Toggle>().ForEach(toggle =>
        {
            toggle.value = false;
        }
);
        currentToggle.value = true;
        toggleFieldName[currentI - 1] = index;

        if (currentI == 1 || currentI == 2)
        {
            if ((toggleFieldName[0] != -1) && (toggleFieldName[1] != -1))
            {
                Debug.Log(toggleFieldName[0] +" "+ toggleFieldName[1]);
                enableAllBottomPane();
            }
        }

        if (currentI == 3 || currentI == 4)
        {
            if ((toggleFieldName[2] != -1) && (toggleFieldName[3] != -1))
            {
                Debug.Log(toggleFieldName[2] + " " + toggleFieldName[3]);
                enableAllBottomPane();
            }
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

    /*
    Method to instantiate a given array with a certain value. 
    For now used to have starting values of newly created arrays with -1
    */
    public void Populate(int [] arr, int value)
    {
        for (int i = 0; i < arr.Length; i++)
        {
            arr[i] = value;
        }
    }

    public void CreateAndWriteFile() {
        DateTime localDate = DateTime.Now;
        string filePath = path+ DateTime.Now.ToString("yyyyMMdd_hhmmss") + ".txt";
        if (!File.Exists(filePath))
        {
            Debug.Log("Writing file " +filePath );
            // Create a file to write to.
            using (StreamWriter sw = File.CreateText(filePath))
            {
                //writeForArray(VEQ_AC, sw, "VEQ_AC");
                sw.WriteLine(ResultToCSV());

            }
        }

    }

    public void writeForArray(int[] array, StreamWriter writer,String arrayName) {
        for (int i = 0; i < array.Length; i++){
            writer.WriteLine(arrayName + i + " has value: " + array[i]);
        }

    }

    public void appendToCSVFileFromInt(int[] array, StringBuilder builder)
    {
        for (int i = 0; i < array.Length; i++)
        {
            builder.Append(',').Append(array[i]);
        }

    }

    public void appendToCSVFileFromString(String[] array, StringBuilder builder)
    {
        for (int i = 0; i < array.Length; i++)
        {
            builder.Append(',').Append(array[i]);
        }

    }

    public void appendToCSVFileFromDates(DateTime[] array, StringBuilder builder)
    {
        for (int i = 0; i < array.Length; i++)
        {
            builder.Append(',').Append(array[i]);
        }

    }

    public string ResultToCSV()
    {

        String tableHeader = "Test";
        tableHeader += ",AROUSAL1,AROUSAL2,AROUSAL3,AROUSAL4,AROUSAL5";
        tableHeader += ",DOMINANCE1,DOMINANCE2,DOMINANCE3,DOMINANCE4,DOMINANCE5";
        tableHeader += ",VALENCE1,VALENCE2,VALENCE3,VALENCE4,VALENCE5";
        tableHeader += ",COLOR1,COLOR2,COLOR3,COLOR4,COLOR5";
        tableHeader += ",VEQ_AC11,VEQ_AC12,VEQ_AC13,VEQ_AC14";
        tableHeader += ",VEQ_CO11,VEQ_CO12,VEQ_CO13,VEQ_CO14";
        tableHeader += ",VEQ_CH11,VEQ_CH12,VEQ_CH13,VEQ_CH14";
        tableHeader += ",VEQ_AC21,VEQ_AC22,VEQ_AC23,VEQ_AC24";
        tableHeader += ",VEQ_CO21,VEQ_CO22,VEQ_CO23,VEQ_CO24";
        tableHeader += ",VEQ_CH21,VEQ_CH22,VEQ_CH23,VEQ_CH24";
        tableHeader += ",VEQ_AC31,VEQ_AC32,VEQ_AC33,VEQ_AC34";
        tableHeader += ",VEQ_CO31,VEQ_CO32,VEQ_CO33,VEQ_CO34";
        tableHeader += ",VEQ_CH31,VEQ_CH32,VEQ_CH33,VEQ_CH34";
        tableHeader += ",VEQ_AC41,VEQ_AC42,VEQ_AC43,VEQ_AC44";
        tableHeader += ",VEQ_CO41,VEQ_CO42,VEQ_CO43,VEQ_CO44";
        tableHeader += ",VEQ_CH41,VEQ_CH42,VEQ_CH43,VEQ_CH44";
        for (int i = 1; i <= numberOfPresence; i++)
        {
            tableHeader += ",Presence" + i;
        }
        for (int i = 1; i <= numberOfPages; i++) {
            tableHeader += ",Date" + i;
        }



        var sb = new StringBuilder(tableHeader);
        sb.Append("\n").Append("Test");
        appendToCSVFileFromInt(arousal, sb);
        appendToCSVFileFromInt(dominance, sb);
        appendToCSVFileFromInt(valence, sb);
        appendToCSVFileFromString(colorsPicked, sb);
        appendToCSVFileFromInt(VEQ_AC1, sb);
        appendToCSVFileFromInt(VEQ_CO2, sb);
        appendToCSVFileFromInt(VEQ_CH1, sb);
        appendToCSVFileFromInt(VEQ_AC2, sb);
        appendToCSVFileFromInt(VEQ_CO2, sb);
        appendToCSVFileFromInt(VEQ_CH2, sb);
        appendToCSVFileFromInt(VEQ_AC3, sb);
        appendToCSVFileFromInt(VEQ_CO3, sb);
        appendToCSVFileFromInt(VEQ_CH3, sb);
        appendToCSVFileFromInt(VEQ_AC4, sb);
        appendToCSVFileFromInt(VEQ_CO4, sb);
        appendToCSVFileFromInt(VEQ_CH4, sb);
        appendToCSVFileFromInt(presenceResult, sb);
        appendToCSVFileFromString(dates, sb);

        return sb.ToString();
    }

    private void pickColor(String colorPickedFrom, int currentIndex) {
        colorsPicked[currentIndex-1] = colorPickedFrom;
        root.Query("ColorTogglePane"+currentIndex).Children<Toggle>().ForEach(toggle =>
        {
            toggle.value = false;
            if (toggle.name.Equals(colorPickedFrom)) {
                toggle.value = true;
            }
        });
        enableAllBottomPane();

    }


    public void enableAllArrow() {
        arrow1.SetActive(true);
        arrow2.SetActive(true);
        arrow3.SetActive(true);
    }
 
 IEnumerator EnableBottomPaneAfterSeconds(int secs)
 {
     yield return new WaitForSeconds(secs);
        arrow1.GetComponent<ArrowAnimation>().ResetPosition();
        arrow1.SetActive(false);
        arrow2.GetComponent<ArrowAnimation>().ResetPosition();
        arrow2.SetActive(false);
        arrow3.GetComponent<ArrowAnimation>().ResetPosition();
        arrow3.SetActive(false);
        enableAllBottomPane();
 }
}
