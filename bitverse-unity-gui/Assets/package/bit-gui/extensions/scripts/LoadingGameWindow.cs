using UnityEngine;

[RequireComponent(typeof(BitStage))]
public class LoadingGameWindow : MonoBehaviour
{
    public BitWindow LoadGameWindow;

    public void Start()
    {
    }

    public void DestroyLoadGameWindow()
    {
        if (LoadGameWindow != null)
        {
            LoadGameWindow.QueueDestroy(DestroyAll);
        }
    }

    private void DestroyAll(int x)
    {
        Destroy(this);
    }

}
