using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace nHydrate.Dsl.Objects
{
	public class UserDefinedScript : nHydrate.Dsl.IPrecedence
	{
		public UserDefinedScript(string fileName, EnvDTE.Project project)
		{
			if (!File.Exists(fileName))
				throw new Exception("The file '" + fileName + "' does not exists.");

			this.FileName = fileName;
			var text = File.ReadAllText(this.FileName);
			var lines = text.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
			var line = lines.FirstOrDefault(x => x.StartsWith("--MODELID: "));
			if (!string.IsNullOrEmpty(line))
			{
				//Set type
				if (fileName.ToLower().Contains(@"\stored procedures\user defined"))
					this.TypeName = "Stored Procedure";
				else if (fileName.ToLower().Contains(@"\views\user defined"))
					this.TypeName = "View";
				else if (fileName.ToLower().Contains(@"\functions\user defined"))
					this.TypeName = "Function";

				var gText = line.Replace("--MODELID:", string.Empty).Trim();
				var pText = "0";
				if (gText.Contains(","))
				{
					var cindex = gText.IndexOf(",");
					pText = gText.Substring(cindex + 1, gText.Length - cindex - 1).Replace("PrecedenceOrder:", string.Empty).Trim();
					gText = gText.Substring(0, cindex);
				}

				this.IsValid = true;
				this.Name = (new FileInfo(this.FileName)).Name;

				Guid g;
				if (Guid.TryParse(gText, out g)) this.ID = g;
				else this.IsValid = false;

				int prec;
				if (int.TryParse(pText, out prec)) this.PrecedenceOrder = prec;
				else this.IsValid = false;

			}

		}

		public void Save()
		{
			if (!this.IsValid) return;

			if (!File.Exists(this.FileName))
				throw new Exception("The file '" + this.FileName + "' does not exists.");

			var text = File.ReadAllText(this.FileName);
			var lines = text.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None).ToList();
			var line = lines.FirstOrDefault(x => x.StartsWith("--MODELID: "));
			if (!string.IsNullOrEmpty(line))
			{
				lines.Remove(x => x.StartsWith("--MODELID: "));
				text = string.Join(Environment.NewLine, lines);
				File.WriteAllText(this.FileName, "--MODELID: " + this.ID + ",PrecedenceOrder:" + this.PrecedenceOrder + Environment.NewLine + text);
			}
		}

		private string FileName { get; set; }

		#region IPrecedence Members

		public int PrecedenceOrder { get; set; }
		public string Name { get; set; }
		public string TypeName { get; private set; }
		public Guid ID { get; private set; }
		public bool IsValid { get; private set; }

		#endregion
	}
}

