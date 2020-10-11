using System.Collections.Generic;

namespace MonoRPG
{
    public class SkillsComponent : Component
    {
        public List<Skill> Skills { get; private set; } = new List<Skill>();
        public Skill[] HotkeySkills { get; set; } = new Skill[10];

        public Signal SkillUsed { get; private set; } = new Signal();

        public SkillsComponent(Entity _owner) : base(_owner)
        {
            owner.AddComponent<SkillsComponent>(this);
        }

        public override void Initialize()
        {
            // TODO remove all this once an equip skill function is established
            FireballSkill fireballSkill = new FireballSkill(owner);
            MolotovSkill molotovSkill = new MolotovSkill(owner);

            Skills.Add(fireballSkill);
            HotkeySkills[0] = fireballSkill;

            Skills.Add(molotovSkill);
            HotkeySkills[1] = molotovSkill;
        }

        public override void Update(float deltaTime)
        {
            for (int i = 0; i < HotkeySkills.Length; ++i)
            {
                if (HotkeySkills[i] != null)
                    HotkeySkills[i].Update(deltaTime);
            }
        }
    }
}