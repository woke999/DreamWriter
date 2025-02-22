﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static DreamWriter.EditorWndw;

namespace DreamWriter.Handler.Plugin
{
    public class PluginLoader
    {
        public static List<IPlugin> Plugins { get; set; }

        public void LoadPlugins()
        {
            Plugins = new List<IPlugin>();

            //load dlls from the plugin directory (Constants.cs)
            if (Directory.Exists(Constants.FolderName))
            {
                string[] files = Directory.GetFiles(Constants.FolderName);
                foreach (string file in files)
                {
                    if (file.EndsWith(".dll"))
                    {
                        Assembly.LoadFile(Path.GetFullPath(file));
                    }
                }
            }

            Type interfaceType = typeof(IPlugin);
            //feth all types that are the interface IPlugin and are a class
            Type[] types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(p => interfaceType.IsAssignableFrom(p) && p.IsClass)
                .ToArray();
            foreach (Type type in types)
            {
                //create a new instance of all found types
                Plugins.Add((IPlugin)Activator.CreateInstance(type));
            }
        }

        public static void StartPluginLoader()
        {
            PluginLoader loader = new PluginLoader();
            loader.LoadPlugins();
        }
    }
}
