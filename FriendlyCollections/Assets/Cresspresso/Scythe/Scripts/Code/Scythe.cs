namespace Cresspresso.Scythe
{
	using System.IO;
	using Serialization;

	public class Scythe
	{
		private string rootPath;
		private XmlDocComments doc;
		private StreamWriter writer;

		public Scythe(string rootPath, XmlDocComments doc)
		{
			this.rootPath = rootPath;
			this.doc = doc;
		}

		public void Generate()
		{
			CreateIndexPage();
		}

		private void CreateIndexPage()
		{
			Directory.CreateDirectory(rootPath);
			using (writer = new StreamWriter(Path.Combine(rootPath, "index.html")))
			{
				WriteHtml();
			}
		}

		private void WriteHtml()
		{
			writer.WriteLine("<!DOCTYPE html>");
			writer.WriteLine("<html>");
			WriteHead();
			WriteBody();
			writer.WriteLine("</html>");
		}

		private void WriteHead()
		{
			writer.WriteLine("<head>");
			writer.WriteLine("<title>{0}</title>", doc.assembly.name);
			writer.WriteLine("</head>");
		}

		private void WriteBody()
		{
			writer.WriteLine("<body>");
			writer.WriteLine("<h1>{0}</h1>", doc.assembly.name);
			writer.WriteLine("</body>");
		}
	}
}
