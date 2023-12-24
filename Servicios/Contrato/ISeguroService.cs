﻿using BackendConsultorioSeguros.DTOs;
using BackendConsultorioSeguros.Models;

namespace BackendConsultorioSeguros.Servicios.Contrato
{
    public interface ISeguroService
    {
        Task<List<SeguroDto>> GetListSeguros();
        Task<SeguroDto> GetSeguroById(int idSeguro);
        Task<SeguroDto> GetSeguroByCodigo(string codigoSeguro);
        Task<SeguroDto> CreateSeguro(SeguroDto seguro);
        Task<SeguroDto> UpdateSeguro(int seguroId, SeguroDto seguro);
        Task<bool> InactivarSeguro(int seguroId);

    }
}
