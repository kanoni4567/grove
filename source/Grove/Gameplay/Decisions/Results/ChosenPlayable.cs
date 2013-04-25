﻿namespace Grove.Gameplay.Decisions.Results
{
  using Grove.Infrastructure;

  [Copyable]
  public class ChosenPlayable
  {
    public Playable Playable { get; set; }
    public bool WasPriorityPassed { get { return Playable.WasPriorityPassed; } }

    public static implicit operator ChosenPlayable(Playable playable)
    {
      return new ChosenPlayable
        {
          Playable = playable
        };
    }
  }
}