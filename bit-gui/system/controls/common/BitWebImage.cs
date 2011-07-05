using UnityEngine;


public class BitWebImage : BitPicture
{
    public Texture2D LoadingImage;

    public string URL;

    private bool isLoadingImage;
    private WWW www;

    public enum LoadImageResponse
    {
        ALREADY_LOADED,
        OTHER_LOADING,
        OK
    }

    protected override void DoDraw()
    {
        if (Image == null)
        {
            if (!string.IsNullOrEmpty(URL))
            {
                string url = URL;
                URL = null;
                LoadImage(url);
            }
        }
        else if (!isLoadingImage && URL != Text)
        {
            Image = null;
        }

        //loading control
        if (isLoadingImage)
        {
            if (www.isDone)
            {
                if (Image == null)
                    Image = new Texture2D((int)Position.width, (int)Position.height, TextureFormat.DXT1, false);
                www.LoadImageIntoTexture((Texture2D)Image);
                isLoadingImage = false;
                Text = URL = www.url;
                www = null;
            }
        }

        base.DoDraw();
    }

    public LoadImageResponse LoadImage(string url)
    {
        //already loaded
        if (URL == url)
        {
            return LoadImageResponse.ALREADY_LOADED;
        }

        //already loading an image
        if (isLoadingImage)
        {
            return LoadImageResponse.OTHER_LOADING;
        }

        //Loading image here, if needed
        //Image = new Texture2D((int)Position.width, (int)Position.height, TextureFormat.DXT1, false);

        www = new WWW(url);
        isLoadingImage = true;

        return LoadImageResponse.OK;
    }

    public float GetProgress()
    {
        if (www == null)
            return 0;

        return www.progress;
    }

    public bool IsLoading()
    {
        return isLoadingImage;
    }
}