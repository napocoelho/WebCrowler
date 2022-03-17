using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.IO;

using CrowlerLib;

namespace SiteDownloader
{
    public partial class Main : Form
    {
        private CrowlerLib.WebCrowler WebCrowler { get; set; }

        public Main()
        {
            InitializeComponent();

            /*
            this.Limpar(@"c:\LeLivros\linksEncontrados.txt");
            //this.Limpar(@"c:\LeLivros\linksIgnorados.txt");
            this.Limpar(@"c:\LeLivros\linksProcessados.txt");
            this.Limpar(@"c:\LeLivros\linksProcessando.txt");
            this.Limpar(@"c:\LeLivros\linksQuebrados.txt");
            this.Limpar(@"c:\LeLivros\links.txt");
            */

            this.WebCrowler = new CrowlerLib.WebCrowler();

        }

        private void Limpar(string arquivo)
        {   
            ConcurrentHashSet<string> set = new ConcurrentHashSet<string>();

            //arquivo = @"c:\LeLivros\linksQuebrados.txt";
            

            using (StreamReader reader = File.OpenText(arquivo))
            {
                while (!reader.EndOfStream)
                {
                    string url = reader.ReadLine().Trim();

                    if (url.Trim() == string.Empty)
                        continue;

                    /*
                    if (url.EndsWithAny("/feed/", "/feed"))
                        continue;
                    */

                    try
                    {
                        Uri uri = new Uri(url);

                        if (uri.Fragment != string.Empty)
                        {
                            string urlTest = url.Replace(uri.Fragment, "");
                            new Uri(urlTest); //--> verificando se é um url válido
                            url = urlTest.Trim();
                        }

                        set.Add(url);
                    }
                    catch (Exception ex)
                    {
                        string msg = ex.Message;
                    }
                }
            }


            string[] ordered = set.ToArray();
            Array.Sort(ordered);
            File.Delete(arquivo);
            File.WriteAllLines(arquivo, ordered);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            BtnBuscar.Enabled = false;

            try
            {
                this.WebCrowler.IsDoNotDuplicateFilesActived = chkNãoDuplicarArquivos.Checked;
                this.WebCrowler.IgnoreUrlFragmentPart = chkIgnoreUrlFramentPart.Checked;

                this.WebCrowler.TaskEnqueuer.MaxSimultaneousThreads = int.Parse("0" + txtMáximoDeThreads.Text);
                this.WebCrowler.DownloadFolderPath = txtDir.Text;

                if (chkReiniciarComArquivos.Checked)
                {
                    this.RecuperarLog();
                    this.WebCrowler.AddThread(this.WebCrowler.ProcessingQueue.Count());
                }

                string listaDeLinks = this.txtListaDeLinksIgnorados.Text;
                listaDeLinks = listaDeLinks.Replace("\r", "");

                foreach (string link in listaDeLinks.Split("\n", StringSplitOptions.RemoveEmptyEntries))
                {
                    string url = !link.StartsWith("http") ? "http://" + link : link;
                    this.WebCrowler.IgnoredLinks.Add(url);
                }



                listaDeLinks = txtFiltrarLinksIniciadosCom.Text;
                listaDeLinks = listaDeLinks.Replace("\r", "");

                foreach (string link in listaDeLinks.Split("\n", StringSplitOptions.RemoveEmptyEntries))
                {
                    string url = !link.StartsWith("http") ? "http://" + link : link;
                    this.WebCrowler.LinksStartedWithFilter.Add(url);
                }

                listaDeLinks = txtFiltrarLinksTerminadosCom.Text;
                listaDeLinks = listaDeLinks.Replace("\r", "");

                foreach (string text in listaDeLinks.Split("\n", StringSplitOptions.RemoveEmptyEntries))
                {   
                    this.WebCrowler.LinksEndedWithFilter.Add(text);
                }



                listaDeLinks = txtExtensõesDeArquivos.Text;
                listaDeLinks = listaDeLinks.Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace(" ", "").ToLower();

                foreach (string link in listaDeLinks.Split(";", StringSplitOptions.RemoveEmptyEntries))
                {
                    this.WebCrowler.FileExtensionFilter.Add(link);
                }



                listaDeLinks = txtListaDeLinksParaExplorar.Text;
                listaDeLinks = listaDeLinks.Replace("\r", "");

                foreach (string link in listaDeLinks.Split("\n", StringSplitOptions.RemoveEmptyEntries))
                {
                    string url = !link.StartsWith("http") ? "http://" + link : link;
                    this.WebCrowler.AnalyseUrl(url, true);
                }
                
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                BtnBuscar.Enabled = true;
                return;
            }



            System.Threading.Thread.Sleep(500);

            do
            {
                Dispatcher.Run(() =>
                    {
                        StringBuilder builder = new StringBuilder();

                        builder.AppendLine("Threads ativas:    \t" + this.WebCrowler.TaskEnqueuer.ProcessingTasksCount.ToString());
                        builder.AppendLine("Links encontrados: \t" + this.WebCrowler.FoundLinks.Count.ToString());
                        builder.AppendLine("Links processando: \t" + this.WebCrowler.ProcessingQueue.Count.ToString());
                        builder.AppendLine("Links processados: \t" + this.WebCrowler.ProcessedLinks.Count.ToString());
                        builder.AppendLine("Links ignorados:   \t" + this.WebCrowler.IgnoredLinks.Count.ToString());
                        builder.AppendLine("Links quebrados:   \t" + this.WebCrowler.BrokedLinks.Count.ToString());
                        builder.AppendLine("LeLivros:          \t" + this.WebCrowler.LeLivrosDownloadLinks.Count.ToString());

                        txtMonitor.Text = builder.ToString();
                    });




                System.Threading.Thread.Sleep(200);
                Application.DoEvents();
            } while (!this.WebCrowler.TaskEnqueuer.IsCompleted);



            // ;;Gravando LOG;;
            //---------------------------------------------------------------------------------------------------//

            this.GravarLog();

            //---------------------------------------------------------------------------------------------------//


            BtnBuscar.Enabled = true;
        }

