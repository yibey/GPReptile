using GPReptile.Models;
using HtmlAgilityPack;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace GPReptile.Service
{
    public class NeteaseDTCrawler
    {
        private List<DayTransact> dtList;

        public List<DayTransact> getDtList()
        {
            return dtList;
        }
        public Dictionary<string, string> downloadGP()
        {

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            // Encoding encoding = Encoding.GetEncoding("GB2312");
            string url = @"http://quote.eastmoney.com/stock_list.html";
            var webGet = new HtmlWeb()
            {
                OverrideEncoding = Encoding.GetEncoding("GBK"),
                AutoDetectEncoding = true
            };
            var document = webGet.Load(url);
            var div = document.DocumentNode.SelectNodes("//div[@id='quotesearch']/ul/li/a[@target='_blank']");

            Dictionary<string, string> re = new Dictionary<string, string>();

            foreach (HtmlNode node in div)
            {
                var txt = node.InnerText;

                var name = txt.Substring(0, txt.IndexOf('('));
                var code = txt.Substring(txt.IndexOf('(') + 1).TrimEnd(')');


                if (code.StartsWith("6") || code.StartsWith("3") || code.StartsWith("0"))
                {
                    re.Add(code, name);
                }
            }
            return re;
        }


        public void download(String originalCode, String name, String fromDate, String toDate)
        {
            try
            {
                var _http = new HttpHelper(getReqUrl(originalCode, fromDate, toDate));

                String rawText = _http.CreateGetHttpResponse();
                String json = rawText.Substring(22, rawText.Length - 25);
                dynamic obj = JsonConvert.DeserializeObject<dynamic>(json);

                dtList = new List<DayTransact>();
                foreach (var item in obj.hq)
                {
                    DayTransact dt = new DayTransact();
                    dt.setCode(originalCode);
                    dt.setName(name);
                    dt.setDay(item[0].ToString());
                    dt.setTopen(Convert.ToDouble(item[1]));
                    dt.setTclose(Convert.ToDouble(item[2]));
                    dt.setChg(Convert.ToDouble(item[3]));
                    dt.setPchg(Convert.ToDouble(item[4].ToString().Replace("%", "")));
                    dt.setLow(Convert.ToDouble(item[5]));
                    dt.setHigh(Convert.ToDouble(item[6]));
                    dt.setVoturnover(Convert.ToInt64(item[7]));
                    dt.setVaturnover(Convert.ToDouble(item[8]));
                    dt.setTurnover(Convert.ToDouble(item[9].ToString().Replace("%", "")));
                    dtList.Add(dt);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }
        private String getReqUrl(String code, String startDate, String endDate)
        {
            return "http://q.stock.sohu.com/hisHq?code=cn_" + code + "&start=" + startDate + "&end=" + endDate + "&stat=1&order=D&period=d&callback=historySearchHandler&rt=jsonp";
        }
    }
}
