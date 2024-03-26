using api.Data;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories;

public class CommentRepository(ApplicationDbContext context) : ICommentRepository
{
    private readonly ApplicationDbContext _context = context;


    public async Task<Comment> CreateAsync(Comment CommentModel)
    {
        System.Diagnostics.Debug.WriteLine(CommentModel);
        await _context.Comments.AddAsync(CommentModel);

        await _context.SaveChangesAsync();

        return CommentModel;
    }


    public async Task<Comment?> DeleteAsync(int id)
    {
        var commetnModel = await GetByIdAsync(id);
        if (commetnModel is null)
            return null;

        _context.Remove(commetnModel);

        await _context.SaveChangesAsync();

        return commetnModel;
    }


    public async Task<List<Comment>> GetAllAsync()
        => await _context.Comments
            .Include(comment => comment.AppUser)
            .ToListAsync();


    public async Task<Comment?> GetByIdAsync(int id)
        => await _context.Comments
            .Include(comment => comment.AppUser)
            .FirstOrDefaultAsync(comment => comment.Id == id);


    public async Task<Comment?> UpdateAsync(int id, Comment comment)
    {
        var commetnModel = await GetByIdAsync(id);
        if (commetnModel is null)
            return null;

        commetnModel.Content = comment.Content;
        commetnModel.Title = comment.Title;

        // _context.Comments.Update();

        await _context.SaveChangesAsync();

        return commetnModel;
    }
}