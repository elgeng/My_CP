using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyCP
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            testwl();
            String red, blue;
            gethtmltest(out red, out blue);
        }

        private void testcmp()
        {
            String test1 = "";
            int ret = 0;
            richTextBox1.Clear();

            ret = redcmp("01,02,03,04,05,06", "01,02,03,04,05,06", out test1);
            richTextBox1.Text += "ret:" + ret + ", test:" + test1 + "\n";

            ret = redcmp("01,02,03,04,05,06", "06,05,04,03,02,01", out test1);
            richTextBox1.Text += "ret:" + ret + ", test:" + test1 + "\n";

            ret = redcmp("31,02,03,04,15,06", "16,05,04,03,22,01", out test1);
            richTextBox1.Text += "ret:" + ret + ", test:" + test1 + "\n";

            ret = redcmp("31,02,33,24,15,06", "16,15,34,23,22,11", out test1);
            richTextBox1.Text += "ret:" + ret + ", test:" + test1 + "\n";

            ret = redcmp("31,02,33,24,05,06", "16,15,34,23,22,11", out test1);
            richTextBox1.Text += "ret:" + ret + ", test:" + test1 + "\n";
        }

        private void testwl()
        {
            String test1 = "";
            double wmoney = 0;
            int ret = 0;
            richTextBox1.Clear();

            ret = calcwinlevel("01,02,03,04,05,06", "01,02,03,04,05,06", "06", "06", out test1, out wmoney);
            richTextBox1.Text += "ret:" + ret + ", test:" + test1 + ", wmoney:" + wmoney + "\n";

            ret = calcwinlevel("01,02,03,04,05,06", "06,05,04,03,02,01", "06", "09", out test1, out wmoney);
            richTextBox1.Text += "ret:" + ret + ", test:" + test1 + ", wmoney:" + wmoney + "\n";

            ret = calcwinlevel("31,02,03,04,15,06", "16,05,04,03,22,01", "06", "06", out test1, out wmoney);
            richTextBox1.Text += "ret:" + ret + ", test:" + test1 + ", wmoney:" + wmoney + "\n";

            ret = calcwinlevel("31,02,33,24,15,06", "16,15,34,23,22,11", "06", "06", out test1, out wmoney);
            richTextBox1.Text += "ret:" + ret + ", test:" + test1 + ", wmoney:" + wmoney + "\n";

            ret = calcwinlevel("31,02,33,24,05,06", "16,15,34,23,22,11", "06", "06", out test1, out wmoney);
            richTextBox1.Text += "ret:" + ret + ", test:" + test1 + ", wmoney:" + wmoney + "\n";

            ret = calcwinlevel("31,02,03,04,15,06", "16,05,04,03,22,01", "06", "09", out test1, out wmoney);
            richTextBox1.Text += "ret:" + ret + ", test:" + test1 + ", wmoney:" + wmoney + "\n";

            ret = calcwinlevel("31,02,33,24,15,06", "31,02,33,24,05,06", "06", "06", out test1, out wmoney);
            richTextBox1.Text += "ret:" + ret + ", test:" + test1 + ", wmoney:" + wmoney + "\n";

            ret = calcwinlevel("31,02,33,24,05,06", "31,02,33,24,05,16", "06", "09", out test1, out wmoney);
            richTextBox1.Text += "ret:" + ret + ", test:" + test1 + ", wmoney:" + wmoney + "\n";

            ret = calcwinlevel("31,02,33,24,05,06", "31,02,33,14,25,16", "06", "06", out test1, out wmoney);
            richTextBox1.Text += "ret:" + ret + ", test:" + test1 + ", wmoney:" + wmoney + "\n";

            ret = calcwinlevel("31,02,33,24,05,06", "31,02,33,14,25,16", "06", "02", out test1, out wmoney);
            richTextBox1.Text += "ret:" + ret + ", test:" + test1 + ", wmoney:" + wmoney + "\n";
        }


        private int redcmp(string redwin, string myred, out string resred)
        {
            String[] redwins = redwin.Split(',');
            String[] myreds = myred.Split(',');
            resred = "";

            List<String> resredlist = new List<string>();

            foreach (string w in redwins)
            {
                foreach (string m in myreds)
                {
                    if (m == w)
                    {
                        resredlist.Add(m);
                    }
                }
            }

            foreach (string s in resredlist)
            {
                resred += s + ",";
            }

            return resredlist.Count;
        }

        private int calcwinlevel(string redwin, string myred, string bluewin, string myblue, out string resred, out double winmoney)
        {
            resred = "";
            winmoney = 0;

            int redret = redcmp(redwin, myred, out resred);

            if (bluewin == myblue)
            {
                switch (redret)
                {
                    case 0:
                    case 1:
                    case 2:
                        winmoney = 5;
                        return 6;
                    case 3:
                        winmoney = 10;
                        return 5;
                    case 4:
                        winmoney = 200;
                        return 4;
                    case 5:
                        winmoney = 3000;
                        return 3;
                    case 6:
                        winmoney = 5000000;
                        return 1;
                }
            }
            else
            {
                switch (redret)
                {
                    case 4:
                        winmoney = 10;
                        return 5;
                    case 5:
                        winmoney = 200;
                        return 4;
                    case 6:
                        winmoney = 300000;
                        return 2;
                }
            }
            return 0;
        }

        private int gethtmltest(out String redballs, out String blueball)
        {
            redballs = blueball = null;
            try
            {
                WebClient webClient = new WebClient();
                webClient.Credentials = CredentialCache.DefaultCredentials;//获取或设置用于向Internet资源的请求进行身份验证的网络凭据  

                Byte[] pageData = webClient.DownloadData("http://caipiao.163.com/award/ssq/2013125.html");

                String charSet = "";
                string pageHtml = Encoding.Default.GetString(pageData);

                //获取网页字符编码描述信息
                Match charSetMatch = Regex.Match(pageHtml, "<meta([^<]*)charset=([^<]*)\"", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                string webCharSet = charSetMatch.Groups[2].Value;
                if (charSet == null || charSet == "")
                    charSet = webCharSet;

                if (charSet != null && charSet != "" && Encoding.GetEncoding(charSet) != Encoding.Default)
                {
                    pageHtml = Encoding.GetEncoding(charSet).GetString(pageData);
                }

                Match zjMatch = Regex.Match(pageHtml, "<p id=\"zj_area\">(.*?)</p>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
                if (zjMatch.Success)
                {
                    String zjString = zjMatch.Groups[0].Value;
                    redballs = blueball = "";
                    int redct = 0;
                    Match ballMatch = Regex.Match(zjString, "<span class=\"red_ball\">([^<]*)</span>", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                    while (ballMatch.Success)
                    {
                        redct++;
                        redballs += ballMatch.Groups[1].Value;
                        ballMatch = ballMatch.NextMatch();
                    }
                    if (redct == 6)
                    {
                        ballMatch = Regex.Match(zjString, "<span class=\"blue_ball\">([^<]*)</span>", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        if (ballMatch.Success)
                        {
                            blueball = ballMatch.Groups[1].Value;
                            return 1;
                        }
                    }

                }

                return -1;

            }
            catch (Exception webEx)
            {
                MessageBox.Show(webEx.Message);
                return -9;
            }
        }



        ///*
        //* 财付通 彩票 json
        //* http://caipiao.tenpay.com/lottery/query_bonus_notice.cgi?type=ssq&issue=2013127&jsonObj=json_award
        //*var json_award = {"drawTime":"2013-10-29 21:30:00","retcode":"0","dateIssueList":null,"issue":"2013127","todayIssue":false,"issueList":null,"type":"ssq","pools":"197309615","info":[{"name":"一等奖","money":"11767862","globalCount":5,"totalMoney":"58839310"},{"name":"二等奖","money":"165550","globalCount":216,"totalMoney":"35758800"},{"name":"三等奖","money":"3000","globalCount":1160,"totalMoney":"3480000"},{"name":"四等奖","money":"200","globalCount":66620,"totalMoney":"13324000"},{"name":"五等奖","money":"10","globalCount":1237003,"totalMoney":"12370030"},{"name":"六等奖","money":"5","globalCount":7245112,"totalMoney":"36225560"}],"sales":"376726836","codes":"02,03,13,20,22,33|14","retmsg":"OK","queryDate":null}; /


        //QQ彩票 http://888.qq.com/static/kaijiang/ssq/13127.shtml

    }

}
