using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Noyau.Cards.model
{
    interface IEquipment
    {
        void Equipe(Player target);
        void Unequipe(Player target);
    }
}
