using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Edge;
using System.Text.RegularExpressions;
using System.Globalization;
using System;

namespace Scraping_TJSP
{
    public class Navegacao
    {
        private IWebDriver driver;
        public IDictionary<string, object> vars { get; private set; }
        private IJavaScriptExecutor js;

        protected void TearDown()
        {
            driver.Quit();
        }
        public void Tjsp()
        {
            driver = new EdgeDriver();
            js = (IJavaScriptExecutor)driver;
            vars = new Dictionary<string, object>();
            driver.Navigate().GoToUrl("https://esaj.tjsp.jus.br/cjsg/resultadoCompleta.do");
            Thread.Sleep(1000);
            driver.Manage().Window.Size = new System.Drawing.Size(1552, 832);
            Thread.Sleep(1000);
            driver.FindElement(By.Id("iddados.buscaInteiroTeor")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.Name("dados.buscaEmenta")).SendKeys("\"provido\" \"PROVIDO\"");
            Thread.Sleep(1000);
            driver.FindElement(By.Id("botaoProcurar_assuntos")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.Id("assuntos_tree_node_12194")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector(".popupBar:nth-child(3) .spwBotaoDefaultGrid")).Click();
            {
                var element = driver.FindElement(By.Id("Dcheckbox"));
                Actions builder = new Actions(driver);
                builder.MoveToElement(element).Perform();
            }
            Thread.Sleep(1000);
            driver.FindElement(By.Id("Dcheckbox")).Click();
            {
                var element = driver.FindElement(By.TagName("body"));
                Actions builder = new Actions(driver);
                builder.MoveToElement(element, 0, 0).Perform();
            }
            Thread.Sleep(1000);
            driver.FindElement(By.Id("pbSubmit")).Click();
            Thread.Sleep(1000);
            var decisoes = new List<string>();
            for (int i = 0; i < 271; i++)
            {
                this.CapturaDecisoes(decisoes);
                driver.FindElement(By.Name("A" + (i + 2))).Click();
                Thread.Sleep(1000);
            }
            var decisoesComStatus = this.ApagaDecisoesSemStatus(decisoes);
            this.IncorporaNoBanco(decisoesComStatus);
        }

        private List<string> ApagaDecisoesSemStatus(List<string> decisoes)
        {
            var decisoesComStatus = new List<string>();
            for (int i = 0; i < decisoes.Count; i++)
            {
                var decisao = decisoes.ElementAt(i);
                if (decisao.Contains("provido"))
                {
                    decisoesComStatus.Add(decisao);
                }
            }
            return decisoesComStatus;
        }

        private void IncorporaNoBanco(List<string> decisoesComStatus)
        {
            var id = 1;
            foreach (var decisaoComStatus in decisoesComStatus)
            {
                var regexEmenta = new Regex("ementa: (?<conteudo>[\\s\\S]{1,})");
                var regexComarca = new Regex("comarca: (?<comarca>.{1,})[\\s\\S]{1,}(órgão)");
                var regexRelator = new Regex("relator(\\(a\\)){0,1}: (?<relator>.{1,})[\\s\\S](comarca)");
                var regexPublicacao = new Regex("publicação: (?<publicacao>.{1,})[\\s\\S](ementa)");
                var regexAssunto = new Regex("assunto: (?<assunto>.{1,})[\\s\\S](relator)");
                var ementa = regexEmenta.Match(decisaoComStatus).Groups["conteudo"].Value;
                var relator = regexRelator.Match(decisaoComStatus).Groups["relator"].Value.Replace("\r", "");
                var comarca = regexComarca.Match(decisaoComStatus).Groups["comarca"].Value.Replace("\r", "");
                var publicacao = DateTime.Parse(regexPublicacao.Match(decisaoComStatus).Groups["publicacao"].Value.Replace("\r", "").Replace("/", "-"), new CultureInfo("pt-br"));
                var assunto = regexAssunto.Match(decisaoComStatus).Groups["assunto"].Value.Replace("\r", "");
                var relatorBanco = new Relator() { NomeRelatorId = relator };
                var decisaoBanco = new Decisao() { Ementa = ementa, Assunto = assunto, NomeRelatorId = relator, Comarca = comarca, Publicacao = publicacao };
                id++;
                try
                {
                    using var context = new DatabaseConfiguracao();
                    context.Add(relatorBanco);
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    Console.Write(ex.ToString());
                }
                try
                {
                    using var context = new DatabaseConfiguracao();
                    context.Add(decisaoBanco);
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }

        private void CapturaDecisoes(List<string> decisoes)
        {
            var decisoesTags = driver.FindElements(By.ClassName("fundocinza1"));
            foreach (var ele in decisoesTags)
            {
                decisoes.Add(ele.Text.ToString().ToLower());
            }
        }
    }
}