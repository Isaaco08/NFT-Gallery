using AutoMapper;
using ProyectoNFTs.Application.DTOs;
using ProyectoNFTs.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoNFTs.Application.Profiles;

public class PaisProfile : Profile
{
    public PaisProfile()
    {
        // Means: Source   , Destination and Reverse :)  
        CreateMap<PaisDTO, Pais>().ReverseMap();
    }
}
