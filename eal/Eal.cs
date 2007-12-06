/*
 * EAL
 * http://smithydll.id.au/
 * Copyright © 2007, David Lachlan Smith
 * 
 * $Id: Eal.cs,v 1.1 2007-12-06 03:58:47 smithydll Exp $
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License version 3 as
 * published by the Free Software Foundation.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */

using System;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using Phpbb.ModTeam.Tools;
using Phpbb.ModTeam.Tools.DataStructures;
using Phpbb.ModTeam.Tools.Validation;
using ICSharpCode.SharpZipLib.Zip;

namespace Phpbb.ModTeam.eal
{
	/// <summary>
	/// Eal, MOD Team pre-validation tool.
	/// </summary>
	class Eal
	{

		private static AppDomain domain = AppDomain.CreateDomain("EALdomain");

		private static string modPath;
		private static bool ModFileIncluded;
		private static bool ModxContentIncluded;
		private static string[] FunctionList;
		private static string[] Phpbb2LanguageList;
		private static string[] Phpbb3LanguageList;
		private static string ModxRoot = "";
		private static int PhpbbVersion = 2;
		private static string ZipFile = "";
		private static string currentLanguage = "english";
		private static bool inLanguages = false;
		private static bool languageSubDir = false;
		private static bool adminInclude = false;
		private static bool hasIncludes = false;
        private static string[] EnglishWordsList;

		const string notice = "[b][ [color=blue]NOTICE[/color] ][/b]";
		const string error = "[b][ [color=red]ERROR[/color] ][/b]";
		const string fail = "[b][ [color=red]FAIL[/color] ][/b]";
		const string warning = "[b][ [color=orange]WARNING[/color] ][/b]";
		const string ok = "[b][ [color=green]OK[/color] ][/b]";
		const string pass = "[b][ [color=green]PASS[/color] ][/b]";
		const string info = "[b][ [color=purple]INFO[/color] ][/b]";
        const string style = "[b][ [color=#FF00FF]STYLE[/color] ][/b]";

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			Console.WriteLine("----------------------------------------------------------");
			Console.Write("pp  pppp    hh          pp  pppp    ");
			Console.WriteLine("BBBBBBBB    BBBBBBBB");
			Console.Write("pppp    pp  hh          pppp    pp  ");
			Console.WriteLine("BB      BB  BB      BB");
			Console.Write("pp      pp  hh          pp      pp  ");
			Console.WriteLine("BB      BB  BB      BB");
			Console.Write("pp      pp  hh          pp      pp  ");
			Console.WriteLine("BB      BB  BB      BB");
			Console.Write("pppppppp    hhhhhhhh    pppppppp    ");
			Console.WriteLine("BBBBBBBB    BBBBBBBB");
			Console.Write("pp          hh      hh  pp          ");
			Console.WriteLine("BB      BB  BB      BB");
			Console.Write("pp          hh      hh  pp          ");
			Console.WriteLine("BB      BB  BB      BB");
			Console.Write("pp          hh      hh  pp          ");
			Console.WriteLine("BBBBBBBB    BBBBBBBB");
			Console.WriteLine("----------------------------------------------------------");
#if eal
			Console.WriteLine("The phpBB MOD Team automatic MOD parsing script.");
#elif meal
			Console.WriteLine("The phpBB MOD Team MOD pre-validation script.");
#else
#error please compile in eal or meal mode
#endif
			switch (args.Length)
			{
				case 0:
#if eal
					Console.Write("Enter the path to a mod or it's catDB shortname: ");
#elif meal
					Console.Write("Please enter the path to a folder of zip to validate: ");
#endif
					modPath = Console.ReadLine();
					break;
				case 1:
				switch (args[0])
				{
					case "help":
					case "-help":
						if (args.Length > 1)
						{
							switch (args[1])
							{
								case "validate":
									break;
							}
						}
						else
						{
							Console.WriteLine("Welcome to the mEAL help system.");
							Console.WriteLine("{This is incomplete}");
						}
						Environment.Exit(Environment.ExitCode);
						break;
					default:
						modPath = args[0];
						break;
				}
					break;
			}

			// continue
			// old, backwards support for Linux & OSX
			string validatedPath = Path.Combine(domain.BaseDirectory, "validated");
			if (Environment.OSVersion.Platform == System.PlatformID.Win32NT && Environment.OSVersion.Version.Major <= 5)
			{
				// Windows NT <= XP
				validatedPath = Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "My Mods"), "Validated");
			}
			else if (Environment.OSVersion.Platform == System.PlatformID.Win32NT && Environment.OSVersion.Version.Major >= 6)
			{
				// Windows NT >= Vista
				validatedPath = Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "Mods"), "Validated");
			}

			if (modPath.ToLower().StartsWith("http://"))
			{
#if eal
                if (modPath.ToLower().StartsWith("http://www.phpbb.com/files/contribdb/"))
                {
                    string modUrl = modPath;
                    modPath = modPath.Remove(0, 38);
                    string zipPath = DownloadMod(modUrl, Path.Combine(validatedPath, UnrollUri(modPath)));
                    if (modPath.ToLower().EndsWith(".zip"))
                    {
                        modPath = Path.Combine(validatedPath, UnrollUri(modPath));
                        ExtractMod(zipPath);
                        ZipFile = zipPath;
                    }
                    else if (modPath.ToLower().EndsWith(".mod") ||
                        modPath.ToLower().EndsWith(".txt") ||
                        modPath.ToLower().EndsWith(".xml"))
                    {
                        modPath = Path.Combine(validatedPath, UnrollUri(modPath));
                    }
                }
                else
                {
                    string zipPath = DownloadMod(modPath, Path.Combine(validatedPath, UnrollUri(modPath)));
                    if (modPath.ToLower().EndsWith(".zip"))
                    {
                        modPath = Path.Combine(validatedPath, UnrollUri(modPath));
                        ExtractMod(zipPath);
                        ZipFile = zipPath;
                    }
                    else if (modPath.ToLower().EndsWith(".mod") ||
                        modPath.ToLower().EndsWith(".txt") ||
                        modPath.ToLower().EndsWith(".xml"))
                    {
                        modPath = Path.Combine(validatedPath, UnrollUri(modPath));
                    }
                }
#elif meal
				Console.WriteLine("Invalid path");
				Environment.Exit(Environment.ExitCode);
#endif
			}
			else if (modPath.ToLower().EndsWith(".mod") || 
				modPath.ToLower().EndsWith(".txt") || 
				modPath.ToLower().EndsWith(".xml") || 
				modPath.ToLower().EndsWith(".zip")
				&& modPath.IndexOf(Path.DirectorySeparatorChar) < 0)
			{
#if eal
				string zipPath = DownloadMod("http://www.phpbb.com/files/mods/" + modPath.Replace(" ", "%20"), Path.Combine(validatedPath, UnrollUri(modPath)));
				if (modPath.ToLower().EndsWith(".zip"))
				{
					ExtractMod(zipPath);
					ZipFile = zipPath;
				}
				modPath = Path.Combine(validatedPath, UnrollUri(modPath));
#elif meal
				Console.WriteLine("Invalid path");
				Environment.Exit(Environment.ExitCode);
#endif
			}
			else if (modPath.IndexOf(Path.DirectorySeparatorChar) >= 0)
			{
				if (modPath.ToLower().EndsWith(".zip"))
				{
					ExtractMod(modPath, UnrollUri(modPath));
					//ZipFile = zipPath;
					FileInfo zipInfo = new FileInfo(modPath);
					modPath = Path.Combine(zipInfo.Directory.ToString(), UnrollUri(modPath));
				}
				/*Console.WriteLine("Currently Unsupported.");
				Console.Read();
				Environment.Exit(Environment.ExitCode);*/
			}
			else
			{
				Console.WriteLine("Cannot validate MOD, Press Enter to Exit.");
				Console.Read();
				Environment.Exit(Environment.ExitCode);
			}

			//modPath = Path.Combine(validatedPath, modPath);

			StringBuilder report = new StringBuilder();

            LoadEnglishDictionary();

			ValidateDirectory(modPath, report);

			if (!ModFileIncluded)
			{
				report.Append("\n\n[color=red][b]No MOD Template installation file found[/b][/color]\n\n");
			}

			if (ModxContentIncluded)
			{
				// scan for packaging requirements
				report.Append("\n\n[b]Verifying MODX packaging guidelines[/b]\n\n");
				bool modxError = false;

				int filesInRoot = Directory.GetFiles(ModxRoot).Length;
				int styleSheetsIncluded = Directory.GetFiles(ModxRoot, "*.xsl").Length;
				int xmlInRoot = Directory.GetFiles(ModxRoot, "*.xml").Length;
				bool licenseIncluded = File.Exists(Path.Combine(ModxRoot, "license.txt"));
				bool zipInRoot;
				try
				{
					zipInRoot = ((new FileInfo(ZipFile)).DirectoryName == ModxRoot);
				}
				catch
				{
					zipInRoot = false;
				}
				bool reportInRoot = File.Exists(Path.Combine(ModxRoot, "report.log"));
				bool htmlReportInRoot = File.Exists(Path.Combine(ModxRoot, "report.htm"));

				if (xmlInRoot > 1)
				{
					report.Append(string.Format("{0} More than one MODX file included in '/' (see 5.1. The Root Directory).\n",
						fail));
					modxError = true;
				}

				if (styleSheetsIncluded < 1)
				{
					report.Append(string.Format("{0} No XML stylesheets have been included.\n",
						fail));
					modxError = true;
				}
				else if (styleSheetsIncluded > 1)
				{
					report.Append(string.Format("{0} More than one XML stylesheets have been included, it is recommended you only include one.\n",
						warning));
					modxError = true;
				}

				if (!licenseIncluded)
				{
					report.Append(string.Format("{0} No license.txt file has been included, please include the full license for the MOD (see 5.1. The Root Directory).\n",
						fail));
					modxError = true;
				}

				if (filesInRoot - ((zipInRoot) ? 1 : 0) - ((reportInRoot) ? 1 : 0) - ((htmlReportInRoot) ? 1 : 0) > styleSheetsIncluded + xmlInRoot + ((licenseIncluded) ? 1 : 0))
				{
					report.Append(string.Format("{0} Unauthorised files included in '/' (see 5.1. The Root Directory).\n",
						fail));
					modxError = true;
				}

				int dirsInRoot = Directory.GetDirectories(ModxRoot).Length;
				bool contribExists = Directory.Exists(Path.Combine(ModxRoot, "contrib"));
				bool languagesExists = Directory.Exists(Path.Combine(ModxRoot, "languages"));
				bool templatesExists = Directory.Exists(Path.Combine(ModxRoot, "templates"));
				bool rootExists = Directory.Exists(Path.Combine(ModxRoot, "root"));

				int incDirs = 0;
				if (contribExists) incDirs++;
				if (languagesExists) incDirs++;
				if (templatesExists) incDirs++;
				if (rootExists) incDirs++;

				if (dirsInRoot != incDirs)
				{
					report.Append(string.Format("{0} Extra directorys included under root, please re-organise (see 5. Packaging Mods).\n",
						fail));
					modxError = true;
				}

				if (languagesExists)
				{
					if (PhpbbVersion == 2)
					{
						LoadPhpbb2Languages();
						string[] languages = Directory.GetFiles(Path.Combine(ModxRoot, "languages"), "*.xml");
						bool allLanguagesValid = true;
						foreach (string language in languages)
						{
							FileInfo fi = new FileInfo(language);
							string lang = fi.Name.Substring(0, fi.Name.Length - fi.Extension.Length);
							if (Array.BinarySearch(Phpbb2LanguageList, lang) >= 0)
							{
								Console.WriteLine("Found language {0}", lang);
							}
							else
							{
								report.Append(string.Format("{0} Language files found for a language phpBB2 does not have a language pack for, {1}.\n",
									error, lang));
								allLanguagesValid = false;
							}
						}

						if (!allLanguagesValid)
						{
							report.Append(string.Format("{0} Improper use of language files '/' (see 5.3. The Directory '/languages').\n",
								error));
						}
					}
					else if (PhpbbVersion == 3)
					{
						LoadPhpbb3Languages();
						string[] languages = Directory.GetFiles(Path.Combine(ModxRoot, "languages"), "*.xml");
						bool allLanguagesValid = true;
						foreach (string language in languages)
						{
							FileInfo fi = new FileInfo(language);
							string lang = fi.Name.Substring(0, fi.Name.Length - fi.Extension.Length);
							if (Array.BinarySearch(Phpbb3LanguageList, lang) >= 0)
							{
								Console.WriteLine("Found language {0}", lang);
							}
							else
							{
								report.Append(string.Format("{0} Language files found for a language phpBB3 does not have a language pack for, {1}.\n",
									error, lang));
								allLanguagesValid = false;
							}
						}

						if (!allLanguagesValid)
						{
							report.Append(string.Format("{0} Improper use of language files '/' (see 5.3. The Directory '/languages').\n",
								error));
							modxError = true;
						}
					}
				}

				if (!modxError)
				{
					report.Append(string.Format("{0} MODX content complies to packaging specifications\n",
						pass));
				}
			}

            if (PhpbbVersion == 3)
            {
                report.Append(string.Format("\n{0} Validating a phpBB3.0 MOD, [i]This message is informational only[/i]\n",
                    info));
            }
            else
            {
                report.Append(string.Format("\n{0} Validating a phpBB2.0 MOD, [i]This message is informational only[/i]\n",
                    info));
            }
            report.Append("If the above phpBB version is wrong, then you haven't properly packaged this MOD.");

