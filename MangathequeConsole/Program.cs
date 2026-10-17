using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Reflection.Metadata;
using System.Runtime.ConstrainedExecution;
using System.Numerics;


MangathequeXml xml = new MangathequeXml("../MaMangatheque.xml");
MangathequeJson json = new MangathequeJson("../MaMangatheque.json");
System.Console.WriteLine(" _    _                         _   _      __\n|  \\/  |                       | | | |     \\_\\\n| \\  / | __ _ _ __   __ _  __ _| |_| |__   ___  __ _ _   _  ___\n| |\\/| |/ _` | '_ \\ / _` |/ _` | __| '_ \\ / _ \\/ _` | | | |/ _ \\\n| |  | | (_| | | | | (_| | (_| | |_| | | |  __/ (_| | |_| |  __/\n|_|  |_|\\__,_|_| |_|\\__, |\\__,_|\\__|_| |_|\\___|\\__, |\\__,_|\\___|\n                     __/ |                        | |\n                    |___/                         |_|");
System.Console.WriteLine("\n \nBienvenue! \n\nChoissisez une option:");

string? input = "";
while (input != "0")
{
    System.Console.WriteLine("\n\n1. Afficher la mangathèque \n2. Ajouter un manga dans une série existante \n3. Mettre à jour un manga \n4. Supprimer un manga \n\n0. Quitter");
    input = System.Console.ReadLine();
    switch (input)
    {
        case "1":
            Console.Clear();
            xml.ReadAll();
            break;
        case "2":
            Console.Clear();
            string? nomSerie = "";
            int? possede = 0;
            int? langue = 0;
            int? tome = 0;
            string? dateParution = "";
            System.Console.WriteLine("Entrez le nom de la série");
            nomSerie = System.Console.ReadLine();
            System.Console.WriteLine("1. Physique ?\n2. Digital ?");
            possede = int.Parse(System.Console.ReadLine);
            System.Console.WriteLine("1. Français ? \n 2. Anglais ?");
            langue = int.Parse(System.Console.ReadLine);
            System.Console.WriteLine("Quel est le numéro du tome ?");
            tome = int.Parse(System.Console.ReadLine);
            System.Console.WriteLine("Quel est sa date de parution ?\nFormat jj.mm.aaaa");
            dateParution = System.Console.ReadLine;
            xml.AddManga(nomSerie, possede, langue, tome, dateParution);
            json.AddManga();
            break;
        case "3":
            Console.Clear();
            xml.UpdateManga();
            json.UpdateManga();
            break;
        case "4":
            Console.Clear();
            xml.DeleteManga();
            json.DeleteManga();
            break;
        default :
            System.Console.WriteLine("Choix invalide.");
            break;
    }
}












// Charger les documents
XDocument mangathequeXml = XDocument.Load("../MaMangatheque.xml"); //xml
string mangathequeJsonFile = File.ReadAllText("../MaMangatheque.json"); //json

// Vérifier que le doc à une racine et afficher cette racine
XElement racineXml = mangathequeXml.Root
    ?? throw new InvalidDataException("Racine XML absente.");
Console.WriteLine($"Racine XML : {racineXml.Name}");

// Est-ce qu'on trouves bien le Json + création tableau d'auteurs qui sera utilisé plus tard
JsonNode mangathequeJson = JsonNode.Parse(mangathequeJsonFile)
    ?? throw new InvalidDataException("Racine Json nulle.");
JsonArray auteursJson = mangathequeJson["auteur"]?.AsArray()
    ?? throw new InvalidDataException("Tableau d'auteurs absent.");

//Ok on a contrôlé que tout est là



// Pour la démo on va faire le CRUD avec xml et json pour avoir toujours les deux fichiers égaux

    // Read en XML (sort l'auteur, le titre de la série et les numéros possédés)

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
    // Read en Json
foreach (JsonNode? auteurJson in auteursJson)
{
    string auteur = auteurJson?["nom"]?.GetValue<string>() ?? "(absent)";
    System.Console.WriteLine($"{auteur} JSON");
    JsonArray seriesJson = auteurJson["serie"]?.AsArray();
    foreach (JsonNode? serieJson in seriesJson)
    {
        string titre = serieJson?["titre"]?.GetValue<string>() ?? "(absent)";
        System.Console.WriteLine(($" - {titre}"));
        JsonArray tomesJson = serieJson["tome"]?.AsArray();
        foreach (JsonNode? tomeJson in tomesJson)
        {
            int numero = tomeJson["numero"].GetValue<int>();
            System.Console.WriteLine($"    - {numero}");
        }
    }
}

    // Create en XML - // Create en Json

