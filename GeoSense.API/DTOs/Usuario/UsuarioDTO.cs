namespace GeoSense.API.DTOs.Usuario
{
    /// <summary>
    /// Representa os dados necessários para cadastrar ou atualizar um Usuário.
    /// </summary>
    public class UsuarioDTO
    {
        // Para POST, Id não é obrigatório
        public long? Id { get; set; }
        public required string Nome { get; set; }
        public required string Email { get; set; }
        public required string Senha { get; set; }
        public int Tipo { get; set; } // int para facilitar o mapeamento do enum
    }
}