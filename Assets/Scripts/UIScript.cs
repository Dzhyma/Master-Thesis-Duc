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
    public int currentPage = 1;
    public VisualElement root;
    public int numberOfSAM;
    public UQueryBuilder<VisualElement> test;
    public int[] valence;
    public int[] arousal;
    public int[] dominance;
    public int[] VEQ_AC;
    public int[] VEQ_CO;
    public int[] VEQ_CH;
    public RenderTexture textureVideo1;
    public UnityEngine.Video.VideoPlayer videoPlayer1;

    public UnityEngine.Video.VideoPlayer videoPlayer2;
    public RenderTexture textureVideo2;

    public GameObject firstAvatar;
    public GameObject secondAvatar;


    //Path where to save file to
    private String path = @"C:\Users\Nguyen\Desktop\Master\Result\MyTestFrom";
    public void changeAvatar() {
        Debug.Log("changing avatar");

        String[] bodyPartsToChange = { "Wolf3D_Hair", "EyeLeft","EyeRight", "Wolf3D_Body", "Wolf3D_Head", "Wolf3D_Outfit_Bottom", "Wolf3D_Outfit_Footwear", "Wolf3D_Outfit_Top", "Wolf3D_Teeth" };
        foreach (String body in bodyPartsToChange) {
            firstAvatar.transform.Find(body).GetComponent<SkinnedMeshRenderer>().materials = secondAvatar.transform.Find(body).GetComponent<SkinnedMeshRenderer>().materials;
            firstAvatar.transform.Find(body).GetComponent<SkinnedMeshRenderer>().sharedMesh = secondAvatar.transform.Find(body).GetComponent<SkinnedMeshRenderer>().sharedMesh;
        }
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

        Populate(valence, -1);
        Populate(arousal, -1);
        Populate(dominance, -1);

        VEQ_AC = new int[4];
        VEQ_CO = new int[4];
        VEQ_CH = new int[4];
        Populate(VEQ_AC, -1);
        Populate(VEQ_CO, -1);
        Populate(VEQ_CH, -1);


        root = GetComponent<UIDocument>().rootVisualElement;
        //var index = 0;
        /*
        button_next = root.Q<Button>("NextButton");
        button_next.RegisterCallback<ClickEvent>(ev => loadNextPage());
        */

        button_next = root.Q<Button>("Change");
        button_next.RegisterCallback<ClickEvent>(ev => changeAvatar());

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

        //Set AC from VEQ
        for (int i = 1; i <= 4; i++)
        {
            var index = 0;
            var currentI = i;
            root.Query("EmbodimentTogglePaneAC"+i).Children<Toggle>().ForEach(toggle =>
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

    }


    private void loadNextPage() {
        CreateAndWriteFile();
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
        Debug.Log("Current Index chosen: " + currentI);
        root.Query(currentField+currentI+"TogglePane").Children<Toggle>().ForEach(toggle =>
        {
            toggle.value = false;
        }
);
        currentToggle.value = true;
        toggleFieldName[currentI-1] = index;

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

        if ((toggleFieldName[0] != -1) && (toggleFieldName[1] != -1) && (toggleFieldName[2] != -1) && (toggleFieldName[3] != -1))
        {
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

    public void appendToCSVFile(int[] array, StringBuilder builder)
    {
        for (int i = 0; i < array.Length; i++)
        {
            builder.Append(',').Append(array[i]);
        }

    }

    public string ResultToCSV()
    {

        String tableHeader = "Test";
        tableHeader += ",VEQ_AC1,VEQ_AC2,VEQ_AC3,VEQ_AC4";
        tableHeader += ",VEQ_CO1,VEQ_CO2,VEQ_CO3,VEQ_CO4";
        tableHeader += ",VEQ_CH1,VEQ_CH2,VEQ_CH3,VEQ_CH4";



        var sb = new StringBuilder(tableHeader);
        sb.Append("\n").Append("Test");
        appendToCSVFile(VEQ_AC, sb);
        appendToCSVFile(VEQ_CO, sb);
        appendToCSVFile(VEQ_CH, sb);
        return sb.ToString();
    }
}