foreach (XElement auteurXml in racineXml.Elements("auteur"))
{
    foreach (XElement serieXml in auteurXml.Element("series").Elements("serie"))
    {
        string titre = serieXml.Attribute("titre")?.Value ?? "(absent)";
        if (titre == "Bakuman")
        {
            serieXml.Add
            (
                    new XElement ("tome",
                    new XAttribute("possede", "Physique"),
                    new XAttribute("langue", "Francais"),
                    new XElement ("numero", 4),
                    new XElement ("dateParution", "05.11.2010"))
            );
        }
    }
}

foreach (JsonNode? auteurJson in auteursJson)
{
    JsonArray seriesJson = auteurJson["serie"]?.AsArray();
    foreach (JsonNode? serieJson in seriesJson)
    {
        string titre = serieJson?["titre"]?.GetValue<string>() ?? "(absent)";
        if (titre == "Bakuman")
        {
            JsonArray tomesJson = serieJson["tome"]?.AsArray();
            tomesJson.Add( new JsonObject
            {
                ["possede"] = "Physique",
                ["langue"] = "Francais",
                ["numero"] = 4,
                ["dateParution"] = "05.11.2010"
            });
        }
    }
}





    // Update en XML  - // Update en Json
    
foreach (XElement auteurXml in racineXml.Elements("auteur"))
{
    foreach (XElement serieXml in auteurXml.Element("series").Elements("serie"))
    {
        string titre = serieXml.Attribute("titre")?.Value ?? "(absent)";
        if (titre == "Bakuman")
        {
            foreach (XElement tomeXml in serieXml.Elements("tome"))
            {
                int numero = int.Parse(tomeXml.Element("numero").Value);
                if (numero == 4)
                {
                    tomeXml.SetAttributeValue("langue", "Allemand");
                }
            }
        }
    }
}


foreach (JsonNode? auteurJson in auteursJson)
{
    JsonArray seriesJson = auteurJson["serie"]?.AsArray();
    foreach (JsonNode? serieJson in seriesJson)
    {
        string titre = serieJson?["titre"]?.GetValue<string>() ?? "(absent)";
        if (titre == "Bakuman")
        {
            foreach (JsonNode? tomeJson in serieJson["tome"]?.AsArray())
            {
                int numero = tomeJson["numero"]?.GetValue<int>() ?? 0;
                if (numero == 4)
                {
                    tomeJson["langue"] = "Allemand";
                }
            }
        }
    }
}

    // Delete en XML - // Delete en Json
foreach (XElement auteurXml in racineXml.Elements("auteur"))
{
    foreach (XElement tomeXml in auteurXml.Element("series").Elements("serie").Elements("tome"))
    {
        int numero = int.Parse(tomeXml.Element("numero")?.Value);
        if (numero == 4)
        {
            tomeXml.Remove
            ();
            break;
        }
    }
}


foreach (JsonNode? auteurJson in auteursJson)
{
    JsonArray seriesJson = auteurJson["serie"]?.AsArray();
    foreach (JsonNode? serieJson in seriesJson)
    {
        string titre = serieJson?["titre"]?.GetValue<string>() ?? "(absent)";

        if (titre == "Bakuman")
        {
            JsonArray tomesJson = serieJson["tome"]?.AsArray();
            foreach (JsonNode? tomeJson in tomesJson)
            {
                int numero = tomeJson["numero"]?.GetValue<int>() ?? 0;
                if (numero == 4)
                {
                    tomesJson.Remove(tomeJson);
                    break;
                }
            }
        }
    }
}



//Petite histoire
//On regarde notre mangathèque
//On commande un livre manquant sur Facebook et on l'entre dans la base
//On reçoit le livre en on update la langue car on s'est trompé
//C'est de l'allemand et je n'ai pas cette option donc erreur
//On le renvoie au vendeur donc on le supprime des bases



*/