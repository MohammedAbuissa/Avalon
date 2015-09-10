using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using System.Runtime.Serialization;
namespace Avalon
{
    [DataContract]
    public struct SerializableAvalon
    {
        [DataMember]
        public int Definition { get; set; }
        [DataMember]
        public bool isActive { get; set; }
        [DataMember]
        public Color fill { get; set; }
        [DataMember]
        public double TranslateX { get; set; }
        [DataMember]
        public double TranslateY { get; set; }
        [DataMember]
        public double Rotation { get; set; }
        [DataMember]
        public double Scale { get; set; }
    }
}
