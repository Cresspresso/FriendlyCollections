namespace Cresspresso.Scythe.Serialization
{
	using System.IO;
	using System.Xml.Serialization;

	[XmlRoot("doc")]
	public class XmlDocComments
	{
		public Assembly assembly;
		
		[XmlArray("members"), XmlArrayItem("member")]
		public Member[] members;
	}
	
	public class Assembly
	{
		public string name;
	}

	public class Member
	{
		public string summary;
	}
	
	//public class Member
	//{
	//	public string summary;

	//	[XmlElement]
	//	public Typeparam[] typeparam;

	//	[XmlElement]
	//	public Param[] param;

	//	public string returns;
	//}

	//public class Typeparam
	//{
	//	[XmlAttribute]
	//	public string name;
	//}

	//public class Param
	//{
	//	[XmlAttribute]
	//	public string name;
	//}
}
