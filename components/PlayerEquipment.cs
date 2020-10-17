using Microsoft.Xna.Framework;

namespace MonoRPG
{
    public class PlayerEquipment : Component
    {
        public Equipment LHand { get; private set; }
        public Equipment RHand { get; private set; }
        public Equipment Head { get; private set; }
        public Equipment Legs { get; private set; }
        public Equipment Feet { get; private set; }
        public Equipment Accessory { get; private set; }

        Stats playerStats;

        public PlayerEquipment(Entity _owner) : base(_owner)
        {
            owner.AddComponent<PlayerEquipment>(this);
        }

        public override void Initialize()
        {
            playerStats = owner.GetComponent<Stats>();
        }

        public void Equip(Equipment _equipmentToEquip)
        {
            switch(_equipmentToEquip.EquipPosition)
            {
                case EquipPosition.L_HAND:
                {
                    if (LHand != null)
                    {
                        Remove(EquipPosition.L_HAND);
                    }
                    LHand = _equipmentToEquip;
                    break;
                }

                case EquipPosition.R_HAND:
                {
                    if (RHand != null)
                    {
                        Remove(EquipPosition.R_HAND);
                    }
                    RHand = _equipmentToEquip;
                    break;
                }

                case EquipPosition.HEAD:
                {
                    if (Head != null)
                    {
                        Remove(EquipPosition.HEAD);
                    }
                    Head = _equipmentToEquip;
                    break;
                }

                case EquipPosition.LEGS:
                {
                    if (Legs != null)
                    {
                        Remove(EquipPosition.LEGS);
                    }
                    Legs = _equipmentToEquip;
                    break;
                }

                case EquipPosition.FEET:
                {
                    if (Feet != null)
                    {
                        Remove(EquipPosition.FEET);
                    }
                    Feet = _equipmentToEquip;
                    break;
                }

                case EquipPosition.ACESSORY:
                {
                    if (Accessory != null)
                    {
                        Remove(EquipPosition.ACESSORY);
                    }
                    Accessory = _equipmentToEquip;
                    break;
                }
            }

            playerStats.MaxHP += _equipmentToEquip.HP;
            playerStats.MaxMP += _equipmentToEquip.MP;
            playerStats.ATK += _equipmentToEquip.ATK;
            playerStats.DEF += _equipmentToEquip.DEF;
            playerStats.INT += _equipmentToEquip.INT;
            playerStats.RES += _equipmentToEquip.RES;
            playerStats.LUK += _equipmentToEquip.LUK;
            playerStats.SPD += _equipmentToEquip.SPD;
        }

        public void Remove(EquipPosition _position)
        {
            Equipment equipmentToRemove = null;

            switch (_position)
            {
                case EquipPosition.L_HAND:
                {
                    equipmentToRemove = LHand;
                    LHand = null;
                    break;
                }

                case EquipPosition.R_HAND:
                {
                    equipmentToRemove = RHand;
                    RHand = null;
                    break;
                }

                case EquipPosition.HEAD:
                {
                    equipmentToRemove = Head;
                    Head = null;
                    break;
                }

                case EquipPosition.LEGS:
                {
                    equipmentToRemove = Legs;
                    Legs = null;
                    break;
                }

                case EquipPosition.FEET:
                {
                    equipmentToRemove = Legs;
                    Feet = null;
                    break;
                }

                case EquipPosition.ACESSORY:
                {
                    equipmentToRemove = Accessory;
                    Accessory = null;
                    break;
                }
            }

            playerStats.MaxHP -= equipmentToRemove.HP;
            playerStats.MaxMP -= equipmentToRemove.MP;
            playerStats.ATK -= equipmentToRemove.ATK;
            playerStats.DEF -= equipmentToRemove.DEF;
            playerStats.INT -= equipmentToRemove.INT;
            playerStats.RES -= equipmentToRemove.RES;
            playerStats.LUK -= equipmentToRemove.LUK;
            playerStats.SPD -= equipmentToRemove.SPD;
        }
    }
}