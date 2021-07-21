using System;
using UnityEngine;
using UnityEngine.UIElements;
using System.IO;
using System.Text;

public class UIScript : MonoBehaviour
{
    private Button button_next;
    private Button button_minimize;
    private VisualElement mainPane;
    public int currentPage = 19;
    public VisualElement root;
    public int numberOfSAM = 5;
    public UQueryBuilder<VisualElement> test;
    public int[] arousal;
    public int[] dominance;
    public int[] valence;

    public int[] VEQ_AC;
    public int[] VEQ_CO;
    public int[] VEQ_CH;
    public String[] colorsPicked;
    public DateTime[] dates;


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
    private String path = @"C:\Users\Nguyen\Desktop\Master\Result\MyTestFrom";

    public String colorPicked;
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
        fourthAvatar.SetActive(true);
    }

    //Change von Female to Male
    public void changeAvatarDifferentGender()
    {
        secondAvatar.SetActive(false);
        femaleChair.SetActive(false);
        maleChair.SetActive(true);
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

        VEQ_AC = new int[4];
        VEQ_CO = new int[4];
        VEQ_CH = new int[4];
        Populate(VEQ_AC, -1);
        Populate(VEQ_CO, -1);
        Populate(VEQ_CH, -1);

        dates = new DateTime[6];
        dates[0] = DateTime.Now;


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
        for (int i = 1; i <= numberOfSAM; i++) {
            var value = 0;
            root.Query("Arousal" + i + "TogglePane").Children<Toggle>().ForEach(toggle =>
             {
                 var currentIndex = i;
                 var currentValue = value;
                 toggle.RegisterCallback<ClickEvent>(ev => setOnlyThisToggleActive2(toggle, "Arousal", currentIndex, currentValue, arousal));
                 value++;
             }
            );

            value = 0; ;
            root.Query("Dominance" + i + "TogglePane").Children<Toggle>().ForEach(toggle =>
            {
                var currentIndex = i;
                var currentValue = value;
                toggle.RegisterCallback<ClickEvent>(ev => setOnlyThisToggleActive2(toggle, "Dominance", currentIndex, currentValue, dominance));
                value++;
            }
            );


            value = 0;
            root.Query("Valence" + i + "TogglePane").Children<Toggle>().ForEach(toggle =>
            {
                var currentIndex = i;
                var currentValue = value;
                toggle.RegisterCallback<ClickEvent>(ev => setOnlyThisToggleActive2(toggle, "Valence", currentIndex, currentValue, valence));
                value++;
            }
            );


        }

        //Set AC from VEQ
        for (int i = 1; i <= 4; i++)
        {
            var index = 0;
            var currentI = i;
            root.Query("EmbodimentTogglePaneAC" + i).Children<Toggle>().ForEach(toggle =>
              {
                  var currentIndex = index;
                  toggle.RegisterCallback<ClickEvent>(ev => setThisToggleActiveVEQ(toggle, "EmbodimentTogglePaneAC", currentI, currentIndex, VEQ_AC));
                  index++;
              }
            );

            index = 0;
            root.Query("EmbodimentTogglePaneCO" + i).Children<Toggle>().ForEach(toggle =>
            {
                var currentIndex = index;
                toggle.RegisterCallback<ClickEvent>(ev => setThisToggleActiveVEQ(toggle, "EmbodimentTogglePaneCO", currentI, currentIndex, VEQ_CO));
                index++;
            }
            );

            index = 0; ;
            root.Query("EmbodimentTogglePaneCH" + i).Children<Toggle>().ForEach(toggle =>
            {
                var currentIndex = index;
                toggle.RegisterCallback<ClickEvent>(ev => setThisToggleActiveVEQ(toggle, "EmbodimentTogglePaneCH", currentI, currentIndex, VEQ_CH));
                index++;
            }
            );

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
        if (currentPage == 26) {
            CreateAndWriteFile();
        }

        var newPage = root.Q("Page" + currentPage);
        newPage.style.display = DisplayStyle.Flex;
        ChangeAvatarIfOnPage();
        disableAllBottomPane();
        if (currentPage == 1 || currentPage == 5 || currentPage == 9 || currentPage == 13 || currentPage == 17) {
            enableAllBottomPane();

        }
    }

    private void WriteDateOnPage() {
        if (currentPage == 3)
        {
            dates[1] = DateTime.Now;
        }

        if (currentPage == 9)
        {
            dates[2] = DateTime.Now;
        }

        if (currentPage == 13)
        {
            dates[3] = DateTime.Now;
        }

        if (currentPage == 17)
        {
            dates[4] = DateTime.Now;
        }


        if (currentPage == 20)
        {
            dates[5] = DateTime.Now;
        }

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

        if (currentPage == 8)
        {
            videoPlayer2.Play();
            videoPlayer2.loopPointReached += EndReached;
            changeAvatarSameGenderFirstSecond();
        }

        if (currentPage == 12)
        {
            videoPlayer3.Play();
            videoPlayer3.loopPointReached += EndReached;
            changeAvatarDifferentGender();
        }

        if (currentPage == 16)
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

    private void setOnlyThisToggleActive2(Toggle currentToggle, String currentField, int currentI, int value, int[] toggleFieldName)
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

    private void setThisToggleActiveVEQ(Toggle currentToggle, String currentField, int currentI, int index, int[] toggleFieldName) {
        Debug.Log("Current Index chosen: " + currentI);
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
                enableAllBottomPane();
            }
        }

        if (currentI == 3 || currentI == 4)
        {
            if ((toggleFieldName[2] != -1) && (toggleFieldName[3] != -1))
            {
                enableAllBottomPane();
            }
        }


        /*
        if ((toggleFieldName[0] != -1) && (toggleFieldName[1] != -1) && (toggleFieldName[2] != -1) && (toggleFieldName[3] != -1))
        {
            enableAllBottomPane();
        }*/

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
        tableHeader += ",VEQ_AC1,VEQ_AC2,VEQ_AC3,VEQ_AC4";
        tableHeader += ",VEQ_CO1,VEQ_CO2,VEQ_CO3,VEQ_CO4";
        tableHeader += ",VEQ_CH1,VEQ_CH2,VEQ_CH3,VEQ_CH4";
        tableHeader += ",Date1,Date2,Date3,Date4,Date5,Date6";



        var sb = new StringBuilder(tableHeader);
        sb.Append("\n").Append("Test");
        appendToCSVFileFromInt(arousal, sb);
        appendToCSVFileFromInt(dominance, sb);
        appendToCSVFileFromInt(valence, sb);
        appendToCSVFileFromString(colorsPicked, sb);
        appendToCSVFileFromInt(VEQ_AC, sb);
        appendToCSVFileFromInt(VEQ_CO, sb);
        appendToCSVFileFromInt(VEQ_CH, sb);
        appendToCSVFileFromDates(dates, sb);

        return sb.ToString();
    }

    private void pickColor(String colorPickedFrom, int currentIndex) {
        colorPicked = colorPickedFrom;
        colorsPicked[currentIndex-1] = colorPickedFrom;
        root.Query("ColorTogglePane"+currentIndex).Children<Toggle>().ForEach(toggle =>
        {
            toggle.value = false;
            if (toggle.name.Equals(colorPicked)) {
                toggle.value = true;
            }
        });
        enableAllBottomPane();

    }
}
