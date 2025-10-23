using System.Xml.Linq;

namespace AutoGenerateXML
{
    public abstract class ObjectXMLGenerator
    {
        public static List<ObjectXMLGenerator> List = new();

        public string OutputPath = string.Empty;
        public abstract string Class { get; }

        public virtual string Generate()
        {
            throw new NotImplementedException($@"Method {nameof(ObjectXMLGenerator)}::{nameof(Generate)}(){nameof(String)} is not implemented.");
        }
    }

    public abstract class ItemXMLGenerator : ObjectXMLGenerator
    {
        public override string Class => "Item";
        public string Name = string.Empty;
        public string Identifier = string.Empty;
        public List<string> SelfTags = new();
        public string Category = string.Empty;
        public string SubCategory = UserDefinedGlobal.ModName;

        public float Scale = 0.5f;

        public override string? ToString()
        {
            return $@"{Class} (id: {Identifier})";
        }
    }
}
