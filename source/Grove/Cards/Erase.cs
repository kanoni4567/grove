﻿namespace Grove.Cards
{
  using System.Collections.Generic;
  using Artifical.TargetingRules;
  using Artifical.TimingRules;
  using Gameplay.Effects;
  using Gameplay.Misc;

  public class Erase : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Erase")
        .ManaCost("{W}")
        .Type("Instant")
        .Text("Exile target enchantment.")
        .FlavorText("Perception is more pleasing than truth.")
        .Cast(p =>
          {
            p.Effect = () => new ExileTargets();
            p.TargetSelector.AddEffect(trg => trg.Is.Enchantment().On.Battlefield());
            p.TargetingRule(new Exile());
            p.TimingRule(new TargetRemoval());
          });
    }
  }
}