#if eal
			SaveTextFile(report.ToString().Replace("\n", System.Environment.NewLine), Path.Combine(modPath, "report.log"));
#elif meal
			string html_header = "<html><head><title>MOD pre-Validation Report</title><style>body {font-family: Arial, Verdana, Sans-Serif; font-size: 10pt;}\nblockquote {border: solid 1px black; margin: 5px; padding: 3px;}\npre {border: solid 1px #666666; color: green; margin: 5px; padding: 3px;}</style></head><body>";
			string html_footer = "</body></html>";
			SaveTextFile(html_header + Phpbb.ModTeam.Tools.Validator.BbcodeToHtml(report.ToString()).Replace("\n", System.Environment.NewLine) + html_footer, Path.Combine(modPath, "report.htm"));
#endif
			// Borrowed from http://www.c-sharpcorner.com/Code/2002/July/ShellCommandsInCS.asp
			// I could've found in .net SDK therefore it's public domain
			System.Diagnostics.Process proc = new System.Diagnostics.Process();
			proc.EnableRaisingEvents = false;
#if eal
			proc.StartInfo.FileName = Path.Combine(modPath, "report.log");
#elif meal
			proc.StartInfo.FileName = Path.Combine(modPath, "report.htm");
#endif
			proc.Start();
		}

		private static void ValidateDirectory(string dir, StringBuilder report)
		{
			Console.WriteLine("Opening Directory: {0}", dir.Replace(modPath, ""));
			DirectoryInfo thisDirInf = new DirectoryInfo(dir);

			// validate the directory name as a language
			if (PhpbbVersion == 2 && inLanguages && !languageSubDir)
			{
				bool languageIsValid = false;
				for (int i = 0; i < Phpbb2LanguageList.Length; i++)
				{
					if (Phpbb2LanguageList[i] == thisDirInf.Name)
					{
						languageIsValid = true;
					}
				}

				if (!languageIsValid)
				{
					report.Append(string.Format("\n{0} Folder '{1}' in translations folder is not a valid phpBB translation\n",
						warning, thisDirInf.Name));
					currentLanguage = "english";
				}
				else
				{
					currentLanguage = thisDirInf.Name;
					Console.WriteLine("Switched to language {0}", currentLanguage);
				}
			}

            if (thisDirInf.Name == "languages" || thisDirInf.Name == "translations")
			{
				inLanguages = true;
				Console.WriteLine("Validating Translations");
                if (PhpbbVersion == 2)
                {
                    LoadPhpbb2Languages();
                }
                else if (PhpbbVersion == 3)
                {
                    LoadPhpbb3Languages();
                }
			}
			if (inLanguages)
			{
				languageSubDir = false;
			}

			foreach (string file in Directory.GetFiles(dir, "*.xml")) 
			{
				FileInfo langFileInf = new FileInfo(file);
				if (thisDirInf.Name == "languages")
				{
					bool languageIsValid = false;
					string languageOfFile = langFileInf.Name.Remove(langFileInf.Name.Length - langFileInf.Extension.Length, langFileInf.Extension.Length);
					/*if (languageOfFile.StartsWith ("lang_"))
					{
						languageOfFile = languageOfFile.Remove(0, 5);
					}*/
                    if (PhpbbVersion == 2)
                    {
                        LoadPhpbb2Languages();
                        for (int i = 0; i < Phpbb2LanguageList.Length; i++)
                        {
                            if (Phpbb2LanguageList[i] == languageOfFile)
                            {
                                languageIsValid = true;
                            }
                        }
                    }
                    else if (PhpbbVersion == 3)
                    {
                        LoadPhpbb3Languages();
                        for (int i = 0; i < Phpbb3LanguageList.Length; i++)
                        {
                            if (Phpbb3LanguageList[i] == languageOfFile)
                            {
                                languageIsValid = true;
                            }
                        }
                    }
					if (!languageIsValid)
					{
						report.Append(string.Format("\n{0} File '{1}' in translations folder is not a valid phpBB translation\n", 
							warning, languageOfFile));
						currentLanguage = "english";
					}
					else
					{
						currentLanguage = languageOfFile;
						Console.WriteLine("Switched to language {0}", currentLanguage);
					}
				}
				report.Append(ValidateMod(file));
			}
			foreach (string file in Directory.GetFiles(dir, "*.txt"))
			{
				if (OpenTextFile(file).StartsWith("##"))
				{
					FileInfo langFileInf = new FileInfo(file);
					if (thisDirInf.Name == "translations")
					{
						bool languageIsValid = false;
						string languageOfFile = langFileInf.Name.Remove(langFileInf.Name.Length - langFileInf.Extension.Length, langFileInf.Extension.Length);
						if (languageOfFile.StartsWith ("lang_"))
						{
							languageOfFile = languageOfFile.Remove(0, 5);
						}
                        LoadPhpbb2Languages();
						for (int i = 0; i < Phpbb2LanguageList.Length; i++)
						{
							if (Phpbb2LanguageList[i] == languageOfFile)
							{
								languageIsValid = true;
							}
						}
						if (!languageIsValid)
						{
							report.Append(string.Format("\n{0} File '{1}' in translations folder is not a valid phpBB translation\n", 
								warning, languageOfFile));
							currentLanguage = "english";
						}
						else
						{
							currentLanguage = languageOfFile;
						}
						Console.WriteLine("Switched to language {0}", currentLanguage);
					}
					report.Append(ValidateMod(file));
				}
				else
				{
					if (file.ToLower().EndsWith("license.txt"))
					{
						report.Append(string.Format("{0} Found license.txt\n",
							ok));
					}
					else
					{
						report.Append(string.Format("{0} {1} not a MOD Template file\n",
							ok, file.Replace(modPath, "")));
					}
				}
			}
			foreach (string file in Directory.GetFiles(dir, "*.mod"))
			{
				FileInfo langFileInf = new FileInfo(file);
				if (thisDirInf.Name == "translations")
				{
					bool languageIsValid = false;
					string languageOfFile = langFileInf.Name.Remove(langFileInf.Name.Length - langFileInf.Extension.Length, langFileInf.Extension.Length);
					if (languageOfFile.StartsWith ("lang_"))
					{
						languageOfFile = languageOfFile.Remove(0, 5);
					}
					for (int i = 0; i < Phpbb2LanguageList.Length; i++)
					{
						if (Phpbb2LanguageList[i] == languageOfFile)
						{
							languageIsValid = true;
						}
					}
					if (!languageIsValid)
					{
						report.Append(string.Format("\n{0} File '{1}' in translations folder is not a valid phpBB translation\n", 
							warning, languageOfFile));
						currentLanguage = "english";
					}
					else
					{
						currentLanguage = languageOfFile;
					}
					Console.WriteLine("Switched to language {0}", currentLanguage);
				}
				report.Append(ValidateMod(file));
			}

			if (ModxRoot == "")
			{
				// check to see if this folder is the MODX root
				if (ModxContentIncluded)
				{
					// this is the root dir
					ModxRoot = dir;
				}
			}

			foreach(string file in Directory.GetFiles(dir, "*.php"))
			{
				if (PhpbbVersion == 2)
				{
					if (file.IndexOf("admin_") >= 0 || file.IndexOf(Path.DirectorySeparatorChar + "admin" + Path.DirectorySeparatorChar) >= 0)
					{
						adminInclude = true;
					}
					else
					{
						adminInclude = false;
					}
				}
				else if (PhpbbVersion == 3)
				{
					if (file.IndexOf("adm_") >= 0 || file.IndexOf(Path.DirectorySeparatorChar + "acp" + Path.DirectorySeparatorChar) >= 0)
					{
						adminInclude = true;
					}
					else
					{
						adminInclude = false;
					}
                    if (file.Contains(string.Format("{0}language{0}en{0}", Path.DirectorySeparatorChar)) || file.Contains(string.Format("{0}language{0}lang_english{0}", Path.DirectorySeparatorChar)))
                    {
                        // spell check
                        string languageFile = OpenTextFile(file).Replace("\r\n", "\n");
                        string[] languageFileItems = languageFile.Split(new char[] { ' ', '\t', '\'', '\n', '?', ',', '.', '(', ')', '[', ']' });
                        bool spellCheckOk = true;
                        for (int i = 0; i < languageFileItems.Length; i++)
                        {
                            if (Regex.IsMatch(languageFileItems[i], @"^([a-zA-Z0-9\-]+)$"))
                            {
                                if (Array.IndexOf(EnglishWordsList, languageFileItems[i].ToLower()) < 0)
                                {
                                    if (spellCheckOk)
                                    {
                                        spellCheckOk = false;
                                        report.Append(string.Format("[quote=\"Running Spell Check on {0}\"]\n", file));
                                    }
                                    report.Append(string.Format("{0} Word incorrectly spelt '{1}'.\n",
                                        warning, languageFileItems[i]));
                                }
                            }
                        }

                        if (!spellCheckOk)
                        {
                            report.Append("[/quote]\n");
                        }
                        else
                        {
                            report.Append(string.Format("{0} All words spelt correctly in {1}\n",
                                ok, file));
                        }
                    }
				}
				hasIncludes = false;
				string phpReport = ValidatePhp(OpenTextFile(file));
				if ((adminInclude && hasIncludes) || phpReport.Length > 0)
				{
					report.Append(string.Format("\n[quote=\"Validating {0}\"]\n", file.Replace(modPath, "")));
				}
				if (adminInclude && hasIncludes)
				{
					report.Append(string.Format("\n{0} This file was detected to be an admin panel file, if this is not the case check all [u]includes[/u] manually.\n",
						notice));
				}
				if (phpReport.Length > 0)
				{
					report.Append(phpReport);
				}
				else
				{
					report.Append(string.Format("{0} Automated pre-scan of PHP found no problems with {1}, proceed with manual check.\n",
						pass, file.Replace(modPath, "")));
				}
				if ((adminInclude && hasIncludes) || phpReport.Length > 0)
				{
					report.Append("[/quote]\n");
				}
				string htmlReport = ValidateHtml(OpenTextFile(file));
				if (htmlReport.Length > 0)
				{
					report.Append(string.Format("\n[quote=\"Validating {0}\"]\n", file.Replace(modPath, "")));
					report.Append(htmlReport);
					report.Append("[/quote]\n");
				}
				else
				{
					report.Append(string.Format("{0} Automated pre-scan of HTML found no problems with {1}, proceed with manual check.\n",
						pass, file.Replace(modPath, "")));
				}
			}
			foreach(string file in Directory.GetFiles(dir, "*.inc"))
			{
				if (PhpbbVersion == 2)
				{
					if (file.IndexOf("admin_") >= 0 || file.IndexOf(Path.DirectorySeparatorChar + "admin" + Path.DirectorySeparatorChar) >= 0)
					{
						adminInclude = true;
					}
					else
					{
						adminInclude = false;
					}
				}
				else if (PhpbbVersion == 3)
				{
					if (file.IndexOf("adm_") >= 0 || file.IndexOf(Path.DirectorySeparatorChar + "acp" + Path.DirectorySeparatorChar) >= 0)
					{
						adminInclude = true;
					}
					else
					{
						adminInclude = false;
					}
				}
				hasIncludes = false;
				string phpReport = ValidatePhp(OpenTextFile(file));
				if ((adminInclude && hasIncludes) || phpReport.Length > 0)
				{
					report.Append(string.Format("\n[quote=\"Validating {0}\"]\n", file.Replace(modPath, "")));
				}
				if (adminInclude && hasIncludes)
				{
					report.Append(string.Format("\n{0} This file was detected to be an admin panel file, if this is not the case check all [u]includes[/u] manually.\n",
						notice));
				}
				if (phpReport.Length > 0)
				{
					report.Append(phpReport);
				}
				else
				{
					report.Append(string.Format("{0} Automated pre-scan of PHP found no problems with {1}, proceed with manual check.\n",
						pass, file.Replace(modPath, "")));
				}
				if ((adminInclude && hasIncludes) || phpReport.Length > 0)
				{
					report.Append("[/quote]\n");
				}
				string htmlReport = ValidateHtml(OpenTextFile(file));
				if (htmlReport.Length > 0)
				{
					report.Append(string.Format("\n[quote=\"Validating {0}\"]\n", file.Replace(modPath, "")));
					report.Append(htmlReport);
					report.Append("[/quote]\n");
				}
				else
				{
					report.Append(string.Format("{0} Automated pre-scan of HTML found no problems with {1}, proceed with manual check.\n",
						pass, file.Replace(modPath, "")));
				}
			}

			// 2.0 templates
			foreach(string file in Directory.GetFiles(dir, "*.tpl"))
			{
				string htmlReport = ValidateHtml(OpenTextFile(file));
				if (htmlReport.Length > 0)
				{
					report.Append(string.Format("\n[quote=\"Validating {0}\"]\n", file.Replace(modPath, "")));
					report.Append(htmlReport);
					report.Append("[/quote]\n");
				}
				else
				{
					report.Append(string.Format("{0} Automated pre-scan of HTML found no problems with {1}, proceed with manual check.\n",
						pass, file.Replace(modPath, "")));
				}
			}
			// 3.0 templates
			foreach(string file in Directory.GetFiles(dir, "*.html"))
			{
				string htmlReport = ValidateHtml(OpenTextFile(file));
				if (htmlReport.Length > 0)
				{
					report.Append(string.Format("\n[quote=\"Validating {0}\"]\n", file.Replace(modPath, "")));
					report.Append(htmlReport);
					report.Append("[/quote]\n");
				}
				else
				{
					report.Append(string.Format("{0} Automated pre-scan of HTML found no problems with {1}, proceed with manual check.\n",
						pass, file.Replace(modPath, "")));
				}
			}

			foreach (string dir_ in Directory.GetDirectories(dir))
			{
				if (thisDirInf.Name == "translations")
				{
					languageSubDir = false;
				}
				ValidateDirectory(dir_, report);
			}
			if (!languageSubDir && inLanguages)
			{
				inLanguages = false;
				currentLanguage = "english";
				Console.WriteLine("Switched to language {0}", currentLanguage);
			}
		}

		private static string DownloadMod(string modToDownload, string path)
		{
			if (!System.IO.Directory.Exists(path)) System.IO.Directory.CreateDirectory(path);
			Uri mtd = new Uri(modToDownload);

			System.Net.WebClient Downloader = new WebClient();
			Console.WriteLine("Downloading...");

			string[] mtda = mtd.AbsolutePath.Split('/');
			string fn = mtda[mtda.GetUpperBound(0)];

			string retValue = System.Web.HttpUtility.UrlDecode(Path.Combine(path, fn));
			Downloader.DownloadFile(modToDownload, retValue);
			Console.WriteLine("Saved To {0}", path);
			Downloader.Dispose();
			return retValue;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="fileToExtract"></param>
		private static void ExtractMod(string fileToExtract)
		{
			ExtractMod(fileToExtract, "");
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="fileToExtract"></param>
		/// <param name="pathToExtractTo">Name of a sub-directory to extract to</param>
		private static void ExtractMod(string fileToExtract, string pathToExtractTo)
		{
			Console.WriteLine("Extracting...");
			ZipInputStream s = new ZipInputStream(File.OpenRead(fileToExtract));
			FileInfo fi = new FileInfo(fileToExtract);

			string dirName = fi.DirectoryName;
			if (pathToExtractTo.Length > 0)
			{
				dirName = Path.Combine(dirName, pathToExtractTo);
				if (!Directory.Exists(dirName))
				{
					Directory.CreateDirectory(dirName);
				}
			}

			ZipEntry theEntry;
			while ((theEntry = s.GetNextEntry()) != null)
			{
				Console.WriteLine(theEntry.Name);
				string DirName = Path.GetDirectoryName(theEntry.Name);
				string FileName = Path.GetFileName(theEntry.Name);

				Directory.CreateDirectory(Path.Combine(dirName, DirName));

				if (FileName != string.Empty)
				{
					FileStream streamWriter = File.Create(Path.Combine(dirName, theEntry.Name));
						
					int size = 2048;
					byte[] data = new byte[2048];
					while(true)
					{
						size = s.Read(data, 0, data.Length);
						if (size > 0)
						{
							streamWriter.Write(data, 0, size);
						}
						else
						{
							break;
						}
					}
					streamWriter.Close();
				}
			} // unzip
			s.Close();
		}

		private static string ValidateMod(string fileName)
		{
			StringBuilder report = new StringBuilder();
			report.Append(string.Format("\n[quote=\"Validating {0}\"]\n", fileName.Replace(modPath, "")));
			FileInfo modFileInfo = new FileInfo(fileName);
			Console.WriteLine("Validating: {0}", fileName.Replace(modPath, ""));
			switch(modFileInfo.Extension.ToLower())
			{
				case ".mod":
				case ".txt":
					try
					{
						report.Append("[quote=\"Validating Text Template\"]\n");
						if (PhpbbVersion == 3)
						{
							report.Append(string.Format("{0} Text Template included in a phpBB3.0 MOD package",
								error));
						}
						TextMod thisMod = new TextMod();
						try
						{
							thisMod.Read(fileName);
							if (thisMod.Header.Version.Major < 1 || (thisMod.Header.Version.Major > 1 && thisMod.Header.Version.Minor % 2 == 1))
							{
								report.Append(string.Format("{0} MOD Version number ({1}) indicates development release, expecting stable release\n\n",
									notice, thisMod.Header.Version.ToString()));
							}
						}
						catch (Phpbb.ModTeam.Tools.DataStructures.ModAuthorParseException ex)
						{
							thisMod.ReadActions(OpenTextFile(fileName));
							report.Append(string.Format("{0} Error Parsing MOD Author\n",
								warning));
							//report.Append(string.Format("[quote]{0}[/quote]", ex.ToString()));
						}
						catch (System.FormatException ex)
						{
							thisMod.ReadActions(OpenTextFile(fileName));
							report.Append(string.Format("{0} Format Exception, this is usually caused by an improperly formed dates in the MOD History section.\n",
								warning));
							//report.Append(string.Format("[quote]{0}[/quote]", ex.ToString()));
						}
						if (PhpbbVersion == 2)
						{
							report.Append(thisMod.Validate(fileName, currentLanguage, new ModVersion(2, 0, 0), false).ToString(false));
						}
						else if (PhpbbVersion == 3)
						{
							report.Append(thisMod.Validate(fileName, currentLanguage, new ModVersion(3, 0, 0), false).ToString(false));
						}
						Console.WriteLine("Validated MOD Expecting {0}", currentLanguage);
						report.Append("[/quote]\n");
						string actionPhpReport = ValidatePhpInMod(thisMod.Actions);
						if (actionPhpReport.Length > 0)
						{
							report.Append("[quote=\"Validating PHP\"]\n");
							report.Append(actionPhpReport);
							report.Append("[/quote]\n");
						}
						else
						{
							report.Append(string.Format("{0} Automated pre-scan of PHP found no problems, proceed with manual check.\n",
								pass));
						}
						string actionHtmlReport = ValidateHtmlInMod(thisMod.Actions);
						if (actionHtmlReport.Length > 0)
						{
							report.Append("[quote=\"Validating HTML\"]\n");
							report.Append(actionHtmlReport);
							report.Append("[/quote]\n");
						}
						else
						{
							report.Append(string.Format("{0} Automated pre-scan of HTML found no problems, proceed with manual check.\n",
								pass));
						}

                        report.Append(ValidateLanguageInMod(thisMod.Actions));

						ModFileIncluded = true;
					}
					catch (NotATextModException)
					{
						report.Append(string.Format("{0} Error validating file\n",
							error));
						report.Append("[/quote]\n");
					}
					catch (NotAModVersionException)
					{
						TextMod thisMod = new TextMod();
						report.Append("Error reading MOD Versions in this file, proceeding to run text template validator, some checks aborted.");
						report.Append("[quote=\"Validating Text Template\"]\n");
						report.Append(thisMod.Validate(fileName, currentLanguage, new ModVersion(PhpbbVersion, 0, 0), false).ToString());
						report.Append("[/quote]\n");
						thisMod.ReadActions(OpenTextFile(fileName));
						string actionPhpReport = ValidatePhpInMod(thisMod.Actions);
						if (actionPhpReport.Length > 0)
						{
							report.Append("[quote=\"Validating PHP\"]\n");
							report.Append(actionPhpReport);
							report.Append("[/quote]\n");
						}
						else
						{
							report.Append(string.Format("{0} Automated pre-scan of PHP found no problems, proceed with manual check.\n",
								pass));
						}
						string actionHtmlReport = ValidateHtmlInMod(thisMod.Actions);
						if (actionHtmlReport.Length > 0)
						{
							report.Append("[quote=\"Validating HTML\"]\n");
							report.Append(actionHtmlReport);
							report.Append("[/quote]\n");
						}
						else
						{
							report.Append(string.Format("{0} Automated pre-scan of HTML found no problems, proceed with manual check.\n",
								pass));
						}
                        
                        report.Append(ValidateLanguageInMod(thisMod.Actions));

						report.Append("[/quote]\n");
						ModFileIncluded = true;
					}
					break;
				case ".xml":
					try
					{
						XmlTextReader xr = new XmlTextReader(fileName);
						while(xr.Read() && xr.Name != "mod");
						if(xr.NamespaceURI == @"http://www.phpbb.com/mods/xml/modx-1.0.xsd" ||
                            xr.NamespaceURI == @"http://www.phpbb.com/mods/xml/modx-1.0.1.xsd")
						{
							ModxMod thisMod = new ModxMod();
							thisMod.Read(fileName);

                            /* *** BEFORE *** */
                            try
                            {
                                ModVersion tempVersion = ModVersion.Parse(thisMod.Header.PhpbbVersion.Primary);
                                PhpbbVersion = tempVersion.Major;
                                Console.WriteLine("Switching to phpBB Version: {0}",
                                    PhpbbVersion);
                            }
                            catch (NotAModVersionException)
                            {
                                foreach (TargetVersionCase versionCase in thisMod.Header.PhpbbVersion)
                                {
                                    if (versionCase.Part == TargetVersionPart.Major && versionCase.Comparisson == TargetVersionComparisson.EqualTo)
                                    {
                                        PhpbbVersion = int.Parse(versionCase.GetValue);
                                        break;
                                    }
                                }
                                Console.WriteLine("Switching to phpBB Version: {0}",
                                    PhpbbVersion);
                            }
                            catch
                            {
                                Console.WriteLine("Error extracting a major version number from MODX");
                                PhpbbVersion = 2;
                            }
                            /* ************** */

							report.Append("[quote=\"Validating MODX Format\"]\n");
							if (PhpbbVersion == 2)
							{
								report.Append(thisMod.Validate(fileName, currentLanguage, new ModVersion(2, 0, 0), false).ToString(false));
							}
							else if (PhpbbVersion == 3)
							{
								report.Append(thisMod.Validate(fileName, currentLanguage, new ModVersion(3, 0, 0), false).ToString(false));
							}
							report.Append("[/quote]\n");
							string actionPhpReport = ValidatePhpInMod(thisMod.Actions);
							if (actionPhpReport.Length > 0)
							{
								report.Append("[quote=\"Validating PHP\"]\n");
								report.Append(actionPhpReport);
								report.Append("[/quote]\n");
							}
							else
							{
								report.Append(string.Format("{0} Automated pre-scan of PHP found no problems, proceed with manual check.\n",
									pass));
							}
							string actionHtmlReport = ValidateHtmlInMod(thisMod.Actions);
							if (actionHtmlReport.Length > 0)
							{
								report.Append("[quote=\"Validating HTML\"]\n");
								report.Append(actionHtmlReport);
								report.Append("[/quote]\n");
							}
							else
							{
								report.Append(string.Format("{0} Automated pre-scan of HTML found no problems, proceed with manual check.\n",
									pass));
							}

                            report.Append(ValidateLanguageInMod(thisMod.Actions));

							ModFileIncluded = true;
							ModxContentIncluded = true;
							/*if (thisMod.Header.PhpbbVersion != null)
							{
								PhpbbVersion = thisMod.Header.PhpbbVersion.Major;
							}*/
							
						}
						else
						{
							report.Append("Not a MODX file, this may be intentional on the authors part, check.");
						}
					}
					catch (Exception ex)
					{
						report.Append(string.Format("{0} Error validating file\n",
							error));
						Console.WriteLine(ex.ToString());
					}
					break;
			}
			report.Append("[/quote]\n");
			return report.ToString();
		}

        public static string ValidateLanguageInMod(Phpbb.ModTeam.Tools.DataStructures.ModActions actions)
        {
            string lastFileOpen = "";
            char[] trimChars = { '\n', '\r', '\t', ' ', '\b' };
            bool spellCheckOk = true;
            StringBuilder report = new StringBuilder();
            foreach (ModAction action in actions)
            {
                if (action.Type == "OPEN")
                {
                    if (lastFileOpen.StartsWith("langauge/en/") && PhpbbVersion == 3)
                    {
                        if (!spellCheckOk)
                        {
                            report.Append("[/quote]\n");
                        }
                        else
                        {
                            report.Append(string.Format("{0} All words spelt correctly in {1}\n",
                                ok, lastFileOpen));
                        }
                    }

                    lastFileOpen = action.Body.Trim(trimChars);
                    spellCheckOk = true;
                }

                {
                    if ((lastFileOpen.StartsWith("langauge/en/") && PhpbbVersion == 3) || (lastFileOpen.StartsWith("langauge/lang_english/") && PhpbbVersion == 2))
                    {
                        if (action.Type == "REPLACE WITH" ||
                            action.Type == "AFTER, ADD" ||
                            action.Type == "BEFORE, ADD" ||
                            action.Type == "IN-LINE REPLACE WITH" ||
                            action.Type == "IN-LINE AFTER, ADD" ||
                            action.Type == "IN-LINE BEFORE, ADD")
                        {
                            // spell check
                            string languageFile = action.Body.Replace("\r\n", "\n");
                            string[] languageFileItems = languageFile.Split(new char[] { ' ', '\t', '\'', '\n', '?', ',', '.', '(', ')', '[', ']' });
                            for (int i = 0; i < languageFileItems.Length; i++)
                            {
                                if (Regex.IsMatch(languageFileItems[i], @"^([a-zA-Z0-9\-]+)$"))
                                {
                                    if (Array.IndexOf(EnglishWordsList, languageFileItems[i].ToLower()) < 0)
                                    {
                                        if (spellCheckOk)
                                        {
                                            spellCheckOk = false;
                                            report.Append(string.Format("[quote=\"Running Spell Check on {0}\"]\n", lastFileOpen));
                                        }
                                        report.Append(string.Format("{0} Word incorrectly spelt '{1}'.\n",
                                            warning, languageFileItems[i]));
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return report.ToString();
        }

		private static string ValidatePhpInMod(Phpbb.ModTeam.Tools.DataStructures.ModActions actions)
		{
			string lastFileOpen = "";
			char[] trimChars = {'\n', '\r', '\t', ' ', '\b'};
			StringBuilder report = new StringBuilder();
			foreach (ModAction action in actions)
			{
				if (action.Type == "OPEN" || action.Type == "SAVE/CLOSE ALL FILES")
				{
					if (PhpbbVersion == 2)
					{
						if ((lastFileOpen.IndexOf("admin_") >= 0 || lastFileOpen.IndexOf("admin/") == 0) && hasIncludes)
						{
							report.Append(string.Format("\n{0} A file edited by the MOD was detected to be an admin panel file ({1}), if this is not the case check all [u]includes[/u] manually.\n",
								notice, lastFileOpen));
						}
					}
					else if (PhpbbVersion == 3)
					{
						if ((lastFileOpen.IndexOf("adm_") >= 0 || lastFileOpen.IndexOf("acp/") == 0) && hasIncludes)
						{
							report.Append(string.Format("\n{0} A file edited by the MOD was detected to be an admin panel file ({1}), if this is not the case check all [u]includes[/u] manually.\n",
								notice, lastFileOpen));
						}
					}
				}
				if (action.Type == "OPEN")
				{
					lastFileOpen = action.Body.Trim(trimChars);
					hasIncludes = false;
				}
				if (action.Type == "REPLACE WITH" ||
					action.Type == "AFTER, ADD" ||
					action.Type == "BEFORE, ADD" ||
					action.Type == "IN-LINE REPLACE WITH" ||
					action.Type == "IN-LINE AFTER, ADD" ||
					action.Type == "IN-LINE BEFORE, ADD")
				{
					if (PhpbbVersion == 2)
					{
						if ((lastFileOpen.IndexOf("admin_") >= 0 || lastFileOpen.IndexOf("admin/") == 0))
						{
							adminInclude = true;
						}
						else
						{
							adminInclude = false;
						}
					}
					else if (PhpbbVersion == 3)
					{
						if ((lastFileOpen.IndexOf("adm_") >= 0 || lastFileOpen.IndexOf("acp/") == 0))
						{
							adminInclude = true;
						}
						else
						{
							adminInclude = false;
						}
					}
                    string actionReport = "";
                    if (lastFileOpen.EndsWith(".php") || lastFileOpen.EndsWith(".cfg") || lastFileOpen.EndsWith(".inc"))
                    {
                        actionReport = ValidatePhp(action.Body, action.StartLine, true);
                    }
                    else
                    {
					    
                    }
					if (actionReport.Length > 0)
					{
						report.Append(actionReport);
					}
				}
			}
			return report.ToString();
		}

		private static string ValidateHtmlInMod(Phpbb.ModTeam.Tools.DataStructures.ModActions actions)
		{
			StringBuilder report = new StringBuilder();
			foreach (ModAction action in actions)
			{
				if (action.Type == "REPLACE WITH" ||
					action.Type == "AFTER, ADD" ||
					action.Type == "BEFORE, ADD" ||
					action.Type == "IN-LINE REPLACE WITH" ||
					action.Type == "IN-LINE AFTER, ADD" ||
					action.Type == "IN-LINE BEFORE, ADD")
				{
					string actionReport = ValidateHtml(action.Body, action.StartLine, true);
					if (actionReport.Length > 0)
					{
						report.Append(actionReport);
					}
				}
			}
			return report.ToString();
		}

		private static string ValidateHtml(string input)
		{
			return ValidateHtml(input, 0, false);
		}

		private static string ValidateHtml(string input, bool fragment)
		{
			return ValidateHtml(input, 0, fragment);
		}

		/// <summary>
		/// Validate HTML fragments
		/// </summary>
		/// <param name="input"></param>
		/// <param name="indexOffset"></param>
		/// <param name="fragment"></param>
		/// <returns></returns>
		private static string ValidateHtml(string input, int indexOffset, bool fragment)
		{
			StringBuilder report = new StringBuilder();
			string[] lines = input.Replace("\r\n", "\n").Split('\n');

			bool forbiddenFlag = false;
			bool inBLockComment = false;
			for (int i = 0; i < lines.Length; i++)
			{
				string line = lines[i];

				//
				// <font>
				//
				if (line.IndexOf("font") >= 0)
				{
					if (Regex.IsMatch(line, "<(\\s*)font(\\s+)", System.Text.RegularExpressions.RegexOptions.IgnoreCase))
					{
						Console.WriteLine("phpBB xHTML Rule Violation on line: {0}", i + 1 + indexOffset);
						report.Append(string.Format("{0} phpBB xHTML Rule Violation on line: {1}, use of font tag is prohibited\n[code]{2}[/code]\n\n",
                            warning, i + 1 + indexOffset, lines[i]));
					}
				}

				//
				// <img>
				//
				if (line.IndexOf("img") >= 0)
				{
					if (Regex.IsMatch(line, "<(\\s*)img(\\s+)(([a-zA-Z_0-9]+=(\"[\\w\\s\\{\\}\\\\\\/\\.]*\"|'[\\w\\s\\{\\}\\\\\\/\\.]*')(\\s*))*)(?!\\/)(\\s*)>", System.Text.RegularExpressions.RegexOptions.IgnoreCase))
					{
						Console.WriteLine("phpBB xHTML Rule Violation on line: {0}", i + 1 + indexOffset);
						report.Append(string.Format("{0} phpBB xHTML Rule Violation on line: {1}, must close image tag for valid xHTML output\n[code]{2}[/code]\n\n",
                            fail, i + 1 + indexOffset, lines[i]));
					}
				}

				//
				// <br>
				//
				if (line.IndexOf("br") >= 0)
				{
					if (Regex.IsMatch(line, "<(\\s*)br(\\s*)(([a-zA-Z_0-9]+=(\"[\\w\\s\\{\\}\\\\\\/\\.]*\"|'[\\w\\s\\{\\}\\\\\\/\\.]*')(\\s*))*)(?!\\/)(\\s*)>", System.Text.RegularExpressions.RegexOptions.IgnoreCase))
					{
						Console.WriteLine("phpBB xHTML Rule Violation on line: {0}", i + 1 + indexOffset);
						report.Append(string.Format("{0} phpBB xHTML Rule Violation on line: {1}, must close line break tag for valid xHTML output\n[code]{2}[/code]\n\n",
                            fail, i + 1 + indexOffset, lines[i]));
					}
				}

				//
				// <input>
				//
				if (line.IndexOf("input") >= 0)
				{
					if (Regex.IsMatch(line, "<(\\s*)input(\\s+)(([a-zA-Z_0-9]+=(\"[\\w\\s\\{\\}\\\\\\/\\.]*\"|'[\\w\\s\\{\\}\\\\\\/\\.]*')(\\s*))*)(?!\\/)(\\s*)>", System.Text.RegularExpressions.RegexOptions.IgnoreCase))
					{
						Console.WriteLine("phpBB xHTML Rule Violation on line: {0}", i + 1 + indexOffset);
						report.Append(string.Format("{0} phpBB xHTML Rule Violation on line: {1}, must close input tag for valid xHTML output\n[code]{2}[/code]\n\n",
                            fail, i + 1 + indexOffset, lines[i]));
					}
				}

				//
				// <form>
				//
				if (line.IndexOf("form") >= 0)
				{
					if (Regex.IsMatch(line, "<(\\s*)form(\\s+)(([a-zA-Z_0-9]+=(\"[\\w\\s\\{\\}\\\\\\/\\.]*\"|'[\\w\\s\\{\\}\\\\\\/\\.]*')(\\s*))*)(\\s*)>", System.Text.RegularExpressions.RegexOptions.IgnoreCase))
					{
						if (!Regex.IsMatch(line, "<(\\s*)form(\\s+)(([a-zA-Z_0-9]+=(\"[\\w\\s\\{\\}\\\\\\/\\.]*\"|'[\\w\\s\\{\\}\\\\\\/\\.]*')(\\s*))*)(action=\"\\{([a-z_0-9\\-\\.]*)([A-Z_0-9\\-]+)}\\\")(\\s*)(([a-zA-Z_0-9]+=(\"[\\w\\s\\{\\}\\\\\\/\\.]*\"|'[\\w\\s\\{\\}\\\\\\/\\.]*')(\\s*))*)(\\s*)>", System.Text.RegularExpressions.RegexOptions.IgnoreCase))
						{
							Console.WriteLine("phpBB xHTML Rule Violation on line: {0}", i + 1 + indexOffset);
							report.Append(string.Format("{0} phpBB xHTML Rule Violation on line: {1}, forms must have not hardcoded actions to facilitate passing SID\n[code]{2}[/code]\n\n",
                                fail, i + 1 + indexOffset, lines[i]));
						}
					}
				}

                // let's make sure all tags are valid
                if (!fragment)
                {
                    if (line.IndexOf("<") >= 0)
                    {
                        MatchCollection tagMatches = Regex.Matches(line, @"<([a-zA-Z]+)(>| )");

                        foreach (Match tagMatch in tagMatches)
                        {
                            string tagName = tagMatch.Groups[1].Value;
                            if (tagName != tagName.ToLower())
                            {
                                report.Append(string.Format("{0} phpBB xHTML Rule Violation on line: {1}, tags must all be in lower case\n[code]{2}[/code]\n\n",
                                    fail, i + 1 + indexOffset, lines[i]));
                            }
                        }
                    }
                }
			}

			return report.ToString();
		}

		private static string ValidatePhp(string input)
		{
			return ValidatePhp(input, 0, false);
		}

		private static string ValidatePhp(string input, bool fragment)
		{
			return ValidatePhp(input, 0, fragment);
		}

		private static string ValidatePhp(string input, int indexOffset)
		{
			return ValidatePhp(input, indexOffset, (indexOffset > 0));
		}

		/// <summary>
		/// Validate PHP fragments
		/// </summary>
		/// <param name="input"></param>
		/// <param name="indexOffset"></param>
		/// <param name="fragment"></param>
		/// <returns></returns>
		private static string ValidatePhp(string input, int indexOffset, bool fragment)
		{
			StringBuilder report = new StringBuilder();
			if (PhpbbVersion == 2)
			{
				if (FunctionList == null) LoadPhpFunctions();
			}
			else if (PhpbbVersion == 3)
			{
				if (FunctionList == null) LoadPhpFunctions3();
			}

			string[] lines = input.Replace("\r\n", "\n").Split('\n');
			string group = "";
            
            int indentLevel = 0;
            bool foundFragementIndentLevel = false;
            bool inSqlStatement = false;
            bool inMultiLineArray = false;

            int inMultiLineStatement = 0;
            int unClosedBrackets = 0;
            bool lastLineControlStatement = false;
            int braceOnLine = 0;

            bool badIndentSpaces = false;
            bool badIndentTabs = false;
            bool badBraces = false;
            bool badVariableCasing = false;

			//
			// Function list
			//
			bool forbiddenFlag = false;
			bool inBLockComment = false;
			for (int i = 0; i < lines.Length; i++)
			{
				string line = lines[i];

                if (line.Trim(new char[] { '\t', ' ' }).Length == 0)
                {
                    continue;
                }

                if (fragment)
                {
                    if ((foundFragementIndentLevel == false) && (line.Length > 0))
                    {
                        foundFragementIndentLevel = true;
                        indentLevel = line.Length - line.TrimStart(new char[] { '\t' }).Length;
                    }
                }

				foreach (string function in FunctionList)
				{
					if (function.StartsWith("#") || function.Equals(""))
					{
						char[] trimChars = {'#', ' '};
						group = function.Trim(trimChars);
					}
					else
					{
						// ignore functions that are commented out
						int funIndex = line.IndexOf(function);
						int commentIndex = line.IndexOf("//");
						int startBlockIndex = line.IndexOf("/*");
						if (commentIndex < 0) commentIndex = startBlockIndex;
						if (commentIndex < 0) commentIndex = line.Length;
						int endBlockIndex = line.IndexOf("*/");
						if (endBlockIndex > 0)
						{
							inBLockComment = false;
						}
						if (funIndex >= 0 && funIndex < commentIndex && funIndex > endBlockIndex && !inBLockComment)
						{
							if (Regex.IsMatch(line, @"([ \t\(\.\[\{\+-\=\*\&\^\%\@\!\,\/\\\|\~\:\;\<\>]+)" + function + @"(\s*)\(.\)"))
							{
								Console.WriteLine("Found '{0}' of '{1}', on line: {2}",
									function, group, i + 1 + indexOffset);
								report.Append(string.Format("{0} Found forbidden PHP function '{1}' belonging to the '{2}' group  of forbidden functions, on line: {3}\n",
									fail, function, group, i + 1 + indexOffset));
								report.Append(string.Format("[code]{0}[/code]\n",
                                    lines[i]));
								forbiddenFlag = true;
							}
						}
						if (endBlockIndex > 0)
						{
							inBLockComment = false;
						}
						else if (startBlockIndex >= 0)
						{
							inBLockComment = true;
						}
					}
				}
			}

            foundFragementIndentLevel = false;
            indentLevel = 0;

			//
			// Security
			//
			bool IN_PHPBB = false;
			bool IN_PHPBB_NOTICE = false;
			bool phpbb_root_path = true;
			bool inMultiLineComment = false;
			for (int i = 0; i < lines.Length; i++)
			{
				string line = lines[i];

                if (line.Trim(new char[] { '\t', ' ' }).Length == 0)
                {
                    continue;
                }

                if (fragment)
                {
                    if ((foundFragementIndentLevel == false) && (line.Length > 0))
                    {
                        foundFragementIndentLevel = true;
                        indentLevel = line.Length - line.TrimStart(new char[] { '\t' }).Length;

                        if (Regex.IsMatch(line, "^([\t ]*)}"))
                        {
                            indentLevel++;
                        }
                    }
                }

				// strip out comments
				line = Regex.Replace(line, @"(\s*)\/\/.+$", "");
				if (line.IndexOf("*/") >=0)
				{
					inMultiLineComment = false;
					line = Regex.Replace(line, "^.\\*/", "");
				}
				if (line.IndexOf("/*") >=0)
				{
					inMultiLineComment = true;
					line = Regex.Replace(line, "/\\*.$", "");
				}
				else
				{
					if (inMultiLineComment)
					{
						line = "";
					}
				}

                if (line.Length == 0)
                {
                    continue;
                }

				// securing user input
				if (PhpbbVersion == 2)
				{
					if (line.IndexOf("$HTTP_POST_VARS") >= 0 || line.IndexOf("$HTTP_GET_VARS") >= 0)
					{
						// ignore evaluations to a boolean
						if (Regex.Replace(line, @"([ \t\(\.\[\{\+-\=\*\&\^\%\@\!\,\/\\\|\~\:\;\<\>]+)(isset|empty)(\s*)\((.*)\)", "").IndexOf("$HTTP_POST_VARS") >= 0 ||
							Regex.Replace(line, @"([ \t\(\.\[\{\+-\=\*\&\^\%\@\!\,\/\\\|\~\:\;\<\>]+)(isset|empty)(\s*)\((.*)\)", "").IndexOf("$HTTP_GET_VARS") >= 0)
						{
							string tempLine = Regex.Replace(line, "(htmlspecialchars|intval)(\\s*)\\((\\s*)\\$HTTP_(GET|POST)_VARS\\[([a-zA-Z0-9\\.\\-\\'\\\"]+)\\](\\s*)\\)","");
							// ******************************************************************
							//  CHANGED SIGN OF >= TO < AS ON REVIEW IT LOOKED WRONG, VERIFY !!!
							// ******************************************************************
							//if (tempLine.IndexOf("htmlspecialchars") < 0 && tempLine.IndexOf("intval") < 0)
							if ((tempLine.IndexOf("$HTTP_POST_VARS") >= 0 || tempLine.IndexOf("$HTTP_GET_VARS") >= 0) && (tempLine.IndexOf("htmlspecialchars") < 0 && tempLine.IndexOf("intval") < 0))
							{
								// ******************************************
								//  REMOVED !Regex. AND REPLACED WITH Regex.
								// ******************************************
								if (Regex.IsMatch(tempLine, "(?![\\!<>])=(?!=)", System.Text.RegularExpressions.RegexOptions.Compiled))
								{
									bool capturedVar = false;
									string assignedVar = "";
									try
									{
										char[] trimWhitespace = {' ', '\t', '\b', '\r', '\n'};
										assignedVar = tempLine.TrimStart(trimWhitespace).Split('=')[0].TrimEnd(trimWhitespace);
										capturedVar = true;
									}
									catch
									{
										capturedVar = false;
									}
#if eal
								Console.WriteLine("Possible security concern on line: {0}", i + 1 + indexOffset);
								report.Append(string.Format("{0} Possible security concern on line: {1}\n[code]{2}[/code]\n\n", 
									fail, i + 1 + indexOffset, lines[i]));
#elif meal
									report.Append(string.Format("{0} Assignment from $HTTP_*_VARS without htmlspecialchars() or intval() sanitisation on line: {1}\n[code]{2}[/code]\n\n", 
										warning, i + 1 + indexOffset, lines[i]));
#endif
                                for (int j = i; j < lines.Length && capturedVar; j++)
									{
										string linej = lines[j];
										if (Regex.Replace(linej, "(htmlspecialchars|intval)(\\s*)\\((\\s*)" + Regex.Escape(assignedVar) + "(\\s*)\\)", "") != linej)
										{
											report.Append(string.Format("{0} Found variable '{3}' assigned from variable assigned from HTTP_*_VARS sanitised with htmlspecialchars() or intval() on line: {1}\n[code]{2}[/code]\n\n", 
												info, i + 1 + indexOffset, line, assignedVar));
										}
									}
								}
							}
						}
					}
				}

				// secure includes
				if (PhpbbVersion == 2 || PhpbbVersion == 3)
				{
					int includeIndex = line.IndexOf("include(");
					if (includeIndex < 0) includeIndex = line.IndexOf("include_once(");
					if (includeIndex < 0) includeIndex = line.IndexOf("require(");
					if (includeIndex < 0) includeIndex = line.IndexOf("require_once(");
					if (includeIndex >= 0) hasIncludes = true;
					if (!adminInclude)
					{
						if (includeIndex >= 0 && line.IndexOf("$phpbb_root_path") < includeIndex)
						{
#if eal
							Console.WriteLine("Possible security concern on line: {0}", i + 1 + indexOffset);
							report.Append(string.Format("{0} Possible security concern on line: {1}, include() without $phpbb_root_path\n[code]{2}[/code]\n\n", 
								warning, i + 1, lines[i]));
#elif meal
							report.Append(string.Format("{0} include() without $phpbb_root_path on line: {1}, must use $phpbb_root_path when including\n[code]{2}[/code]\n\n", 
								fail, i + 1, lines[i]));
#endif
                        }
					}
					else
					{
						if (includeIndex >= 0 && (line.IndexOf("'./") < includeIndex && line.IndexOf("$phpbb_root_path") < includeIndex))
						{
#if eal
							Console.WriteLine("Possible security concern on line: {0}", i + 1 + indexOffset);
							report.Append(string.Format("{0} Possible security concern on line: {1}, include() without $phpbb_root_path or './[...]'\n[code]{2}[/code]\n\n",
                                warning, i + 1, lines[i]));
#elif meal
							report.Append(string.Format("{0} include() without $phpbb_root_path or './[...]' on line: {1}, must use $phpbb_root_path or './[...]' when including\n[code]{2}[/code]\n\n", 
								fail, i + 1, lines[i]));
#endif
                        }
					}

					if (includeIndex >= 0 && line.IndexOf("$phpEx") < includeIndex && line.IndexOf(".php") > includeIndex)
					{
#if eal
						report.Append(string.Format("{0} include() php file with hardcoded file extension, on line: {1}, must use $phpEx\n[code]{2}[/code]\n\n",
                            fail, i + i, lines[i]));
#elif meal
						report.Append(string.Format("{0} include() php file with hardcoded file extension, on line: {1}, must use $phpEx\n[code]{2}[/code]\n\n",
							fail, i+i, lines[i]));
#endif
                    }
				}

				// <?
				if (PhpbbVersion == 2 || PhpbbVersion == 3)
				{
					if (line.IndexOf("<?") >= 0)
					{
						if (line.IndexOf("<?php") < 0 && line.IndexOf("<?=") < 0)
						{
							report.Append(string.Format("{0} Use of short php start tag '<?' on line: {1}\n[code]{2}[/code]\n\n",
                                fail, i + 1 + indexOffset, lines[i]));
						}
					}
				}

				// <?=
				if (PhpbbVersion == 2 || PhpbbVersion == 3)
				{
					if (line.IndexOf("<?=") >= 0)
					{
						report.Append(string.Format("{0} Use of inline echo '<?=' on line: {1}\n[code]{2}[/code]\n\n",
                            fail, i + 1 + indexOffset, lines[i]));
					}
				}

				// $_POST
				if (PhpbbVersion == 2)
				{
					if (line.IndexOf("$_POST") >= 0)
					{
#if eal
						Console.WriteLine("PHP 4 variable $_POST on line: {0}", i + 1 + indexOffset);
						report.Append(string.Format("{0} Possible security concern. PHP 4 variable $_POST on line: {1}\n[code]{2}[/code]\n\n",
                            warning, i + 1 + indexOffset, lines[i]));
#elif meal
						report.Append(string.Format("{0} Use of PHP 4 variable $_POST on line: {1}\n[code]{2}[/code]\n\n", 
							fail, i + 1 + indexOffset, lines[i]));
#endif
                    }
				}
				else if (PhpbbVersion == 3)
				{
                    // ignore evaluations to bool
                    if (Regex.Replace(line, @"([ \t\(\.\[\{\+-\=\*\&\^\%\@\!\,\/\\\|\~\:\;\<\>]+)(isset|empty)(\s*)\((.*)\)", "").IndexOf("$_POST") >= 0)
					{
						if (line.IndexOf("isset(") < line.IndexOf("$_POST"))
						{
#if eal
							Console.WriteLine("PHP 4 variable $_POST on line: {0}", i + 1 + indexOffset);
							report.Append(string.Format("{0} Possible security concern. Use of PHP 4 variable $_POST on line: {1}\n[code]{2}[/code]\n\n",
                                notice, i + 1 + indexOffset, lines[i]));
#elif meal
							report.Append(string.Format("{0} Use of PHP 4 variable $_POST on line: {1}\n[code]{2}[/code]\n\n", 
							notice, i + 1 + indexOffset, lines[i]));
#endif
                        }
						else
						{
#if eal
							Console.WriteLine("PHP 4 variable $_POST on line: {0}", i + 1 + indexOffset);
							report.Append(string.Format("{0} Security concern. Unsecured use of PHP 4 variable $_POST on line: {1}\n[code]{2}[/code]\n\n",
                                fail, i + 1 + indexOffset, lines[i]));
#elif meal
							report.Append(string.Format("{0} Use of PHP 4 variable $_POST on line: {1}\n[code]{2}[/code]\n\n", 
							fail, i + 1 + indexOffset, lines[i]));
#endif
                        }
					}
				}

				// $_GET
				if (PhpbbVersion == 2)
				{
					if (line.IndexOf("$_GET") >= 0)
					{
#if eal
						Console.WriteLine("PHP 4 variable $_GET on line: {0}", i + 1 + indexOffset);
						report.Append(string.Format("{0} Possible security concern. PHP 4 variable $_GET on line: {1}\n[code]{2}[/code]\n\n",
                            warning, i + 1 + indexOffset, lines[i]));
#elif meal
						report.Append(string.Format("{0} Use of PHP 4 variable $_GET on line: {1}\n[code]{2}[/code]\n\n", 
							fail, i + 1 + indexOffset, lines[i]));
#endif
                    }
				}
				else if (PhpbbVersion == 3)
				{
                    // ignore evaluations to bool
                    if (Regex.Replace(line, @"([ \t\(\.\[\{\+-\=\*\&\^\%\@\!\,\/\\\|\~\:\;\<\>]+)(isset|empty|unset)(\s*)\((.*)\)", "").IndexOf("$_GET") >= 0)
					{
#if eal
						Console.WriteLine("PHP 4 variable $_GET on line: {0}", i + 1 + indexOffset);
                        report.Append(string.Format("{0} Security concern. PHP 4 variable $_GET on line: {1}\n[code]{2}[/code]\n\n",
                            fail, i + 1 + indexOffset, lines[i]));
#elif meal
						report.Append(string.Format("{0} Use of PHP 4 variable $_GET on line: {1}\n[code]{2}[/code]\n\n", 
							fail, i + 1 + indexOffset, lines[i]));
#endif
                    }
				}

				// $_SERVER
				if (PhpbbVersion == 2 || PhpbbVersion == 3)
				{
					if (line.IndexOf("$_SERVER") >= 0)
					{
#if eal
						Console.WriteLine("PHP 4 variable $_SERVER on line: {0}", i + 1 + indexOffset);
						report.Append(string.Format("{0} Possible security concern. PHP 4 variable $_SERVER on line: {1}\n[code]{2}[/code]\n\n",
                            warning, i + 1 + indexOffset, lines[i]));
#elif meal
						report.Append(string.Format("{0} Use of PHP 4 variable $_SERVER on line: {1}\n[code]{2}[/code]\n\n", 
							fail, i + 1 + indexOffset, lines[i]));
#endif
                    }
				}

				// $_COOKIE
				if (PhpbbVersion == 2)
				{
					if (line.IndexOf("$_COOKIE") >= 0)
					{
#if eal
						Console.WriteLine("PHP 4 variable $_COOKIE on line: {0}", i + 1 + indexOffset);
						report.Append(string.Format("{0} Possible security concern. PHP 4 variable $_COOKIE on line: {1}\n[code]{2}[/code]\n\n",
                            warning, i + 1 + indexOffset, lines[i]));
#elif meal
						report.Append(string.Format("{0} Use of PHP 4 variable $_COOKIE on line: {1}\n[code]{2}[/code]\n\n", 
							fail, i + 1 + indexOffset, lines[i]));
#endif
                    }
				}
				else if (PhpbbVersion == 3)
				{
					if (line.IndexOf("$_COOKIE") >= 0)
					{
#if eal
						Console.WriteLine("PHP 4 variable $_COOKIE on line: {0}", i + 1 + indexOffset);
						report.Append(string.Format("{0} Possible security concern. PHP 4 variable $_COOKIE on line: {1}\n[code]{2}[/code]\n\n",
                            notice, i + 1 + indexOffset, lines[i]));
#elif meal
						report.Append(string.Format("{0} Use of PHP 4 variable $_COOKIE on line: {1}\n[code]{2}[/code]\n\n", 
							fail, i + 1 + indexOffset, lines[i]));
#endif
                    }
				}

#if eal

				// $HTTP_POST_VARS
				if (PhpbbVersion == 3)
				{
					if (line.IndexOf("$HTTP_POST_VARS") >= 0)
					{
						Console.WriteLine("$HTTP_POST_VARS on line: {0}", i + 1 + indexOffset);
						report.Append(string.Format("{0} Security concern. $HTTP_POST_VARS on line: {1}, use request_var()\n[code]{2}[/code]\n\n",
                            fail, i + 1 + indexOffset, lines[i]));
					}
				}

				// $HTTP_GET_VARS
				if (PhpbbVersion == 3)
				{
					if (line.IndexOf("$HTTP_GET_VARS") >= 0)
					{
						Console.WriteLine("$HTTP_GET_VARS on line: {0}", i + 1 + indexOffset);
						report.Append(string.Format("{0} Security concern. $HTTP_GET_VARS on line: {1}, use request_var()\n[code]{2}[/code]\n\n",
                            fail, i + 1 + indexOffset, lines[i]));
					}
				}

				// in_array()
				if (PhpbbVersion == 2 || PhpbbVersion == 3)
				{
					int in_array_index = line.IndexOf("in_array");
					if (in_array_index >= 0)
					{
						if (Regex.IsMatch(line, "in_array(\\s*)\\("))
						{
							string temp = line.Substring(in_array_index + 10 - 1, line.Length - in_array_index - 10);
							temp = Regex.Replace(temp, @"\((.*?)\)", "");
							Console.WriteLine(temp);
							if (temp.Split(',').Length == 2)
							{
								Console.WriteLine("Possible security concern on line: {0}", i + 1 + indexOffset);
								report.Append(string.Format("{0} Possible security concern on line: {1}, in_array() used without third argument\n[code]{2}[/code]\n\n",
                                    warning, i + 1 + indexOffset, lines[i]));
							}
						}
					}
				}

				// PHP_SELF
				if (PhpbbVersion == 2 || PhpbbVersion == 3)
				{
					if (line.IndexOf("PHP_SELF") >= 0)
					{
						if (Regex.IsMatch(line, "$(HTTP|)_SERVER(_VARS)\\[('|\"|)PHP_SELF('|\"|)\\]"))
						{
							Console.WriteLine("Use of PHP_SELF on line: {0}", i + 1 + indexOffset);
							report.Append(string.Format("{0} PHP_SELF used on line: {1}\n[code]{2}[/code]\n\n",
                                warning, i + 1 + indexOffset, lines[i]));
						}
					}
				}

				// HTTP_POST_FILES
				if (PhpbbVersion == 2)
				{
					if (line.IndexOf("$HTTP_POST_FILES") >= 0)
					{

						Console.WriteLine("Use of $HTTP_POST_FILES on line: {0}", i + 1 + indexOffset);
						report.Append(string.Format("{0} $HTTP_POST_FILES used on line: {1}\n[code]{2}[/code]\n\n",
                            warning, i + 1 + indexOffset, lines[i]));
					}
					if (line.IndexOf("$_FILES") >= 0)
					{
						Console.WriteLine("PHP 4 variable $_FILES on line: {0}", i + 1 + indexOffset);
						report.Append(string.Format("{0} Possible security concern. PHP 4 variable $_FILES on line: {1}\n[code]{2}[/code]\n\n",
                            warning, i + 1 + indexOffset, lines[i]));
					}
				}
				else if (PhpbbVersion == 3)
				{
					if (line.IndexOf("$HTTP_POST_FILES") >= 0)
					{

						Console.WriteLine("Use of $HTTP_POST_FILES on line: {0}", i + 1 + indexOffset);
						report.Append(string.Format("{0} $HTTP_POST_FILES used on line: {1}\n[code]{2}[/code]\n\n",
                            fail, i + 1 + indexOffset, lines[i]));
					}
					if (line.IndexOf("$_FILES") >= 0)
					{
						Console.WriteLine("PHP 4 variable $_FILES on line: {0}", i + 1 + indexOffset);
						report.Append(string.Format("{0} Possible security concern. PHP 4 variable $_FILES on line: {1}\n[code]{2}[/code]\n\n",
                            warning, i + 1 + indexOffset, lines[i]));
					}
				}

#endif

				// IN_PHPBB
				if (PhpbbVersion == 2)
				{
						if (line.IndexOf("IN_PHPBB") >= 0)
						{
							if (Regex.IsMatch(line, @"define(\s*)\((\s*)\'IN_PHPBB\',(\s*)(1|true)(\s*)\);"))
							{
								IN_PHPBB = true;
								if (adminInclude)
								{
									if (line.Replace("$phpbb_root_path = './../';", "") == line)
									{
										phpbb_root_path = false;
									}
								}
								else
								{
									if (line.Replace("$phpbb_root_path = './';", "") == line)
									{
										phpbb_root_path = false;
									}
								}
							}
						}

						if (line.IndexOf("IN_PHPBB") >= 0)
						{
							if (Regex.IsMatch(line, @"if(\s*)\((\s*)\!(\s*)defined(\s*)\((\s*)\'IN_PHPBB\'(\s*)\)(\s*)\)"))
							{
								IN_PHPBB = true;
								// not a user facing file, so we don' t have to worry about it
								phpbb_root_path = false;
								if (Regex.IsMatch(line, @"if(\s*)\((\s*)!defined\('IN_PHPBB'\)(\s*)\)"))
								{
									// notice
									IN_PHPBB_NOTICE = true;
								}
							}
						}
					/*}
					else
					{
						if (line.IndexOf("define('IN_PHPBB', true);") >= 0)
						{
							IN_PHPBB = true;
							if (line.Replace("$phpbb_root_path = './';", "") == line)
							{
								phpbb_root_path = false;
							}
						}

						//if (line.IndexOf("if ( !defined('IN_PHPBB') )") >= 0)
						if (line.IndexOf("IN_PHPBB") >= 0)
						{
							if (Regex.IsMatch(line, @"if(\s*)\((\s*)\!(\s*)defined(\s*)\((\s*)\'IN_PHPBB\'(\s*)\)(\s*)\)"))
							{
								IN_PHPBB = true;
								// not a user facing file, so we don't have to worry about it
								phpbb_root_path = false;
							}
							else if (Regex.IsMatch(line, @"if(\s*)\((\s*)!defined\('IN_PHPBB'\)(\s*)\)"))
							{
								IN_PHPBB = true;
								phpbb_root_path = false;
								// notice
								IN_PHPBB_NOTICE = true;
							}
						}
					}*/
				}
				else if (PhpbbVersion == 3)
				{
					if (adminInclude)
					{
						// don't need to worry about thanks to the nifty module system
					}
					else
					{
						if (line.IndexOf("define('IN_PHPBB', true);") >= 0)
						{
							IN_PHPBB = true;
							if (line.Replace("$phpbb_root_path = './';", "") == line)
							{
								phpbb_root_path = false;
							}
						}

						if (line.IndexOf("if ( !defined('IN_PHPBB') )") >= 0)
						{
							IN_PHPBB = true;
							// not a user facing file, so we don't have to worry about it
							phpbb_root_path = false;
						}
						else if (Regex.IsMatch(line, @"if(\s*)\((\s*)!defined\('IN_PHPBB'\)(\s*)\)"))
						{
							IN_PHPBB = true;
							phpbb_root_path = false;
							// notice
							IN_PHPBB_NOTICE = true;
						}
					}
				}

#if eal

				// regex -e modifier
				if (PhpbbVersion == 2 || PhpbbVersion == 3)
				{
					int preg_Index = line.IndexOf("preg_replace");
					if (preg_Index >= 0)
					{
						string temp = Regex.Match(line, "preg_replace(\\s*)\\((\\s*)('|\")(.{1})").Value;
						if (temp.Length > 0)
						{
							char container = temp[temp.Length - 1];
							if(Regex.IsMatch(line, "preg_replace(\\s*)\\((\\s*)(\"|')(" + container.ToString() + ")(.*)(" + container.ToString() + ")([imsxADSUXu]*)(e)([imsxADSUXu]*)(\"|'),", RegexOptions.IgnoreCase))
							{
								report.Append(string.Format("{0} Use of e modifier in preg_replace on line: {1}.\n[code]{2}[/code]\n",
                                    warning, i + 1 + indexOffset, lines[i]));
							}
						}
					}
				}

				// extract
				if (PhpbbVersion == 2 || PhpbbVersion == 3)
				{
					if (line.IndexOf("extract") >= 0)
					{
						if (Regex.IsMatch(line, @"([ \t\(\.\[\{\+-\=\*\&\^\%\@\!\,\/\\\|\~\:\;\<\>]+)extract(\s*)\("))
						{
							report.Append(string.Format("{0} Use of extract() function on line: {1}.\n[code]{2}[/code]\n",
                                fail, i + 1 + indexOffset, lines[i]));
						}
					}
				}

				// eval
				if (PhpbbVersion == 2 || PhpbbVersion == 3)
				{
					if (line.IndexOf("eval") >= 0)
					{
						if (Regex.IsMatch(line, @"([ \t\(\.\[\{\+-\=\*\&\^\%\@\!\,\/\\\|\~\:\;\<\>]+)eval(\s*)\("))
						{
							report.Append(string.Format("{0} Use of eval() function on line: {1}.\n[code]{2}[/code]\n",
                                warning, i + 1 + indexOffset, lines[i]));
						}
					}
				}

				// exec
				if (PhpbbVersion == 2 || PhpbbVersion == 3)
				{
					if (line.IndexOf("exec") >= 0)
					{
						if (Regex.IsMatch(line, @"([ \t\(\.\[\{\+-\=\*\&\^\%\@\!\,\/\\\|\~\:\;\<\>]+)exec(\s*)\("))
						{
							report.Append(string.Format("{0} Use of exec() function on line: {1}.\n[code]{2}[/code]\n",
                                warning, i + 1 + indexOffset, lines[i]));
						}
					}
				}

				// getenv
				if (PhpbbVersion == 2 || PhpbbVersion == 3)
				{
					if (line.IndexOf("getenv") >= 0)
					{
						if (Regex.IsMatch(line, @"([ \t\(\.\[\{\+-\=\*\&\^\%\@\!\,\/\\\|\~\:\;\<\>]+)getenv(\s*)\("))
						{
							report.Append(string.Format("{0} Use of getenv() function on line: {1}.\n[code]{2}[/code]\n",
                                notice, i + 1 + indexOffset, lines[i]));
						}
					}
				}

#endif

                //
                // style
                //

                string[] controlKeywords = new string[] { "else if", "if", "else", "while", "switch", "for" };

                // where to put braces
                for (int j = 0; j < controlKeywords.Length; j++)
                {
                    int indexOfIt = line.IndexOf(controlKeywords[j]);
                    if (controlKeywords[j] == "if" && line.IndexOf("else if") > 0)
                    {
                        continue;
                    }
                    bool isIndexOfIt = Regex.IsMatch(line, string.Format(@"([ \t\(\.\[\{{\+-\=\*\&\^\%\@\!\,\/\\\|\~\:\;\<\>]+){0}(\s*)\(", controlKeywords[j]));
                    if (indexOfIt >= 0 && isIndexOfIt)
                    {
                        if (i + 1 < lines.Length)
                        {
                            bool foundBrace = false;
                            for (int k = i + 1; k < Math.Min(i + 7, lines.Length - 1); k++)
                            {

                                if (lines[k].TrimStart(new char[] { ' ', '\t' }).StartsWith("{"))
                                {
                                    foundBrace = true;
                                    braceOnLine = k;
                                    break;
                                }
                                else
                                {
                                    if (lines[k].Length > 0)
                                    {
                                        if (Regex.Match(lines[k], "^([\t]*)").Value.Length != indentLevel + 1)
                                        {
#if eal
                                            badIndentTabs = true;
#elif meal
                                            report.Append(string.Format("{0} Bad use of style, line should be indented {3} tabs on line: {1}.\n[code]{2}[/code]\n",
                                                style, i + 1 + indexOffset, lines[k], indentLevel));
#endif
                                        }
                                    }
                                }
                            }

                            if (!foundBrace)
                            {
#if eal
                                badBraces = true;
#elif meal
                                report.Append(string.Format("{0} Bad use of style, no brace on next line of {4} statement on line: {1}.\n[code]{2}\n{3}[/code]\n",
                                    style, i + 1 + indexOffset, line, lines[i + 1], controlKeywords[j]));
#endif
                                braceOnLine = 0;
                            }
                        }
                        else if (!fragment)
                        {
                            // not a fragment means style violation
#if eal
                            badBraces = true;
#elif meal
                            report.Append(string.Format("{0} Bad use of style, no brace on next line of {4} statement on line: {1}.\n[code]{2}\n{3}[/code]\n",
                                style, i + 1 + indexOffset, line, lines[i + 1], controlKeywords[j]));
#endif
                        }
                    }
                }

                if (braceOnLine == i)
                {
                    braceOnLine = 0;
                }

                if (!inMultiLineArray && !inSqlStatement && braceOnLine == 0)
                {
                    if (Regex.IsMatch(line, "^([\t ]*)}"))
                    {
                        indentLevel--;
                    }
                }

                // tabbing lines
                if (Regex.Match(line, "^([\t ]*)").Value != Regex.Match(line, "^([\t]*)").Value)
                {
#if eal
                    badIndentSpaces = true;
#elif meal
                    report.Append(string.Format("{0} Bad use of style, line tabbed using spaces instead of tabs on line: {1}.\n[code]{2}[/code]\n",
                                style, i + 1 + indexOffset, lines[i]));
#endif
                }

                if (!inMultiLineArray && !inSqlStatement && braceOnLine == 0)
                {
                    /*if (Regex.IsMatch(line, "^([\t ]*)}"))
                    {
                        indentLevel--;
                    }*/

                    if (Regex.IsMatch(line, "^([\t ]*)break;"))
                    {
                        indentLevel--;
                    }

                    if (Regex.Match(line, "^([\t]*)").Value.Length != indentLevel)
                    {
#if eal
                        badIndentTabs = true;
#elif meal
                        report.Append(string.Format("{0} Bad use of style, line should be indented {3} tabs on line: {1}.\n[code]{2}[/code]\n",
                            style, i + 1 + indexOffset, lines[i], indentLevel));
#endif
                    }

                    if (Regex.IsMatch(line, "^([\t ]*){"))
                    {
                        indentLevel++;
                    }

                    if (Regex.IsMatch(line, "^([\t ]*)(case |default:)"))
                    {
                        if (i + 1 < lines.Length)
                        {
                            if (Regex.IsMatch(lines[i + 1], "^([\t ]*)(case |default:)"))
                            {
                                indentLevel--;
                            }
                        }
                        if (i > 0)
                        {
                            if (Regex.IsMatch(lines[i - 1], "^([\t ]*)(case |default:)"))
                            {
                                indentLevel++;
                            } 
                            else if (Regex.IsMatch(lines[i - 1], "^([\t ]*)break;"))
                            {
                                indentLevel++;
                            }
                            else
                            {
                                if (i > 1)
                                {
                                    if (Regex.IsMatch(lines[i - 2], "^([\t ]*)break;"))
                                    {
                                        indentLevel++;
                                    }
                                    else if (i < 5)
                                    {
                                        bool allPrevLinesNull = true;
                                        for (int j = i - 1; j >= 0; j--)
                                        {
                                            if (Regex.Replace(lines[j], @"(\s*)\/\/.+$", "").Trim(new char[] { '\t', ' ' }) != "")
                                            {
                                                allPrevLinesNull = false;
                                            }
                                        }
                                        if (allPrevLinesNull)
                                        {
                                            indentLevel++;
                                        }
                                    }
                                }
                                else
                                {
                                    indentLevel++;
                                }
                            }
                        }
                        else
                        {
                            indentLevel++;
                        }
                    }
                }

                bool prevLineInSqlStatement = inSqlStatement;
                if (Regex.IsMatch(line, @"^([\t ]*)\$sql \="))
                {
                    inSqlStatement = true;
                }

                /*if (inSqlStatement)
                {
                    if (line.EndsWith(";"))
                    {
                        inSqlStatement = false;
                    }
                }*/
                if (inSqlStatement)
                {
                    if (line.EndsWith(";"))
                    {
                        inSqlStatement = false;
                    }
                    else
                    {
                        if (prevLineInSqlStatement)
                        {
                            if (Regex.Match(line, "^([\t]*)").Value.Length <= indentLevel)
                            {
#if eal
                                badIndentTabs = true;
#elif meal
                                report.Append(string.Format("{0} Bad use of style, line should be intented more than {3} tabs on line: {1}.\n[code]{2}[/code]\n",
                                    style, i + 1 + indexOffset, line, indentLevel));
#endif
                            }
                        }
                    }
                }

                bool prevLineInMultiLineArray = inMultiLineArray;
                if (line.EndsWith("array("))
                {
                    inMultiLineArray = true;
                }

                if (inMultiLineArray)
                {
                    if (line.EndsWith(";"))
                    {
                        inMultiLineArray = false;
                    }
                    else
                    {
                        if (prevLineInMultiLineArray)
                        {
                            if (Regex.Match(line, "^([\t]*)").Value.Length <= indentLevel)
                            {
#if eal
                                badIndentTabs = true;
#elif meal
                                report.Append(string.Format("{0} Bad use of style, line should be intented more than {3} tabs on line: {1}.\n[code]{2}[/code]\n",
                                    style, i + 1 + indexOffset, line, indentLevel));
#endif
                            }
                        }
                    }
                }

                // variable casing, camelCase, mIxEdCaSe, PascalCase

                if (line.IndexOf("=") >= 0)
                {
                    Match varMatch = Regex.Match(line, @"([ \t\(\.\[\{\+-\=\*\&\^\%\@\!\,\/\\\|\~\:\;\<\>]+)(\$|\$\$)([a-zA-Z0-9_]+)([ \t\(\.\[\{\+-\=\*\&\^\%\@\!\,\/\\\|\~\:\;\<\>]+)");
                    if (varMatch.Success)
                    {
                        string variable = varMatch.Groups[3].Value;

                        if (variable != variable.ToLower())
                        {
                            string[] varExceptions = { "_POST", "_GET", "GLOBALS", "SID" };
                            Array.Sort(varExceptions);
                            if (Array.IndexOf(varExceptions, variable) < 0)
                            {
#if eal
                                badVariableCasing = true;
#elif meal
                                report.Append(string.Format("{0} Bad use of style, variable names should not use mixed case on line: {1}.\n[quote]{2}[/quote]\n",
                                    style, i + 1 + indexOffset, line.Replace("$" + variable, string.Format("[b]${0}[/b]", variable))));
#endif
                            }
                        }
                    }
                }

                // spacing between tokens

			} // for

			// only enforce for .php files
			if (!fragment)
			{
				if (!(adminInclude && PhpbbVersion == 3))
				{
					if (!IN_PHPBB)
					{
						if (!IN_PHPBB_NOTICE)
						{
							report.Append(fail + " IN_PHPBB not set or checked.\n");
						}
						else
						{
							report.Append(notice + " IN_PHPBB spacing not matches phpBB source");
						}
					}

					if (phpbb_root_path)
					{
						report.Append(fail + " $phpbb_root_path not set for a user facing file.\n");
					}
				}
			}

			if (!forbiddenFlag && !fragment && report.Length > 0)
			{
				report.Insert(0, pass + " No forbidden functions found in file.\n");
			}

#if eal
            if (badBraces)
            {
                report.Append(string.Format("{0}, Bad use of braces found in this file\n",
                    style));
            }

            if (badIndentSpaces)
            {
                report.Append(string.Format("{0}, This file was indented with spaces and should be indented with tabs\n",
                    style));
            }

            if (badIndentTabs)
            {
                report.Append(string.Format("{0}, Bad use of indenting found in this file\n",
                    style));
            }

            if (badVariableCasing)
            {
                report.Append(string.Format("{0}, Bad use of variable naming found in this file, variables should be all lower case\n",
                    style));
            }
#endif

			return report.ToString();
		}

		private static void LoadPhpFunctions()
		{
			FunctionList = OpenTextFile(Path.Combine(domain.BaseDirectory, "phpBB2_PHP_functions.txt")).Replace("\r\n", "\n").Split('\n');
		}

		private static void LoadPhpFunctions3()
		{
			FunctionList = OpenTextFile(Path.Combine(domain.BaseDirectory, "phpBB3_PHP_functions.txt")).Replace("\r\n", "\n").Split('\n');
		}

		private static void LoadPhpbb2Languages()
		{
            if (Phpbb2LanguageList == null || Phpbb2LanguageList.Length == 0)
            {
                Phpbb2LanguageList = OpenTextFile(Path.Combine(domain.BaseDirectory, "languages_2.0.txt")).Replace("\r\n", "\n").Split('\n');
            }
		}

		private static void LoadPhpbb3Languages()
		{
            if (Phpbb3LanguageList == null || Phpbb3LanguageList.Length == 0)
            {
                Phpbb3LanguageList = OpenTextFile(Path.Combine(domain.BaseDirectory, "languages_3.0.txt")).Replace("\r\n", "\n").Split('\n');
            }
		}

        private static void LoadEnglishDictionary()
        {
            EnglishWordsList = OpenTextFile(Path.Combine(domain.BaseDirectory, "words.en.txt")).Replace("\r\n", "\n").ToLower().Split('\n');
            Array.Sort(EnglishWordsList);
        }

		private static string UnrollUri(string input)
		{
			return input.Replace("http://", "__http__").Replace("/", "__").Replace(".", "_");
        }

        #region PHP Usage Detection



        #endregion

        #region file handling

        /// <summary>
		/// Open a text file
		/// </summary>
		/// <param name="fileName"></param>
		/// <returns></returns>
		protected static string OpenTextFile(string fileName)
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

		/// <summary>
		/// Save a text file
		/// </summary>
		/// <param name="fileToSave"></param>
		/// <param name="fileName"></param>
		protected static void SaveTextFile(string fileToSave, string fileName)
		{
			StreamWriter myStreamWriter = File.CreateText(fileName);
			myStreamWriter.Write(fileToSave);
			myStreamWriter.Close();
		}

		#endregion
	}
}
