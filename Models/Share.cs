using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPReptile.Models
{
    /**
     * 每日交易数据实体类
     * @author Ace
     *
     */
    public class Share
    {
        public long id { get; set; }            // ID

        public string name { get; set; }

        public string code { get; set; }
    }

}
