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
            order.CodigoRastreio = id;

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

            order.De = removeTrash(html.DocumentNode.SelectNodes("//*[@id=\"vanInfo\"]/div[1]/p[1]").First().InnerText);
            order.DeLocal = removeTrash(html.DocumentNode.SelectNodes("//*[@id=\"vanInfo\"]/div[1]/p[2]/span[2]").First().InnerText);
            order.DeData = removeTrash(html.DocumentNode.SelectNodes("//*[@id=\"vanInfo\"]/div[1]/p[3]/span").First().InnerText);
            order.DeHora = removeTrash(html.DocumentNode.SelectNodes("//*[@id=\"vanInfo\"]/div[1]/p[4]/span").First().InnerText);

            order.UltimoLocal = removeTrash(html.DocumentNode.SelectNodes("//*[@id=\"vanInfo\"]/div[2]/p[1]/span[2]").First().InnerText);
            order.UltimaData = removeTrash(html.DocumentNode.SelectNodes("//*[@id=\"vanInfo\"]/div[2]/p[2]/span").First().InnerText);
            order.UltimaHora = removeTrash(html.DocumentNode.SelectNodes("//*[@id=\"vanInfo\"]/div[2]/p[3]/span").First().InnerText);

            order.Para = removeTrash(html.DocumentNode.SelectNodes("//*[@id=\"vanInfo\"]/div[3]/p[1]").First().InnerText);
            order.ParaLocal = removeTrash(html.DocumentNode.SelectNodes("//*[@id=\"vanInfo\"]/div[3]/p[2]/span[2]").First().InnerText);
            order.ParaData = removeTrash(html.DocumentNode.SelectNodes("//*[@id=\"vanInfo\"]/div[3]/p[3]/span").First().InnerText);
            order.ParaHora = removeTrash(html.DocumentNode.SelectNodes("//*[@id=\"vanInfo\"]/div[3]/p[4]/span").First().InnerText);

            order.UltimoAttCodigo = removeTrash(html.DocumentNode.SelectNodes("//*[@id=\"objectSearchResult\"]/table/tr[1]/td[1]").First().InnerText);
            order.UltimoAttProduto = removeTrash(html.DocumentNode.SelectNodes("//*[@id=\"objectSearchResult\"]/table/tr[1]/td[2]").First().InnerText);
            order.UltimoAttData = removeTrash(html.DocumentNode.SelectNodes("//*[@id=\"objectSearchResult\"]/table/tr[1]/td[3]").First().InnerText);
            order.UltimoAttHora = removeTrash(html.DocumentNode.SelectNodes("//*[@id=\"objectSearchResult\"]/table/tr[1]/td[4]").First().InnerText);
            order.UltimoAttEstado = removeTrash(html.DocumentNode.SelectNodes("//*[@id=\"objectSearchResult\"]/table/tr[1]/td[5]/a").First().InnerText);

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
                                        historico.UltimoAttHora = removeTrash(item.InnerText);
                                    }
                                    else if (countLegal == 1)
                                    {
                                        historico.UltimoAttEstado = removeTrash(item.InnerText);
                                    }
                                    else if (countLegal == 2)
                                    {
                                        historico.UltimoAttMotivo = removeTrash(item.InnerText);
                                    }
                                    else if (countLegal == 3)
                                    {
                                        historico.UltimoAttLocal = removeTrash(item.InnerText);
                                    }
                                    else if (countLegal == 4)
                                    {
                                        historico.UltimoAttRecetor = removeTrash(item.InnerText);
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
                                    historico.UltimoAttHora = removeTrash(algo.ChildNodes[i].InnerText);
                                }
                                else if (xx == 2)
                                {
                                    historico.UltimoAttEstado = removeTrash(algo.ChildNodes[i].InnerText);
                                }
                                else if (xx == 4)
                                {
                                    historico.UltimoAttMotivo = removeTrash(algo.ChildNodes[i].InnerText);
                                }
                                else if (xx == 6)
                                {
                                    historico.UltimoAttLocal = removeTrash(algo.ChildNodes[i].InnerText);
                                }
                                else if (xx == 8)
                                {
                                    historico.UltimoAttRecetor = removeTrash(algo.ChildNodes[i].InnerText);
                                }
                                var teste = removeTrash(algo.ChildNodes[i].InnerText);
                            }

                            i++;
                        }
                    }

                    if (!string.IsNullOrEmpty(historico.UltimoAttEstado))
                    {
                        historico.Data = date;
                        order.HistoricoStatus.Add(historico);
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
