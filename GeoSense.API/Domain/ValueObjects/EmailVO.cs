namespace GeoSense.API.Domain.ValueObjects
{
    using System.Text.RegularExpressions;

    /// <summary>
    /// Value Object que representa um email validado no domínio.
    /// Imutável, valida formato e normaliza para lowercase.
    /// </summary>
    public sealed class EmailVO
    {
        private static readonly Regex EmailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public string Value { get; }

        public EmailVO(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email não pode ser vazio.", nameof(email));

            var normalized = email.Trim();

            if (!EmailRegex.IsMatch(normalized))
                throw new ArgumentException("Email em formato inválido.", nameof(email));

            Value = normalized.ToLowerInvariant();
        }

        public override string ToString() => Value;

        public override bool Equals(object? obj)
        {
            if (obj is EmailVO other) return Value == other.Value;
            if (obj is string s) return Value == s;
            return false;
        }

        public override int GetHashCode() => Value.GetHashCode(StringComparison.Ordinal);

        public static implicit operator string(EmailVO vo) => vo.Value;
        public static explicit operator EmailVO(string s) => new EmailVO(s);

        public static bool operator ==(EmailVO? a, EmailVO? b) => Equals(a, b);
        public static bool operator !=(EmailVO? a, EmailVO? b) => !Equals(a, b);
    }
}