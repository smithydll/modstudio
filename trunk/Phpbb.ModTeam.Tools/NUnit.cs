using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Specialized;
using System.IO;

namespace Phpbb.ModTeam.Tools.DataStructures
{

    /// <summary>
    /// 
    /// </summary>
	[TestFixture]
	public class ModVersionTest
	{
        /// <summary>
        /// 
        /// </summary>
		[Test]
		public void TestToString()
		{
			string[] inputArray = {"1.0.0",
				"2.0.0",
				"2.0.1",
				"2.1.0",
				"2.2.1",
				"3.0.0a",
				"3.0.0b",
				"3.0.0z",
				"3.1.0z",
				"3.2.1z",
				"3.2.1O",
				"3.2.1I",
				"3.2.11a",
				"3.2.11",
				"3.21.11",
				"1.14.9",
                "1.14.B9",
                "1.14.B9b",
                "2.15.RC2",
                "2.15.RC3a",
				"43.32.21a"};
			foreach (string input in inputArray)
			{
				ModVersion mv = ModVersion.Parse(input);
				string output = mv.ToString();
				Assert.AreEqual(input, output);
			}
		}

        
		// not really expected, but thats what is happening
        /// <summary>
        /// 
        /// </summary>
		[Test]
		//[ExpectedException(typeof(NotAModVersionException))]
		public void AdvancedTestToString()
		{
			StringDictionary testCases = new StringDictionary();
			testCases.Add("1.0.0ab", "1.0.0a");
			testCases.Add("1.0.0za", "1.0.0z");
			testCases.Add("1.0.0z RC1", "1.0.0z");
			testCases.Add("1.0.0z BETA", "1.0.0z");
			testCases.Add("1.0.0 RC1", "1.0.0");
			testCases.Add("1.0.0 BETA", "1.0.0");

			foreach(string input in testCases.Keys)
			{
				ModVersion mv = ModVersion.Parse(input);
				string output = mv.ToString();
				string expected = testCases[input];
				Assert.AreEqual(expected, output);
			}
		}

        /// <summary>
        /// 
        /// </summary>
		[Test]
		[ExpectedException(typeof(NotAModVersionException))]
		public void AdvancedTestWithException()
		{
			StringDictionary testCases = new StringDictionary();
			testCases.Add("1.0", "1.0.0");
			testCases.Add("1.2", "1.2.0");
			testCases.Add("1.0a", "1.0.0a");
			testCases.Add("1a", "1.0.0a");
			testCases.Add("1", "1.0.0");
			testCases.Add("1 beta1", "1.0.0");
			testCases.Add("beta1", "1.0.0");
			testCases.Add("RC1", "1.0.0");

			foreach(string input in testCases.Keys)
			{
				ModVersion mv = ModVersion.Parse(input);
				string output = mv.ToString();
				string expected = testCases[input];
				Assert.AreEqual(expected, output);
			}
		}
	}

    /// <summary>
    /// 
    /// </summary>
	[TestFixture]
	public class ModAuthorTest
	{

