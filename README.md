# RichTextUtility

How To Use?

		string content = "<color=red><i>Italic #ColorTest</color> is End!</i>";
		
		RichTextParser parser = new RichTextParser();
		List<RichTextData> datas = parser.Analyze(content);
		for (int i = 0; i < datas.Count; i++) {
			RichTextData data = datas[i];
			Debug.Log(data);
		}
