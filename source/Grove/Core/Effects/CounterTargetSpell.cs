﻿namespace Grove.Core.Effects
{
  using System;
  using Ai;
  using Decisions;
  using Decisions.Results;
  using Mana;
  using Targeting;

  public class CounterTargetSpell : Effect, IProcessDecisionResults<BooleanResult>
  {
    private readonly int? _controllerLifeloss;
    private readonly DynParam<int> _doNotCounterCost;
    private readonly bool _tapLandsAndEmptyManaPool;

    private CounterTargetSpell() {}

    public CounterTargetSpell(DynParam<int> doNotCounterCost = null, int? controllerLifeloss = null,
      bool tapLandsAndEmptyManaPool = false)
    {
      _controllerLifeloss = controllerLifeloss;
      _doNotCounterCost = doNotCounterCost;
      _tapLandsAndEmptyManaPool = tapLandsAndEmptyManaPool;
      Category = EffectCategories.Counterspell;

      RegisterDynamicParameters(doNotCounterCost);
    }

    public void ProcessResults(BooleanResult results)
    {
      if (results.IsTrue)
        return;

      CounterSpell();
    }

    protected override void ResolveEffect()
    {
      var targetSpellController = Target.Effect().Controller;

      if (_doNotCounterCost == null)
      {
        CounterSpell();
        return;
      }

      Enqueue<PayOr>(targetSpellController, p =>
        {
          p.ManaAmount = _doNotCounterCost.Value.Colorless();
          p.Text = FormatText(string.Format("Pay {0}?", _doNotCounterCost));
          p.ProcessDecisionResults = this;
        });
      return;
    }

    private void CounterSpell()
    {
      var targetSpellController = Target.Effect().Controller;
      
      if (_controllerLifeloss.HasValue)
      {
        targetSpellController.Life -= _controllerLifeloss.Value;
      }

      if (_tapLandsAndEmptyManaPool)
      {
        foreach (var land in targetSpellController.Battlefield.Lands)
        {
          land.Tap();
        }

        targetSpellController.EmptyManaPool();
      }

      Stack.Counter(Target.Effect());
    }
  }
}