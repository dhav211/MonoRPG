using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace MonoRPG
{
    public class TurnManager
    {
        public List<Entity> Entities { get; private set; } = new List<Entity>();
        public Entity CurrentEntity { get; private set; }
        int currentEntityIndex = 0;

        ///<summary>
        /// End turn by setting the next entity as current entity and then emitting the signal to begin that ones next turn
        ///</summary>
        public void TurnEnded()
        {
            CurrentEntity = SetNextEntity();
            CurrentEntity.TurnStarted.Emit();
        }

        ///<summary>
        /// Return next entity in list, checking wether the next entity is living or not
        ///</summary>
        private Entity SetNextEntity()
        {
            bool hasFoundNextEntity = false;
            Entity tempCurrentEntity = null;

            while (!hasFoundNextEntity)
            {
                currentEntityIndex++;

                if (currentEntityIndex == Entities.Count)
                    currentEntityIndex = 0;
                
                tempCurrentEntity = Entities[currentEntityIndex];

                if (!tempCurrentEntity.IsAlive)
                    continue;
                
                hasFoundNextEntity = true;
            }

            return tempCurrentEntity;
        }

        private void SortEntitiesBySpeed() {}

        public void SetCurrentEntity(Entity _entity)
        {
            CurrentEntity = _entity;
            CurrentEntity.TurnStarted.Emit();
        }
    }
}