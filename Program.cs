﻿using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

using Content = System.Collections.Generic.Dictionary<string, string>;

const string CONFIG = ".config";
string pantry_id = "";
const string DEFAULT_BASKET = "save";



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

HttpClient httpClient = InitzalizeHttpClient();
string baseURL = $"http://getpantry.cloud/apiv1/pantry/{pantry_id}/";
string basketURL = $"{baseURL}basket/";

HttpResponseMessage response = await httpClient.GetAsync(baseURL);

if( response.StatusCode == System.Net.HttpStatusCode.OK)
{
    string pantryRawContent = (await response.Content.ReadAsStringAsync()) ?? "";
    
    Pantry? pantry = JsonSerializer.Deserialize<Pantry>(pantryRawContent);
    
    if(pantry != null)
    {
        BasketListing saveBasket = null;
        if(pantry?.baskets != null)
        {
            foreach(BasketListing basket in pantry.baskets)
            {
                if(basket.name == DEFAULT_BASKET)
                {
                    saveBasket = basket;
                }
            }
        
            if(saveBasket == null)
            {
                response = await httpClient.PostAsJsonAsync(basketURL, new Content() { });
                Console.WriteLine(await response.Content.ReadAsStringAsync());
            }
        }
        else
        {
            Console.WriteLine("Error, No conection");
        }
    
    
    }
}
else
{
    Console.WriteLine(response.StatusCode);
    Console.WriteLine(response.StatusCode.ReadAsStringAsync());
}

response = await httpClient.GetAsync(basketURL);
Console.WriteLine(response); 

HttpClient InitzalizeHttpClient()
{
    HttpClient httpClient = new HttpClient();
    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("applicastion/json"));
    return httpClient;
}

class Pantry
{
    public string? name {get; set;}
    
    public string? description {get; set;}
    
    public string? errors {get; set;}
    
    public bool? notificastion {get; set;}
    
    public double? precentfull {get; set;}
    
    public BasketListing[]? baskets {get; set;}
}

class BasketListing
{
    public string? name {get; set;}
    
    public int? ttl {get; set;}
}