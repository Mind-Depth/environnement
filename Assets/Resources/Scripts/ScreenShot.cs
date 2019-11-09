using System;
using UnityEngine;

public class ScreenShot : MonoBehaviour
{
    public static byte[] Capture(Camera cam)
    {
        // Prepare buffers
        int size = cam.pixelWidth;
        RenderTexture renderTexture = new RenderTexture(size, size, 24);
        Texture2D texture = new Texture2D(size, size);

        // Backup settings
        RenderTexture active = RenderTexture.active;
        RenderTexture target = cam.targetTexture;

        // Overwrite settings
        RenderTexture.active = renderTexture;
        cam.targetTexture = renderTexture;

        // Take Screenshot
        cam.Render();
        texture.ReadPixels(new Rect(0, 0, size, size), 0, 0);

        // Restore settings
        RenderTexture.active = active;
        cam.targetTexture = target;

        return texture.EncodeToPNG();
    }
}