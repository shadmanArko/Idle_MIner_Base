using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MineSelectionCard : MonoBehaviour
{
    [SerializeField] private MineSelectionButton mineSelectionButton;
    [SerializeField] private TMP_Text mineNameText;
    [SerializeField] private TMP_Text mineDescriptionText;
    
    public MineSelectionButton MineSelectionButton => mineSelectionButton;
    public TMP_Text MineNameText => mineNameText;
    public TMP_Text MineDescriptionText => mineDescriptionText;
}
