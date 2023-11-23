//Lo primero veremos la forma de introducir un archivo sin ruta absoluta, dándole solo el nombre
//creamos un fichero y lo buscamos en nuestro proyecto (lo que yo hice)

using System.Collections.Immutable;
using System.Security.Cryptography;
using System.Text;

using (StreamWriter writer = new StreamWriter("Archivo1.txt"))
{
    writer.WriteLine("BuenosDías");
    writer.WriteLine("BuenosDías2");
    writer.WriteLine("BuenosDías3");
    writer.WriteLine("BuenosDías4");
    writer.WriteLine("BuenosDías5");
    writer.WriteLine("BuenosDías6");
    //el using cierra el archivo automáticamente (writer.close())
}

//con esto podemos meter todas las listas en una colección
List<string> allLinesText = File.ReadAllLines("Archivo1.txt").ToList();
//imprimos todo
foreach (var pass in allLinesText)
{
    Console.WriteLine(pass);
}

var random = new Random();
var itemRandom = random.Next(allLinesText.Count);
var password = allLinesText[itemRandom];
var encryptedPassword = Encrypt(password);
var result = BruteForce(Encrypt(password), allLinesText);
if (result != null)
{
    Console.WriteLine(result);
}
else
{
    Console.WriteLine("contraseña null");
}
int numberOfThreads = 4;
var step = allLinesText.Count / numberOfThreads;
for (int i = 0; i < numberOfThreads; i++)
{
    new Thread(() => BruteForce(Encrypt(password), allLinesText.GetRange(i * step, step)));
}

Console.WriteLine(password);
Console.WriteLine(Encrypt(password));

string BruteForce(string hashCode, List<string> passwordList)
{
    foreach (var privpassword in passwordList)
    {
        Console.WriteLine(privpassword);
        if (Encrypt(privpassword) == hashCode) return privpassword;
    }

    return null;
}

static string Encrypt(string originalString)
{
    var result = string.Empty;
    var hashValue = SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(originalString));
    foreach (byte b in hashValue)
    {
        result += $"{b:X2}";
    }

    return result;
}