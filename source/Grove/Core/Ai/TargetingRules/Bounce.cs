﻿namespace Grove.Core.Ai.TargetingRules
{
  using System.Collections.Generic;
  using Targeting;

  public class Bounce : TargetingRule
  {
    protected override IEnumerable<Targets> SelectTargets(TargetingRuleParameters p)
    {
      var candidates = GetBounceCandidates(p);
      return Group(candidates, p.MinTargetCount());
    }
  }
}