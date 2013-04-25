﻿namespace Grove.Gameplay.Decisions.Results
{
  using Grove.Infrastructure;
  using Player;

  [Copyable]
  public class ChosenPlayer
  {
    private ChosenPlayer() {}

    public ChosenPlayer(Player player)
    {
      Player = player;
    }

    public Player Player { get; private set; }
  }
}