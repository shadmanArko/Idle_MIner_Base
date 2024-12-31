using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
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
        [SerializeField] private CircleFadePanel _circleFadeIn;
        [SerializeField] private Transform _mineSelectionCardsParent;
        [SerializeField] private MineSelectionButton _mineSelectionButtonMine01;
        [SerializeField] private MineSelectionButton _mineSelectionButtonMine02;
        
        
        public Button OpenMineSelection => _openMineSelection;
        public PanelAlphaAnimator BlackFadeInFadeOut => _blackFadeInFadeOut;
        public PanelMoveAnimator MineSelection => _mineSelection;
        public Button CloseMineSelection => _closeMineSelection;
        public CircleFadePanel CircleFadeIn => _circleFadeIn;
        public Transform MineSelectionCardsParent => _mineSelectionCardsParent;
        public MineSelectionButton MineSelectionButtonMine01 => _mineSelectionButtonMine01;
        public MineSelectionButton MineSelectionButtonMine02 => _mineSelectionButtonMine02;
    }
}