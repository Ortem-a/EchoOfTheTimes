using EchoOfTheTimes.LevelStates;
using UnityEngine;

namespace EchoOfTheTimes.Utils
{
    public class StateableGizmosDrawer : GizmosDrawer<Stateable>
    {
#if UNITY_EDITOR
        protected override void DrawStates()
        {
            if (component.States != null)
            {
                foreach (var stateParameter in component.States)
                {
                    Gizmos.color = colorStateSettings.GetColor(stateParameter.StateId);

                    if (stateParameter.IsLocalSpace)
                    {
                        DrawInLocalSpace(component.transform.parent, stateParameter);
                    }
                    else
                    {
                        DrawInGlobalSpace(stateParameter);
                    }
                }
            }
        }

        private void DrawInGlobalSpace(StateParameter stateParameter)
        {
            if (mesh != null)
            {
                Gizmos.DrawWireMesh(mesh, stateParameter.Position, Quaternion.Euler(stateParameter.Rotation), stateParameter.LocalScale);
            }
            else if (meshes != null)
            {
                GizmosHelper.DrawWireMeshesByTRS(meshes, stateParameter);
            }
            else
            {
                InitComponents();
            }
        }

        private void DrawInLocalSpace(Transform parent, StateParameter stateParameter)
        {
            if (mesh != null)
            {
                Gizmos.DrawWireMesh(mesh,
                    parent.TransformPoint(stateParameter.Position),
                    Quaternion.Euler(stateParameter.Rotation),
                    stateParameter.LocalScale);
            }
            else if (meshes != null)
            {
                GizmosHelper.DrawWireMeshesByTRS(meshes, stateParameter);
            }
            else
            {
                InitComponents();
            }
        }
#endif
    }
}