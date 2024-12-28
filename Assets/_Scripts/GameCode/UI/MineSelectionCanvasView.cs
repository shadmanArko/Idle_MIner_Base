using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace GameCode.UI
{
    public class MineSelectionCanvasView : MonoBehaviour
    {
        [SerializeField] private Button _openMineSelection;
        [SerializeField] private PanelAlphaAnimator _blackFadeInFadeOut;
        [SerializeField] private PanelMoveAnimator _mineSelection;
        [SerializeField] private Button _closeMineSelection;
        [SerializeField] private Button _goToMine01;
        [SerializeField] private Button _goToMine02;
        [SerializeField] private List<MineSelectionButton> _mineSelectionButtons;
        
        public Button OpenMineSelection => _openMineSelection;
        public PanelAlphaAnimator BlackFadeInFadeOut => _blackFadeInFadeOut;
        public PanelMoveAnimator MineSelection => _mineSelection;
        public Button CloseMineSelection => _closeMineSelection;
        public Button GoToMine01 => _goToMine01;
        public Button GoToMine02 => _goToMine02;
        public List<MineSelectionButton> MineSelectionButtons => _mineSelectionButtons;
    }
}