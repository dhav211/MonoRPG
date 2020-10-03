using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoRPG
{
    public enum SkillState { SELECT_TARGET, EXECUTING, FINISHED, NOT_IN_USE }

    public interface Skill
    {
        public string Name { get; set; }
        public Texture2D Icon { get; set; }
        public int Cost { get; set; }
        public int CooldownPeriod { get; set; }
        public int CurrentCooldown { get; set; }
        public SkillState SkillState { get; set; }
        public Entity Owner { get; set; }

        public Signal OnComplete { get; set; }
        public Signal OnUsed { get; set; }
        public Signal OnCoolDownFinished { get; set; }

        public void Update(float deltaTime);
        public void Execute();
        public void Initiate();
    }
}