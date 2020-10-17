namespace MonoRPG
{
    public class Stats : Component
    {
        public string Name { get; set; }
        public int LVL { get; set; }
        public int MaxHP { get; set; }
        int hp;
        public int HP
        {
            get { return hp; }
            set
            {
                hp = value;

                if (hp < 0)
                {
                    hp = 0;
                }
                else if (hp > MaxHP)
                {
                    hp = MaxHP;
                }
            }
        }
        public int MaxMP { get; set; }
        int mp;
        public int MP
        {
            get { return mp; }
            set
            {
                mp = value;

                if (mp < 0)
                {
                    mp = 0;
                }
                else if (mp > MaxMP)
                {
                    mp = MaxMP;
                }
            }
        }
        public int ATK { get; set; }
        public int DEF { get; set; }
        public int INT { get; set; }
        public int RES { get; set; }
        public int LUK { get; set; }
        public int SPD { get; set; }
        public int EXP { get; set; }

        public int BaseHP { get; set; }
        public int BaseMP { get; set; }
        public int BaseATK { get; set; }
        public int BaseDEF { get; set; }
        public int BaseINT { get; set; }
        public int BaseRES { get; set; }
        public int BaseLUK { get; set; }
        public int BaseSPD { get; set; }

        public Stats(Entity _owner) : base(_owner)
        {
            owner.AddComponent<Stats>(this);
        }

        public void SetStats(string _name, int _lvl, int _hp, int _mp, int _atk, int _def, int _int, int _res, int _luk, int _spd)
        {
            Name = _name;
            LVL = _lvl;
            MaxHP = _hp;
            HP = _hp;
            MaxMP = _mp;
            MP = _mp;
            ATK = _atk;
            DEF = _def;
            INT = _int;
            RES = _res;
            SPD = _spd;
            LUK = _luk;
            EXP = 0;
        }
    }
}