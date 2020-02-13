using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace nHydrate.DslPackage.Objects
{
	internal class Utils
	{
		public static Color ColorInserted
		{
			get { return Color.Yellow; }
		}

		public static Color ColorDeleted
		{
			get { return Color.FromArgb(255, 200, 100); }
		}

		public static Color ColorImaginary
		{
			get { return Color.FromArgb(200, 200, 200); }
		}

		public static Color ColorModified
		{
			get { return Color.FromArgb(220, 220, 255); }
		}

		public static void CompareText(
			FastColoredTextBoxNS.FastColoredTextBox textBox1,
			FastColoredTextBoxNS.FastColoredTextBox textBox2,
			string text1,
			string text2)
		{
			var differ = new DiffPlex.Differ();
			var builder = new DiffPlex.DiffBuilder.SideBySideDiffBuilder(differ);
			var result = builder.BuildDiffModel(text1, text2);

			var brushInserted = new SolidBrush(ColorInserted);
			var brushDeleted = new SolidBrush(ColorDeleted);
			var brushEmpty = new SolidBrush(ColorImaginary);
			var brushModified = new SolidBrush(ColorModified);

			textBox1.Text = string.Join("\n", result.OldText.Lines.Select(x => x.Text));
			textBox2.Text = string.Join("\n", result.NewText.Lines.Select(x => x.Text));

			for (var index = 0; index < result.OldText.Lines.Count; index++)
			{
				var oldLine = result.OldText.Lines[index];
				var newLine = result.NewText.Lines[index];

				if (oldLine.Type == DiffPlex.DiffBuilder.Model.ChangeType.Inserted)
					textBox1.TextSource[index].BackgroundBrush = brushInserted;
				else if (oldLine.Type == DiffPlex.DiffBuilder.Model.ChangeType.Deleted)
					textBox1.TextSource[index].BackgroundBrush = brushDeleted;
				else if (oldLine.Type == DiffPlex.DiffBuilder.Model.ChangeType.Imaginary)
					textBox1.TextSource[index].BackgroundBrush = brushEmpty;
				else if (oldLine.Type == DiffPlex.DiffBuilder.Model.ChangeType.Modified)
					textBox1.TextSource[index].BackgroundBrush = brushModified;

				if (newLine.Type == DiffPlex.DiffBuilder.Model.ChangeType.Inserted)
					textBox2.TextSource[index].BackgroundBrush = brushInserted;
				else if (newLine.Type == DiffPlex.DiffBuilder.Model.ChangeType.Deleted)
					textBox2.TextSource[index].BackgroundBrush = brushDeleted;
				else if (newLine.Type == DiffPlex.DiffBuilder.Model.ChangeType.Imaginary)
					textBox2.TextSource[index].BackgroundBrush = brushEmpty;
				else if (newLine.Type == DiffPlex.DiffBuilder.Model.ChangeType.Modified)
					textBox2.TextSource[index].BackgroundBrush = brushModified;
			}

		}

		/// --------------------------------------------------------------------
		/// <summary>
		/// Determine if a property exists in an object
		/// </summary>
		/// <param name="propertyName">Name of the property </param>
		/// <param name="srcObject">the object to inspect</param>
		/// <returns>true if the property exists, false otherwise</returns>
		/// <exception cref="ArgumentNullException">if srcObject is null</exception>
		/// <exception cref="ArgumentException">if propertName is empty or null </exception>
		/// --------------------------------------------------------------------
		public static bool PropertyExists(object srcObject, string propertyName)
		{
			if (srcObject == null)
				throw new System.ArgumentNullException("srcObject");

			if ((propertyName == null) || (propertyName == String.Empty) || (propertyName.Length == 0))
				throw new System.ArgumentException("Property name cannot be empty or null.");

			var propInfoSrcObj = srcObject.GetType().GetProperty(propertyName);

			return (propInfoSrcObj != null);
		}

		public static T GetPropertyValue<T>(object srcObject, string propertyName)
		{
			if (srcObject == null)
				throw new System.ArgumentNullException("srcObject");

			if ((propertyName == null) || (propertyName == String.Empty) || (propertyName.Length == 0))
				throw new System.ArgumentException("Property name cannot be empty or null.");

			return (T)srcObject.GetType().GetProperty(propertyName).GetValue(srcObject, null);
		}

		public static void SetPropertyValue<T>(object srcObject, string propertyName, T value)
		{
			if (srcObject == null)
				throw new System.ArgumentNullException("srcObject");

			if ((propertyName == null) || (propertyName == String.Empty) || (propertyName.Length == 0))
				throw new System.ArgumentException("Property name cannot be empty or null.");

			srcObject.GetType().GetProperty(propertyName).SetValue(srcObject, value, null);
		}

	}
}
