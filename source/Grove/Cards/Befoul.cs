﻿namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Cards.Effects;
  using Core.Dsl;
  using Core.Mana;
  using Core.Targeting;

  public class Befoul : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Befoul")
        .ManaCost("{2}{B}{B}")
        .Type("Sorcery")
        .Text("Destroy target land or nonblack creature. It can't be regenerated.")
        .FlavorText("'The land putrefied at its touch, turned into an oily bile in seconds.'{EOL}—Radiant, archangel")
        .Effect<DestroyTargetPermanents>(p => p.AllowRegenerate = false)
        .Timing(Timings.FirstMain())
        .Category(EffectCategories.Destruction)
        .Targets(
          selectorAi: TargetSelectorAi.Destroy(),
          effectValidator: TargetValidator(
            TargetIs.Card(card =>
              card.Is().Land || (card.Is().Creature && !card.HasColors(ManaColors.Black))),
            ZoneIs.Battlefield()));
    }
  }
}