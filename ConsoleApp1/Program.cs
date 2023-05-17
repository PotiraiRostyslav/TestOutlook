// See https://aka.ms/new-console-template for more information

Console.WriteLine("Hello, World!");

// configure httpclient
var httpClient = new HttpClient();
httpClient.BaseAddress = new Uri("http://localhost:5217");
// httpClient.DefaultRequestHeaders.Add("User-Agent", "Outlook-iOS-Android/2.0 (Microsoft Outlook 16.0.13901; Pro)");
var result = await httpClient.GetAsync("/email/getuseragent");
Console.WriteLine(result.StatusCode);
Console.WriteLine(result.Content.ReadAsStringAsync().Result);