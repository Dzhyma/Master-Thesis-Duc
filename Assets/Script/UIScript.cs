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
    public int numberOfPages = 2;
    public UQueryBuilder<VisualElement> test;
    public int[] valence;
    public int[] arousal;
    public int[] dominance;

    private void OnEnable()
    {

        valence = new int[numberOfPages];
        arousal = new int[numberOfPages];
        dominance = new int[numberOfPages];
        root = GetComponent<UIDocument>().rootVisualElement;
        //var index = 0;

        button_next = root.Q<Button>("NextButton");
        button_next.RegisterCallback<ClickEvent>(ev => loadNextPage());

        button_minimize = root.Q<Button>("MinimizeButton");
        button_minimize.RegisterCallback<ClickEvent>(ev => toggleMainPane());

        mainPane = root.Q("MainPane");
        /*
        root.Query().Children<Toggle>().ForEach(toggle => {
            var name = toggle.name;
            if (name.EndsWith("Toggle")) {
                Debug.Log(toggle.name);
                //Get the part before the number
                Regex re = new Regex(@"([a-zA-Z]+)(\d+)");
                Match result = re.Match(name);

                string identifier = result.Groups[1].Value;
                //string numberPart = result.Groups[2].Value;

               var currentPage = Regex.Match(name, @"\d+").Value;
               Debug.Log(identifier);
               Debug.Log(currentPage);

                switch (identifier) {
                    case "Arousal": toggle.RegisterCallback<ClickEvent>(ev => setOnlyThisToggleActive(toggle, Int32.Parse(currentPage), 0));
                }

            }



            if(toggle.name == "Dominance1Toggle"|| toggle.name == "Valence1Toggle" || toggle.name == "Arousal1Toggle") { 
            toggle.RegisterCallback<ClickEvent>(ev =>
            {

                   Debug.Log("testmich"+toggle.name);
            });
            }
            //toggle.RegisterCallback<ClickEvent>(ev => setOnlyThisToggleActive(toggle, index, 0));
        }) ;
        */
        for (int i = 1; i <= numberOfPages; i++) {
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


    //Bug regarding property?
    private void loadNextPage() {

        //var root = GetComponent<UIDocument>().rootVisualElement;
        var curPage = root.Q("Page" + currentPage);
        currentPage++;
        curPage.style.display = DisplayStyle.None;
        //toggleShowElement(curPage);
        var newPage = root.Q("Page" + currentPage);
        newPage.style.display = DisplayStyle.Flex;
        //toggleShowElement(newPage);
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
   
    private void setOnlyThisToggleActive(Toggle currentToggle, int index, int valueToSaveTo) {
        var root = GetComponent<UIDocument>().rootVisualElement;
        root.Query("Arousal1Toggle").Children<Toggle>().ForEach(toggle =>
        {
            toggle.value = false;
        }
        );
        currentToggle.value = true;
        //valueToSaveTo = index;
        Debug.Log("Selected Index " + index);
    
    }

 

    private void setOnlyThisToggleActive2(Toggle currentToggle,String currentField, int pageNumber, int index, int[] toggleFieldName)
    {
        Debug.Log(pageNumber);
        root.Query(currentField+pageNumber+"TogglePane").Children<Toggle>().ForEach(toggle =>
        {
            toggle.value = false;
        }
);
        currentToggle.value = true;
        toggleFieldName[pageNumber-1] = index;
    }
}
