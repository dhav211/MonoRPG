namespace MonoRPG
{
    public class Stats : Component
    {
        public string Name { get; set; }
        public int LVL { get; set; }
        public int MaxHP { get; set; }
        public int HP { get; set; }
        public int MaxMP { get; set; }
        public int MP { get; set; }
        public int ATK { get; set; }
        public int DEF { get; set; }
        public int INT { get; set; }
        public int RES { get; set; }

        public Stats(Entity _owner) : base(_owner)
        {
            owner.AddComponent<Stats>(this);
        }

        public void SetStats(string _name, int _lvl, int _hp, int _mp, int _atk, int _def, int _int, int _res)
        {
            Name = _name;
            LVL = _lvl;
            HP = _hp;
            MaxHP = _hp;
            MP = _mp;
            MaxMP = _mp;
            ATK = _atk;
            DEF = _def;
            INT = _int;
            RES = _res;
        }
    }
}