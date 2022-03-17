using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using System.Threading;
using System.IO;

namespace CrowlerLib
{
    public class WebCrowler : BindableBase
    {
        private object LOCK_QUEUING = new object();

        public bool IsDoNotDuplicateFilesActived { get { return base.GetSync<bool>(); } set { base.SetSync(value); } }
        public bool IgnoreUrlFragmentPart { get { return base.GetSync<bool>(); } set { base.SetSync(value); } }

        public TaskEnqueuer TaskEnqueuer { get; private set; }

        public ConcurrentHashSet<string> BrokedLinks { get { return base.GetSync<ConcurrentHashSet<string>>(); } private set { base.SetSync(value); } }
        public ConcurrentHashSet<string> FoundLinks { get { return base.GetSync<ConcurrentHashSet<string>>(); } private set { base.SetSync(value); } }
        public ConcurrentHashSet<string> IgnoredLinks { get { return base.GetSync<ConcurrentHashSet<string>>(); } private set { base.SetSync(value); } }
        public ConcurrentHashSet<string> ProcessedLinks { get { return base.GetSync<ConcurrentHashSet<string>>(); } private set { base.SetSync(value); } }
        public ConcurrentQueue<string> ProcessingQueue { get { return base.GetSync<ConcurrentQueue<string>>(); } private set { base.SetSync(value); } }

        public ConcurrentHashSet<string> LeLivrosDownloadLinks { get { return base.GetSync<ConcurrentHashSet<string>>(); } private set { base.SetSync(value); } }

        public int TryTimesToBrokedLinks { get { return base.GetSync<int>(); } set { base.SetSync(value); } }
        public int IntervalBetweenTriesToBrokedLinks { get { return base.GetSync<int>(); } set { base.SetSync(value); } }

        public ConcurrentHashSet<HttpRequestException> ExceptionList { get; private set; }

        public List<String> LinksStartedWithFilter { get; private set; }
        public List<String> LinksEndedWithFilter { get; private set; }
        public ConcurrentHashSet<string> FileExtensionFilter { get { return base.GetSync<ConcurrentHashSet<string>>(); } private set { base.SetSync(value); } }

        public string DownloadFolderPath { get { return base.GetSync<string>(); } set { if (!this.TaskEnqueuer.IsProcessing) { base.SetSync(value); } } }

        public event EventHandler WorkIsCompleteEvent;

        private void OnWorkIsComplete()
        {
            if (this.WorkIsCompleteEvent != null)
            {
                Dispatcher.Run(() =>
                {
                    this.WorkIsCompleteEvent(this, new EventArgs());
                });
            }
        }



        public WebCrowler()
        {
            this.IsDoNotDuplicateFilesActived = true;
            this.IgnoreUrlFragmentPart = true;

            this.TryTimesToBrokedLinks = 3;
            //this.IntervalBetweenTriesToBrokedLinks = 5000;

            this.TaskEnqueuer = new TaskEnqueuer();
            this.TaskEnqueuer.MaxSimultaneousThreads = 10;

            this.BrokedLinks = new ConcurrentHashSet<string>();
            this.IgnoredLinks = new ConcurrentHashSet<string>();
            this.FoundLinks = new ConcurrentHashSet<string>();
            this.ProcessedLinks = new ConcurrentHashSet<string>();
            this.ProcessingQueue = new ConcurrentQueue<string>();

            this.LeLivrosDownloadLinks = new System.Collections.Concurrent.ConcurrentHashSet<string>();

            this.DownloadFolderPath = "";

            this.LinksStartedWithFilter = new List<string>();
            this.LinksEndedWithFilter = new List<string>();
            this.FileExtensionFilter = new ConcurrentHashSet<string>();

            this.ExceptionList = new ConcurrentHashSet<HttpRequestException>();

            this.TaskEnqueuer.BeforeIsComplete += (object sender, EventArgs e) =>
            {

            };

            this.TaskEnqueuer.AfterIsComplete += (object sender, EventArgs e) =>
            {
                this.OnWorkIsComplete();
            };
        }


