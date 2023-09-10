using Scraping_TJSP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVP_Especializacao
{
    public class Orquestrador
    {
        public static void Main(string[] args) 
        {
            var navegacao = new Navegacao();
            navegacao.Tjsp();
        }
    }
}
