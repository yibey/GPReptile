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
    public class DayTransact
    {
        public long id { get; set; }            // ID
        public String day { get; set; }        // 日期
        public String code { get; set; }        // 代号
        public String name { get; set; }        // 名称
        public double tclose { get; set; }        // 收盘价
        public double high { get; set; }        // 最高价
        public double low { get; set; }        // 最低价
        public double topen { get; set; }        // 开盘价
        public double lclose { get; set; }        // 前日收盘价
        public double chg { get; set; }        // 涨跌额
        public double pchg { get; set; }        // 涨跌幅
        public double turnover { get; set; }    // 换手率
        public long voturnover { get; set; }    // 成交量
        public double vaturnover { get; set; }    // 成交金额
        public double tcap { get; set; }        // 总市值
        public double mcap { get; set; }        // 流通市值

        public DayTransact()
        {

        }


        public DayTransact(String[] arr)
        {
            if (arr.Length != 15)
            {
                throw new Exception("Array size should be 15 but now it is " + arr.Length);
            }

            String dataLine = String.Join(",", arr);

            day = arr[0];

            try
            {
                tclose = Double.Parse(arr[3]);
            }
            catch (Exception ex)
            {
                throw new Exception("Can not get tclose from string:" + arr[3] + " dataLine:" + dataLine);
            }

            try
            {
                high = Double.Parse(arr[4]);
            }
            catch (Exception ex)
            {
                throw new Exception("Can not get high from string:" + arr[4] + " dataLine:" + dataLine);
            }

            try
            {
                low = Double.Parse(arr[5]);
            }
            catch (Exception ex)
            {
                throw new Exception("Can not get low from string:" + arr[5] + " dataLine:" + dataLine);
            }

            try
            {
                topen = Double.Parse(arr[6]);
            }
            catch (Exception ex)
            {
                throw new Exception("Can not get topen from string:" + arr[6] + " dataLine:" + dataLine);
            }

            try
            {
                lclose = Double.Parse(arr[7]);
            }
            catch (Exception ex)
            {
                throw new Exception("Can not get lclose from string:" + arr[7] + " dataLine:" + dataLine);
            }

            try
            {
                chg = Double.Parse(arr[8]);
            }
            catch (Exception ex)
            {
                throw new Exception("Can not get chg from string:" + arr[8] + " dataLine:" + dataLine);
            }

            try
            {
                pchg = Double.Parse(arr[9]);
            }
            catch (Exception ex)
            {
                throw new Exception("Can not get pchg from string:" + arr[9] + " dataLine:" + dataLine);
            }

            try
            {
                turnover = Double.Parse(arr[10]);
            }
            catch (Exception ex)
            {
                throw new Exception("Can not get turnover from string:" + arr[10] + " dataLine:" + dataLine);
            }

            try
            {
                voturnover = long.Parse(arr[11]);
            }
            catch (Exception ex)
            {
                throw new Exception("Can not get voturnover from string:" + arr[11] + " dataLine:" + dataLine);
            }

            try
            {
                vaturnover = Double.Parse(arr[12]);
            }
            catch (Exception ex)
            {
                throw new Exception("Can not get vaturnover from string:" + arr[12] + " dataLine:" + dataLine);
            }

            try
            {
                tcap = Double.Parse(arr[13]);
            }
            catch (Exception ex)
            {
                throw new Exception("Can not get tcap from string:" + arr[13] + " dataLine:" + dataLine);
            }

            try
            {
                mcap = Double.Parse(arr[14]);
            }
            catch (Exception ex)
            {
                throw new Exception("Can not get mcap from string:" + arr[14] + " dataLine:" + dataLine);
            }

        }

        public String toString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("id:" + id);
            sb.Append(" 日期day:" + day);
            sb.Append(" 代号code:" + code);
            sb.Append(" 名称name:" + name);
            sb.Append(" 收盘价tclose:" + tclose);
            sb.Append(" 最高价high:" + high);
            sb.Append(" 最低价low:" + low);
            sb.Append(" 开盘价topen:" + topen);
            sb.Append(" 前日收盘价lclose:" + lclose);
            sb.Append(" 涨跌额chg:" + chg);
            sb.Append(" 涨跌幅pchg:" + pchg);
            sb.Append(" 换手率turnover:" + turnover);
            sb.Append(" 成交量voturnover:" + voturnover);
            sb.Append(" 成交金额vaturnover:" + vaturnover);
            sb.Append(" 总市值tcap:" + tcap);
            sb.Append(" 流通市值mcap:" + mcap);

            return sb.ToString();//"code:"+code+" name:"+name+" date:"+day+" tclose:"+tclose;
        }

        public long getId()
        {
            return id;
        }
        public void setId(long id)
        {
            this.id = id;
        }
        public String getDay()
        {
            return day;
        }
        public void setDay(String day)
        {
            this.day = day;
        }
        public String getCode()
        {
            return code;
        }
        public void setCode(String code)
        {
            this.code = code;
        }
        public String getName()
        {
            return name;
        }
        public void setName(String name)
        {
            this.name = name;
        }
        public double getTclose()
        {
            return tclose;
        }
        public void setTclose(double tclose)
        {
            this.tclose = tclose;
        }
        public double getHigh()
        {
            return high;
        }
        public void setHigh(double high)
        {
            this.high = high;
        }
        public double getLow()
        {
            return low;
        }
        public void setLow(double low)
        {
            this.low = low;
        }
        public double getTopen()
        {
            return topen;
        }
        public void setTopen(double topen)
        {
            this.topen = topen;
        }
        public double getLclose()
        {
            return lclose;
        }
        public void setLclose(double lclose)
        {
            this.lclose = lclose;
        }
        public double getChg()
        {
            return chg;
        }
        public void setChg(double chg)
        {
            this.chg = chg;
        }
        public double getPchg()
        {
            return pchg;
        }
        public void setPchg(double pchg)
        {
            this.pchg = pchg;
        }
        public double getTurnover()
        {
            return turnover;
        }
        public void setTurnover(double turnover)
        {
            this.turnover = turnover;
        }
        public long getVoturnover()
        {
            return voturnover;
        }
        public void setVoturnover(long voturnover)
        {
            this.voturnover = voturnover;
        }
        public double getVaturnover()
        {
            return vaturnover;
        }
        public void setVaturnover(double vaturnover)
        {
            this.vaturnover = vaturnover;
        }
        public double getTcap()
        {
            return tcap;
        }
        public void setTcap(double tcap)
        {
            this.tcap = tcap;
        }
        public double getMcap()
        {
            return mcap;
        }
        public void setMcap(double mcap)
        {
            this.mcap = mcap;
        }

    }

}
