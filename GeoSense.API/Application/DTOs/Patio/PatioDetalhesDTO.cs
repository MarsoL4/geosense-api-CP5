﻿using GeoSense.API.Application.DTOs.Vaga;

namespace GeoSense.API.Application.DTOs.Patio
{
    /// <summary>
    /// DTO de detalhes do pátio, incluindo vagas.
    /// </summary>
    public class PatioDetalhesDTO
    {
        public long Id { get; set; }
        public required string Nome { get; set; }
        public List<VagaDTO> Vagas { get; set; } = new List<VagaDTO>();
    }
}