namespace GameLayer.CharacterSystem
{
    public sealed class Character : ICharacter
    {
        public Character(ICharacterComponent component)
        {
            CharacterComponent = component;
        }
        
        public ICharacterComponent CharacterComponent { get; }
    }
}