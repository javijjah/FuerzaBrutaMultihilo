﻿using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

namespace fuerzaBrutaMultihilo;

public static class TareaFuerzaBruta
{
    private static bool _passEncontrada = false;
    public static void Main()
    {
        //Zona de acceso y lectura del fichero
        List<String> passList = new List<string>();
        string path = "2151220-passwords.txt";
        //zona de invocación de los métodos que realizan las tareas
        RellenarLista(passList, path);
        //Zona de la contraseña de la que tenemos el hash, convirtiéndola y obteniendo su valor
        byte[] passBase = GetRandomPassword(passList);
        var singleClock = Stopwatch.StartNew();
        CrackearPassword(passList, passBase);
        singleClock.Stop();
        Console.WriteLine("Tiempo con 1 hilo:" + singleClock.ElapsedMilliseconds);
        //var multiClock = Stopwatch.StartNew();
        CrackearPasswordMultihilo(passList, passBase);
        //Podemos crear el reloj si queremos, pero al ejecutarse la aplicación de forma asíncrona, no funcionará bien.
        //multiClock.Stop();
        //Console.WriteLine("Tiempo con 4 hilos:" + multiClock.ElapsedMilliseconds);
    }

    //Esto devuelve el hash en bytes
    public static byte[] GetHash(string password)
    {
        using (HashAlgorithm algoritmo = SHA256.Create())
            return algoritmo.ComputeHash(Encoding.UTF8.GetBytes(password));
    }
    //Este método devolverá una contraseña aleatoria
    public static byte[] GetRandomPassword(List<String> passList)
    {
        return GetHash(passList[(new Random()).Next(passList.Count())]);
    }
    //Esto convierte el hash en un String hexadecimal que podemos leer
    public static string DevolverHash(byte[] passHash)
    {
        string resultado = "";
        foreach (var b in passHash)
        {
            resultado += $"{b:X2}";
        }

        return resultado;
    }

    //Esto revisa cada string de la lista de contraseñas pasado por hash para buscar la coincidencia
    public static void CrackearPassword(List<String> passList, byte[] passString)
    {
        foreach (var passtemp in passList)
        {
            if (_passEncontrada)
            {
                break;
            }
            if (GetHash(passtemp).SequenceEqual(passString))
            {
                Console.WriteLine("Contraseña encontrada: " + passtemp);
                _passEncontrada = true;
                break;
            }
        }
    }

    public static void CrackearPasswordMultihilo(List<String> passList,byte[] pass)
    {
        _passEncontrada = false;
        int numHilos = 4;
        var pos = passList.Count / numHilos;
        for (int i = 0; i < numHilos; i++)
        {
            var i1 = i;
            new Thread(() => CrackearPassword(passList.GetRange(i1 * pos, pos),pass)).Start(); //
        }
    }
    public static void RellenarLista(List<String> passList, string pathFichero)
    {
        using (StreamReader sr =
               File.OpenText(pathFichero))
        {
            string line;
            while ((line = sr.ReadLine()!) != null)
            {
                passList.Add(line);
            }
        }
    }
    /*
     Mi código utilizado antes de corregirlo
     
        //GestionarMultihilo(passList, passString); el método utilizado antes de corregir mi código
         public static void CrackearPasswordMultihilo1(List<String> passList, byte[] passString)
    {
        for (var i = 0; i < passList.Count; i++)
        {
            if (GetHash(passList[i]).Equals(passString))
            {
                Console.WriteLine("Contraseña encontrada en hilo 1: " + passList[i]);
            }
        }
    }

    public static void CrackearPasswordMultihilo2(List<String> passList, byte[] passString)
    {
        for (var i = (passList.Count * 1) / 4; i < passList.Count; i++)
        {
            if (GetHash(passList[i]).Equals(passString))
            {
                Console.WriteLine("Contraseña encontrada en hilo 2: " + passList[i]);
            }
        }
    }

    public static void CrackearPasswordMultihilo3(List<String> passList, byte[] passString)
    {
        for (var i = (passList.Count * 2) / 4; i < passList.Count; i++)
        {
            if (GetHash(passList[i]).Equals(passString))
            {
                Console.WriteLine("Contraseña encontrada en hilo 3: " + passList[i]);
            }
        }
    }

    public static void CrackearPasswordMultihilo4(List<String> passList, byte[] passString)
    {
        for (var i = (passList.Count * 3) / 4; i < passList.Count; i++)
        {
            if (GetHash(passList[i]).Equals(passString))
            {
                Console.WriteLine("Contraseña encontrada en hilo 4: " + passList[i]);
            }
        }
    }

    public static void GestionarMultihilo(List<String> passList, byte[] passString)
    {
        Thread t1 = new Thread(() => CrackearPasswordMultihilo1(passList, passString));
        Thread t2 = new Thread(() => CrackearPasswordMultihilo2(passList, passString));
        Thread t3 = new Thread(() => CrackearPasswordMultihilo3(passList, passString));
        Thread t4 = new Thread(() => CrackearPasswordMultihilo4(passList, passString));
        var multiClock = Stopwatch.StartNew();
        t1.Start();
        t2.Start();
        t3.Start();
        t4.Start();
        t1.Join();
        t2.Join();
        t3.Join();
        t4.Join();
        multiClock.Stop();
        Console.WriteLine("Tiempo con 4 hilos:" + multiClock.ElapsedMilliseconds);
    }
    
     */

    //Esto rellena la lista de contraseñas con las del documento

}