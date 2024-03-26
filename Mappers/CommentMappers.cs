using api.DTOs.Comment;
using api.Models;

namespace api.Mappers;

public static class CommentMappers
{
    public static CommentDTO ToCommentDTO(this Comment comment)
    {
        return new CommentDTO
        {
            Id = comment.Id,
            Content = comment.Content,
            Title = comment.Title,
            CreatedOn = comment.CreatedOn,
            CreatedBy = comment.AppUser.UserName ?? "in ToCommentDTO() Functions is Null",
            StockId = comment.StockId,
        };
    }

    public static Comment ToCommentFromCreateDTO(this CreateCommentRequestDTO commentDTO, int stockId, AppUser appUser)
    {
        return new Comment
        {
            Title = commentDTO.Title,
            Content = commentDTO.Content,
            StockId = stockId,
            AppUser = appUser,
            AppUserId = appUser.Id,
        };
    }


    public static Comment ToCommentFromUpdate(this UpdateCommentRequestDTO commentDto, int stockId, AppUser appUser)
    {
        return new Comment
        {
            Title = commentDto.Title,
            Content = commentDto.Content,
            StockId = stockId,
            AppUser = appUser,
            AppUserId = appUser.Id,
        };
    }
}