        /// <summary>
        /// Asynchronous analyse.
        /// </summary>
        /// <param name="tryTimes"></param>
        public void AnalyseBrokedLinks()
        {
            List<string> urls = new List<string>();

            foreach (string url in this.BrokedLinks)
            {
                urls.Add(url);
            }

            foreach (string url in urls)
            {
                this.AnalyseUrl(url);
            }
        }


        /// <summary>
        /// Assynchronous analyser.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="forceIfNotProcessed">Verify again if the [url] was already verified</param>
        public void AnalyseUrl(string url, bool forceIfNotProcessed = false)
        {
            this.AnalyseUrl(new UrlNode(url), forceIfNotProcessed);
        }

        /// <summary>
        /// Assynchronous analyser.
        /// </summary>
        /// <param name="urlNode"></param>
        /// <param name="forceIfNotProcessed">Verify again if the [url] was already verified</param>
        public void AnalyseUrl(UrlNode urlNode, bool forceIfNotProcessed = false)
        {
            string url = urlNode.Url;
            url = !url.StartsWith("http") ? "http://" + url : url;  // se fosse para ser https, o uso já seria obrigatório

            // Verifica se é uma url bem formada;
            try
            {
                Uri uri = new Uri(url);

                if (this.IgnoreUrlFragmentPart && uri.Fragment != string.Empty)
                {
                    string urlTest = url.Replace(uri.Fragment, "");
                    new Uri(urlTest); //--> verificando se é um url válido
                    url = urlTest;
                }
            }
            catch (Exception ex)
            {
                this.BrokedLinks.Add(url);
                return;
            }



            // Descarta URLs já processadas:
            if (this.ProcessedLinks.Contains(url))
            {
                this.BrokedLinks.Remove(url);
                this.IgnoredLinks.Remove(url);
                return;
            }


            // Força o re-processamento:
            if (forceIfNotProcessed)
            {
                this.BrokedLinks.Remove(url);
                this.IgnoredLinks.Remove(url);
                this.FoundLinks.Remove(url);
            }

            // Descarta URLs já encontradas:
            if (this.FoundLinks.Contains(url))
                return;

            // Descarta URLs já ignorados:
            if (this.IgnoredLinks.Contains(url))
                return;

            this.ProcessingQueue.Enqueue(url);

            this.TaskEnqueuer.Enqueue(() =>
            {
                this.ProcessQueuedUrl();
            });

            if (!this.TaskEnqueuer.IsProcessing)
            {
                this.TaskEnqueuer.RunToComplete();
            }
        }

        

        /// <summary>
        /// Assynchronous analyser.
        /// </summary>
        /// <param name="urls"></param>
        /// <param name="forceIfNotProcessed">Verify again if the [url] was already verified</param>
        public void AnalyseUrl(string[] urls, bool forceIfNotProcessed = false)
        {
            if (urls != null)
            {
                foreach (string url in urls)
                {
                    this.AnalyseUrl(url, forceIfNotProcessed);
                }
            }
        }

        public void AddThread(int count = 1)
        {
            for (int idx = 0; idx < count; idx++)
            {
                this.TaskEnqueuer.Enqueue(() =>
                {
                    this.ProcessQueuedUrl();
                });
            }

            if (!this.TaskEnqueuer.IsProcessing)
            {
                this.TaskEnqueuer.RunToComplete();
            }
        }

