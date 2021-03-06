﻿namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Grove.Effects;
  using Grove.Modifiers;
  using Grove.Triggers;

  public class EnergyField : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Energy Field")
        .ManaCost("{1}{U}")
        .Type("Enchantment")
        .Text(
          "Prevent all damage that would be dealt to you by sources you don't control.{EOL}When a card is put into your graveyard from anywhere, sacrifice Energy Field.")
        .StaticAbility(p => p.Modifier(() => new AddDamagePrevention(modifier => new PreventDamageToTarget(
          creatureOrPlayer: modifier.SourceCard.Controller,
          sourceFilter: card => card.Controller != modifier.SourceCard.Controller)))
         )                  
        .TriggeredAbility(p =>
          {
            p.Text = "When a card is put into your graveyard from anywhere, sacrifice Energy Field.";
            
            p.Trigger(new OnZoneChanged(
              to: Zone.Graveyard,
              filter: (c, a, g) => a.OwningCard.Controller == c.Owner));
            
            p.Effect = () => new SacrificeOwner();
            p.TriggerOnlyIfOwningCardIsInPlay = true;
          });
    }
  }
}