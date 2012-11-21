﻿namespace Grove.Core.Cards.Effects
{
  using Decisions;
  using Zones;

  public class EachPlayerReturnsCardFromGraveyardToBattlefield : Effect
  {
    private void ChooseCreatureToPutIntoPlay(Player player)
    {
      Game.Enqueue<SelectCardsPutToBattlefield>(
        controller: player,
        init: p =>
          {
            p.MinCount = 1;
            p.MaxCount = 1;
            p.Text = FormatText("Select a creature card in your graveyard");
            p.Validator = card => card.Is().Creature;
            p.Zone = zone => zone == Zone.Graveyard;
          }
        );
    }

    protected override void ResolveEffect()
    {
      ChooseCreatureToPutIntoPlay(Players.Active);
      ChooseCreatureToPutIntoPlay(Players.Passive);
    }
  }
}