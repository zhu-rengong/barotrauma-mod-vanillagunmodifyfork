using System.Xml.Linq;

namespace AutoGenerateXML
{
    public abstract class ItemXMLGenerator
    {
        public abstract string OutputPath { get; }

        public virtual string Generate()
        {
            throw new NotImplementedException($@"Method {nameof(ItemXMLGenerator)}::{nameof(Generate)}(){nameof(XElement)} is not implemented.");
        }
    }
}
