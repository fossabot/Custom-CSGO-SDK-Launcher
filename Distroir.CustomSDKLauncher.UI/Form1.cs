﻿/*
Custom SDK Launcher
Copyright (C) 2017 Distroir

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/
using Distroir.Configuration;
using Distroir.CustomSDKLauncher.Core;
using System;
using System.Drawing;
using System.Resources;
using System.Windows.Forms;

namespace Distroir.CustomSDKLauncher.UI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            //Load configuration
            Utils.CheckDirs();
            Config.Load();
            LanguageManager.LoadLanguageInfo();

            //Load profiles, tutorials and templates
            ProfileManager.LoadProfiles();
            TutorialManager.LoadTutorials();
            TemplateManager.LoadTemplates();

            //Unused: Load theme
            //Reason: Themes on winforms does not look good
            //UIThemeManager.LoadThemes();
            //UIThemeManager.LoadCurrentTheme();

            //Check if it's first launch
            CheckIfItsFirstLaunch();

            //Migrate csgo directory from first version of Custom SDK Launcher
            //It happens only, when you are launching newer version for the first time
            //And you had przeviously used version 1
            SetCsgoDirectoryFromConfig();

            //This is not first launch anymore
            Config.AddVariable("FirstLaunch", 0);

            //Create controls
            InitializeComponent();

            //Apply translations to controls
            UpdateToolsGroupBoxText();

            //Unused: Apply theme to UI
            //ApplyTheme();
        }

        #region Form events

        #region Button click events

        private void launchHammerButton_Click(object sender, EventArgs e)
        {
            //Utils.Launch("hammer.exe", "-nop4");
            Utils.TryLaunchTool(SDKApplication.Hammer);
        }

        private void launchModelViewerButton_Click(object sender, EventArgs e)
        {
            //Utils.Launch("hlmv.exe");
            Utils.TryLaunchTool(SDKApplication.HLMV);
        }

        private void launchFacePoserButton_Click(object sender, EventArgs e)
        {
            //Utils.Launch("hlfaceposer.exe", "-nop4");
            Utils.TryLaunchTool(SDKApplication.FacePoser);
        }

        private void fmponeButton_Click(object sender, EventArgs e)
        {
            Utils.ShellLaunch("https://www.youtube.com/channel/UCrVkmwv-AHBAo-92OeSh9YQ/videos");
        }

        private void topHattWaffleButton_Click(object sender, EventArgs e)
        {
            Utils.ShellLaunch("https://www.youtube.com/playlist?list=PL-454Fe3dQH0WCzAsmydsr24NFaFrNC_h");
        }

        private void csgoSdkButton_Click(object sender, EventArgs e)
        {
            Utils.ShellLaunch("https://developer.valvesoftware.com/wiki/Counter-Strike:_Global_Offensive_Level_Creation");
        }

        private void kliksButton_Click(object sender, EventArgs e)
        {
            Utils.ShellLaunch("https://www.youtube.com/playlist?list=PLfwtcDG7LpxF7-uH_P9La76dgCMC_lfk3");
        }

        private void settingsButton_Click(object sender, EventArgs e)
        {
            var d = new Dialogs.SettingsDialog();
            d.ShowDialog();

            //Update toolsGroupBoxText
            UpdateToolsGroupBoxText();
        }

        private void moreTutorialsLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var v = new Dialogs.TutorialsDialog();
            v.ShowDialog();
        }

        #endregion

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //Save config
            Config.Save();
            //Save list of profiles
            ProfileManager.SaveProfiles();
        }

        #endregion

        #region Methods

        void CheckIfItsFirstLaunch()
        {
            if (Config.TryReadInt("FirstLaunch") == 1)
            {
                //Create dialog
                var v = new Dialogs.FirstLaunchDialog();

                //Show dialog
                if (!(v.ShowDialog() == DialogResult.OK))
                {
                    //If user closes dialog without selecting csgo directory
                    //Inform user that he needs to select his csgo directory
                    MessageBox.Show("Can not continue. You need to select your csgo directory", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    //Cannot continue
                    //Close application
                    Environment.Exit(0);
                }
            }
        }

        void SetCsgoDirectoryFromConfig()
        {
            //Set gamedir
            if (!string.IsNullOrEmpty(Config.TryReadString("CSGO_DIR")))
            {
                //Create profile
                Profile p = new Profile();
                p.ProfileName = "Counter-Strike: Global Offensive";
                p.GameDir = Config.TryReadString("CSGO_DIR");
                p.GameinfoDirName = "csgo";

                //Add profile to list
                ProfileManager.Profiles.Add(p);

                //Select profile
                Config.AddVariable("SelectedProfileId", 0);

                //Remove variable
                Config.RemoveVariable("CSGO_DIR");
            }
        }

        /// <summary>
        /// Applies translations to 
        /// </summary>
        void ApplyTranslations()
        {
            //Set language
            ResourceManager rm = new ResourceManager(LanguageResourcesList.Form1Res, typeof(Form1).Assembly);
            
            //GroupBoxes
            UpdateToolsGroupBoxText();
            tutorialsGroupBox.Text = rm.GetString("tutorialsGroupBox_text", LanguageManager.Culture);
            //Form1 controls
            settingsButton.Text = rm.GetString("settingsButton_text", LanguageManager.Culture);
            //toolsGroupBox controls
            launchHammerButton.Text = rm.GetString("launchHammerButton_text", LanguageManager.Culture);
            launchModelViewerButton.Text = rm.GetString("launchModelViewerButton_text", LanguageManager.Culture);
            launchFacePoserButton.Text = rm.GetString("launchFacePoserButton_text", LanguageManager.Culture);
            //tutorialsGroupBox controls
            fmponeButton.Text = rm.GetString("fmponeButton_text", LanguageManager.Culture);
            topHattWaffleButton.Text = rm.GetString("topHattWaffleButton_text", LanguageManager.Culture);
            csgoSdkButton.Text = rm.GetString("csgoSdkButton_text", LanguageManager.Culture);
            moreTutorialsLabel.Text = rm.GetString("moreTutorialsLabel_text", LanguageManager.Culture);
            moreTutorialsLabel.Location = new Point(Convert.ToInt32(rm.GetString("moreTutorialsLabel_X", LanguageManager.Culture)), moreTutorialsLabel.Location.Y);
        }

        #region Utilies

        /// <summary>
        /// Gets current profile name
        /// </summary>
        /// <returns>Current profile name</returns>
        string GetCurrentProfileName()
        {
            //Get selected profile
            Profile p;
            Utils.TryGetSelectedProfile(out p);

            //Get and return profile name
            return p.ProfileName;
        }

        ///// <summary>
        ///// Changes text inside toolsGroupBox control
        ///// </summary>
        //void UpdateToolsGroupBoxText()
        //{
        //    ResourceManager rm = new ResourceManager(LanguageResourcesList.Form1Res, typeof(Form1).Assembly);
        //    UpdateToolsGroupBoxText(rm);
        //}

        /// <summary>
        /// Changes text inside toolsGroupBox control
        /// </summary>
        /// <param name="rm">Resource manager</param>
        [Obsolete]
        void UpdateToolsGroupBoxText(ResourceManager rm)
        {
            if (Config.TryReadInt("DisplayCurrentProfileName") == 1 && !string.IsNullOrEmpty(GetCurrentProfileName()))
            {
                //Set text
                string text = string.Format("{0} - {1}", rm.GetString("toolsGroupBox_text", LanguageManager.Culture), GetCurrentProfileName());
                text = CutStringIfTooBig(text, 39);
                toolsGroupBox.Text = text;
            }
            else
            {
                //Set text
                toolsGroupBox.Text = rm.GetString("toolsGroupBox_text", LanguageManager.Culture);
            }
        }

        /// <summary>
        /// Changes text inside toolsGroupBox control
        /// </summary>
        /// <param name="rm">Resource manager</param>
        void UpdateToolsGroupBoxText()
        {
            if (Config.TryReadInt("DisplayCurrentProfileName") == 1 && !string.IsNullOrEmpty(GetCurrentProfileName()))
            {
                //Set text
                string text = string.Format("Tools - {0}", GetCurrentProfileName());
                text = CutStringIfTooBig(text, 40);
                toolsGroupBox.Text = text;
            }
            else
            {
                //Set text
                toolsGroupBox.Text = "Tools";
            }
        }

        /// <summary>
        /// Cuts string if it's too big
        /// </summary>
        /// <param name="s">Input string</param>
        /// <param name="length">Maximal length of string</param>
        /// <returns></returns>
        string CutStringIfTooBig(string s, int length)
        {
            if (s.Length > length)
            {
                //Shorten string
                char[] buffer = new char[length];
                s.CopyTo(0, buffer, 0, length);
                //Get string from buffer
                s = charArrayToString(buffer) + "...";
                return s;
            }

            //String is the same, return it
            return s;
        }

        /// <summary>
        /// Converts char array to string
        /// </summary>
        /// <param name="array">Char array</param>
        /// <returns></returns>
        string charArrayToString(char[] array)
        {
            string returnvalue = string.Empty;

            foreach (char c in array)
                returnvalue += c;

            return returnvalue;
        }

        #endregion

        #endregion

        #region Unused

        /// <summary>
        /// Applies current theme to control
        /// </summary>
        void ApplyTheme()
        {
            //Get theme
            UITheme t = UIThemeManager.CurrentTheme;

            //Set colors
            BackColor = t.BackgroundColor;
            ForeColor = t.TextColor;

            toolsGroupBox.ForeColor = t.TextColor;
            tutorialsGroupBox.ForeColor = t.TextColor;

            foreach (Control c in Controls)
            {
                try
                {
                    Button b = (Button)c;
                    b.BackColor = t.ButtonColor;
                }
                catch { }
            }

            foreach (Control c in toolsGroupBox.Controls)
            {
                try
                {
                    Button b = (Button)c;
                    b.BackColor = t.ButtonColor;
                }
                catch { }
            }

            foreach (Control c in tutorialsGroupBox.Controls)
            {
                try
                {
                    Button b = (Button)c;
                    b.BackColor = t.ButtonColor;
                }
                catch { }
            }

            moreTutorialsLabel.ForeColor = t.LinkColor;
            moreTutorialsLabel.LinkColor = t.LinkColor;
            moreTutorialsLabel.VisitedLinkColor = t.LinkClickedColor;
        }

        #endregion
    }
}