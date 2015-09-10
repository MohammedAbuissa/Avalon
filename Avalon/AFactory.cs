namespace Avalon
{
    using System;
    using Windows.UI.Xaml.Media;
    using Windows.Foundation;
    using Windows.UI.Xaml.Input;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.IO;
    using Windows.Storage;
    using Windows.Storage.Streams;
    using System.Threading.Tasks;
    public class AFactory
    {
        Activation Attach, Detach;
        ManipulationDeltaEventHandler Router;
        public SolidColorBrush Fill { get; set; }
        List<GeometryFactory> Data;
        List<Avalon> FactoredItems = new List<Avalon>();
        Point DefaultLocation;
        public AFactory(List<GeometryFactory> Data, Activation Attach, Activation Detach, ManipulationDeltaEventHandler Router, Point DefaultLocation)
        {
            this.Attach = Attach;
            this.Detach = Detach;
            this.Router = Router;
            this.Data = Data;
            this.DefaultLocation = DefaultLocation;
            Fill = new SolidColorBrush();
        }
        public Avalon Produce(byte Type)
        {
            Avalon Pizza = new Avalon(Data[Type], Attach, Detach, Router, Type);
            Pizza.fill = Fill;
            (Pizza.RenderTransform as CompositeTransform).TranslateX = DefaultLocation.X;
            (Pizza.RenderTransform as CompositeTransform).TranslateY = DefaultLocation.Y;
            FactoredItems.Add(Pizza);
            return Pizza;
        }
        public async Task<List<Avalon>> Restore(StorageFile file)
        {
            List<Avalon> Return = new List<Avalon>();
            List<SerializableAvalon> temp = new List<SerializableAvalon>();
            using (IInputStream Stream = await file.OpenSequentialReadAsync())
            {
                DataContractSerializer dcs = new DataContractSerializer(typeof(List<SerializableAvalon>));
                temp = (List<SerializableAvalon>)dcs.ReadObject(Stream.AsStreamForRead());
            }
            foreach (SerializableAvalon item in temp)
            {
                Avalon Pizza = Produce(item.Definition);
                Pizza.RestorState(item);
                Return.Add(Pizza);
            }
            return Return;
        }
        public MemoryStream Save()
        {
            MemoryStream Return = new MemoryStream();
            List<SerializableAvalon> Serialize = new List<SerializableAvalon>();
            foreach (Avalon item in FactoredItems)
            {
                Serialize.Add(item.SaveState());
            }
            DataContractSerializer dcs = new
                        DataContractSerializer(typeof(List<SerializableAvalon>));
            dcs.WriteObject(Return, Serialize);
            return Return;
        }
    }
}
