using System.Collections.Generic;
using UnityEngine;

public class TextAttributeBase
{
	public TextAttributeType Type;
	public string Name;
	public string Value;

	public enum TextAttributeType
	{
		Open,
		Close
	}
	public override string ToString()
	{
		return $"({Type}-{Name}){Value}";
	}
}

public class RichTextData
{
	public string message;
	public List<TextAttributeBase> attributeList;

	public RichTextData(string msg)
	{
		message = msg;
		attributeList = new List<TextAttributeBase>();
	}

	public void AddAttribute(TextAttributeBase att)
	{
		attributeList.Add(att);
	}
	public override string ToString()
	{
		string s = "";
		foreach (var att in attributeList) {
			s += att + ",";
		}
		return $"{message} -> ({s})";
	}
}

public class ColorAttribute : TextAttributeBase
{
	public Color Color;
}

public class FontAttribute : TextAttributeBase
{

}

public class LinkAttribute : TextAttributeBase
{
	public string url;
}


public class BoldAttribute : TextAttributeBase
{

}

public class ItalicsAttribute : TextAttributeBase
{

}
