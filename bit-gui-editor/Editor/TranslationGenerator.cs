using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;


public class GuiTranslationGenerator
{
    public static string ClassSuffix = "GUITranslation";
    private static List<string> _strings = new List<string>();

    private static string labelusage = "DefaultTranslation.Usage.Label";
    private static string buttonusage = "DefaultTranslation.Usage.Button";


    [MenuItem("Tools/GUI/Generate Translation Table from Selection")]
    private static void GenerateTranslation()
    {
        Object[] selections = Selection.GetFiltered(typeof(Object), SelectionMode.TopLevel);

        if(selections.Length > 1)
        {
            Debug.LogError("Multiple selection not supported");
            return;
        }

        if(!(selections[0] is GameObject))
        {
            Debug.LogError("Selection is of type " + selections[0].GetType() + " but expected GameObject");
            return;
        }

        GameObject go = (GameObject)selections[0];

        //Make "hej_hopp" etc into HejHopp

        string classname = PrettifyandCorrect(go.name); // +"_" + ClassSuffix;
        Debug.Log("Class name: " + classname);

        StringBuilder main = new StringBuilder();

        main.AppendLine("[TranslationSource]");
        main.AppendLine("public static class " + classname);
        main.AppendLine("{");

        //Will traverse each component resursively
        Component[] comps = go.GetComponents<BitControl>();
        Debug.Log("Gameobject nr components:" + comps.Length);

        for (int i = 0; i < comps.Length; i++)
            GenTSection((BitControl) comps[i], main);

        main.AppendLine("}");

        string path = EditorUtility.SaveFilePanel("Save Generated Gui Acessor", "", classname + ".cs", "cs");

        if (path.CompareTo("") == 0)
        {
            Debug.Log("Translation generation cancelled");
            return;
        }

        StringBuilder intro = new StringBuilder();

        intro.AppendLine("using System;");
        intro.AppendLine();

        intro.AppendLine("// Auto generated translation file, use GUI tool to generate");
        intro.AppendLine("// Let's Do This For Object: " + go.name);
        intro.AppendLine();

        TextWriter tw = new StreamWriter(path);

        tw.Write(intro.ToString());
        tw.Write(main.ToString());
        tw.Close();
    }

    private static void GenTSection(BitControl control, StringBuilder builder)
    {
        if (control == null)
        {
            Debug.LogError("Control was null");
            return;
        }

        if (control is BitLabel)
            GenTRow(control, labelusage, builder);
        else if (control is BitButton)
            GenTRow(control, buttonusage, builder);
        else if (control is BitContainer)
        {
            if (control is BitWindow)
                GenTRow(control, "DefaultTranslation.Usage.WindowTitle", builder);
            else if (control is BitList)
                GenTRow(control, "DefaultTranslation.Usage.ListTitle", builder);

            BitContainer cont = (BitContainer) control;

            for (int i = 0; i < cont.ControlCount; i++)
                GenTSection(cont.GetControlAt(i), builder);

            return;
        }
        else
        {
            GenTRow(control, "", builder);
        }
            
        
    }

    private static void GenTRow(BitControl control, string type, StringBuilder builder)
    {
        if(control.Content == null || control.Content.text.CompareTo("") == 0)
            return;

        if(IsNumeric(control.Content.text))
        {
            Debug.Log("BitControl content is numeric, skipping: " + control.name);
            return;
        }

        if(isNonLetterChar(control.Content.text))
        {
            Debug.Log("BitControl content is a non-letter char, skipping: " + control.name);
            return;
        }

        builder.Append("\t[DefaultTranslation(\"" + control.Content.text + "\"");
        builder.AppendLine(type.CompareTo("") == 0 ? "]" : (", " + type + ")]"));
        builder.AppendLine("\tpublic static readonly Guid " + CreateVarString(control) + " = new Guid(\"" + System.Guid.NewGuid() + "\");");
        builder.AppendLine();
    }

    public static bool isNonLetterChar(string s)
    {
        s.Trim();

        return s.Length == 1 && !char.IsLetter(s[0]);
    }

    public static bool IsNumeric(string s)
    {
        s.Trim();

        foreach(char c in s)
        {
            if (!char.IsDigit(c) && c != ',' && c != '.')
                return false;
        }

        return true;
    }

    private static string PrettifyandCorrect(string ugly)
    {
        string[] parts = ugly.Split('_');
        string result = "";

        foreach(string s in parts)
            if(s.Length > 0)
                result += char.ToUpper(s[0]) + s.Substring(1, s.Length - 1).ToLower();

        //If result starts with a number prefix it with a letter and send a warning
        if(char.IsDigit(result[0]))
        {
            Debug.LogWarning("BitControl name starting with digit, prefixing with 'n': " + result);
            result = "n" + result;
        }

        return result;
    }

    private static string CreateVarString(BitControl control)
    {
        string str = PrettifyandCorrect(control.name);
        string candidate = str;
        int i = 2;

        if(_strings.Contains(candidate))
            Debug.LogWarning("BitControl name collision: " + control.name);

        while (_strings.Contains(candidate))
            candidate = str + i++;

        _strings.Add(candidate);
        return candidate;
    }
}
