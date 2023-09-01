using System.Diagnostics;
using System.Text;

class Program
{
    public static string Key = string.Empty;

    public const int min = 1;
    public const int max = 10000;
    static void Main(string[] args)
    {
        List<string> tested = new List<string>();
        var vowels = " ,A,E,I,O,U,Á,É,Í,Ó,Ú,À,È,Ì,Ò,Ù,Â,Ê,Î,Ô,Û,Ã,Õ,Ä,Ë,Ï,Ö,Ü,B,C,D,F,G,H,J,K,L,M,N,P,Q,R,S,T,V,W,X,Y,Z";
        var vowelsList = vowels.Split(",");
        var r = new Random();
        var stopwatch = new Stopwatch();

        Key = GenerateRandomKey(r, vowelsList.ToArray());
        stopwatch.Start();

        while (true)
        {
            vowelsList.AsParallel().ForAll(async vog =>
            {
                var randomNum = r.Next(min, max);
                var key = $"{(vog + "").ToLower()}{randomNum}";

                Console.WriteLine(key);
                if (await SendKey(tested, key, stopwatch))
                    Environment.Exit(0);
            });

            vowelsList.AsParallel().ForAll(async vog =>
            {
                var randomNum = r.Next(min, max);
                var key = $"{randomNum}{(vog + "").ToLower()}";

                Console.WriteLine(key);
                if (await SendKey(tested, key, stopwatch))
                    Environment.Exit(0);
            });

            vowelsList.AsParallel().ForAll(async vog =>
            {
                var randomNum = r.Next(min, max);
                var key = $"{(vog + "").ToUpper()}{randomNum}";

                Console.WriteLine(key);
                if (await SendKey(tested, key, stopwatch))
                    Environment.Exit(0);
            });

            vowelsList.AsParallel().ForAll(async vog =>
            {
                var randomNum = r.Next(min, max);
                var key = $"{randomNum}{(vog + "").ToUpper()}";

                Console.WriteLine(key);
                if (await SendKey(tested, key, stopwatch))
                    Environment.Exit(0);
            });
        }
    }

    public static async Task<bool> SendKey(List<string> tested, string key, Stopwatch stopwatch)
    {
        if (tested.Contains(key))
            return await Task.FromResult(false);

        var client = new HttpClient();
        var request = new HttpRequestMessage(HttpMethod.Post, "https://fiap-inaugural.azurewebsites.net/fiap");
        var content = new StringContent("{\r\n    \"key\": \"" + key + "\"\r\n}", null, "application/json");
        request.Content = content;
        var response = await client.SendAsync(request);
        response.EnsureSuccessStatusCode();
        Console.WriteLine(await response.Content.ReadAsStringAsync());

        tested.Add(key);
        if (response.StatusCode == System.Net.HttpStatusCode.OK)
        {
            Console.WriteLine("Chave: " + key);
            Console.WriteLine("Tentativas: " + tested.Count);
            Console.WriteLine("Tempo: " + (stopwatch.ElapsedMilliseconds / 1000) + " seg");
            return await Task.FromResult(true);
        }
        else
        {
            return await Task.FromResult(false);
        }
    }

    public static string GenerateRandomKey(Random r, string[] vowels)
    {
        var randomNum = r.Next(min, max);

        var selectedVowelB = vowels[r.Next(vowels.Length)];
        var selectedVowelA = vowels[r.Next(vowels.Length)];
        bool upperCaseB = r.Next(2) == 0;
        bool upperCaseA = r.Next(2) == 0;

        StringBuilder sb = new StringBuilder();
        sb.Append(upperCaseB ? selectedVowelB.ToUpper() : selectedVowelB.ToLower());
        sb.Append(randomNum.ToString());
        sb.Append(upperCaseA ? selectedVowelA.ToUpper() : selectedVowelA.ToLower());

        return sb.ToString().Trim();
    }
}
