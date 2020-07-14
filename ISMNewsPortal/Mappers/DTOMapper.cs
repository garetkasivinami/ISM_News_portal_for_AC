﻿using AutoMapper;
using ISMNewsPortal.BLL.BusinessModels;
using ISMNewsPortal.BLL.DTO;
using ISMNewsPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ISMNewsPortal.Mappers
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

        public static NewsPost MapNewsPost(NewsPostDTO newsPostDTO)
        {
            return newsPostMapper.Map<NewsPostDTO, NewsPost>(newsPostDTO);
        }

        public static List<NewsPost> MapNewsPosts(IEnumerable<NewsPostDTO> newsPostsDTO)
        {
            return newsPostMapper.Map<IEnumerable<NewsPostDTO>, List<NewsPost>>(newsPostsDTO);
        }

        public static NewsPostDTO MapNewsPostDTO(NewsPost newsPost)
        {
            return newsPostMapperToDTO.Map<NewsPost, NewsPostDTO>(newsPost);
        }

        public static List<NewsPostDTO> MapNewsPostsDTO(IEnumerable<NewsPost> newsPosts)
        {
            return newsPostMapperToDTO.Map<IEnumerable<NewsPost>, List<NewsPostDTO>>(newsPosts);
        }

        public static NewsPost MapComment(CommentDTO commentDTO)
        {
            return commentMapper.Map<CommentDTO, NewsPost>(commentDTO);
        }

        public static List<Comment> MapComments(IEnumerable<CommentDTO> commentsDTO)
        {
            return commentMapper.Map<IEnumerable<CommentDTO>, List<Comment>>(commentsDTO);
        }

        public static CommentDTO MapCommentDTO(Comment comment)
        {
            return commentMapperToDTO.Map<Comment, CommentDTO>(comment);
        }

        public static List<CommentDTO> MapCommentsDTO(IEnumerable<Comment> comments)
        {
            return commentMapperToDTO.Map<IEnumerable<Comment>, List<CommentDTO>>(comments);
        }

        public static Admin MapAdmin(AdminDTO adminDTO)
        {
            return adminMapper.Map<AdminDTO, Admin>(adminDTO);
        }

        public static List<Admin> MapAdmins(IEnumerable<AdminDTO> adminsDTO)
        {
            return adminMapper.Map<IEnumerable<AdminDTO>, List<Admin>>(adminsDTO);
        }

        public static AdminDTO MapAdminDTO(Admin admin)
        {
            return adminMapperToDTO.Map<Admin, AdminDTO>(admin);
        }

        public static List<AdminDTO> MapAdminsDTO(IEnumerable<Admin> admins)
        {
            return adminMapperToDTO.Map<IEnumerable<Admin>, List<AdminDTO>>(admins);
        }
    }
}