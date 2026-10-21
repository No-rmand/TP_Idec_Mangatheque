using System.Xml.Linq;

public class MangathequeXml
{
    private XDocument _documentXml;

    public MangathequeXml(string chemin)
    {
        _documentXml = XDocument.Load(chemin);
    }

    public string ReadAll()
    {
        XElement racineXml = _documentXml.Root
            ?? throw new InvalidDataException("Racine XML absente.");
        foreach (XElement auteurXml in racineXml.Elements("auteur"))
        {
            string auteur = auteurXml.Attribute("nom")?.Value ?? "(absent)";
            System.Console.WriteLine($"{auteur}");
            XElement seriesXml = auteurXml.Element("series")
                ?? throw new InvalidDataException("Element series absent.");
            foreach (XElement serieXml in seriesXml.Elements("serie"))
            {
                string titre = serieXml.Attribute("titre")?.Value ?? "(absent)";
                System.Console.WriteLine($"  -> {titre}");
                foreach (XElement tomeXml in serieXml.Elements("tome"))
                {
                string numero = tomeXml.Element("numero")?.Value ?? "(absent)";
                    System.Console.WriteLine($"     - {numero}");
                }
            }
        } 
        return("");
    }

    public bool AddManga(string nomSerie, string possede, string langue, int tome, string dateParution)
    {
        bool found = false;
        XElement racineXml = _documentXml.Root
            ?? throw new InvalidDataException("Racine XML absente.");
        foreach (XElement auteurXml in racineXml.Elements("auteur"))
        {
            XElement seriesXml = auteurXml.Element("series")
                ?? throw new InvalidDataException("Element series absent.");
            foreach (XElement serieXml in seriesXml.Elements("serie"))
            {
                string titre = serieXml.Attribute("titre")?.Value ?? "(absent)";
                if (titre == nomSerie)
                {
                    serieXml.Add
                    (
                            new XElement ("tome",
                            new XAttribute("possede", possede),
                            new XAttribute("langue", langue),
                            new XElement ("numero", tome),
                            new XElement ("dateParution", dateParution))
                            
                    );
                    found = true;
                    SaveXml();
                }
            }
        }
        return(found);
    }

    public bool UpdateManga(string nomSerie, int tome, string langue)
    {
        bool found = false;
        XElement racineXml = _documentXml.Root
            ?? throw new InvalidDataException("Racine XML absente.");
        foreach (XElement auteurXml in racineXml.Elements("auteur"))
        {
            XElement seriesXml = auteurXml.Element("series")
                ?? throw new InvalidDataException("Element series absent.");
            foreach (XElement serieXml in seriesXml.Elements("serie"))
            {
                string titre = serieXml.Attribute("titre")?.Value ?? "(absent)";
                if (titre == nomSerie)
                {
                    foreach (XElement tomeXml in serieXml.Elements("tome"))
                    {
                        int numero = int.Parse(tomeXml.Element("numero")?.Value
                            ?? throw new InvalidDataException("Element numero absent."));
                        if (numero == tome)
                        {
                            tomeXml.SetAttributeValue("langue", langue);
                            found = true;
                            SaveXml();
                        }
                    }
                }
            }
        }
        return(found);
    }

    public bool DeleteManga(string nomSerie, int tome)
    {
        bool found = false;
        XElement racineXml = _documentXml.Root
            ?? throw new InvalidDataException("Racine XML absente.");
        foreach (XElement auteurXml in racineXml.Elements("auteur"))
        {
            XElement seriesXml = auteurXml.Element("series")
                ?? throw new InvalidDataException("Element series absent.");
            foreach (XElement serieXml in seriesXml.Elements("serie"))
            {
                string titre = serieXml.Attribute("titre")?.Value ?? "(absent)";
                if (titre == nomSerie)
                {
                    foreach (XElement tomeXml in serieXml.Elements("tome"))
                    {
                        int numero = int.Parse(tomeXml.Element("numero")?.Value
                            ?? throw new InvalidDataException("Element numero absent."));
                        if (numero == tome)
                        {
                            tomeXml.Remove();
                            found = true;
                            SaveXml();
                        }
                    }
                }
            }
        }
        return(found);
    }

    public void SaveXml()
    {
        _documentXml.Save("MaMangatheque_modified.xml");
    }
}