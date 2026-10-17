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
            System.Console.WriteLine($"{auteur} XML");
            foreach (XElement serieXml in auteurXml.Element("series").Elements("serie"))
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

    public bool AddManga(string nomSerie, int possede, int langue, int tome, string dateParution)
    {
        
        return(true);
    }

    public bool UpdateManga()
    {
        return(true);
    }

    public bool DeleteManga()
    {
        return(true);
    }
}