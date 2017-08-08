using UnityEngine;
using UnityEngine.UI;

public class Photographer : MonoBehaviour
{
    public Texture2D photoTexture2d;
    public RawImage rawimage;
    public WebCamTexture webcamTexture;
    public Button btnOverride;
    public Button btnCapture;


    void Start()
    {
        this.btnCapture.onClick.AddListener(() => TakePhoto());
        this.btnOverride.onClick.AddListener(() => OverrideImage());

        this.webcamTexture = new WebCamTexture();

        var filename = Application.persistentDataPath + "/my_photo.png";
        var bytes = System.IO.File.ReadAllBytes(filename);
        if (bytes != null)
        {
            var texture = new Texture2D(800, 800);
            texture.LoadImage(bytes);
            this.rawimage.material.mainTexture = texture;

            this.btnOverride.interactable = true;
            this.btnCapture.interactable = false;
        }
    }

    public void OverrideImage()
    {
        this.btnOverride.interactable = false;
        this.btnCapture.interactable = true;

        this.rawimage.texture = this.webcamTexture;
        this.rawimage.material.mainTexture = this.webcamTexture;
        this.webcamTexture.Play();
    }

    public void TakePhoto()
    {
        this.btnOverride.interactable = true;
        this.btnCapture.interactable = false;

        this.photoTexture2d = new Texture2D(rawimage.texture.width, rawimage.texture.height);
        this.photoTexture2d.SetPixels(this.webcamTexture.GetPixels());
        this.photoTexture2d.Apply();

        this.webcamTexture.Pause();

        this.rawimage.texture = this.photoTexture2d;

        System.IO.File.WriteAllBytes(Application.persistentDataPath + "/my_photo.png", this.photoTexture2d.EncodeToPNG());
    }
}