        /// <summary>
        /// 
        /// </summary>
		[Test]
		public void TestToString()
		{
			string[] inputArray = {"smithy_dll < n/a > (David Smith) http://phpbbstuff.ddblog.org",
			"smithy_dll < n/a > (David Smith) http://david.smigit.com",
			"wGEric < myemail@mysite.com > (Eric Faerber) http://mods.best-dev.com",
			"wGEric < n/a > (Eric Faerber) http://mods.best-dev.com",
			"wGEric < myemail@mysite.com > (n/a) http://mods.best-dev.com",
			"wGEric < n/a > (Eric Faerber) n/a",
			"wGEric < n/a > (n/a) n/a",
			"ycl6 < n/a > (Y C Lin) http://macphpbbmod.sourceforge.net/",
			"jwacalex < jwacalex@yahoo.de > (Alexander Böhm) http://www.s8d.de",
			"damnian < damnian-no-spam at damnian dot com > (Dmitry Shechtman) http://damnian.com/dev",
			"I'm Only 16 < n/a > (n/a) n/a",
			"Evil<3 < n/a > (n/a) n/a",
			"Afterlife(69) < n/a > (n/a) n/a"};
			foreach (string input in inputArray)
			{
				ModAuthor ma = ModAuthor.Parse("## MOD Author: " + input);
				string output = ma.ToString();
				Assert.AreEqual(input, output);
			}
			foreach (string input in inputArray)
			{
				ModAuthor ma = ModAuthor.Parse("## MOD Author:		" + input);
				string output = ma.ToString();
				Assert.AreEqual(input, output);
			}
			foreach (string input in inputArray)
			{
				ModAuthor ma = ModAuthor.Parse("## MOD Author, Secondary: " + input);
				string output = ma.ToString();
				Assert.AreEqual(input, output);
			}
			foreach (string input in inputArray)
			{
				ModAuthor ma = ModAuthor.Parse(input);
				string output = ma.ToString();
				Assert.AreEqual(input, output);
			}
		}
	}

    /// <summary>
    /// 
    /// </summary>
	[TestFixture]
	public class InstallationTimeTest
	{

        /// <summary>
        /// 
        /// </summary>
		[Test]
		public void Test()
		{
			Hashtable testCases = new Hashtable();
			testCases.Add("2 minute", 120);
			testCases.Add("1 hour", 3600);
			testCases.Add("2 second", 2);
			testCases.Add("approx 2 minute", 120);
			testCases.Add("2 minute (approx)", 120);
			testCases.Add("~10 minutes", 600);
			testCases.Add("~1 hours", 3600);
			testCases.Add("~2 minute", 120);
			testCases.Add("2 seconds", 2);

			foreach(object input in testCases.Keys)
			{
				int output = Mod.StringToSeconds((string)input);
				int expected = (int)testCases[input];
				Assert.AreEqual(expected, output);
			}
		}
	}

    /// <summary>
    /// 
    /// </summary>
	[TestFixture]
	public class InstallationLevelTest
	{

        /// <summary>
        /// 
        /// </summary>
		[Test]
		public void Test()
		{
			StringDictionary testCases = new StringDictionary();
			testCases.Add("Easy", "Easy");
			testCases.Add("", "Easy");
			testCases.Add(" ", "Easy");
			testCases.Add("Intermediate", "Intermediate");
			testCases.Add("Moderate", "Intermediate");
			testCases.Add("advanced", "Advanced");
			testCases.Add("Hard", "Advanced");
			testCases.Add("\t", "Easy");
			testCases.Add("asdf", "Easy");

			foreach(string input in testCases.Keys)
			{
				string output = Mod.InstallationLevelParse(input).ToString();
				string expected = testCases[input];
				Assert.AreEqual(expected, output);
			}
			foreach(string input in testCases.Keys)
			{
				string output = Mod.InstallationLevelParse(input.ToLower()).ToString();
				string expected = testCases[input];
				Assert.AreEqual(expected, output);
			}
			foreach(string input in testCases.Keys)
			{
				string output = Mod.InstallationLevelParse(input.ToUpper()).ToString();
				string expected = testCases[input];
				Assert.AreEqual(expected, output);
			}
		}
	}

    /// <summary>
    /// 
    /// </summary>
	[TestFixture]
	public class InputOutputTextModTest
	{
        /// <summary>
        /// 
        /// </summary>
		public AppDomain domain = AppDomain.CreateDomain("NunitMODXdomain");

        /// <summary>
        /// 
        /// </summary>
		[Test]
		public void SimpleTest()
		{
            DirectoryInfo di = new DirectoryInfo(@"C:\Users\Lachlan\Documents\Visual Studio Projects\Phpbb.ModTeam.Tools\bin\Debug\simpletest");
			FileInfo[] modFiles = di.GetFiles();
			foreach(FileInfo modFile in modFiles)
			{
				string input = OpenTextFile(modFile.FullName);
                TextMod newMod = new TextMod(@"C:\Users\Lachlan\Documents\Visual Studio Projects\Phpbb.ModTeam.Tools\bin\Debug");
				newMod.ReadString(input);
				input = input.Replace("\r\n", "\n");
				input = input.Replace("\r", "\n");
                Console.Out.WriteLine(modFile.FullName);
				Assert.AreEqual(input, newMod.ToString());
			}
		}

