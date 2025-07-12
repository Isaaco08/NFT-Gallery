using AutoMapper;
using ProyectoNFTs.Application.DTOs;
using ProyectoNFTs.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoNFTs.Application.Profiles;

public class NftProfile : Profile
{
    public NftProfile()
    {
        // Means: Source   , Destination and Reverse :)  
        CreateMap<NftDTO, Nft>().ReverseMap();
    }
}
