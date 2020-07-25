using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ISMNewsPortal.BLL.Models;
using ISMNewsPortal.BLL.BusinessModels;

namespace ISMNewsPortal.BLL.Mappers
{
    public static class Automapper
    {
        public static T Map<T>(T original, T target)
        {
            return new MapperConfiguration(cfg => cfg.CreateMap<T, T>()).CreateMapper().Map(original, target);
        }
        #region Map
        public static Admin MapToAdminDTO<T>(T target)
        {
            IMapper mapper = new MapperConfiguration(cfg => cfg.CreateMap<T, Admin>()).CreateMapper();
            return mapper.Map<Admin>(target);
        }

        public static T MapFromAdminDTO<T>(Admin target)
        {
            IMapper mapper = new MapperConfiguration(cfg => cfg.CreateMap<Admin, T>()).CreateMapper();
            return mapper.Map<T>(target);
        }

        public static NewsPost MapToNewsPostDTO<T>(T target)
        {
            IMapper mapper = new MapperConfiguration(cfg => cfg.CreateMap<T, NewsPost>()).CreateMapper();
            return mapper.Map<NewsPost>(target);
        }

        public static T MapFromNewsPostDTO<T>(NewsPost target)
        {
            IMapper mapper = new MapperConfiguration(cfg => cfg.CreateMap<NewsPost, T>()).CreateMapper();
            return mapper.Map<T>(target);
        }

        public static Comment MapToCommentDTO<T>(T target)
        {
            IMapper mapper = new MapperConfiguration(cfg => cfg.CreateMap<T, Comment>()).CreateMapper();
            return mapper.Map<Comment>(target);
        }

        public static T MapFromCommentDTO<T>(Comment target)
        {
            IMapper mapper = new MapperConfiguration(cfg => cfg.CreateMap<Comment, T>()).CreateMapper();
            return mapper.Map<T>(target);
        }

        public static FileModel MapToFileDTO<T>(T target)
        {
            IMapper mapper = new MapperConfiguration(cfg => cfg.CreateMap<T, FileModel>()).CreateMapper();
            return mapper.Map<FileModel>(target);
        }

        public static T MapFromFileDTO<T>(FileModel target)
        {
            IMapper mapper = new MapperConfiguration(cfg => cfg.CreateMap<FileModel, T>()).CreateMapper();
            return mapper.Map<T>(target);
        }

        public static ToolsDTO MapToToolsDTO<T>(T target)
        {
            IMapper mapper = new MapperConfiguration(cfg => cfg.CreateMap<T, ToolsDTO>()).CreateMapper();
            return mapper.Map<ToolsDTO>(target);
        }

        public static T MapFromToolsDTO<T>(ToolsDTO target)
        {
            IMapper mapper = new MapperConfiguration(cfg => cfg.CreateMap<ToolsDTO, T>()).CreateMapper();
            return mapper.Map<T>(target);
        }

        public static U MapToUDTO<T, U>(T target)
        {
            IMapper mapper = new MapperConfiguration(cfg => cfg.CreateMap<T, U>()).CreateMapper();
            return mapper.Map<U>(target);
        }

        public static T MapFromUDTO<T, U>(U target)
        {
            IMapper mapper = new MapperConfiguration(cfg => cfg.CreateMap<U, T>()).CreateMapper();
            return mapper.Map<T>(target);
        }
        #endregion
        #region MapList 
        public static List<Admin> MapToAdminDTOList<T>(IEnumerable<T> target)
        {
            IMapper mapper = new MapperConfiguration(cfg => cfg.CreateMap<T, Admin>()).CreateMapper();
            var result = new List<Admin>();
            foreach (T item in target)
                result.Add(mapper.Map<Admin>(item));
            return result;
        }

        public static List<T> MapFromAdminDTOList<T>(IEnumerable<Admin> target)
        {
            IMapper mapper = new MapperConfiguration(cfg => cfg.CreateMap<Admin, T>()).CreateMapper();
            var result = new List<T>();
            foreach (Admin item in target)
                result.Add(mapper.Map<T>(item));
            return result;
        }

        public static List<NewsPost> MapToNewsPostDTOList<T>(IEnumerable<T> target)
        {
            IMapper mapper = new MapperConfiguration(cfg => cfg.CreateMap<T, NewsPost>()).CreateMapper();
            var result = new List<NewsPost>();
            foreach (T item in target)
                result.Add(mapper.Map<NewsPost>(item));
            return result;
        }

        public static List<T> MapFromNewsPostDTOList<T>(IEnumerable<NewsPost> target)
        {
            IMapper mapper = new MapperConfiguration(cfg => cfg.CreateMap<NewsPost, T>()).CreateMapper();
            var result = new List<T>();
            foreach (NewsPost item in target)
                result.Add(mapper.Map<T>(item));
            return result;
        }

        public static List<Comment> MapToCommentDTOList<T>(IEnumerable<T> target)
        {
            IMapper mapper = new MapperConfiguration(cfg => cfg.CreateMap<T, Comment>()).CreateMapper();
            var result = new List<Comment>();
            foreach (T item in target)
                result.Add(mapper.Map<Comment>(item));
            return result;
        }

        public static List<T> MapFromCommentDTOList<T>(IEnumerable<Comment> target)
        {
            IMapper mapper = new MapperConfiguration(cfg => cfg.CreateMap<Comment, T>()).CreateMapper();
            var result = new List<T>();
            foreach (Comment item in target)
                result.Add(mapper.Map<T>(item));
            return result;
        }

        public static List<FileModel> MapToFileDTOList<T>(IEnumerable<T> target)
        {
            IMapper mapper = new MapperConfiguration(cfg => cfg.CreateMap<T, FileModel>()).CreateMapper();
            var result = new List<FileModel>();
            foreach (T item in target)
                result.Add(mapper.Map<FileModel>(item));
            return result;
        }

        public static List<T> MapFromFileDTOList<T>(IEnumerable<FileModel> target)
        {
            IMapper mapper = new MapperConfiguration(cfg => cfg.CreateMap<Comment, T>()).CreateMapper();
            var result = new List<T>();
            foreach (FileModel item in target)
                result.Add(mapper.Map<T>(item));
            return result;
        }

        public static List<U> MapToUDTOList<T, U>(IEnumerable<T> target)
        {
            IMapper mapper = new MapperConfiguration(cfg => cfg.CreateMap<T, U>()).CreateMapper();
            var result = new List<U>();
            foreach (T item in target)
                result.Add(mapper.Map<U>(item));
            return result;
        }

        public static List<T> MapFromUDTOList<T, U>(IEnumerable<U> target)
        {
            IMapper mapper = new MapperConfiguration(cfg => cfg.CreateMap<U, T>()).CreateMapper();
            var result = new List<T>();
            foreach (U item in target)
                result.Add(mapper.Map<T>(item));
            return result;
        }
        #endregion
    }
}
