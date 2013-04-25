﻿namespace Grove.Gameplay.Card.Abilities
{
  using System.Linq;
  using Grove.Infrastructure;
  using Modifiers;

  [Copyable]
  public class StaticAbilities : IStaticAbilities, IModifiable, IHashable
  {
    private readonly TrackableList<StaticAbility> _abilities = new TrackableList<StaticAbility>();
    private INotifyChangeTracker _changeTracker = new NullTracker();

    public int CalculateHash(HashCalculator calc)
    {
      return calc.Calculate(_abilities);
    }

    public void Accept(IModifier modifier)
    {
      modifier.Apply(this);
    }

    public bool Deathtouch { get { return Has(Static.Deathtouch); } }
    public bool Defender { get { return Has(Static.Defender); } }
    public bool Fear { get { return Has(Static.Fear); } }
    public bool Flying { get { return Has(Static.Flying); } }
    public bool Haste { get { return Has(Static.Haste); } }
    public bool Hexproof { get { return Has(Static.Hexproof); } }
    public bool Indestructible { get { return Has(Static.Indestructible); } }
    public bool Lifelink { get { return Has(Static.Lifelink); } }
    public bool Shroud { get { return Has(Static.Shroud); } }
    public bool Trample { get { return Has(Static.Trample); } }
    public bool Unblockable { get { return Has(Static.Unblockable); } }
    public bool FirstStrike { get { return Has(Static.FirstStrike); } }
    public bool DoubleStrike { get { return Has(Static.DoubleStrike); } }
    public bool Reach { get { return Has(Static.Reach); } }
    public bool Vigilance { get { return Has(Static.Vigilance); } }
    public bool Swampwalk { get { return Has(Static.Swampwalk); } }
    public bool CannotAttack { get { return Has(Static.CannotAttack); } }
    public bool CannotBlock { get { return Has(Static.CannotBlock); } }
    public bool Islandwalk { get { return Has(Static.Islandwalk); } }
    public bool Mountainwalk { get { return Has(Static.Mountainwalk); } }
    public bool AssignsDamageAsThoughItWasntBlocked { get { return Has(Static.AssignsDamageAsThoughItWasntBlocked); } }

    public bool DoesNotUntap { get { return Has(Static.DoesNotUntap); } }
    public bool AnyEvadingAbility { get { return Fear || Flying || Trample || Unblockable || AssignsDamageAsThoughItWasntBlocked; } }

    public void Initialize(INotifyChangeTracker changeTracker, IHashDependancy hashDependancy)
    {
      _changeTracker = changeTracker;
      _abilities.Initialize(changeTracker, hashDependancy);

      foreach (var staticAbility in _abilities)
      {
        staticAbility.Initialize(changeTracker);
      }
    }

    public void Add(Static ability)
    {
      _abilities.Add(new StaticAbility(ability).Initialize(_changeTracker));
    }

    public bool Remove(Static ability)
    {
      var staticAbility = _abilities
        .Where(x => x.Value == ability)
        .OrderBy(x => x.IsEnabled ? 0 : 1)
        .FirstOrDefault();

      if (staticAbility == null)
      {
        return false;        
      }

      _abilities.Remove(staticAbility);
      return true;
    }

    private bool Has(Static ability)
    {
      return _abilities.Any(x => x.IsEnabled && x.Value == ability);
    }

    public void Disable()
    {
      foreach (var staticAbility in _abilities)
      {
        staticAbility.Disable();
      }
    }

    public void Enable()
    {
      foreach (var staticAbility in _abilities)
      {
        staticAbility.Enable();
      }
    }
  }
}