        /// <summary>
        /// 
        /// </summary>
		[Test]
		public void AdvancedTest_DeepCompare()
		{
            DirectoryInfo di = new DirectoryInfo(@"C:\Users\Lachlan\Documents\Visual Studio Projects\Phpbb.ModTeam.Tools\bin\Debug\advancedtest");
			FileInfo[] modFiles = di.GetFiles();
			foreach(FileInfo modFile in modFiles)
			{
				string input = OpenTextFile(modFile.FullName);
				Console.WriteLine(modFile.FullName);
                TextMod newMod = new TextMod(@"C:\Users\Lachlan\Documents\Visual Studio Projects\Phpbb.ModTeam.Tools\bin\Debug");
				newMod.ReadString(input);
                TextMod ultraMod = new TextMod(@"C:\Users\Lachlan\Documents\Visual Studio Projects\Phpbb.ModTeam.Tools\bin\Debug");
				ultraMod.ReadString(newMod.ToString());
				string temp = ultraMod.ToString();
				Console.Error.WriteLine("Actions");
				Assert.AreEqual(true, newMod.Actions.Equals(ultraMod.Actions));
				Console.Error.WriteLine("Header");
				Assert.AreEqual(true, newMod.Header.Equals(ultraMod.Header));
			}
		}

        /// <summary>
        /// 
        /// </summary>
		[Test]
		public void TextToXmlAndBack()
		{
            DirectoryInfo di = new DirectoryInfo(@"C:\Users\Lachlan\Documents\Visual Studio Projects\Phpbb.ModTeam.Tools\bin\Debug\xmlandback");
			FileInfo[] modFiles = di.GetFiles();
			foreach(FileInfo modFile in modFiles)
			{
				string input = OpenTextFile(modFile.FullName);
				Console.WriteLine(modFile.FullName);
                TextMod newMod = new TextMod(@"C:\Users\Lachlan\Documents\Visual Studio Projects\Phpbb.ModTeam.Tools\bin\Debug");
				newMod.ReadString(input);
				string xmlFileName = Path.Combine(Path.Combine(modFile.DirectoryName, "xml"), modFile.Name + ".xml");
				((ModxMod)newMod).Write(xmlFileName);
                TextMod ultraMod = new TextMod(@"C:\Users\Lachlan\Documents\Visual Studio Projects\Phpbb.ModTeam.Tools\bin\Debug");
				((ModxMod)ultraMod).Read(xmlFileName);
				ultraMod.AuthorNotesIndent = newMod.AuthorNotesIndent;
				ultraMod.AuthorNotesStartLine = newMod.AuthorNotesStartLine;
				ultraMod.DescriptionIndent = newMod.DescriptionIndent;
				ultraMod.ModFilesToEditIndent = newMod.ModFilesToEditIndent;
				ultraMod.ModIncludedFilesIndent = newMod.ModIncludedFilesIndent;
				string temp = ultraMod.ToString();
				/*Console.Error.WriteLine("Actions");
				Assert.AreEqual(true, newMod.Actions.Equals(ultraMod.Actions));
				Console.Error.WriteLine("Header");
				Assert.AreEqual(true, newMod.Header.Equals(ultraMod.Header));*/
				Assert.AreEqual(newMod.ToString(), temp);
			}
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
		private static string OpenTextFile(string fileName)
		{
			StreamReader myStreamReader;
			string temp;
			try 
			{
				myStreamReader = File.OpenText(fileName);
				temp = myStreamReader.ReadToEnd();
				myStreamReader.Close();
			} 
			catch
			{
				temp = "";
			}
			return temp;
		}
	}
}