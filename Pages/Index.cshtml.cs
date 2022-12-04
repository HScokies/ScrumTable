using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ScrumTable.Hubs;

namespace ScrumTable.Pages
{
    public class IndexModel : PageModel
    {
        BridgeController DB = new BridgeController();
        private readonly ILogger<IndexModel> _logger;
        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }
        public List<ItemInfo> Items_list;
        public void OnGet()
        {
            Items_list = DB.GetData();
        }
    }
}
