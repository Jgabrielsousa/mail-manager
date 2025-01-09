using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailManager.Application.Abstraction.Config;


public class AppSettings
{
    public List<Sources> Sources { get; set; }
}
public class Sources {
        public string Name { get; set; }
        public string Url { get; set; }
        public string ApiKey { get; set; }
        public string ApiId { get; set; }
}

