using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;


public class RichTextParser
{
	private List<TextAttributeBase> _attributeList = new List<TextAttributeBase>();
	private Stack<RichTextData> _messageStack = new Stack<RichTextData>();

	private List<RichTextData> _dataList = new List<RichTextData>();
	public List<RichTextData> DataList => _dataList;


	public List<RichTextData> Analyze(string richText)
	{
		TextTokenizer tokenizer = new TextTokenizer();
		List<TextToken> tokenList = tokenizer.Parse(richText);

		RichTextParser richTextParser = new RichTextParser();
		List<RichTextData> dataList = richTextParser.Parse(tokenList);
		//for (int i = 0; i < dataList.Count; i++) {
		//	RichTextData data = dataList[i];
		//	//Debug.Log(data);
		//}
		return dataList;
	}

	private List<RichTextData> Parse(List<TextToken> tokenList)
	{
		_attributeList.Clear();

		int len = tokenList.Count;
		for (int i = 0; i < len; ++i) {
			TextToken token = tokenList[i];
			switch (token.Type) {
				case TextToken.TextTokenType.Message:
					SetMessage(token);
					break;
				case TextToken.TextTokenType.Attribute:
					SetAttribute(token);
					break;
				default:
					break;
			}
		}

		while (_messageStack.Count > 0) {
			_dataList.Add(_messageStack.Pop());
		}
		_dataList.Reverse();

		return _dataList;
	}

	private void SetMessage(TextToken token)
	{
		RichTextData data = new RichTextData(token.Value);
		AddAttributeToText(data);
		_messageStack.Push(data);
	}

	private void SetAttribute(TextToken token)
	{
		TextAttributeBase attr = RichTextParseHelper.ParseTextAttribute(token.Value);
		//Debug.Log(attr);
		if (attr.Type == TextAttributeBase.TextAttributeType.Close) {
			TextAttributeBase lastOpenAttr = _attributeList.FindLast((t) => t.Name == attr.Name && t.Type == TextAttributeBase.TextAttributeType.Open);
			if (lastOpenAttr != null) {
				_attributeList.Remove(lastOpenAttr);
				//Debug.Log($"Remove: {lastOpenAttr}");
			}
		}
		else {
			//Debug.Log($"Add: {attr}");
			_attributeList.Add(attr);
		}
	}

	private void AddAttributeToText(RichTextData data)
	{
		for (int i = 0; i < _attributeList.Count; i++) {
			TextAttributeBase attr = _attributeList[i];
			data.AddAttribute(attr);
		}
	}
}

public class RichTextParseHelper
{
	public static Regex CLOSE_TAG_REG = new Regex("</([a-z]+)>");
	public static Regex OPEN_TAG_REG = new Regex(@"<([a-z]+)(=?\S+)?(=#?\w+)?>");

	public static TextAttributeBase ParseTextAttribute(string tag)
	{
		TextAttributeBase rlt = null;
		if (CLOSE_TAG_REG.IsMatch(tag)) {
			rlt = new TextAttributeBase();
			Match match = CLOSE_TAG_REG.Match(tag);
			string name = match.Groups[1].Value;
			rlt.Type = TextAttributeBase.TextAttributeType.Close;
			rlt.Name = name;
			Debug.Log($"<color=red>{rlt.Type}</color>, {rlt.Name}");
		}
		else if (OPEN_TAG_REG.IsMatch(tag)) {
			rlt = new TextAttributeBase();
			Match match = OPEN_TAG_REG.Match(tag);
			string name = match.Groups[1].Value;
			if (match.Groups.Count > 2) {
				string value = match.Groups[2].Value;
				rlt.Value = value.Replace("=", "");
			}
			rlt.Type = TextAttributeBase.TextAttributeType.Open;
			rlt.Name = name;
			Debug.Log($"<color=green>{rlt.Type}</color>, {rlt.Name}, {rlt.Value}");
		}
		return rlt;
	}
}