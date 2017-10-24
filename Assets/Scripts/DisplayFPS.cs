using UnityEngine;

public class DisplayFPS : MonoBehaviour
{
    void OnGUI()
    {
        float mspf = Time.deltaTime * 1000.0f;
        float fps = 1.0f / Time.deltaTime;

        string text = string.Format("{0:0.0} mspf {1:0} fps", mspf, fps);

        GUI.Label(new Rect(25, 25, 100, 30), text);
    }
}