
using System.Collections.Generic;
using System.Drawing;

public class TextTokenizer
{
	private string _text;
	private int _index;

	public TextTokenizer()
	{

	}

	public List<TextToken> Parse(string text)
	{
		List<TextToken> tokens = new List<TextToken>();
		_text = text;
		int len = _text.Length;
		_index = 0;
		while (_index < len) {
			char c = Peek();
			if (c == '<') {
				string tag = ParseTag();
				tokens.Add(new TextToken(TextToken.TextTokenType.Attribute, tag));
				//UnityEngine.Debug.Log(tag);
			}
			else {
				string txt = ParseText();
				tokens.Add(new TextToken(TextToken.TextTokenType.Message, txt));
				//UnityEngine.Debug.Log(txt);
			}
		}
		return tokens;
	}

	private string ParseText()
	{
		string rlt = "";
		while (_index < _text.Length && Peek() != '<') {
			char c = Consume();
			rlt += c;
		}
		return rlt;
	}

	private string ParseTag()
	{
		string rlt = "";
		char c;
		do {
			c = Consume();
			rlt += c;
		} while (_index < _text.Length && c != '>');
		return rlt;
	}

	private char Peek()
	{
		char c = _text[_index];
		return c;
	}

	private char Consume()
	{
		char c = _text[_index];
		_index++;
		return c;
	}
}


public class TextToken
{
	public TextTokenType Type;
	public string Value;

	public TextToken(TextTokenType type, string value)
	{
		Type = type;
		Value = value;
	}

	public enum TextTokenType
	{
		Message,
		Attribute
	}

	public override string ToString()
	{
		return $"({Type}){Value}";
	}
}