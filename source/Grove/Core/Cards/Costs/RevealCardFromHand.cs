﻿namespace Grove.Core.Cards.Costs
{
  using Grove.Core.Targeting;

  public class RevealCardFromHand : Cost
  {
    public override bool CanPay(ref int? maxX)
    {
      return Controller.Hand.Count > 0;
    }

    public override void Pay(ITarget target, int? x)
    {
      target.Card().Reveal();
    }
  }
}