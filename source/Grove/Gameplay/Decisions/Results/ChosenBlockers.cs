﻿namespace Grove.Gameplay.Decisions.Results
{
  using System;
  using System.Collections;
  using System.Collections.Generic;
  using System.Linq;
  using System.Runtime.Serialization;
  using Infrastructure;
  using Persistance;

  [Copyable, Serializable]
  public class ChosenBlockers : IEnumerable<ChosenBlockers.AttackerBlockerPair>
  {
    private readonly List<AttackerBlockerPair> _pairs = new List<AttackerBlockerPair>();

    public IEnumerator<AttackerBlockerPair> GetEnumerator()
    {
      return _pairs.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }

    public void Add(Card blocker, Card attacker)
    {
      _pairs.Add(new AttackerBlockerPair {Blocker = blocker, Attacker = attacker});
    }

    public bool ContainsBlocker(Card blocker)
    {
      return _pairs.Any(x => x.Blocker == blocker);
    }

    public void Remove(Card blocker)
    {
      var pair = _pairs.First(x => x.Blocker == blocker);
      _pairs.Remove(pair);
    }

    public bool IsValid()
    {
      var attackers = _pairs
        .GroupBy(x => x.Attacker)
        .Select(x => new
          {
            Card = x.Key,
            BlockerCount = x.Count()
          })
        .ToList();

      return attackers.All(attacker => attacker.Card.MinimalBlockerCount <= attacker.BlockerCount);
    }

    [Copyable, Serializable]
    public class AttackerBlockerPair : ISerializable
    {
      public AttackerBlockerPair() {}

      private AttackerBlockerPair(SerializationInfo info, StreamingContext context)
      {
        var ctx = (SerializationContext) context.Context;

        var attackerId = info.GetInt32("attacker");
        var blockerId = info.GetInt32("blocker");

        Attacker = (Card) ctx.Recorder.GetObject(attackerId);
        Blocker = (Card) ctx.Recorder.GetObject(blockerId);
      }

      public Card Attacker { get; set; }
      public Card Blocker { get; set; }

      public void GetObjectData(SerializationInfo info, StreamingContext context)
      {
        info.AddValue("attacker", Attacker.Id);
        info.AddValue("blocker", Blocker.Id);
      }
    }
  }
}