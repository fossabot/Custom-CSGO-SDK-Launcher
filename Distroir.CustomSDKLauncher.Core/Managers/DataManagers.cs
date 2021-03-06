﻿/*
Custom SDK Launcher
Copyright (C) 2017-2019 Distroir

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
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Distroir.CustomSDKLauncher.Core.AppLauncher;
using Distroir.CustomSDKLauncher.Core.CommunityContent;
using Distroir.CustomSDKLauncher.Core.Managers.Serializers;

namespace Distroir.CustomSDKLauncher.Core.Managers
{
    public class DataManagers
    {
        /// <summary>
        /// Full path of file containing list of profiles
        /// </summary>
        public static string ProfileListFilename = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Distroir", "Custom SDK Launcher", "profiles.xml");
        /// <summary>
        /// Full path of file containing list of applications
        /// </summary>
        public static string AppListFilename = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Distroir", "Custom SDK Launcher", "applications.xml");

        /// <summary>
        /// Profile manager
        /// </summary>
        public static Manager<Profile> ProfileManager = new Manager<Profile>(new XmlFileSerializer<Profile>(ProfileListFilename));

        /// <summary>
        /// Template manager
        /// </summary>
        public static Manager<Template> TemplateManager = new Manager<Template>(new XmlStringSerializer<Template>(Data.TemplatesXML));

        /// <summary>
        /// Tutorial serializer
        /// </summary>
        public static Manager<Tutorial> TutorialManager = new Manager<Tutorial>(new XmlStringSerializer<Tutorial>(Data.TutorialsXML));


        /// <summary>
        /// App serializer
        /// </summary>
        public static Manager<AppInfo> AppManager = new Manager<AppInfo>(new XmlFileSerializer<AppInfo>(AppListFilename));

        /*
        /// <summary>
        /// Content serializer
        /// </summary>
        public static Manager<ContentGroup> ContentManager = new Manager<ContentGroup>(new XmlFileSerializer<ContentGroup>(@"C:\users\x\desktop\contents.xml"));
        */
    }
}