        private void btnPausar_Click(object sender, EventArgs e)
        {
            if (!this.WebCrowler.TaskEnqueuer.IsPaused)
            {
                this.WebCrowler.TaskEnqueuer.IsPaused = true;
                this.btnPausar.Text = "Continuar";
            }
            else
            {
                this.WebCrowler.TaskEnqueuer.IsPaused = false;
                this.btnPausar.Text = "Pausar";
            }
        }

        private void btnParar_Click(object sender, EventArgs e)
        {
            this.WebCrowler.TaskEnqueuer.Stop();
        }

        private void txtMáximoDeThreads_Validated(object sender, EventArgs e)
        {
            try
            {
                this.WebCrowler.TaskEnqueuer.MaxSimultaneousThreads = int.Parse(txtMáximoDeThreads.Text);
            }
            catch (Exception ex)
            {
                txtMáximoDeThreads.Text = this.WebCrowler.TaskEnqueuer.MaxSimultaneousThreads.ToString();
            }
        }

        private void btnDir_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.Description = "Escolha a pasta para fazer os downloads.";
            folderBrowserDialog1.ShowNewFolderButton = true;
            folderBrowserDialog1.ShowDialog(this);

            if (System.IO.Directory.Exists(folderBrowserDialog1.SelectedPath))
            {
                txtDir.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void btnLog_Click(object sender, EventArgs e)
        {
            this.GravarLog();
        }

        private void btnObterDominiosEncontrados_Click(object sender, EventArgs e)
        {
            HashSet<string> set = new HashSet<string>();

            Dispatcher.Run(() =>
            {

                foreach (string url in this.WebCrowler.FoundLinks.ToList())
                {
                    Uri uri = new Uri(url, UriKind.RelativeOrAbsolute);

                    if (uri.IsAbsoluteUri)
                    {
                        string domain = uri.Host;
                        set.Add(domain);
                    }
                }
            });

            StringBuilder builder = new StringBuilder("");

            foreach (string domain in set)
            {
                builder.AppendLine(domain);
            }

            txtListaDominiosEncontrados.Text = builder.ToString();
        }

        private void btnResetar_Click(object sender, EventArgs e)
        {
            this.WebCrowler.BrokedLinks.Clear();
            this.WebCrowler.FoundLinks.Clear();
            this.WebCrowler.IgnoredLinks.Clear();
            this.WebCrowler.ProcessedLinks.Clear();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            //txtListaProblemasEncontrados.Text = string.Join(Environment.NewLine, this.WebCrowler.ExceptionList.ToArray());

            StringBuilder builder = new StringBuilder("");

            Dispatcher.Run(() =>
            {

                foreach (HttpRequestException exception in this.WebCrowler.ExceptionList.ToList())
                {
                    builder.AppendLine("Message: " + exception.Message);
                    builder.AppendLine("Url: " + exception.Url);
                    builder.AppendLine("*************************************");
                }
            });

            txtListaProblemasEncontrados.Text = builder.ToString();
        }

        private void GravarLog()
        {
            Dispatcher.Run(() =>
                {
                    Statement state = this.WebCrowler.GetStatementCopy();

                    GravarLinksEmArquivo(Path.Combine(this.WebCrowler.DownloadFolderPath, "linksQuebrados.txt"), state.BrokedLinks.ToArray());

                    GravarLinksEmArquivo(Path.Combine(this.WebCrowler.DownloadFolderPath, "linksIgnorados.txt"), state.IgnoredLinks.ToArray());

                    GravarLinksEmArquivo(Path.Combine(this.WebCrowler.DownloadFolderPath, "linksProcessados.txt"), state.ProcessedLinks.ToArray());

                    GravarLinksEmArquivo(Path.Combine(this.WebCrowler.DownloadFolderPath, "linksEncontrados.txt"), state.FoundLinks.ToArray());

                    GravarLinksEmArquivo(Path.Combine(this.WebCrowler.DownloadFolderPath, "linksProcessando.txt"), state.ProcessingQueue.ToArray());

                    GravarLinksEmArquivo(Path.Combine(this.WebCrowler.DownloadFolderPath, "linksDocumentosDoLeLivros.txt"), state.LeLivrosDownloadLinks.ToArray());
                });
        }

        private void RecuperarLog()
        {
            this.WebCrowler.BrokedLinks.AddRange(LoadFile(this.WebCrowler.DownloadFolderPath, "linksQuebrados.txt"));
            this.WebCrowler.IgnoredLinks.AddRange(LoadFile(this.WebCrowler.DownloadFolderPath, "linksIgnorados.txt"));
            this.WebCrowler.ProcessedLinks.AddRange(LoadFile(this.WebCrowler.DownloadFolderPath, "linksProcessados.txt"));
            this.WebCrowler.FoundLinks.AddRange(LoadFile(this.WebCrowler.DownloadFolderPath, "linksEncontrados.txt"));
            this.WebCrowler.LeLivrosDownloadLinks.AddRange(LoadFile(this.WebCrowler.DownloadFolderPath, "linksDocumentosDoLeLivros.txt"));
            this.WebCrowler.ProcessingQueue.AddRange(LoadFile(this.WebCrowler.DownloadFolderPath, "linksProcessando.txt"));
        }

        private ConcurrentHashSet<string> LoadFile(string directoryPath, string file)
        {
            return LoadFile(Path.Combine(directoryPath, file));
        }

        private ConcurrentHashSet<string> LoadFile(string path)
        {
            ConcurrentHashSet<string> content = new ConcurrentHashSet<string>();

            try
            {
                string text = "";

                using (StreamReader reader = File.OpenText(path))
                {
                    text = reader.ReadToEnd();
                }

                foreach (string row in text.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries))
                {
                    content.Add(row.Trim());
                }
            }
            finally { }

            return content;
        }

        private void GravarLinksEmArquivo(string fullPath, string[] links)
        {
            Array.Sort(links);
            System.IO.File.WriteAllLines(fullPath, links);
        }

        private void chkNãoDuplicarArquivos_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}