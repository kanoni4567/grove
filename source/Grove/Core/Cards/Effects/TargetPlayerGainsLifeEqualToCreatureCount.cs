﻿namespace Grove.Core.Cards.Effects
{
  using System.Linq;
  using Grove.Core.Targeting;

  public class TargetPlayerGainsLifeEqualToCreatureCount : Effect
  {
    public int Multiplier = 1;
    
    protected override void ResolveEffect()
    {
      // count on resolve
      Target().Player().Life += Players.Permanents().Count(x => x.Is().Creature) * Multiplier;
    }
  }
}