using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using CockpitBuilder.Core.Contracts;

namespace CockpitBuilder.Core.Model
{

    [DataContract]
    public class PluginProperty 
    {
        public PluginProperty(string[] name)
        {
            Name = name;
        }




        //public void SetSize(string file)
        //{

        //    Size = new[] { img.Width, img.Height };
        //}

        [DataMember]
        public string[] Name { get; set; }
        [DataMember]
        public int[] Size { get; set; }
    }
}