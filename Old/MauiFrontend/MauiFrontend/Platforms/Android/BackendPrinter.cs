using Android.Runtime;
using Java.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiFrontend.Platforms.Android
{
    public class BackendPrinter
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public BackendPrinter(string id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
