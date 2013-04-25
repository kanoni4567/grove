﻿namespace Grove.Gameplay.Effects
{
  using System.Collections.Generic;
  using System.Linq;
  using Decisions.Results;
  using Grove.Ai;

  public class DestroyAllLandsOrCreatures : CustomizableEffect
  {
    public DestroyAllLandsOrCreatures()
    {
      Category = EffectCategories.Destruction;
    }

    public override ChosenOptions ChooseResult(List<object> candidates)
    {
      var opponentCreatureCount = Controller.Opponent.Battlefield.Creatures.Count();
      var yourCreatureCount = Controller.Battlefield.Creatures.Count();

      return opponentCreatureCount - yourCreatureCount > 0
        ? new ChosenOptions(EffectOption.Creatures)
        : new ChosenOptions(EffectOption.Lands);
    }

    public override void ProcessResults(ChosenOptions results)
    {
      if (results.Options[0].Equals(EffectOption.Lands))
      {
        Players.DestroyPermanents(card => card.Is().Land);
        return;
      }

      Players.DestroyPermanents(
        card => card.Is().Creature, allowToRegenerate: false);
    }

    public override string GetText()
    {
      return "Destroy all #0.";
    }

    public override IEnumerable<object> GetChoices()
    {
      yield return new DiscreteEffectChoice(
        EffectOption.Lands, 
        EffectOption.Creatures);
    }
  }
}