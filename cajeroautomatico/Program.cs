using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;

namespace Trabajo
{
    // Clase que representa una transacción individual
    public class Transaccion
    {
        public string Descripcion { get; set; }  // Descripción del tipo de transacción (ej. "Depósito", "Retiro")
        public double Cantidad { get; set; }     // Monto asociado a la transacción
        public DateTime FechaTransaccion { get; set; }  // Fecha y hora en la que ocurrió la transacción

        // Constructor que inicializa los valores al crear una nueva transacción
        public Transaccion(string descripcion, double cantidad)
        {
            Descripcion = descripcion;
            Cantidad = cantidad;
            FechaTransaccion = DateTime.Now;  // Se registra la fecha actual de la transacción
        }
    }

    internal class Program
    {
        // Lista que almacena todas las transacciones realizadas
        static List<Transaccion> transacciones = new List<Transaccion>();

        // Método para cargar las transacciones desde el archivo "datos.json" si existe
        public static void CargarDesdeArchivo()
        {
            if (File.Exists("datos.json"))  // Verifica si el archivo existe
            {
                string contenidoJson = File.ReadAllText("datos.json");  // Lee el contenido del archivo
                transacciones = JsonConvert.DeserializeObject<List<Transaccion>>(contenidoJson) ?? new List<Transaccion>();
                // Deserializa el contenido JSON en la lista de transacciones; si es nulo, inicializa una lista vacía
            }
            else
            {
                transacciones = new List<Transaccion>();  // Si el archivo no existe, crea una lista nueva vacía
            }
        }

        // Método para guardar todas las transacciones actuales en el archivo "datos.json"
        public static void GuardarEnArchivo()
        {
            string contenidoJson = JsonConvert.SerializeObject(transacciones, Formatting.Indented);  // Serializa la lista en formato JSON con indentación
            File.WriteAllText("datos.json", contenidoJson);  // Escribe el JSON en el archivo
        }


        static void Main(string[] args)
        {
            CargarDesdeArchivo();  // Carga el historial de transacciones al iniciar el programa

            // Resto del código del programa (interfaz de usuario, operaciones de cajero, etc.)

            // Por ejemplo, para agregar una nueva transacción:
            transacciones.Add(new Transaccion("Depósito", 1000));  // Agregar una transacción de depósito

            GuardarEnArchivo();  // Guarda las transacciones actualizadas en el archivo



            string tarj = "", pass = "", cuenNueva = "", pass2 = "";
            int cont = 0, op = 0;
            double saldo = 100, mon = 0, dep = 0;

            // Proceso de autenticación
            while (tarj != "1234567890" || pass != "1234")
            {
                Console.Clear();
                Console.WriteLine("CAJERO AUTOMATICO  ");
                Console.Write("\nIngrese su número de tarjeta: ");
                tarj = Console.ReadLine();
                Console.Write("Ingrese la clave de su tarjeta: ");
                pass = Console.ReadLine();

                if (tarj != "1234567890" || pass != "1234")
                {
                    cont++;
                    if (cont == 3)
                    {
                        Console.WriteLine("Clave inválida más de 3 veces. Tu tarjeta está retenida.");
                        Console.ReadLine();
                        return;
                    }
                }
            }

            // Menú principal
            do
            {
                Console.Clear();
                Console.WriteLine("\nCAJERO AUTOMATICO  ");
                Console.WriteLine("\nSeleccione la operación que desea realizar:");
                Console.WriteLine("1.- Retiro de saldo.");
                Console.WriteLine("2.- Consulta de saldo.");
                Console.WriteLine("3.- Depósito de efectivo.");
                Console.WriteLine("4.- Crear una nueva cuenta.");
                Console.WriteLine("5.- Ver historial de transacciones.");
                Console.WriteLine("6.- Salir.");
                Console.Write("Ingrese su opción: ");
                op = int.Parse(Console.ReadLine());

                switch (op)
                {
                    case 1: // Retiro de dinero
                        Console.Clear();
                        Console.Write("Ingrese el monto a retirar: ");
                        mon = double.Parse(Console.ReadLine());

                        if (saldo >= mon)
                        {
                            saldo -= mon;
                            transacciones.Add(new Transaccion("Retiro", mon));
                            GuardarEnArchivo();
                            Console.WriteLine($"Retiro exitoso de Q{mon}. Saldo actual: Q{saldo}");
                        }
                        else
                        {
                            Console.WriteLine("Saldo insuficiente.");
                        }
                        break;

                    case 2: // Consulta de saldo
                        Console.Clear();
                        Console.WriteLine($"Su saldo actual es: Q{saldo}");
                        transacciones.Add(new Transaccion("Consulta de saldo", saldo));
                        GuardarEnArchivo();
                        break;

                    case 3: // Depósito de efectivo
                        Console.Clear();
                        Console.Write("Ingrese la cantidad a depositar: Q");
                        dep = double.Parse(Console.ReadLine());

                        if (dep > 0 && dep <= 25000)
                        {
                            saldo += dep;
                            transacciones.Add(new Transaccion("Depósito", dep));
                            GuardarEnArchivo();
                            Console.WriteLine($"Depósito exitoso de Q{dep}. Saldo actual: Q{saldo}");
                        }
                        else
                        {
                            Console.WriteLine("Monto no permitido.");
                        }
                        break;

                    case 4: // Crear nueva cuenta
                        Console.Clear();
                        Console.Write("Ingrese la nueva cuenta: ");
                        cuenNueva = Console.ReadLine();
                        Console.Write("Ingrese la clave: ");
                        pass2 = Console.ReadLine();
                        Console.WriteLine($"Nueva cuenta registrada: {cuenNueva}");
                        break;

                    case 5: // Ver historial de transacciones
                        Console.Clear();
                        Console.WriteLine("HISTORIAL DE TRANSACCIONES:");
                        if (transacciones.Count > 0)
                        {
                            foreach (var transaccion in transacciones)
                            {
                                Console.WriteLine($"{transaccion.Descripcion} - Q{transaccion.Cantidad} - {transaccion.FechaTransaccion}");
                            }
                        }
                        else
                        {
                            Console.WriteLine("No hay transacciones registradas.");
                        }
                        break;

                    case 6: // Salir
                        Console.WriteLine("Gracias por su visita. Hasta pronto.");
                        return;
                }

                Console.WriteLine("\nPresione cualquier tecla para continuar...");
                Console.ReadKey();

            } while (op != 6);
        }

    }
}





