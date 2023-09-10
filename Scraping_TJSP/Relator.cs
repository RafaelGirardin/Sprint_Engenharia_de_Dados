using Microsoft.EntityFrameworkCore;

namespace Scraping_TJSP
{
    [PrimaryKey("NomeRelatorId")]
    public class Relator
    {
        public string NomeRelatorId { get; set; }
        public char Sexo { get; set; }
    }
}
