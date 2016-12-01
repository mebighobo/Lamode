using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Lamode.Models
{
    public class ModelsArtist
    {
      
        public static async Task StartCrawlerAsync()
        {
            var url = "http://www.imdb.com/imdbpicks/celebrity-doppelgangers/rg1875155712?page=2&pf_rd_m=A2FGELUUNOQJNL&pf_rd_p=&pf_rd_r=14GHZ2NXBEQYQVGK47TX&pf_rd_s=center-3&pf_rd_t=15081&pf_rd_i=&ref_=pks_mg_mi_mi_sm";
            var httpClient = new HttpClient();
            var html = await httpClient.GetStringAsync(url);

        }

        
    }
}
