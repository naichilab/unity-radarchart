using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace naichilab
{

	/// <summary>
	/// レーダーチャートのポリゴンを描画
	/// </summary>
	public class RadarChartPolygonUGUI : Graphic
	{
		/// <summary>
		/// 描画半径
		/// </summary>
		const float RADIUS = 0.5f;

		[Header ("General")]
		[SerializeField]
		[Range (3, 30)]
		private int VerticesCount = 5;

		[SerializeField]
		[Range (0f, 1f)]
		private float MaxVolume = 1f;

		[SerializeField]
		private float[] Volumes;

		private void SetVolume (int idx, float value)
		{
			if (this.Volumes.Length < idx) {
				Array.Resize (ref this.Volumes, idx + 1);
			}
			this.Volumes [idx] = value;
		}

		private float GetVolume (int idx)
		{
			if (Volumes.Length - 1 < idx) {
				return 0;
			}
			float v = this.Volumes [idx];
			return v > this.MaxVolume ? this.MaxVolume : v;
		}

		protected override void OnPopulateMesh (VertexHelper vh)
		{
			vh.Clear ();
			var v = UIVertex.simpleVert;
			v.color = color;

			Vector2 center = CreatePos (0.5f, 0.5f);
			v.position = center;
			vh.AddVert (v);

			//各頂点座標
			for (int i = 1; i <= VerticesCount; i++) {
				float rad = (90f - (360f / (float)VerticesCount) * (i - 1)) * Mathf.Deg2Rad;
				float x = 0.5f + Mathf.Cos (rad) * RADIUS * GetVolume (i - 1);
				float y = 0.5f + Mathf.Sin (rad) * RADIUS * GetVolume (i - 1);

				Vector2 p = CreatePos (x, y);
				v.position = p;
				vh.AddVert (v);

				vh.AddTriangle (
					0,
					i,
					i == VerticesCount ? 1 : i + 1
				);
			}
		
		}

		private Vector2 CreatePos (float x, float y)
		{
			Vector2 p = Vector2.zero;
			p.x -= rectTransform.pivot.x;
			p.y -= rectTransform.pivot.y;
			p.x += x;
			p.y += y;
			p.x *= rectTransform.rect.width;
			p.y *= rectTransform.rect.height;
			return p;
		}
	}
}