/* SceneHandler.cs*/
using System;
using UnityEngine;
using Valve.VR.Extras;
using UnityEngine.UIElements;

public class SceneHandler : MonoBehaviour
{
    public SteamVR_LaserPointer_UI laserPointer;
    public PanelSettings TargetPanel;
    private Func<Vector2, Vector2> m_DefaultRenderTextureScreenTranslation;
    public PointerEventArgs pEvent;
    private Vector2 pos;

    void Awake()
    {
        laserPointer.PointerIn += PointerInside;
        laserPointer.PointerOut += PointerOutside;
        laserPointer.PointerClick += PointerClick;
    }

    void OnEnable()
    {
        if (TargetPanel != null)
        {
            if (m_DefaultRenderTextureScreenTranslation == null)
            {
                m_DefaultRenderTextureScreenTranslation = (pos) => ScreenCoordinatesToRenderTexture(pos);
            }

            TargetPanel.SetScreenToPanelSpaceFunction(m_DefaultRenderTextureScreenTranslation);
            Debug.Log("Test");
        }
    }

    void OnDisable()
    {
        //we reset it back to the default behavior
        if (TargetPanel != null)
        {
            TargetPanel.SetScreenToPanelSpaceFunction(null);
        }
        TargetPanel.SetScreenToPanelSpaceFunction(null);
    }

    public void PointerClick(object sender, PointerEventArgs e)
    {
        Debug.Log("Testing Ray");
        pEvent = e;
        Debug.Log(ScreenCoordinatesToRenderTexture(e));
        MouseOperations.MouseEvent(MouseOperations.MouseEventFlags.LeftUp | MouseOperations.MouseEventFlags.LeftDown);
    }

    private Vector2 ScreenCoordinatesToRenderTexture(PointerEventArgs e)
    {
        var invalidPosition = new Vector2(float.NaN, float.NaN);

        var targetTexture = TargetPanel.targetTexture;
        RaycastHit hit;
        if (!Physics.Raycast(e.raycast, out hit))
        {
            return invalidPosition;
        }

        MeshRenderer rend = hit.transform.GetComponent<MeshRenderer>();

        if (rend == null || rend.sharedMaterial.mainTexture != targetTexture)
        {
            return invalidPosition;
        }

        Vector2 pixelUV = hit.textureCoord;

        //since y screen coordinates are usually inverted, we need to flip them
        pixelUV.y = 1 - pixelUV.y;

        pixelUV.x *= targetTexture.width;
        pixelUV.y *= targetTexture.height;
        pos = pixelUV;
        return pixelUV;
    }


    private Vector2 ScreenCoordinatesToRenderTexture(Vector2 position)
    {
        return pos;
    }
    /*
    public void PointerInside(object sender, PointerEventArgs e)
    {
        if (e.target.name == "Cube")
        {
            Debug.Log("Cube was entered");
        }
        else if (e.target.name == "Button")
        {
            Debug.Log("Button was entered");
        }
        else
        {
            Debug.Log(e.target.name);
        }
    }
    */

    public void PointerInside(object sender, PointerEventArgs e)
    {
        /*
        Debug.Log("in Ray");
        pEvent = e;
        Debug.Log(ScreenCoordinatesToRenderTexture(e));
        */
    }

    public void PointerOutside(object sender, PointerEventArgs e)
    {
        /*
        Debug.Log("out Ray");
        pEvent = e;
        Debug.Log(ScreenCoordinatesToRenderTexture(e));
        */
    }
}