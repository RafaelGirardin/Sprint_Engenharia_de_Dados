using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Scraping_TJSP
{
    [PrimaryKey("Id")]
    public class Decisao
    {
        public int Id { get; set; }
        public string Ementa { get; set; }
        public string Assunto { get; set; }
        public DateTime Publicacao { get; set; }
        public string NomeRelatorId { get; set; }
        public string Comarca { get; set; }
    }
}
