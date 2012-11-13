﻿namespace Grove.Core.Cards
{
  using Grove.Infrastructure;
  using Modifiers;

  public class CardTypeCharacteristic : Characteristic<CardType>, IModifiable
  {
    private CardTypeCharacteristic() {}

    public CardTypeCharacteristic(CardType value, ChangeTracker changeTracker, IHashDependancy hashDependancy)
      : base(value, changeTracker, hashDependancy) {}

    public void Accept(IModifier modifier)
    {
      modifier.Apply(this);      
    }
  }
}