using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    [SerializeField] private CellBlocker _cellBlocker;
    [SerializeField] private Cell _lastCellToDig;
    [SerializeField] private TutorialCamera _tutorialCamera;
    [SerializeField] private FingerCellTutuorial _fingerCellTutuorial;
    [SerializeField] private InputRoot _inputRoot;

    private const string SaveWord = "TutorialSaveWord";

    private void OnEnable()
    {
        if (PlayerPrefs.HasKey(SaveWord))
            return;

        _tutorialCamera.Init(_inputRoot);
        _lastCellToDig.Digged += OnLastCellDigged;
    }

    private void OnDisable()
    {
        if (PlayerPrefs.HasKey(SaveWord))
            return;
        _lastCellToDig.Digged -= OnLastCellDigged;
    }

    public void TryStartTutorial()
    {
        if (PlayerPrefs.HasKey(SaveWord))
            return;


        _inputRoot.OnZooming(5);
        _fingerCellTutuorial.ShowFinger();

        _cellBlocker.Block();
    }

    private void OnLastCellDigged(Cell cell)
    {
        cell.Digged -= OnLastCellDigged;

        _tutorialCamera.Move(OnTuturialEnd);
    }

    private void OnTuturialEnd()
    {
        PlayerPrefs.SetInt(SaveWord, 1);
        _cellBlocker.UnBlock();
    }
}
