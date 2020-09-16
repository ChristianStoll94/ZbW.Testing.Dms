using System;
using System.IO;
using System.Reflection.Emit;

namespace ZbW.Testing.Dms.Client.Model
{
    public class MetadataItem
    {
        public string _bezeichnung { get; set; }

        public string _itemTyp { get; set; }

        public string _stichwoerter { get; set; }

        public string _benutzer { get; set; }

        public string _path { get; set; }

        public DateTime? _valutaDatum { get; set; }

        public DateTime _erfassungsdatum { get; set; }

        public MetadataItem(string bezeichnung, string itemTyp, string stichwoerter, string benutzer,
            DateTime? valutaDatum, DateTime erfassungsdatum, string path)
        {
            _bezeichnung = bezeichnung;
            _itemTyp = itemTyp;
            if (stichwoerter == null)
            {
                stichwoerter = "";

            }
            _stichwoerter = stichwoerter;
            _benutzer = benutzer;
            _valutaDatum = valutaDatum;
            _erfassungsdatum = erfassungsdatum;
            _path = path;
        }

        public MetadataItem() { }

    }


}