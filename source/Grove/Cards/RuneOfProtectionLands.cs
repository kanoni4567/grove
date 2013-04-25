﻿namespace Grove.Cards
{
  using System.Collections.Generic;
  using Ai.TargetingRules;
  using Ai.TimingRules;
  using Core;
  using Gameplay.Card.Costs;
  using Gameplay.Card.Factory;
  using Gameplay.Effects;
  using Gameplay.Mana;

  public class RuneOfProtectionLands : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Rune of Protection: Lands")
        .ManaCost("{1}{W}")
        .Type("Enchantment")
        .Text(
          "{W}: The next time a land source of your choice would deal damage to you this turn, prevent that damage.{EOL}Cycling {2} ({2}, Discard this card: Draw a card.)")
        .Cast(p => p.TimingRule(new FirstMain()))
        .Cycling("{2}")
        .ActivatedAbility(p =>
          {
            p.Text =
              "{W}: The next time a land source of your choice would deal damage to you this turn, prevent that damage.";
            p.Cost = new PayMana(Mana.White, ManaUsage.Abilities);
            p.Effect = () => new PreventNextDamageFromSourceToController();
            
            p.TargetSelector.AddEffect(trg =>
              {
                trg.Is.Card(c => c.Is().Land).On.BattlefieldOrStack();
                trg.Message = "Select damage source.";
              });

            p.TargetingRule(new PreventDamageFromSourceToController());
          });
    }
  }
}