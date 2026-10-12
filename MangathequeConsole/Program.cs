using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Reflection.Metadata;

// Charger les documents
XDocument mangathequeXml = XDocument.Load("../MaMangatheque.xml");
string mangathequeJsonFile = File.ReadAllText("../MaMangatheque.json");

// Vérifier que le doc à une racine et afficher cette racine
XElement racineXml = mangathequeXml.Root
    ?? throw new InvalidDataException("Racine XML absente.");
Console.WriteLine($"Racine XML : {racineXml.Name}");

// Est-ce qu'on trouves bien le Json (nombre d'auteurs)
JsonNode mangathequeJson = JsonNode.Parse(mangathequeJsonFile)
    ?? throw new InvalidDataException("Texte Json absent.");
Console.WriteLine($"Racine Json : {mangathequeJson["auteur"]?.AsArray().Count}");

//Ok on a contrôlé que tout est là



// Pour la démo on va faire le CRUD avec xml et json pour avoir toujours les deux fichiers égaux

    // Read en XML

    // Read en Json

    // Create en XML - // Create en Json

    // Update en XML - // Update en Json

    // Delete en XML - // Delete en Json


//Petite histoire
//On regarde notre mangathèque
//On commande un livre manquant sur Facebook et on l'entre dans la base
//On reçoit le livre en on update la langue car on s'est trompé
//C'est de l'allemand et je n'ai pas cette option donc erreur
//On le met en Anglais en attendant
//On le renvoie au vendeur donc on le supprime des bases






//Boucler dans le document pour afficher le contenu complet
foreach (XElement auteurXml in racineXml.Elements("auteur"))
{
    string auteur = auteurXml.Attribute("nom")?.Value ?? "(absent)";
    Console.WriteLine($"{auteur}");
}


