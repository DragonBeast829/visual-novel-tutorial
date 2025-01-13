using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TESTING {
    public class GraphicLayerTesting : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            GraphicPanel panel = GraphicPanelManager.instance.GetPanel("Background");
            GraphicLayer layer = panel.GetLayer(0, true);

            layer.SetTexture("Graphics/BG Images/2");

            Texture blendTex = Resources.Load<Texture>("Graphics/Transition Effects/hurricane");
            layer.SetTexture("Graphics/BG Images/2", blendingTexture: blendTex);
        }
    }
}
