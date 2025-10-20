namespace GeoSense.API.Domain.ValueObjects
{
    using System.Text.RegularExpressions;

    /// <summary>
    /// Value Object que representa uma placa validada no domínio.
    /// Imutável, valida um padrão razoável (letras maiúsculas / números / '-') e normaliza.
    /// Ajuste o regex conforme a regra de negócio desejada (ex.: padrões locais).
    /// </summary>
    public sealed class PlacaVO
    {
        // Permite letras maiúsculas, números e hífen, entre 1 e 10 caracteres
        private static readonly Regex PlacaRegex = new Regex(@"^[A-Z0-9-]{1,10}$", RegexOptions.Compiled);

        public string Value { get; }

        public PlacaVO(string placa)
        {
            if (string.IsNullOrWhiteSpace(placa))
                throw new ArgumentException("Placa não pode ser vazia.", nameof(placa));

            var normalized = placa.Trim().ToUpperInvariant();

            if (!PlacaRegex.IsMatch(normalized))
                throw new ArgumentException("Placa em formato inválido.", nameof(placa));

            Value = normalized;
        }

        public override string ToString() => Value;

        public override bool Equals(object? obj)
        {
            if (obj is PlacaVO other) return Value == other.Value;
            if (obj is string s) return Value == s;
            return false;
        }

        public override int GetHashCode() => Value.GetHashCode(StringComparison.Ordinal);

        public static implicit operator string(PlacaVO vo) => vo.Value;
        public static explicit operator PlacaVO(string s) => new PlacaVO(s);

        public static bool operator ==(PlacaVO? a, PlacaVO? b) => Equals(a, b);
        public static bool operator !=(PlacaVO? a, PlacaVO? b) => !Equals(a, b);
    }
}