﻿namespace Grove.Cards
{
  using System;
  using System.Collections.Generic;
  using Artifical.TimingRules;
  using Gameplay;
  using Gameplay.Abilities;
  using Gameplay.Characteristics;
  using Gameplay.Effects;
  using Gameplay.Misc;
  using Gameplay.Modifiers;
  using Gameplay.States;
  using Gameplay.Triggers;

  public class VeiledApparition : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Veiled Apparition")
        .ManaCost("{1}{U}")
        .Type("Enchantment")
        .Text(
          "When an opponent casts a spell, if Veiled Apparition is an enchantment, Veiled Apparition becomes a 3/3 Illusion creature with flying and 'At the beginning of your upkeep, sacrifice Veiled Apparition unless you pay {1}{U}.'")        
        .Cast(p => p.TimingRule(new FirstMain()))
        .TriggeredAbility(p =>
          {
            p.Text =
              "When an opponent casts a spell, if Veiled Apparition is an enchantment, Veiled Apparition becomes a 3/3 Illusion creature with flying and 'At the beginning of your upkeep, sacrifice Veiled Apparition unless you pay {1}{U}.'";
            p.Trigger(new OnCastedSpell(
              filter: (ability, card) => ability.OwningCard.Controller != card.Controller && ability.OwningCard.Is().Enchantment));

            p.Effect = () => new ApplyModifiersToSelf(
              () => new Gameplay.Modifiers.ChangeToCreature(
                power: 3,
                toughness: 3,
                type: "Creature Illusion",
                colors: L(CardColor.Blue)),
              () => new AddStaticAbility(Static.Flying),
              () =>
                {
                  var tp = new TriggeredAbilityParameters();
                  tp.Text = "At the beginning of your upkeep, sacrifice Veiled Apparition unless you pay {1}{U}.";
                  tp.Trigger(new OnStepStart(Step.Upkeep));
                  tp.Effect = () => new PayManaOrSacrifice("{1}{U}".Parse(), "Pay upkeep? (or sacrifice Veiled Apparition)");
                  tp.TriggerOnlyIfOwningCardIsInPlay = true;
                
                  return new AddTriggeredAbility(new TriggeredAbility(tp));
                });

            p.TriggerOnlyIfOwningCardIsInPlay = true;
          });
    }
  }
}