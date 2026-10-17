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

        return("");
    }

    public bool AddManga()
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