        private void ProcessQueuedUrl()
        {
            string url = null;
            //Uri uri = new Uri(url);


            lock (LOCK_QUEUING)
            {
                // Obtém a próxima url da fila:
                if (!this.ProcessingQueue.TryDequeue(out url))
                {
                    return;
                }

                if (this.BrokedLinks.Contains(url))
                {
                    return;
                }
                else if (!this.FoundLinks.Add(url))
                {
                    return;
                }

                // se IgnoredLinks já tiver a url, retorna;
                if (this.IgnoredLinks.Contains(url))
                {
                    return;
                }



                // Exclusivo para o site LeLivros:
                bool éDocumento = url.ToLower().EndsWith("ext=.epub") || url.ToLower().EndsWith("ext=.mobi") || url.ToLower().EndsWith("ext=.pdf");

                if (url.StartsWith("http://ler-agora.jegueajato.com/") && éDocumento)
                {
                    if (!this.LeLivrosDownloadLinks.Contains(url))
                    {
                        this.LeLivrosDownloadLinks.Add(url);
                    }

                    this.ProcessedLinks.Add(url);
                    return;
                }
            }

            // Aceitar início da url:
            if (!url.StartsWithAny(this.LinksStartedWithFilter.ToArray()))
            {
                this.IgnoredLinks.Add(url);
                return;
            }

            // Ignorar final da url:
            if (url.EndsWithAny(this.LinksEndedWithFilter.ToArray()))
            {
                this.IgnoredLinks.Add(url);
                return;
            }



            HttpWebRequest request = null;
            HttpWebResponse response = null;

            try
            {
                request = (HttpWebRequest)WebRequest.Create(url);
                //request.AutomaticDecompression = DecompressionMethods.GZip;

                request.AllowAutoRedirect = true;
                request.KeepAlive = true;
                request.Headers.Add("Upgrade-Insecure-Requests", "1");
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.87 Safari/537.36";    // request.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.87 Safari/537.36");
                request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";                                          // request.Headers.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
                request.Headers.Add("Accept-Encoding", "gzip, deflate, sdch");
                request.Headers.Add("Accept-Language", "pt-BR,pt;q=0.8,en-US;q=0.6,en;q=0.4");




                response = (HttpWebResponse)request.GetResponse();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    string type = response.Headers["content-type"];


                    if (type.StartsWith(@"text/"))
                    {
                        this.ProcessHtml(response, url);
                        this.ProcessedLinks.Add(url);
                        this.BrokedLinks.Remove(url);
                    }
                    else if (type.StartsWith(@"application/") ||
                            type.StartsWith(@"video/") ||
                            type.StartsWith(@"audio/") ||
                            type.StartsWith(@"image/") ||
                            type.StartsWith(@"multipart/"))
                    {
                        this.ProcessFile(response, url);
                        this.ProcessedLinks.Add(url);
                        this.BrokedLinks.Remove(url);
                    }
                }
                else
                {
                    //this.BrokedLinks.AddOrUpdate(url, 1, (key, value) => { return value + 1; });
                    this.BrokedLinks.Add(url);
                }
            }
            catch (Exception ex)
            {
                //this.BrokedLinks.AddOrUpdate(url, 1, (key, value) => { return value + 1; });
                this.BrokedLinks.Add(url);
                this.ExceptionList.Add(new HttpRequestException(ex.Message, ex.InnerException, url));
            }
            finally
            {
                if (response != null)
                {
                    response.Close();
                }
            }

