using System;
using UnityEngine;


public class WebImageTest : MonoBehaviour
{
    private BitTextArea consoleTextarea;
    private BitWebImage pictureWebimage;
    private BitHorizontalProgressBar pictureHorizontalprogressbar;

    public void Start()
    {
        Component[] windows = gameObject.GetComponents(typeof(BitWindow));
        BitWindow window = null;

        for (int i = 0; i < windows.Length; i++)
        {
            if (windows[i].name == "webimagetest_window")
            {
                window = (BitWindow)windows[i];
                break;
            }
        }

        if (window == null)
        {
            Debug.LogError("Main window not found.");
            return;
        }

        BitButton loadButton = window.FindControl<BitButton>("load_button");
        BitTextField urlTextfield = window.FindControl<BitTextField>("url_textfield");
        pictureWebimage = window.FindControl<BitWebImage>("picture_webimage");
        consoleTextarea = window.FindControl<BitTextArea>("console_textarea");
        pictureHorizontalprogressbar = window.FindControl<BitHorizontalProgressBar>("picture_horizontalprogressbar");

        loadButton.MouseClick +=
            delegate
            {
                string url = urlTextfield.Text;
                url = url.Substring(url.LastIndexOf('/') + 1);

                BitWebImage.LoadImageResponse result = pictureWebimage.LoadImage(urlTextfield.Text);
                switch (result)
                {
                    case BitWebImage.LoadImageResponse.ALREADY_LOADED:
                        consoleTextarea.Text += "Image already loaded... " + url + Environment.NewLine;
                        break;
                    case BitWebImage.LoadImageResponse.OTHER_LOADING:
                        consoleTextarea.Text += "Other loading in progress... " + Environment.NewLine;
                        break;
                    case BitWebImage.LoadImageResponse.OK:
                        consoleTextarea.Text += "Loading image... " + url + Environment.NewLine;
                        pictureHorizontalprogressbar.Value = 0;
                        break;
                }
            };
    }

    public void Update()
    {
        if (pictureWebimage.IsLoading())
        {
            float progress = pictureWebimage.GetProgress();
            pictureHorizontalprogressbar.Value = progress * 100;
        }
        else
        {
            if (pictureHorizontalprogressbar.Value < 100 && pictureWebimage.Image != null)
                pictureHorizontalprogressbar.Value = 100;
        }
    }

}