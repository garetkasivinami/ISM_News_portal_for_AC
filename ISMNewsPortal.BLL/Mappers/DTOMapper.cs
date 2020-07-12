using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ISMNewsPortal.DAL.Models;
using ISMNewsPortal.BLL.DTO;
using ISMNewsPortal.BLL.BusinessModels;

namespace ISMNewsPortal.BLL.Mappers
{
    public static class DTOMapper
    {
        private static IMapper adminMapperToDTO = new MapperConfiguration(cfg => cfg.CreateMap<Admin, AdminDTO>()).CreateMapper();
        private static IMapper commentMapperToDTO = new MapperConfiguration(cfg => cfg.CreateMap<Comment, CommentDTO>()).CreateMapper();
        private static IMapper newsPostMapperToDTO = new MapperConfiguration(cfg => cfg.CreateMap<NewsPost, NewsPostDTO>()).CreateMapper();
        private static IMapper fileMapperToDTO = new MapperConfiguration(cfg => cfg.CreateMap<FileModel, FileDTO>()).CreateMapper();
        private static IMapper toolsMapperToDTO = new MapperConfiguration(cfg => cfg.CreateMap<ToolBarModel, ToolsDTO>()).CreateMapper();

        private static IMapper adminMapper = new MapperConfiguration(cfg => cfg.CreateMap<AdminDTO, Admin>()).CreateMapper();
        private static IMapper commentMapper = new MapperConfiguration(cfg => cfg.CreateMap<CommentDTO, Comment>()).CreateMapper();
        private static IMapper newsPostMapper = new MapperConfiguration(cfg => cfg.CreateMap<NewsPostDTO, NewsPost>()).CreateMapper();
        private static IMapper fileMapper = new MapperConfiguration(cfg => cfg.CreateMap<FileDTO, FileModel>()).CreateMapper();
        private static IMapper toolsMapper = new MapperConfiguration(cfg => cfg.CreateMap<ToolsDTO, ToolBarModel>()).CreateMapper();

        public static IMapper AdminMapperToDTO 
        { 
            get => adminMapperToDTO;
        }
        public static IMapper CommentMapperToDTO 
        { 
            get => commentMapperToDTO;
        }
        public static IMapper NewsPostMapperToDTO 
        { 
            get => newsPostMapperToDTO;
        }
        public static IMapper FileMapperToDTO
        {
            get => fileMapperToDTO;
        }
        public static IMapper ToolsMapperToDTO
        {
            get => toolsMapperToDTO;
        }

        public static IMapper AdminMapper
        {
            get => adminMapper;
        }
        public static IMapper CommentMapper
        {
            get => commentMapper;
        }
        public static IMapper NewsPostMapper
        {
            get => newsPostMapper;
        }
        public static IMapper FileMapper
        {
            get => fileMapper;
        }
        public static IMapper ToolsMapper
        {
            get => toolsMapper;
        }
    }
}
