﻿namespace Grove.Core.Cards.Effects
{
  using System.Collections.Generic;
  using Grove.Core.Zones;
  using Modifiers;

  public class ApplyModifiersToSelf : Effect
  {
    public Value ToughnessReduction = 0;
    private readonly List<IModifierFactory> _selfModifiers = new List<IModifierFactory>();

    public override bool AffectsSource { get { return true; } }

    public override int CalculateToughnessReduction(Card creature)
    {
      return creature == Source.OwningCard ? ToughnessReduction.GetValue(X) : 0;
    }

    public void Modifiers(params IModifierFactory[] modifiersFactories)
    {
      _selfModifiers.AddRange(modifiersFactories);
    }

    private IEnumerable<Modifier> CreateSelfModifiers()
    {
      var target = Source.OwningCard;

      return _selfModifiers.CreateModifiers(Source.OwningCard, target, X, Game);
    }

    protected override void ResolveEffect()
    {
      var target = Source.OwningCard;

      if (Source.OwningCard.Zone == Zone.Battlefield)
      {
        foreach (var modifier in CreateSelfModifiers())
        {
          target.AddModifier(modifier);
        }
      }
    }
  }
}