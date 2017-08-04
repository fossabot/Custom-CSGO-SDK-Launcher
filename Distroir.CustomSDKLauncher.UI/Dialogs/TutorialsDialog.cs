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
using Distroir.CustomSDKLauncher.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Distroir.CustomSDKLauncher.UI.Dialogs
{
    public partial class TutorialsDialog : Form
    {
        public TutorialsDialog()
        {
            //Create controls
            InitializeComponent();
            //Load tutorials
            LoadTutorials();
        }

        void LoadTutorials()
        {
            foreach (Tutorial t in TutorialManager.Tutorials)
            {
                //Create listviewitem
                ListViewItem i = new ListViewItem(t.Name);
                i.SubItems.Add(t.Game);
                i.Tag = t;

                listView1.Items.Add(i);
            }
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            //Close dialog
            Close();
        }

        private void listView1_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (e.IsSelected == true)
            {
                //Get tutorial
                Tutorial t = (Tutorial)e.Item.Tag;
                //Set text
                tutorialNameLabel.Text = t.Name;
                gameLabel.Text = t.Game;
                tutorialLinkLabel.Text = t.Url;
            }
        }

        private void tutorialLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (tutorialLinkLabel.Text != string.Empty)
            {
                Utils.ShellLaunch(tutorialLinkLabel.Text);
            }
        }
    }
}