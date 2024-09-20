using EchoOfTheTimes.LevelStates;
using UnityEngine;

namespace EchoOfTheTimes.Utils
{
    public class MovableByButtonGizmosDrawer : GizmosDrawer<MovableByButton>
    {
#if UNITY_EDITOR
        protected override void DrawStates()
        {
            // ¬€–≈«¿À  Œ√ƒ¿ ƒŒ¡¿¬ÀﬂÀ  ÕŒœ » œŒ »Õƒ≈ —¿Ã

            //if (component.Parameter != null)
            //{
            //    Gizmos.color = colorStateSettings.GetColor(component.Parameter.StateId);

            //    if (mesh != null)
            //    {
            //        Gizmos.DrawWireMesh(mesh, component.Parameter.Position,
            //            Quaternion.Euler(component.Parameter.Rotation), component.Parameter.LocalScale);
            //    }
            //    else if (meshes != null)
            //    {
            //        GizmosHelper.DrawWireMeshesByTRS(meshes, component.Parameter);
            //    }
            //    else
            //    {
            //        InitComponents();
            //    }
            //}

            //StateParameter defaultPosition = component.GetDefaultPosition();
            //if (defaultPosition != null)
            //{
            //    Gizmos.color = colorStateSettings.GetColor(defaultPosition.StateId + 1);

            //    if (mesh != null)
            //    {
            //        Gizmos.DrawWireMesh(mesh, defaultPosition.Position, Quaternion.Euler(defaultPosition.Rotation), defaultPosition.LocalScale);
            //    }
            //    else if (meshes != null)
            //    {
            //        GizmosHelper.DrawWireMeshesByTRS(meshes, defaultPosition);
            //    }
            //    else
            //    {
            //        InitComponents();
            //    }
            //}
        }
#endif
    }
}
