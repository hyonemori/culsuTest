using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UiEffect;

namespace TKF
{
    /// <summary>
    /// Smooth text outline.
    /// </summary>
    public class SmoothOutlineUgui : Outline
    {
        public int copyCount = 6;

        public override void ModifyMesh(VertexHelper vh)
        {
            if (!IsActive())
            {
                return;
            }

            List<UIVertex> verts = ListPool<UIVertex>.Get();
            vh.GetUIVertexStream(verts);
            ModifyVerticesSub(verts);
            vh.Clear();
            vh.AddUIVertexTriangleStream(verts);
            ListPool<UIVertex>.Release(verts);
        }

        private void ModifyVerticesSub(List<UIVertex> verts)
        {
            var start = 0;
            var end = verts.Count;

            for (int i = 0; i < copyCount; ++i)
            {
                float x = Mathf.Sin(Mathf.PI * 2f * i / copyCount) * effectDistance.x;
                float y = Mathf.Cos(Mathf.PI * 2f * i / copyCount) * effectDistance.y;
                ApplyShadow(verts, effectColor, start, verts.Count, x, y);
                start = end;
                end = verts.Count;
            }
        }
    }
}