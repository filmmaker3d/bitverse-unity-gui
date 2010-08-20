using Bitverse.Unity.Gui;
using UnityEngine;


public class Initializer : MonoBehaviour
{
    public BitWindow DefaultTooltip;


    public void Start()
    {
        // Add one tooltip provider to the TooltipManager and set it as default.
        TooltipManager tooltipManager = (TooltipManager)FindObjectOfType(typeof(TooltipManager));

        if (tooltipManager == null)
        {
            Debug.LogError("Initializer - There must be a TooltipManager on the scene.");
            return;
        }

        if (DefaultTooltip == null)
        {
            Debug.LogError("Initializer - DefaultTooltip is null.");
            return;
        }

        TextTooltipProvider defaultProvider = (TextTooltipProvider)DefaultTooltip.gameObject.AddComponent(typeof(TextTooltipProvider));
        tooltipManager.Providers.Add(defaultProvider);
        tooltipManager.DefaultTooltipProviderName = defaultProvider.ProviderName();

        // Get the calculator window and add a callback to BeforeTooltip (to show a dynamic tooltip).
        GameObject go = GameObject.Find("calculator_window");
        BitWindow calculatorWindow = go.GetComponent<BitWindow>();
        CalculatorDemo calculatorDemo = go.GetComponent<CalculatorDemo>();

        BitButton equalsButton = calculatorWindow.FindControlInChildren<BitButton>("=");
        equalsButton.BeforeTooltip += delegate(object sender, BeforeTooltipEventArgs e)
                                          {
                                              equalsButton.UserProperties["currentResult"] = calculatorDemo.PreviewResult();
                                          };
    }
}
