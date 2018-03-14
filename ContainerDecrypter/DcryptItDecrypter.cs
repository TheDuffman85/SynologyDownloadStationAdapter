using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Newtonsoft.Json.Linq;

namespace TheDuffman85.ContainerDecrypter
{
	/// <summary>
	/// Decrypts download container (DLC, CCF, RSDF) using http://dcrypt.it/
	/// </summary>
	public class DcryptItDecrypter : DecrypterBase
	{
		private const string DCRYPT_IT_UPLOAD_URL = "http://dcrypt.it/decrypt/upload";
		private static readonly string[] FILE_TYPES = new string[] { ".dlc", ".ccf", ".rsdf" };

		/// <summary>
		/// DcryptItDecrypter constructor
		/// </summary>
		/// <param name="path">Path to container file</param>
		public DcryptItDecrypter(string path)
			: base(path)
		{
			if (!FILE_TYPES.Contains(Path.GetExtension(path).ToLower()))
			{
				throw new ArgumentException("File type " + Path.GetExtension(path) + " isn't supported");
			}
		}

		protected override void Decrypt(string content, out string[] links, out string password)
		{
			string boundary = DateTime.UtcNow.Ticks.ToString().PadLeft(40, '-');

			HttpWebRequest request = (HttpWebRequest)WebRequest.Create(DCRYPT_IT_UPLOAD_URL);
			request.Method = "POST";
			request.ContentType = "	multipart/form-data; boundary=" + boundary;
			request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";

			string data = "--" + boundary +
							 "\r\nContent-Disposition: form-data; name=\"dlcfile\"; filename=\"" + this.Name + "\"" +
							 "\r\nContent-Type: application/force-download\r\n\r\n";

			data += content + "\r\n--" + boundary + "--\r\n";

			request.ContentLength = data.Length;

			using (StreamWriter requestStream = new StreamWriter(request.GetRequestStream()))
			{
				requestStream.Write(data);
			}

			HttpWebResponse response = (HttpWebResponse)request.GetResponse();

			JObject resultObj;

			string result;

			using (StreamReader reader = new StreamReader(response.GetResponseStream()))
			{
				result = reader.ReadToEnd();
				result = result.Substring(10, result.Length - 21);

				resultObj = JObject.Parse(result);
			}

			links = null;
			password = null;

			if (resultObj["success"] != null &&
			   resultObj["success"]["links"] != null)
			{
				string[] resultArr = resultObj["success"]["links"].ToObject<string[]>();

				if (resultArr.Length > 0)
				{
					var linksContainPassword = !resultArr[0].StartsWith("http");

					var passwordOffset = 0;

					if (linksContainPassword)
					{
						password = resultArr[0];
						passwordOffset = 1;
					}

					string[] tmp = new string[resultArr.Length - passwordOffset];

					for (int i = 0; i < tmp.Length; i++)
					{
						tmp[i] = resultArr[i + passwordOffset];
					}

					links = tmp;
				}
			}
			else if (resultObj["form_errors"] != null)
			{

				string msg = string.Empty;
				string err;

				foreach (JProperty item in resultObj["form_errors"])
				{
					err = item.First.First.Value<string>();
					msg += msg.Length > 0 ? "\r\n" + err : err;
				}

				throw new Exception("Error occurred while decoding container: \r\n" + msg);
			}
		}
	}
}