            Thread.Sleep(10);
        }

        private void ProcessHtml(HttpWebResponse response, string url)
        {
            string htmlCode = null;

            using (Stream receiveStream = response.GetResponseStream())
            {
                StreamReader readStream = null;

                try
                {
                    if (response.CharacterSet == null)
                    {
                        readStream = new StreamReader(receiveStream);
                    }
                    else
                    {
                        readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));
                    }

                    htmlCode = readStream.ReadToEnd();

                    if (htmlCode == null)
                        throw new Exception("Problem found openning the url: [" + url + "].");

                    htmlCode = htmlCode.Replace("\n", "");
                    htmlCode = htmlCode.Replace("\t", "");
                    htmlCode = htmlCode.Replace("\r", "");

                    string[] newFoundLinks = GetLinks(htmlCode);

                    foreach (string hrefValue in newFoundLinks)
                    {
                        string finalUrl = null;

                        try
                        {

                            // Tratando a url //////////////////////////////////////////////////////////////////////////////////////////
                            // Se necessário, faz composição de 'URLs relativas' //
                            string urlFound = hrefValue;

                            Uri uri = new Uri(hrefValue, UriKind.RelativeOrAbsolute);

                            if (!uri.IsAbsoluteUri)
                            {
                                // Combine the actual url with the href found:
                                Uri parentUri = new Uri(url);
                                Uri child = new Uri(parentUri, hrefValue);
                                urlFound = child.ToString();
                            }

                            finalUrl = urlFound;
                            ////////////////////////////////////////////////////////////////////////////////////////////////////////////

                            this.AnalyseUrl(finalUrl);
                        }
                        catch (Exception ex)
                        {
                            this.ExceptionList.Add(new HttpRequestException(ex.Message, finalUrl));
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    if (readStream != null)
                    {
                        readStream.Close();
                    }
                }
            }
        }

        private void ProcessFile(HttpWebResponse response, string url)
        {
            string defaultFileName = "sem_nome.xxx";
            string fileName = null;


            //request = (HttpWebRequest)WebRequest.Create(url);
            //response = null;

            try
            {
                //response = (HttpWebResponse)request.GetResponse();

                using (Stream rstream = response.GetResponseStream())
                {
                    if (response.Headers["Content-Disposition"] != null)
                    {
                        fileName = response.Headers["Content-Disposition"].Replace("attachment; filename=", "").Replace("\"", "");
                    }
                    else if (response.Headers["Location"] != null)
                    {
                        fileName = Path.GetFileName(response.Headers["Location"]);
                    }
                    else if (Path.GetFileName(url).Contains('?') || Path.GetFileName(url).Contains('='))
                    {
                        fileName = Path.GetFileName(response.ResponseUri.ToString());
                    }
                    else
                    {
                        fileName = Path.GetFileName(response.ResponseUri.ToString());
                    }

                    fileName = string.IsNullOrEmpty(fileName) ? defaultFileName : fileName;



                    string extension = Path.GetExtension(fileName).ToLower();

                    // Filtrando por extensão:
                    if (!this.FileExtensionFilter.Contains(extension))
                    {
                        return;
                    }


                    // Salvando o arquivo:



                    string hostName = new Uri(url).Host;
                    string fullPath = Path.Combine(this.DownloadFolderPath, hostName);
                    fullPath = Path.Combine(fullPath, fileName);


                    // Criando estrutura de diretórios;
                    if (!Directory.Exists(Path.GetDirectoryName(fullPath)))
                        Directory.CreateDirectory(Path.GetDirectoryName(fullPath));


                    if (File.Exists(fullPath) && this.IsDoNotDuplicateFilesActived)
                    {
                        return;
                    }

                    // Evitando duplicação de arquivos:
                    int duplicatedNumber = 0;

                    while (System.IO.File.Exists(fullPath))
                    {
                        duplicatedNumber++;
                        string newFileName = System.IO.Path.GetFileNameWithoutExtension(fileName) +
                                                " (" + duplicatedNumber + ")" +
                                                System.IO.Path.GetExtension(fileName);

                        fullPath = Path.Combine(this.DownloadFolderPath, newFileName);
                    }


                    // Gravando arquivo no disco rígido:
                    using (Stream receiveStream = response.GetResponseStream())
                    {
                        bool isZeroLengthFile = false;

                        // Read from response and write to file
                        using (FileStream fs = File.Create(fullPath))
                        {
                            // Copy all bytes from the responsestream to the filestream
                            receiveStream.CopyTo(fs);
                            isZeroLengthFile = (fs.Length == 0);
                        }

                        if (isZeroLengthFile)
                        {
                            if (File.Exists(fullPath))
                                File.Delete(fullPath);

                            throw new ZeroLengthFileResponseException("Zero length file response.", url, fullPath);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (response != null)
                {
                    response.Close();
                }
            }
        }

        private string[] GetLinks(string htmlCode)
        {
            List<string> links = new List<string>();

            string[] HREF = new string[] { "href" };

            string[] hrefs = htmlCode.ToLower().Split(HREF, StringSplitOptions.RemoveEmptyEntries).ToList().Skip(1).ToArray();

            foreach (string hrefOriginal in hrefs)
            {
                string href = hrefOriginal.Replace("\\\"", "\"");
                //href = "=\"http://gmpg.org/xfn/11\" /><link rel=\"pingback\" "

                StringBuilder builder = new StringBuilder("");
                bool startFound = false;
                bool endFound = false;

                Func<char, bool> token = (x) => x == '"' || x == '\'';

                for (int i = 0; i < href.Count(); i++)
                {
                    char chr = href[i];

                    if (token(chr))
                    {
                        if (startFound)
                        {
                            endFound = true;
                            break;
                        }
                        else
                        {
                            startFound = true;

                            if (chr == '\'')
                            {
                                token = (x) => x == '\'';
                            }
                            else
                            {
                                token = (x) => x == '"';
                            }
                        }
                    }

                    if (startFound)
                    {
                        builder.Append(chr);
                    }
                }

                if (startFound && endFound)
                {
                    string result = WebUtility.HtmlDecode(builder.ToString().Substring(1));
                    links.Add(result);
                }
            }


            return links.ToArray();
        }

        public Statement GetStatementCopy()
        {
            Statement state = new Statement();

            try
            {
                this.TaskEnqueuer.IsPaused = true;

                Thread.Sleep(2000);

                lock (LOCK_QUEUING)
                {
                    state.BrokedLinks.AddRange(this.BrokedLinks);
                    state.FoundLinks.AddRange(this.FoundLinks);
                    state.IgnoredLinks.AddRange(this.IgnoredLinks);
                    state.ProcessedLinks.AddRange(this.ProcessedLinks);
                    state.ProcessingQueue.AddRange(this.ProcessingQueue);
                    state.LeLivrosDownloadLinks.AddRange(this.LeLivrosDownloadLinks);
                }
            }
            finally
            {
                this.TaskEnqueuer.IsPaused = false;
            }

            return state;
        }
    }

    public class Statement : BindableBase
    {
        public ConcurrentHashSet<string> BrokedLinks { get { return base.GetSync<ConcurrentHashSet<string>>(); } set { base.SetSync(value); } }
        public ConcurrentHashSet<string> FoundLinks { get { return base.GetSync<ConcurrentHashSet<string>>(); } set { base.SetSync(value); } }
        public ConcurrentHashSet<string> IgnoredLinks { get { return base.GetSync<ConcurrentHashSet<string>>(); } set { base.SetSync(value); } }
        public ConcurrentHashSet<string> ProcessedLinks { get { return base.GetSync<ConcurrentHashSet<string>>(); } set { base.SetSync(value); } }
        public ConcurrentQueue<string> ProcessingQueue { get { return base.GetSync<ConcurrentQueue<string>>(); } set { base.SetSync(value); } }

        public ConcurrentHashSet<string> LeLivrosDownloadLinks { get { return base.GetSync<ConcurrentHashSet<string>>(); } set { base.SetSync(value); } }

        public Statement()
        {
            this.BrokedLinks = new System.Collections.Concurrent.ConcurrentHashSet<string>();
            this.FoundLinks = new System.Collections.Concurrent.ConcurrentHashSet<string>();
            this.IgnoredLinks = new System.Collections.Concurrent.ConcurrentHashSet<string>();
            this.ProcessedLinks = new ConcurrentHashSet<string>();
            this.ProcessingQueue = new ConcurrentQueue<string>();

            this.LeLivrosDownloadLinks = new ConcurrentHashSet<string>();
        }
    }

    public class UrlNode
    {
        public string Url { get; set; }
        public UrlNode Parent { get; private set; }

        public UrlNode(string url)
        {
            this.Url = url;
            this.Parent = null;
        }

        public UrlNode(string url, UrlNode parent)
        {
            this.Url = url;
            this.Parent = parent;
        }

        public override string ToString()
        {
            return this.Url;
        }

        public override int GetHashCode()
        {
            return this.Url.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            UrlNode another = obj as UrlNode;

            if (another == null)
                return false;

            return this.Url.Equals(another.Url);
        }

        // User-defined conversion from UrlNode to string
        public static implicit operator string(UrlNode node)
        {
            return node.Url;
        }
        //  User-defined conversion from string to UrlNode
        public static implicit operator UrlNode(string node)
        {
            return new UrlNode(node);
        }
    }
}