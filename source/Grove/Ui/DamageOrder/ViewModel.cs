﻿namespace Grove.Ui.DamageOrder
{
  using System.Collections.Generic;
  using System.Linq;
  using Core;
  using Core.Decisions.Results;
  using Infrastructure;

  public class ViewModel
  {
    private readonly DamageAssignmentOrder _assignmentOrder;
    private readonly Game _game;
    private readonly Attacker _attacker;
    private readonly List<BlockerAssignment> _blockerAssignments;    
    private int _curentRank = 1;

    public ViewModel(Attacker attacker, DamageAssignmentOrder assignmentOrder, Game game)
    {
      _attacker = attacker;

      _blockerAssignments =
        attacker.Blockers.Select(
          blocker => Bindable.Create<BlockerAssignment>(blocker)).ToList();
      
      _assignmentOrder = assignmentOrder;
      _game = game;
    }

    public Card Attacker { get { return _attacker; } }
    public IEnumerable<BlockerAssignment> Blockers { get { return _blockerAssignments; } }

    public bool CanAccept { get { return _curentRank > _attacker.BlockersCount; } }

    public virtual void Accept()
    {
      foreach (var assignment in _blockerAssignments)
      {
        _assignmentOrder.Assign(assignment.Blocker, assignment.AssignmentOrder.Value);
      }

      Close();
    }

    [Updates("CanAccept")]
    public virtual void AssignRank(BlockerAssignment blocker)
    {
      if (!blocker.AssignmentOrder.HasValue)
      {
        blocker.AssignmentOrder = _curentRank++;
      }
    }

    public void ChangePlayersInterest(Card card)
    {
      _game.Publish(new PlayersInterestChanged
        {
          Visual = card
        });
    }

    [Updates("CanAccept")]
    public virtual void Clear()
    {
      _curentRank = 1;

      foreach (var blockerAssignment in _blockerAssignments)
      {
        blockerAssignment.AssignmentOrder = null;
      }
    }

    public virtual void Close() {}

    public interface IFactory
    {
      ViewModel Create(Attacker attacker, DamageAssignmentOrder assignmentOrder);
    }
  }
}