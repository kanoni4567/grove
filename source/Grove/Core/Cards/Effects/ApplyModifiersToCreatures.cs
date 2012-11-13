﻿namespace Grove.Core.Cards.Effects
{
  using System;
  using System.Collections.Generic;
  using Grove.Core.Targeting;
  using Modifiers;

  public class ApplyModifiersToCreatures : Effect
  {
    private readonly List<IModifierFactory> _modifierFactories = new List<IModifierFactory>();

    public Func<IEffectSource, Card, bool> Filter = delegate { return true; };
    public Value ToughnessReduction = 0;

    public override int CalculateToughnessReduction(Card creature)
    {
      if ((Target() == null || creature.Controller == Target()) && Filter(Source, creature))
      {
        return ToughnessReduction.GetValue(X);
      }

      return 0;
    }

    public void Modifiers(params IModifierFactory[] modifiers)
    {
      _modifierFactories.AddRange(modifiers);
    }

    protected override void ResolveEffect()
    {
      if (Target() != null)
      {
        ApplyModifierToPlayersCreatures(Target().Player());
        return;
      }

      foreach (var player in Players)
      {
        ApplyModifierToPlayersCreatures(player);
      }
    }

    private void ApplyModifierToPlayersCreatures(Player player)
    {
      foreach (var creature in player.Battlefield.Creatures)
      {
        if (!Filter(Source, creature))
          continue;

        foreach (var modifier in _modifierFactories.CreateModifiers(Source.OwningCard, creature, X, Game))
        {
          creature.AddModifier(modifier);
        }
      }
    }
  }
}