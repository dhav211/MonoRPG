using System.Collections.Generic;

namespace MonoRPG
{
    public class InteractionComponent : Component
    {
        public enum InteractionType { ATTACK, OPEN_DOOR, OPEN_CHEST, UNLOCK, PICK_UP, EXAMINE, TALK, PICK, LOOT }
        
        public InteractionType MainInteraction { get; set; }
        public List<InteractionType> Interactions { get; set; }

        public InteractionComponent(Entity _owner) : base(_owner)
        {
            owner.AddComponent<InteractionComponent>(this);
            Interactions = new List<InteractionType>();
        }
    }
}