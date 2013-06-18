﻿namespace Grove.Cards
{
  using System.Collections.Generic;
  using Artifical.TimingRules;
  using Gameplay.Effects;
  using Gameplay.Misc;

  public class Windfall : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Windfall")
        .ManaCost("{2}{U}")
        .Type("Sorcery")
        .Text(
          "Each player discards his or her hand, then draws cards equal to the greatest number of cards a player discarded this way.")
        .FlavorText("To fill your mind with knowledge, we must start by emptying it.")
        .Cast(p =>
          {
            p.Effect = () => new EachPlayerDiscardsHandAndDrawsGreatestDiscardedCount();
            p.TimingRule(new SecondMain());
            p.TimingRule(new ControllerHasMoreCardsInHand(-2));
          });
    }
  }
}