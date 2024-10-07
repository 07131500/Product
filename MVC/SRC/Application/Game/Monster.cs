using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    class Monster
    {
        public int MonsterId { get; set; }
        public string MonsterName { get; set; }
        public string AbilityId { get; set; }
        public int HP { get; set; }
        public int Attack { get; set; }
        public int Defense { get; set; }
        public int Exp { get; set; }

        public override string ToString()
        {
            return $"ID: {MonsterId}, Name: {MonsterName}, AbilityId: {AbilityId}, HP: {HP}, Attack: {Attack}, Defense: {Defense}, Exp :{Exp}";
        }



        public static  Monster GenerateRandomMonster(SQL ms)
        {
            DataTable monsterTable = ms.QueryMonster();

            if (monsterTable.Rows.Count == 0)
            {
                return null; // 沒有怪物資料
            }

            Random random = new Random();
            int index = random.Next(monsterTable.Rows.Count);

            DataRow row = monsterTable.Rows[index];
            return new Monster
            {
                MonsterId = (int)row["MonsterId"],
                MonsterName = (string)row["MonsterName"],
                AbilityId = (string)row["AbilityId"],
                HP = (int)row["HP"],
                Attack = (int)row["Attack"],
                Defense = (int)row["Defense"],
                Exp=(int)row["Exp"]
            };
        }

    }
}
