﻿using AutoMapper;
using STEMHub.STEMHub_Data.Data;
using STEMHub.STEMHub_Data.Entities;
using STEMHub.STEMHub_Service.Interfaces;
using STEMHub.STEMHub_Service.Repository;

namespace STEMHub.STEMHub_Service
{
    public class UnitOfWork : IDisposable
    {
        private readonly STEMHubDbContext _context;
        public IMapper Mapper { get; }

        public ICrudRepository<Banner> BannerRepository { get; set; } = null!;
        public ICrudRepository<Ingredients> IngredientsRepository { get; set; } = null!;
        public ICrudRepository<Comment> CommentRepository { get; set; } = null!;
        public ICrudRepository<Lesson> LessonRepository { get; set; } = null!;
        public ICrudRepository<NewspaperArticle> NewspaperArticleRepository { get; set; } = null!;
        public ICrudRepository<STEM> STEMRepository { get; set; } = null!;
        public ICrudRepository<Topic> TopicRepository { get; set; } = null!;
        public ICrudRepository<Video> VideoRepository { get; set; } = null!;
        public ICrudRepository<ApplicationUser> ApplicationUserRepository { get; set; }

        public UnitOfWork(STEMHubDbContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            Mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            InitializeRepositories(mapper);
        }

        private void InitializeRepositories(IMapper mapper)
        {
            BannerRepository = new CrudRepository<Banner>(_context, mapper);
            CommentRepository = new CrudRepository<Comment>(_context, mapper);
            LessonRepository = new CrudRepository<Lesson>(_context, mapper);
            NewspaperArticleRepository = new CrudRepository<NewspaperArticle>(_context, mapper);
            STEMRepository = new CrudRepository<STEM>(_context, mapper);
            TopicRepository = new CrudRepository<Topic>(_context, mapper);
            VideoRepository = new CrudRepository<Video>(_context, mapper);
            ApplicationUserRepository = new CrudRepository<ApplicationUser>(_context, mapper);
            IngredientsRepository = new CrudRepository<Ingredients>(_context, mapper);
        }

        public void Commit()
        {
            _context.SaveChanges();
        }

        public async Task CommitAsync()
        { 
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
        }
    }
}
