using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using CTT_PORTUGAL.Domain.Entities;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;

namespace CTT_PORTUGAL.Service.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(string id)
        {
            var order = new Order();
            order.CodeTracking = id;

            //FA097026122PT
            string url = $"http://www.cttexpresso.pt/feapl_2/app/open/cttexpresso/objectSearch/objectSearch.jspx?objects={id}";
            string markup;

            using (WebClient wc = new WebClient())
            {
                markup = wc.DownloadString(url);
            }

            var html = new HtmlDocument();
            html.OptionFixNestedTags = true;
            var webCliente = new RoboWebClient();

            html.LoadHtml(webCliente.DownloadString(url));

            order.From = removeTrash(html.DocumentNode.SelectNodes("//*[@id=\"vanInfo\"]/div[1]/p[1]").First().InnerText);
            order.FromLocal = removeTrash(html.DocumentNode.SelectNodes("//*[@id=\"vanInfo\"]/div[1]/p[2]/span[2]").First().InnerText);
            order.FromDate = removeTrash(html.DocumentNode.SelectNodes("//*[@id=\"vanInfo\"]/div[1]/p[3]/span").First().InnerText);
            order.FromHour = removeTrash(html.DocumentNode.SelectNodes("//*[@id=\"vanInfo\"]/div[1]/p[4]/span").First().InnerText);

            order.LastLocal = removeTrash(html.DocumentNode.SelectNodes("//*[@id=\"vanInfo\"]/div[2]/p[1]/span[2]").First().InnerText);
            order.LastDate = removeTrash(html.DocumentNode.SelectNodes("//*[@id=\"vanInfo\"]/div[2]/p[2]/span").First().InnerText);
            order.LastHour = removeTrash(html.DocumentNode.SelectNodes("//*[@id=\"vanInfo\"]/div[2]/p[3]/span").First().InnerText);

            order.To = removeTrash(html.DocumentNode.SelectNodes("//*[@id=\"vanInfo\"]/div[3]/p[1]").First().InnerText);
            order.ToLocal = removeTrash(html.DocumentNode.SelectNodes("//*[@id=\"vanInfo\"]/div[3]/p[2]/span[2]").First().InnerText);
            order.ToDate = removeTrash(html.DocumentNode.SelectNodes("//*[@id=\"vanInfo\"]/div[3]/p[3]/span").First().InnerText);
            order.ToHour = removeTrash(html.DocumentNode.SelectNodes("//*[@id=\"vanInfo\"]/div[3]/p[4]/span").First().InnerText);

            order.LastCodeUpdate = removeTrash(html.DocumentNode.SelectNodes("//*[@id=\"objectSearchResult\"]/table/tr[1]/td[1]").First().InnerText);
            order.LastProductUpdate = removeTrash(html.DocumentNode.SelectNodes("//*[@id=\"objectSearchResult\"]/table/tr[1]/td[2]").First().InnerText);
            order.LastDateUpdate = removeTrash(html.DocumentNode.SelectNodes("//*[@id=\"objectSearchResult\"]/table/tr[1]/td[3]").First().InnerText);
            order.LastHourUpdate = removeTrash(html.DocumentNode.SelectNodes("//*[@id=\"objectSearchResult\"]/table/tr[1]/td[4]").First().InnerText);
            order.LastStatusUpdate = removeTrash(html.DocumentNode.SelectNodes("//*[@id=\"objectSearchResult\"]/table/tr[1]/td[5]/a").First().InnerText);

            HtmlDocument htmlDoc = new HtmlDocument();
            var webGet = new HtmlWeb();
            var document = webGet.Load(url);


            var algo = document.DocumentNode.SelectSingleNode("//*[@id=\"details_0\"]/td/table"); //
            int countLegal = 0;
            var date = "";
            for (int i = 0; i < algo.ChildNodes.Count; i++)
            {
                var historico = new StatusOrder();

                if (algo.ChildNodes[i].Name != "#text")
                {
                    if (algo.ChildNodes[i].HasAttributes && algo.ChildNodes[i].Name == "tr")
                    {
                        //historico.Data = removeTrash(algo.ChildNodes[i].InnerText);
                        date = removeTrash(algo.ChildNodes[i].InnerText);
                    }
                    else if (!algo.ChildNodes[i].HasAttributes && algo.ChildNodes[i].Name == "tr")
                    {
                        if (algo.ChildNodes[i].ChildNodes.Count == 11)
                        {
                            foreach (var item in algo.ChildNodes[i].ChildNodes)
                            {
                                if (item.Name != "#text")
                                {
                                    if (countLegal == 0)
                                    {
                                        historico.Hour = removeTrash(item.InnerText);
                                    }
                                    else if (countLegal == 1)
                                    {
                                        historico.Status = removeTrash(item.InnerText);
                                    }
                                    else if (countLegal == 2)
                                    {
                                        historico.Reason = removeTrash(item.InnerText);
                                    }
                                    else if (countLegal == 3)
                                    {
                                        historico.Local = removeTrash(item.InnerText);
                                    }
                                    else if (countLegal == 4)
                                    {
                                        historico.Receiver = removeTrash(item.InnerText);
                                    }

                                    var itemjss = removeTrash(item.InnerText);

                                    countLegal++;
                                }
                            }

                            countLegal = 0;
                        }

                    }
                    else if (algo.ChildNodes[i].Name == "td")
                    {
                        for (int xx = 0; xx < 10; xx++)
                        {
                            if (algo.ChildNodes[i].Name != "#text")
                            {
                                if (xx == 0)
                                {
                                    historico.Hour = removeTrash(algo.ChildNodes[i].InnerText);
                                }
                                else if (xx == 2)
                                {
                                    historico.Status = removeTrash(algo.ChildNodes[i].InnerText);
                                }
                                else if (xx == 4)
                                {
                                    historico.Reason = removeTrash(algo.ChildNodes[i].InnerText);
                                }
                                else if (xx == 6)
                                {
                                    historico.Local = removeTrash(algo.ChildNodes[i].InnerText);
                                }
                                else if (xx == 8)
                                {
                                    historico.Receiver = removeTrash(algo.ChildNodes[i].InnerText);
                                }
                                var teste = removeTrash(algo.ChildNodes[i].InnerText);
                            }

                            i++;
                        }
                    }

                    if (!string.IsNullOrEmpty(historico.Status))
                    {
                        historico.Date = date;
                        order.StatusHistory.Add(historico);
                    }
                }
            }

            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        private string removeTrash(string text)
        {
            try
            {
                var gshsag = WebUtility.HtmlDecode(text);
                return gshsag.Replace("\r", "").Replace("\n", "").Replace("\t", "");
            }
            catch (Exception ex)
            {
                return "deu erro no replace";
            }
        }
    }
}
