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

        //public override string ToString()
        //{
        //    return $"ID: {MonsterId}, Name: {MonsterName}, AbilityId: {AbilityId}, HP: {HP}, Attack: {Attack}, Defense: {Defense}, Exp :{Exp}";
        //}

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

            //MonsterId = (int)row["MonsterId"];
            //MonsterName = (string)row["MonsterName"];
            //AbilityId = (string)row["AbilityId"];
            //HP = (int)row["HP"];
            //Attack = (int)row["Attack"];
            //Defense = (int)row["Defense"];
            //Exp = (int)row["Exp"];

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


       
        public int CalculateDamage(int attackerAttack, int defenderDefense)
        {
            int damage = attackerAttack - defenderDefense;
            return damage > 0 ? damage : 0; // 確保傷害不會為負
        }


        public void Battle(User user, Monster monster)
        {
            while (user.HP > 0 && monster.HP > 0)
            {
                // 玩家攻擊怪物
                int userDamage = CalculateDamage(user.Attack, monster.Defense);
                monster.HP -= userDamage;
                Console.WriteLine($"{user.UserName} 攻擊 {monster.MonsterName}造成了 {userDamage} 傷害。 {monster.MonsterName} 剩餘血量: {monster.HP}");

                if (monster.HP <= 0)
                {
                    Console.WriteLine($"{monster.MonsterName} 已死亡!");
                    user.CurrentExp += monster.Exp;
                    Console.WriteLine($"{user.UserName}獲得了{monster.Exp}點經驗");
                    break;
                }

                // 怪物攻擊玩家
                int monsterDamage = CalculateDamage(monster.Attack, user.Defense);
                user.HP -= monsterDamage;
                Console.WriteLine($"{monster.MonsterName} 攻擊 {user.UserName}造成了 {monsterDamage} 傷害。 {user.UserName} 剩餘血量: {user.HP}");

                if (user.HP <= 0)
                {
                    Console.WriteLine("{user.UserName} 已死亡!");
                    break;
                }
            }
        }



    }
}
