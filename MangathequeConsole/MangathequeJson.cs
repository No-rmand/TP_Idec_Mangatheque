using System.Text.Json;
using System.Text.Json.Nodes;

public class MangathequeJson
{
    private JsonNode _documentJson;

    public MangathequeJson(string chemin)
    {
        string texteJson = File.ReadAllText(chemin);
        _documentJson = JsonNode.Parse(texteJson)
            ?? throw new InvalidDataException("Racine Json nulle.");
    }

    public string ReadAll()
    {
        //pas nécessaire car déjà bien traîtée en xml
        return("");
    }

    public bool AddManga(string nomSerie, string possede, string langue, int tome, string dateParution)
    {
        bool found = false;
        JsonArray auteursJson = _documentJson["auteur"]?.AsArray()
            ?? throw new InvalidDataException("Tableau d'auteurs absent.");
        foreach (JsonNode? auteurJson in auteursJson)
        {
            JsonArray seriesJson = auteurJson["serie"]?.AsArray();
            foreach (JsonNode? serieJson in seriesJson)
            {
                string titre = serieJson?["titre"]?.GetValue<string>() ?? "(absent)";
                if (titre == nomSerie)
                {
                    JsonArray tomesJson = serieJson["tome"]?.AsArray();
                    tomesJson.Add( new JsonObject
                    {
                        ["possede"] = possede,
                        ["langue"] = langue,
                        ["numero"] = tome,
                        ["dateParution"] = dateParution
                    });
                    found = true;
                    SaveJson();
                }
            }
        }
        return(found);
    }

    public bool UpdateManga(string nomSerie, int tome, string langue)
    {
        bool found = false;
        JsonArray auteursJson = _documentJson["auteur"]?.AsArray()
            ?? throw new InvalidDataException("Tableau d'auteurs absent.");
        foreach (JsonNode? auteurJson in auteursJson)
        {
            JsonArray seriesJson = auteurJson["serie"]?.AsArray();
            foreach (JsonNode? serieJson in seriesJson)
            {
                string titre = serieJson?["titre"]?.GetValue<string>() ?? "(absent)";
                if (titre == nomSerie)
                {
                    foreach (JsonNode? tomeJson in serieJson["tome"]?.AsArray())
                    {
                        int numero = tomeJson["numero"]?.GetValue<int>() ?? 0;
                        if (numero == tome)
                        {
                            tomeJson["langue"] = langue;
                            found = true;
                            SaveJson();
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
        JsonArray auteursJson = _documentJson["auteur"]?.AsArray()
            ?? throw new InvalidDataException("Tableau d'auteurs absent.");
        foreach (JsonNode? auteurJson in auteursJson)
        {
            JsonArray seriesJson = auteurJson["serie"]?.AsArray();
            foreach (JsonNode? serieJson in seriesJson)
            {
                string titre = serieJson?["titre"]?.GetValue<string>() ?? "(absent)";

                if (titre == nomSerie)
                {
                    JsonArray tomesJson = serieJson["tome"]?.AsArray();
                    foreach (JsonNode? tomeJson in tomesJson)
                    {
                        int numero = tomeJson["numero"]?.GetValue<int>() ?? 0;
                        if (numero == tome)
                        {
                            tomesJson.Remove(tomeJson);
                            found = true;
                            SaveJson();
                            break;
                        }
                    }
                }
            }
        }
        return(found);
    }

    public void SaveJson()
    {
        var options = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        string resultJson = _documentJson.ToJsonString(options);
        File.WriteAllText("MaMangatheque_modified.json", resultJson);
    }
}