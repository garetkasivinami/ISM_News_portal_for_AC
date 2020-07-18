using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ISMNewsPortal.BLL.DTO;
using ISMNewsPortal.BLL.BusinessModels;

namespace ISMNewsPortal.BLL.Mappers
{
    public static class Automapper
    {
        #region Map
        public static AdminDTO MapToAdminDTO<T>(T target)
        {
            IMapper mapper = new MapperConfiguration(cfg => cfg.CreateMap<T, AdminDTO>()).CreateMapper();
            return mapper.Map<AdminDTO>(target);
        }

        public static T MapFromAdminDTO<T>(AdminDTO target)
        {
            IMapper mapper = new MapperConfiguration(cfg => cfg.CreateMap<AdminDTO, T>()).CreateMapper();
            return mapper.Map<T>(target);
        }

        public static NewsPostDTO MapToNewsPostDTO<T>(T target)
        {
            IMapper mapper = new MapperConfiguration(cfg => cfg.CreateMap<T, NewsPostDTO>()).CreateMapper();
            return mapper.Map<NewsPostDTO>(target);
        }

        public static T MapFromNewsPostDTO<T>(NewsPostDTO target)
        {
            IMapper mapper = new MapperConfiguration(cfg => cfg.CreateMap<NewsPostDTO, T>()).CreateMapper();
            return mapper.Map<T>(target);
        }

        public static CommentDTO MapToCommentDTO<T>(T target)
        {
            IMapper mapper = new MapperConfiguration(cfg => cfg.CreateMap<T, CommentDTO>()).CreateMapper();
            return mapper.Map<CommentDTO>(target);
        }

        public static T MapFromCommentDTO<T>(CommentDTO target)
        {
            IMapper mapper = new MapperConfiguration(cfg => cfg.CreateMap<CommentDTO, T>()).CreateMapper();
            return mapper.Map<T>(target);
        }

        public static FileDTO MapToFileDTO<T>(T target)
        {
            IMapper mapper = new MapperConfiguration(cfg => cfg.CreateMap<T, FileDTO>()).CreateMapper();
            return mapper.Map<FileDTO>(target);
        }

        public static T MapFromFileDTO<T>(FileDTO target)
        {
            IMapper mapper = new MapperConfiguration(cfg => cfg.CreateMap<FileDTO, T>()).CreateMapper();
            return mapper.Map<T>(target);
        }
        #endregion
        #region MapList 
        public static List<AdminDTO> MapToAdminDTOList<T>(IEnumerable<T> target)
        {
            IMapper mapper = new MapperConfiguration(cfg => cfg.CreateMap<IEnumerable<T>, List<AdminDTO>>()).CreateMapper();
            return mapper.Map<List<AdminDTO>>(target);
        }

        public static List<T> MapFromAdminDTOList<T>(IEnumerable<AdminDTO> target)
        {
            IMapper mapper = new MapperConfiguration(cfg => cfg.CreateMap<IEnumerable<AdminDTO>, List<T>>()).CreateMapper();
            return mapper.Map<List<T>>(target);
        }

        public static List<NewsPostDTO> MapToNewsPostDTOList<T>(IEnumerable<T> target)
        {
            IMapper mapper = new MapperConfiguration(cfg => cfg.CreateMap<T, NewsPostDTO>()).CreateMapper();
            var result = new List<NewsPostDTO>();
            foreach (T item in target)
                result.Add(mapper.Map<NewsPostDTO>(item));
            return result;
        }

        public static List<T> MapFromNewsPostDTOList<T>(IEnumerable<NewsPostDTO> target)
        {
            IMapper mapper = new MapperConfiguration(cfg => cfg.CreateMap<NewsPostDTO, T>()).CreateMapper();
            var result = new List<T>();
            foreach (NewsPostDTO item in target)
                result.Add(mapper.Map<T>(item));
            return result;
        }

        public static List<CommentDTO> MapToCommentDTOList<T>(IEnumerable<T> target)
        {
            IMapper mapper = new MapperConfiguration(cfg => cfg.CreateMap<T, CommentDTO>()).CreateMapper();
            var result = new List<CommentDTO>();
            foreach (T item in target)
                result.Add(mapper.Map<CommentDTO>(item));
            return result;
        }

        public static List<T> MapFromCommentDTOList<T>(IEnumerable<CommentDTO> target)
        {
            IMapper mapper = new MapperConfiguration(cfg => cfg.CreateMap<CommentDTO, T>()).CreateMapper();
            var result = new List<T>();
            foreach (CommentDTO item in target)
                result.Add(mapper.Map<T>(item));
            return result;
        }

        public static List<FileDTO> MapToFileDTOList<T>(IEnumerable<T> target)
        {
            IMapper mapper = new MapperConfiguration(cfg => cfg.CreateMap<T, FileDTO>()).CreateMapper();
            var result = new List<FileDTO>();
            foreach (T item in target)
                result.Add(mapper.Map<FileDTO>(item));
            return result;
        }

        public static List<T> MapFromFileDTOList<T>(IEnumerable<FileDTO> target)
        {
            IMapper mapper = new MapperConfiguration(cfg => cfg.CreateMap<CommentDTO, T>()).CreateMapper();
            var result = new List<T>();
            foreach (FileDTO item in target)
                result.Add(mapper.Map<T>(item));
            return result;
        }
        #endregion
    }
}
