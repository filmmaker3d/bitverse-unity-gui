using System.Collections.Generic;
using Bitverse.Unity.Gui;
using UnityEngine;


public class CalculatorDemo : MonoBehaviour
{
    private enum Operations
    {
        None,
        Plus,
        Minus,
        Multiplication,
        Division
    }


    private const string ClearValue = "0.";
    private const int MaxLenght = 12;

    private BitLabel _result;
    private BitGroup _group;
    private bool _isFloat;

    private Operations _operation = Operations.None;
    private double _leftValue;
    private double _rightValue;
    private bool _resetOnNextNumber;
    private bool _newRightValue = true;
    private BitLabel _resultLabel;
    private Dictionary<BitButton, int> _tag;

    public void Start()
    {
        _tag = new Dictionary<BitButton, int>();

        Component[] windows = gameObject.GetComponents(typeof(BitWindow));
        BitWindow window = null;

        for (int i = 0; i < windows.Length; i++)
        {
            if (windows[i].name == "calculator_window")
            {
                window = (BitWindow)windows[i];
            }
        }

        if (window == null)
        {
            Debug.LogError("Main window not found.");
            return;
        }

        _result = window.FindControl<BitLabel>("Result");
        _resultLabel = window.FindControl<BitLabel>("ResultLabel");
        _group = window.FindControl<BitGroup>("Group");

        BitButton b;
        for (int i = 0; i < 10; i++)
        {
            b = _group.FindControl<BitButton>(i.ToString());

            _tag.Add(b, i);
            b.MouseClick += NumericClick;
        }
        b = _group.FindControl<BitButton>(".");
        b.MouseClick += DotClick;

        b = _group.FindControl<BitButton>("+");
        _tag.Add(b, (int)Operations.Plus);
        b.MouseClick += OperationClick;

        b = _group.FindControl<BitButton>("-");
        _tag.Add(b, (int)Operations.Minus);
        b.MouseClick += OperationClick;

        b = _group.FindControl<BitButton>("*");
        //b.Tag = Operations.Multiplication;
        _tag.Add(b, (int)Operations.Multiplication);
        b.MouseClick += OperationClick;

        b = _group.FindControl<BitButton>("/");
        _tag.Add(b, (int)Operations.Division);
        b.MouseClick += OperationClick;

        b = _group.FindControl<BitButton>("=");
        b.MouseClick += EqualsClick;

        b = _group.FindControl<BitButton>("C");
        b.MouseClick += ClearClick;

        b = _group.FindControl<BitButton>("Bk");
        b.MouseClick += BackClick;

        Reset();
    }

    private void NumericClick(object sender, MouseEventArgs e)
    {
        AppendText(_tag[(BitButton)sender].ToString());
    }

    private void AppendText(string number)
    {
        if (_resetOnNextNumber)
        {
            ClearResult();
            _resetOnNextNumber = false;
        }
        if (_result.Content.text.Length + 1 == MaxLenght)
        {
            return;
        }
        if (_result.Content.text == ClearValue)
        {
            if (number != "0")
            {
                if (_isFloat)
                {
                    _result.Content.text += number;
                }
                else
                {
                    _result.Content.text = number + ".";
                }
            }
            return;
        }

        string text = _result.Content.text;
        if (!_isFloat)
        {
            text = text.Substring(0, text.Length - 1) + number + ".";
        }
        else
        {
            text += number;
        }
        _result.Content.text = text;
    }

    private void DotClick(object sender, MouseEventArgs e)
    {
        SetFloat();
    }

    private void SetFloat()
    {
        _isFloat = true;
    }

    private void OperationClick(object sender, MouseEventArgs e)
    {
        SetOperator((Operations)_tag[(BitButton)sender]);
    }

    private void SetOperator(Operations operation)
    {
        _leftValue = double.Parse(_result.Content.text);
        _operation = operation;
        _resetOnNextNumber = true;
        _newRightValue = true;
    }

    private void EqualsClick(object sender, MouseEventArgs e)
    {
        CalculateResult();
    }

    public string PreviewResult()
    {
        double right = _rightValue;

        if (_newRightValue)
        {
            right = double.Parse(_result.Content.text);
        }

        double result;

        switch (_operation)
        {
            case Operations.Plus:
                result = _leftValue + right;
                break;
            case Operations.Minus:
                result = _leftValue - right;
                break;
            case Operations.Multiplication:
                result = _leftValue * right;
                break;
            case Operations.Division:
                result = _leftValue / right;
                break;
            default:
                return "ERROR";
        }

        string s = string.Format("{0:F2}", result);
        if (!s.Contains("."))
        {
            s += ".";
        }

        return s;
    }

