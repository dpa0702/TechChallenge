using System.Collections;
using System.Diagnostics;

class Program
{
    public static string KeyGlobal = string.Empty;

    public static int min = 1;
    public static int max = 1000;

    public static bool Elocal = true;

    public static List<string> testados = new List<string>();

    static void Main(string[] args)
    {
        min = int.Parse(args[0]);
        max = int.Parse(args[1]);
        Elocal = bool.Parse(args[2]);

        var stopwatch = new Stopwatch();

        ArrayList numerosAleatorios = Program.numerosAleatorios(min, max + 1);

        Console.Write("Os números gerados foram:\n\n");
        for (int i = 0; i < numerosAleatorios.Count; i++)
        {
            Console.Write("{0}  ", numerosAleatorios[i]);
        }

        //" ,A,E,I,O,U,Á,É,Í,Ó,Ú,À,È,Ì,Ò,Ù,Â,Ê,Î,Ô,Û,Ã,Õ,Ä,Ë,Ï,Ö,Ü,B,C,D,F,G,H,J,K,L,M,N,P,Q,R,S,T,V,W,X,Y,Z";
        var vogais = " ,A,E,I,O,U,Á,É,Í,Ó,Ú,À,È,Ì,Ò,Ù,Â,Ê,Î,Ô,Û,Ã,Õ,Ä,Ë,Ï,Ö,Ü,B,C,D,F,G,H,J,K,L,M,N,P,Q,R,S,T,V,W,X,Y,Z";
        var listaVogais = vogais.Split(",");
        var ListaCombinacoes = new List<string>();

        Console.Write("Os números gerados foram:\n\n");
        for (int i = 0; i < numerosAleatorios.Count; i++)
        {
            for (int j = 0; j < listaVogais.Length; j++)
            {
                Console.Write("{0}  ", numerosAleatorios[i] + listaVogais[j]);
                ListaCombinacoes.Add(numerosAleatorios[i] + listaVogais[j]);

                Console.Write("{0}  ", listaVogais[j] + numerosAleatorios[i]);
                ListaCombinacoes.Add(listaVogais[j] + numerosAleatorios[i]);

                Console.Write("{0}  ", numerosAleatorios[i] + listaVogais[j].ToLower());
                ListaCombinacoes.Add(numerosAleatorios[i] + listaVogais[j].ToLower());

                Console.Write("{0}  ", listaVogais[j].ToLower() + numerosAleatorios[i]);
                ListaCombinacoes.Add(listaVogais[j].ToLower() + numerosAleatorios[i]);
            }
        }

        if (Elocal)
        {
            KeyGlobal = "5555G";// GenerateRandomKey(ListaCombinacoes.ToArray());
            Console.Write("Chave gerada para descoberta: " + KeyGlobal.ToString());
            StreamWriter sw = new StreamWriter("C:\\Temp\\Test-GenerateRandomKey" + min + "-" + max + ".txt");
            sw.WriteLine("Chave: " + KeyGlobal);
            sw.Close();
        }

        while (true)
        {
            if ((DateTime.UtcNow.Year.Equals(2023)
                && DateTime.Now.Month.Equals(9)
                && DateTime.Now.Day.Equals(4)
                && DateTime.Now.Hour.Equals(10)
                && DateTime.Now.Minute > 0)
                || Elocal
                )
            {
                stopwatch.Start();

                ListaCombinacoes.AsParallel().ForAll(async vog =>
                {
                    Console.WriteLine(vog);
                    if (await SendKey(vog, stopwatch))
                        Environment.Exit(0);
                });

                ListaCombinacoes.AsParallel().ForAll(async vog =>
                {
                    Console.WriteLine(vog);
                    if (await SendKey(vog, stopwatch))
                        Environment.Exit(0);
                });

                ListaCombinacoes.AsParallel().ForAll(async vog =>
                {
                    Console.WriteLine(vog);
                    if (await SendKey(vog, stopwatch))
                        Environment.Exit(0);
                });

                ListaCombinacoes.AsParallel().ForAll(async vog =>
                {
                    Console.WriteLine(vog);
                    if (await SendKey(vog, stopwatch))
                        Environment.Exit(0);
                });
            }
            else
            {
                Console.WriteLine(DateTime.UtcNow.ToString());
            }
        }
    }

    static ArrayList numerosAleatorios(int inicio, int fim)
    {
        Random random = new Random();

        ArrayList numeros = new ArrayList();
        for (int i = inicio; i < fim; i++)
        {
            numeros.Add(i);
        }

        for (int i = 0; i < numeros.Count; i++)
        {
            int a = random.Next(numeros.Count);
            object temp = numeros[i];
            numeros[i] = numeros[a];
            numeros[a] = temp;
        }

        return numeros.GetRange(0, 1000);
    }

    public static async Task<bool> SendKey(string key, Stopwatch stopwatch)
    {
        if (testados.Contains(key))
            return await Task.FromResult(false);

        if (Elocal)
        {
            testados.Add(key);
            if (KeyGlobal == key)
            {
                Console.WriteLine("Chave: " + key);
                Console.WriteLine("Tentativas: " + testados.Count);
                Console.WriteLine("Tempo: " + (stopwatch.ElapsedMilliseconds / 1000) + " seg");
                Console.WriteLine("Programa - " + min + " - " + max);


                StreamWriter sw = new StreamWriter("C:\\Temp\\Test-" + min + "-" + max + ".txt");
                sw.WriteLine("Chave: " + key);
                sw.WriteLine("Tentativas: " + testados.Count);
                sw.WriteLine("Tempo: " + (stopwatch.ElapsedMilliseconds / 1000) + " seg");
                sw.Close();

                return await Task.FromResult(true);
            }
            else
            {
                return await Task.FromResult(false);
            }
        }
        else
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, "https://fiap-inaugural.azurewebsites.net/fiap");
            var content = new StringContent("{\r\n    \"key\": \"" + key + "\"\r\n}", null, "application/json");
            request.Content = content;
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            Console.WriteLine(await response.Content.ReadAsStringAsync());

            testados.Add(key);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                Console.WriteLine("Chave: " + key);
                Console.WriteLine("Tentativas: " + testados.Count);
                Console.WriteLine("Tempo: " + (stopwatch.ElapsedMilliseconds / 1000) + " seg");
                return await Task.FromResult(true);
            }
            else
            {
                return await Task.FromResult(false);
            }
        }
    }

    public static string GenerateRandomKey(string[] vowels)
    {
        Random r = new Random();

        var randomNum = r.Next(min, max);

        return vowels[r.Next(vowels.Length)];
    }

}