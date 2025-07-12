using AutoMapper;
using ProyectoNFTs.Application.DTOs;
using ProyectoNFTs.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoNFTs.Application.Profiles;

public class ClienteProfile : Profile
{
    public ClienteProfile()
    {
        // Means: Source   , Destination and Reverse :)  
        CreateMap<ClienteDTO, Cliente>().ReverseMap();
    }

}
