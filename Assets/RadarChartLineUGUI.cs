using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace naichilab
{

	/// <summary>
	/// レーダーチャートの罫線を描画
	/// </summary>
	public class RadarChartLineUGUI : Graphic
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
		[Tooltip ("線の太さ")]
		[Range (0.001f, 0.03f)]
		private float LineWidth = 0.02f;

		[Header ("Major Grid")]
		[SerializeField]
		[Tooltip ("主罫線を表示するかどうか")]
		private bool DrawMajorGrid = true;

		[SerializeField]
		[Range (0.01f, 1f)]
		[Tooltip ("主罫線の間隔")]
		private float MajorGridInterval = 0.2f;


		protected override void OnPopulateMesh (VertexHelper vh)
		{			
			vh.Clear ();
		
			if (this.MaxVolume == 0)
				return;

			var v = UIVertex.simpleVert;
			v.color = color;

			//Outer Frame
			this.DrawFrame (vh, this.MaxVolume);

			//Axis
			this.DrawAxis (vh, this.MaxVolume);

			//Major Grid
			if (this.DrawMajorGrid && this.MajorGridInterval < this.MaxVolume) {
				int numOfGrid = (int)(this.MaxVolume / this.MajorGridInterval);
				for (int i = 1; i <= numOfGrid; i++) {
					this.DrawFrame (vh, i * this.MajorGridInterval);
				}
			}
		}

		/// <summary>
		/// uGUI座標を作成
		/// </summary>
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


		/// <summary>
		/// 外周を描画
		/// </summary>
		private void DrawFrame (VertexHelper vh, float vol)
		{
			int currentVertCount = vh.currentVertCount;

			var v = UIVertex.simpleVert;
			v.color = color;

			//各頂点座標
			for (int i = 0; i < VerticesCount; i++) {
				float deg = (360f / VerticesCount) * 0.5f;
				float offset = (this.LineWidth / Mathf.Cos (deg * Mathf.Deg2Rad)) / 2f;

				float rad = (90f - (360f / (float)VerticesCount) * i) * Mathf.Deg2Rad;

				float x1 = 0.5f + Mathf.Cos (rad) * (RADIUS * vol - offset);
				float y1 = 0.5f + Mathf.Sin (rad) * (RADIUS * vol - offset);
				float x2 = 0.5f + Mathf.Cos (rad) * (RADIUS * vol + offset);
				float y2 = 0.5f + Mathf.Sin (rad) * (RADIUS * vol + offset);

				Vector2 p1 = CreatePos (x1, y1);
				Vector2 p2 = CreatePos (x2, y2);

				v.position = p1;
				vh.AddVert (v);

				v.position = p2;
				vh.AddVert (v);

				vh.AddTriangle (
					(((i + 0) * 2) + 0) % (VerticesCount * 2) + currentVertCount,
					(((i + 0) * 2) + 1) % (VerticesCount * 2) + currentVertCount,
					(((i + 1) * 2) + 0) % (VerticesCount * 2) + currentVertCount
				);

				vh.AddTriangle (
					(((i + 1) * 2) + 0) % (VerticesCount * 2) + currentVertCount,
					(((i + 0) * 2) + 1) % (VerticesCount * 2) + currentVertCount,
					(((i + 1) * 2) + 1) % (VerticesCount * 2) + currentVertCount
				);
			}

		}

		/// <summary>
		/// 軸を描画
		/// </summary>
		private void DrawAxis (VertexHelper vh, float vol)
		{
			int currentVertCount = vh.currentVertCount;
				
			var v = UIVertex.simpleVert;
			v.color = color;

			for (int i = 0; i < VerticesCount; i++) {
				float halfWidthDeg = 90 * this.LineWidth / (Mathf.PI * RADIUS * vol);
					
				float rad1 = (90f - halfWidthDeg - (360f / (float)VerticesCount) * i) * Mathf.Deg2Rad;
				float rad2 = (90f + halfWidthDeg - (360f / (float)VerticesCount) * i) * Mathf.Deg2Rad;
			
				float x3 = 0.5f + Mathf.Cos (rad1) * RADIUS * vol;
				float y3 = 0.5f + Mathf.Sin (rad1) * RADIUS * vol;
				float x4 = 0.5f + Mathf.Cos (rad2) * RADIUS * vol;
				float y4 = 0.5f + Mathf.Sin (rad2) * RADIUS * vol;
				float x1 = 0.5f + (x3 - x4) / 2f;
				float y1 = 0.5f + (y3 - y4) / 2f;
				float x2 = 0.5f + (x4 - x3) / 2f;
				float y2 = 0.5f + (y4 - y3) / 2f;
			
				Vector2 p1 = CreatePos (x1, y1);
				Vector2 p2 = CreatePos (x2, y2);
				Vector2 p3 = CreatePos (x3, y3);
				Vector2 p4 = CreatePos (x4, y4);
			
				v.position = p1;
				vh.AddVert (v);
			
				v.position = p2;
				vh.AddVert (v);
			
				v.position = p3;
				vh.AddVert (v);
			
				v.position = p4;
				vh.AddVert (v);
			
				vh.AddTriangle (
					((i * 4) + 0) + currentVertCount,
					((i * 4) + 3) + currentVertCount,
					((i * 4) + 2) + currentVertCount
				);
			
				vh.AddTriangle (
					((i * 4) + 0) + currentVertCount,
					((i * 4) + 1) + currentVertCount,
					((i * 4) + 3) + currentVertCount
				);
			}
		}

	}
}