    private void CalculateResult()
    {
        if (_newRightValue)
        {
            _rightValue = double.Parse(_result.Content.text);
            _newRightValue = false;
        }
        double result;
        switch (_operation)
        {
            case Operations.Plus:
                result = _leftValue + _rightValue;
                break;
            case Operations.Minus:
                result = _leftValue - _rightValue;
                break;
            case Operations.Multiplication:
                result = _leftValue * _rightValue;
                break;
            case Operations.Division:
                result = _leftValue / _rightValue;
                break;
            default:
                return;
        }

        string s = result.ToString();
        if (!s.Contains("."))
        {
            s += ".";
        }
        _result.Content.text = s;
        _resetOnNextNumber = true;
        _leftValue = result;
        _resultLabel.Visible = true;
    }

    private void ClearClick(object sender, MouseEventArgs e)
    {
        Reset();
    }

    private void ClearResult()
    {
        _resultLabel.Visible = false;
        _result.Content.text = ClearValue;
        _resetOnNextNumber = false;
        _isFloat = false;
    }

    private void Reset()
    {
        ClearResult();
        _operation = Operations.None;
        _leftValue = 0;
        _newRightValue = true;
    }

    private void BackClick(object sender, MouseEventArgs e)
    {
        RemoveLast();
    }

    private void RemoveLast()
    {
        if (_resetOnNextNumber)
        {
            return;
        }
        string text = _result.Content.text;
        if (text == ClearValue)
        {
            return;
        }

        if (text.Length == ClearValue.Length)
        {
            _result.Content.text = ClearValue;
            return;
        }

        if (text.EndsWith("."))
        {
            if (_isFloat)
            {
                _isFloat = false;
                return;
            }
            _result.Content.text = text.Substring(0, text.Length - 2) + ".";
            return;
        }

        _result.Content.text = text.Substring(0, text.Length - 1);
    }

    public void Update()
    {
        if (Input.GetKeyUp(KeyCode.Alpha0) || Input.GetKeyUp(KeyCode.Keypad0))
        {
            AppendText("0");
        }
        if (Input.GetKeyUp(KeyCode.Alpha1) || Input.GetKeyUp(KeyCode.Keypad1))
        {
            AppendText("1");
        }
        if (Input.GetKeyUp(KeyCode.Alpha2) || Input.GetKeyUp(KeyCode.Keypad2))
        {
            AppendText("2");
        }
        if (Input.GetKeyUp(KeyCode.Alpha3) || Input.GetKeyUp(KeyCode.Keypad3))
        {
            AppendText("3");
        }
        if (Input.GetKeyUp(KeyCode.Alpha4) || Input.GetKeyUp(KeyCode.Keypad4))
        {
            AppendText("4");
        }
        if (Input.GetKeyUp(KeyCode.Alpha5) || Input.GetKeyUp(KeyCode.Keypad5))
        {
            AppendText("5");
        }
        if (Input.GetKeyUp(KeyCode.Alpha6) || Input.GetKeyUp(KeyCode.Keypad6))
        {
            AppendText("6");
        }
        if (Input.GetKeyUp(KeyCode.Alpha7) || Input.GetKeyUp(KeyCode.Keypad7))
        {
            AppendText("7");
        }
        if (Input.GetKeyUp(KeyCode.Alpha8) || Input.GetKeyUp(KeyCode.Keypad8))
        {
            AppendText("8");
        }
        if (Input.GetKeyUp(KeyCode.Alpha9) || Input.GetKeyUp(KeyCode.Keypad9))
        {
            AppendText("9");
        }
        if (Input.GetKeyUp(KeyCode.Plus) || Input.GetKeyUp(KeyCode.KeypadPlus))
        {
            SetOperator(Operations.Plus);
        }
        if (Input.GetKeyUp(KeyCode.Minus) || Input.GetKeyUp(KeyCode.KeypadMinus))
        {
            SetOperator(Operations.Minus);
        }
        if (Input.GetKeyUp("*") || Input.GetKeyUp(KeyCode.KeypadMultiply))
        {
            SetOperator(Operations.Multiplication);
        }
        if (Input.GetKeyUp("/") || Input.GetKeyUp(KeyCode.KeypadDivide))
        {
            SetOperator(Operations.Division);
        }
        if (Input.GetKeyUp(".") || Input.GetKeyUp(KeyCode.KeypadPeriod) || Input.GetKeyUp(","))
        {
            SetFloat();
        }
        if (Input.GetKeyUp("=") || Input.GetKeyUp(KeyCode.KeypadEnter) || Input.GetKeyUp(KeyCode.Return))
        {
            CalculateResult();
        }

        if (Input.GetKeyUp(KeyCode.Delete))
        {
            Reset();
        }
        if (Input.GetKeyUp(KeyCode.Backspace))
        {
            RemoveLast();
        }
    }
}