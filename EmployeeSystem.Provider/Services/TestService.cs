using DocumentFormat.OpenXml.Packaging;
using Spire.Doc;
using System.Diagnostics;
using System.Text;
using System.Xml;

namespace EmployeeSystem.Provider.Services
{
    public class TestService : ITestService
    {
        private static string OpenOfficePath = @"C:\Program Files (x86)\OpenOffice 4\program\soffice.exe"; // Update this path

        int id;
  

        public string Get()
        {
            id++;
            return $"Hello {id}";
        }

        public string ConvertToTextAsync(Stream srcStream)
        {
            try
            {
                srcStream = ConvertDocToDocx(srcStream);
                const string wordXmlNamespace = "http://schemas.openxmlformats.org/wordprocessingml/2006/main";

                Console.WriteLine("Starting text extraction from the document...");

                StringBuilder textBuilder = new();

                // Open the WordProcessingDocument from the stream
                using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(srcStream, false))
                {
                    if (wordDoc == null || wordDoc.MainDocumentPart == null)
                    {
                        return string.Empty;
                    }
                    Console.WriteLine("Word document opened successfully from the stream.");

                    // Manage namespaces to perform XPath queries
                    NameTable nt = new();
                    XmlNamespaceManager namespaceManager = new(nt);
                    namespaceManager.AddNamespace("w", wordXmlNamespace);
                    Console.WriteLine("Namespace manager created and namespace added.");

                    // Get the document part from the package and load the XML in the document part into an XmlDocument instance
                    XmlDocument xmlDoc = new(nt);
                    xmlDoc.Load(wordDoc.MainDocumentPart.GetStream());
                    Console.WriteLine("XML content of the main document part loaded.");

                    // Select all paragraphs in the document
                    XmlNodeList? paragraphNodes = xmlDoc.SelectNodes("//w:p", namespaceManager);
                    if (paragraphNodes == null)
                    {
                        return string.Empty;
                    }
                    Console.WriteLine($"Total paragraphs found: {paragraphNodes.Count}");

                    foreach (XmlNode paragraphNode in paragraphNodes)
                    {

                        // Select all text nodes within each paragraph
                        XmlNodeList? textNodes = paragraphNode.SelectNodes(".//w:t", namespaceManager);
                        if (textNodes == null || textNodes.Count == 0)
                        {
                            continue;
                        }
                        Console.WriteLine($"Total text nodes found in the current paragraph: {textNodes.Count}");
                        foreach (XmlNode textNode in textNodes)
                        {
                            Console.WriteLine($"Text found: {textNode.InnerText}");
                            textBuilder.Append(textNode.InnerText);
                        }
                        textBuilder.Append(Environment.NewLine);
                        Console.WriteLine("New paragraph added to the text.");
                    }
                }

                Console.WriteLine("Text extraction completed.");
                Console.WriteLine("Extracted Text:");
                Console.WriteLine(textBuilder.ToString());

                return textBuilder.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public Stream ConvertDocToDocx(Stream inputStream)
        {
            string tempInputFile = Path.GetTempFileName();
            string tempOutputFile = Path.ChangeExtension(tempInputFile, ".docx");

            
            // Save the input stream to a temporary .doc file
            using (FileStream fileStream = new FileStream(tempInputFile, FileMode.Create, FileAccess.Write))
            {
                inputStream.CopyTo(fileStream);
            }

            // Convert .doc to .docx using FreeSpire.Doc
            Document document = new Document();
            document.LoadFromFile(tempInputFile, FileFormat.Doc);
            document.SaveToFile(tempOutputFile, FileFormat.Docx);

            // Return the converted .docx file as a stream
            FileStream resultStream = new FileStream(tempOutputFile, FileMode.Open, FileAccess.Read);
            return resultStream;
        }

        public Stream ConvertDocToDocx2(Stream inputStream)
        {
            string tempInputFile = Path.GetTempFileName();
            string tempOutputFile = Path.ChangeExtension(tempInputFile, ".docx");


            // Save the input stream to a temporary .doc file
            using (FileStream fileStream = new FileStream(tempInputFile, FileMode.Create, FileAccess.Write))
            {
                inputStream.CopyTo(fileStream);
            }

            // Convert .doc to .docx using OpenOffice command-line tool
            ProcessStartInfo processInfo = new ProcessStartInfo
            {
                FileName = OpenOfficePath,
                Arguments = $"--headless --convert-to docx \"{tempInputFile}\" --outdir \"{Path.GetDirectoryName(tempInputFile)}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (Process process = Process.Start(processInfo))
            {
                process.WaitForExit();

                // Optionally, check for errors
                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();

                if (process.ExitCode != 0)
                {
                    throw new Exception($"Conversion failed: {error}");
                }
            }

            // Return the converted .docx file as a stream
            FileStream resultStream = new FileStream(tempOutputFile, FileMode.Open, FileAccess.Read);
            return resultStream;

        }
    }
}
