using System.Net.Http.Headers;


const string CONFIG = ".config";
string pantry_id = "";

HttpClient httpClient = InitzalizeHttpClient();
string baseURL = $"http://getpantry.cloud/apiv1/pantry/{pantry_id}/";
string basketURL = $"{baseURL}basket/";

if(File.Exists(CONFIG))
{
    string[] configDetails = File.ReadAllLines(CONFIG);
    pantry_id = configDetails[0];
}
else
{
    Console.WriteLine("input your Pantry ID");
    pantry_id = Console.ReadLine() ?? "";
    if(pantry_id.Length > 0 )
    {
        File.WriteAllText(CONFIG, pantry_id);
    }
}

HttpClient InitzalizeHttpClient()
{
    HttpClient httpClient = new HttpClient();
    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("applicastion/json"));
    return httpClient;
}