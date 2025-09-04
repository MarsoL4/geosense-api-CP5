using System.Text.RegularExpressions;

namespace GeoSense.API.src.Domain.ValueObjects
{
    public class Placa : IEquatable<Placa>
    {
        public string Valor { get; }

        public Placa(string valor)
        {
            if (string.IsNullOrWhiteSpace(valor))
                throw new ArgumentException("Placa não pode ser vazia.");

            // Regex básica para validar AAA-0A00 ou AAA0000
            if (!Regex.IsMatch(valor, @"^[A-Z]{3}-?[0-9][A-Z0-9][0-9]{2}$", RegexOptions.IgnoreCase))
                throw new ArgumentException("Placa inválida.");

            Valor = valor.ToUpper();
        }

        public override bool Equals(object obj) => Equals(obj as Placa);

        public bool Equals(Placa other) => other != null && Valor == other.Valor;

        public override int GetHashCode() => Valor.GetHashCode();

        public override string ToString() => Valor;

        public static implicit operator string(Placa placa) => placa.Valor;
    }
}