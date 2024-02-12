using MockTest.Server.Data;
using MockTest.Server.IRepository;
using MockTest.Server.Models;
using MockTest.Shared.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MockTest.Server.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private IGenericRepository<TaskItem> _Tasks;

        private UserManager<ApplicationUser> _userManager;

        public UnitOfWork(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IGenericRepository<TaskItem> Tasks
            => _Tasks ??= new GenericRepository<TaskItem>(_context);

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }

        public async Task Save(HttpContext httpContext)
        {
            //To be implemented
            string user = "System";

            var entries = _context.ChangeTracker.Entries()
                .Where(q => q.State == EntityState.Modified ||
                    q.State == EntityState.Added);

            foreach (var entry in entries)
            {
                ((TaskItem)entry.Entity).CreatedDate = DateTime.Now;
                ((TaskItem)entry.Entity).CreatedBy = user;
                if (entry.State == EntityState.Added)
                {
                    ((TaskItem)entry.Entity).CreatedDate = DateTime.Now;
                    ((TaskItem)entry.Entity).CreatedBy = user;
                }
            }

            await _context.SaveChangesAsync();
        }
    }
}