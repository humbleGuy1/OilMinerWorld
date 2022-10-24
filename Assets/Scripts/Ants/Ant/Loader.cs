using System.Collections;
using UnityEngine;
using DG.Tweening;

namespace Assets.Scripts
{
    public class Loader : Ant
    {
        public override AntType Type => AntType.Loader;

        protected override void OnEndMove(Resource part, float income)
        {
            if (HasPart && enabled)
            {
                float partIncome = part.Price * income;
                House.AddingResourceAnimation.RenderAddingResource(partIncome);
                Wallet.AddResource(partIncome);
                (part as FoodPart).SwitchToWaitingRegrow(Target.Food.transform);
            }

            if (Target != null)
                if (Target.CellAntsHolder.ContainsLoader(this))
                    Target.CellAntsHolder.RemoveLoader(this);

            if (TargetPart != null)
                TargetPart = null;

            View.gameObject.SetActive(false);
        }

        protected override bool CanWork(Cell target) => base.CanWork(target) && target.Food.HasPart(TargetPart);

        protected override IEnumerator Work(Cell cell, float speed, float strenght)
        {
            View.Play(AntView.Load, strenght);
            yield return new WaitForSeconds(View.GetCurrentAnimationDuration());

            float partAnimationDuration = 1 / strenght;
            TakePiece(TargetPart);
            cell.Food.RemovePart(TargetPart);
            TargetPart.Root.DORotate(Quaternion.LookRotation(transform.up).eulerAngles, partAnimationDuration);
            TargetPart.Root.DOMove(TransferPoint.position + Vector3.up * 0.1f, partAnimationDuration)
                .OnComplete(() => TargetPart.Root.SetParent(transform));

            yield return new WaitForSeconds(partAnimationDuration);
            View.Play(AntView.Run, speed);
        }
    }
}
