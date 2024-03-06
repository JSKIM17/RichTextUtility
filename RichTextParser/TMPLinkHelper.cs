using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class TMPLinkHelper : MonoBehaviour, IPointerClickHandler
{
	private TextMeshProUGUI _textMeshPro;
	private Camera _camera;
	private Canvas _canvas;

	public delegate void OnTouchLinkDelegate(TMP_LinkInfo linkInfo);
	public event OnTouchLinkDelegate OnTouchLinkEvent;

	void Start()
	{
		_camera = Camera.main;
		_canvas = gameObject.GetComponentInParent<Canvas>();

		if (_canvas.renderMode == RenderMode.ScreenSpaceOverlay) {
			_camera = null;
		}
		else {
			_camera = _canvas.worldCamera;
		}

		_textMeshPro = gameObject.GetComponent<TextMeshProUGUI>();
		_textMeshPro.ForceMeshUpdate();
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		int linkIndex = TMP_TextUtilities.FindIntersectingLink(_textMeshPro, Input.mousePosition, _camera);

		if (linkIndex != -1) {
			TMP_LinkInfo linkInfo = _textMeshPro.textInfo.linkInfo[linkIndex];
			Debug.Log($"{linkInfo.GetLinkText()}({linkInfo.GetLinkID()})");
			OnTouchLinkEvent?.Invoke(linkInfo);
		}
	}
}
