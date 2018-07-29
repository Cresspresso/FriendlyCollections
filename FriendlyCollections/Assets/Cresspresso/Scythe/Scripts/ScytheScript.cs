namespace Cresspresso.Scythe
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.IO;
	using System.Xml.Serialization;
	using UnityEngine;
	using XmlDocComments = Serialization.XmlDocComments;
	
	public class ScytheScript : ScriptableObject
	{
		public TextAsset xmlAsset;
		public string outputDirectoryPath = "./Assets/Documentation/";

		public void GenerateDocumentation()
		{
			Debug.LogFormat("Deserializing XML Doc Comments asset '{0}',", xmlAsset == null ? "null" : xmlAsset.name);
			XmlDocComments doc = DeserializeDoc();
			Debug.LogFormat("Generating site at '{0}',", outputDirectoryPath);
			new Scythe(outputDirectoryPath, doc).Generate();
			Debug.LogFormat("Generated site at '{0}'.", outputDirectoryPath);
		}

		private XmlDocComments DeserializeDoc()
		{
			if (xmlAsset == null)
				throw new NullReferenceException("xmlAsset is null.");
			
			using (MemoryStream stream = new MemoryStream(xmlAsset.bytes))
			{
				using (StreamReader reader = new StreamReader(stream))
				{
					XmlSerializer serializer = new XmlSerializer(typeof(XmlDocComments));
					return (XmlDocComments)serializer.Deserialize(reader);
				}
			}
		}
	}
}
