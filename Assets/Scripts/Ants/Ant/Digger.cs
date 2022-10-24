using UnityEngine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;

namespace Assets.Scripts
{
    public class Digger : Ant
    {
        private const float DefaultDuration = 1f;
        public override AntType Type => AntType.Digger;

        protected override void OnEndMove(Resource part, float income)
        {
            if (part != null)
            {
                if (part as SlicedHexPart)
                {
                    float partIncome = part.Price * income;
                    House.AddingResourceAnimation.RenderAddingResource(partIncome);
                    Wallet.AddResource(partIncome);
                }

                //Destroy((part as SlicedHexPart).gameObject);
                part.Disable();
            }

            if (Target != null)
                if (Target.CellAntsHolder.ContainsDigger(this))
                    Target.CellAntsHolder.RemoveDigger(this);

            if (TargetPart != null)
                TargetPart = null;

            View.gameObject.SetActive(false);
        }

        protected override bool CanWork(Cell target) => base.CanWork(target) && target.IsDigging/* && target.SlicedHex.HasPart(TargetPart)*/;

        protected override IEnumerator Work(Cell cell, float speed, float strenght)
        {
            View.Play(AntView.Dig, strenght);
            float duration = 1f;

            if (CellData.MultiplierByDifficulty.TryGetValue((CellData.CellDifficult)cell.DiggingDifficult, out int diffcultyMultiplier))
            {
                duration = (DefaultDuration + diffcultyMultiplier * DefaultDuration) / strenght; /*+ View.GetCurrentAnimationDuration()*/
                duration += duration * (cell.Region.PieceMultiply);
            }

            cell.SlicedHex.LightenPartColor(TargetPart, duration);
            yield return new WaitForSeconds(duration);

            float partAnimationDuration = Target.DiggingDifficult*DefaultDuration / strenght;
            partAnimationDuration = 0.01f;

            TakePiece(TargetPart);
            cell.SlicedHex.RemovePart(TargetPart);

            if(TargetPart.Root != null)
            {
                TargetPart.Root.DORotate(Quaternion.LookRotation(transform.up).eulerAngles, partAnimationDuration);
                TargetPart.Root.DOMove(TransferPoint.position + Vector3.up * 0.1f, partAnimationDuration)
                    .OnComplete(() => TargetPart.Root.SetParent(transform));
            }
            
            yield return new WaitForSeconds(partAnimationDuration);
            View.Play(AntView.Run, speed);
        }
    }
}
