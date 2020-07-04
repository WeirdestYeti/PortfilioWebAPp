using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PortfolioWebApp.Data;
using PortfolioWebApp.Models.MyProjects;
using PortfolioWebApp.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace PortfolioWebApp.Services
{
    public class MyProjectService
    {
        private readonly AppDbContext _dbContext;
        private readonly IWebHostEnvironment _environment;


        private string[] permittedImageExtensions = { ".jpg", ".jpeg", ".gif", ".png" };

        public MyProjectService(AppDbContext appDbContext, IWebHostEnvironment environment)
        {
            _dbContext = appDbContext;
            _environment = environment;
        }

        /// <summary>
        /// Gets all projects async.
        /// </summary>
        /// <returns></returns>
        public async Task<List<MyProject>> GetAllAsync()
        {
            return await _dbContext.MyProjects.ToListAsync();
        }


        /// <summary>
        /// Creates my project async.
        /// </summary>
        /// <param name="myProject"></param>
        /// <returns></returns>
        public async Task<(bool, string)> CreateAsync(MyProject myProject, List<IFormFile> formFiles)
        {
            if(myProject != null)
            {
                if(formFiles != null && formFiles.Count > 0)
                {
                    myProject.MyProjectImages = await SaveImagesAsync(formFiles, myProject);
                }

                _dbContext.MyProjects.Add(myProject);

                await _dbContext.SaveChangesAsync();

                return (true, "Project added successfully");
            }

            return (false, "Null object provided.");
        }

        /// <summary>
        /// Updates project to database and adds new images to server.
        /// </summary>
        /// <param name="myProject"></param>
        /// <param name="newImageFiles"></param>
        /// <returns></returns>
        public async Task<(bool, string)> EditAsync(MyProject myProject, List<IFormFile> newImageFiles)
        {
            if(myProject != null)
            {
                MyProject dbProject = await _dbContext.MyProjects.SingleOrDefaultAsync(x => x.Id == myProject.Id);

                if (dbProject != null)
                {
                    if(newImageFiles != null && newImageFiles.Count > 0)
                    {
                        
                        List<MyProjectImage> myProjectImages = await SaveImagesAsync(newImageFiles, dbProject);
                        _dbContext.MyProjectImages.AddRange(myProjectImages);
                    }

                    dbProject.Title = myProject.Title;
                    dbProject.ShortDescription = myProject.ShortDescription;
                    dbProject.IsRepositoryPrivate = myProject.IsRepositoryPrivate;
                    dbProject.RepositoryUrl = myProject.RepositoryUrl;
                    dbProject.ThumbnailUrl = myProject.ThumbnailUrl;
                    dbProject.ShowSlideshow = myProject.ShowSlideshow;
                    dbProject.HTMLContent = myProject.HTMLContent;

                    _dbContext.Update(dbProject);
                    await _dbContext.SaveChangesAsync();

                    return (true, "Project updated successfully.");
                }

                return (false, "Project not found.");
            }

            return (false, "Null object provided.");
        }

        /// <summary>
        /// Gets Project by id async.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<MyProject> GetByIdAsync(int id)
        {
            MyProject myProject = await _dbContext.MyProjects.SingleOrDefaultAsync(x => x.Id == id);
            if(myProject != null)
            {
                myProject.MyProjectImages = await _dbContext.MyProjectImages.Where(x => x.MyProject.Id == myProject.Id).ToListAsync();
            }

            return myProject;
        }

        /// <summary>
        /// Get Images for specific project by id;
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns>Null if project none are found</returns>
        public async Task<List<MyProjectImage>> GetProjectImagesByIdAsync(int projectId)
        {
            if(projectId != 0)
            {
                List<MyProjectImage> projectImages = await _dbContext.MyProjectImages.Where(x => x.MyProject.Id == projectId).ToListAsync();

                return projectImages; 
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<(bool, string)> DeleteMyProjectByIdAsync(int id)
        {
            if(id != 0)
            {
                MyProject myProject = await _dbContext.MyProjects.SingleOrDefaultAsync(x => x.Id == id);
                if(myProject != null)
                {
                    List<MyProjectImage> projectImages = await _dbContext.MyProjectImages.Where(x => x.MyProject.Id == myProject.Id).ToListAsync();
                    
                    if(projectImages != null && projectImages.Count > 0)
                    {
                        for (int i = 0; i < projectImages.Count; i++)
                        {
                            var imgaePath = Path.Combine(_environment.WebRootPath, projectImages[i].Location);

                            if (File.Exists(imgaePath))
                            {
                                File.Delete(imgaePath);
                            }
                        }

                        _dbContext.MyProjectImages.RemoveRange(projectImages);

                    }

                    _dbContext.MyProjects.Remove(myProject);

                    await _dbContext.SaveChangesAsync();

                    return (true, "Project deleted.");
                }
            }

            return (false, "No project found.");
        }


        /// <summary>
        /// Deletes a single image from database and server.
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="imageId"></param>
        /// <returns></returns>
        public async Task<(bool, string)> DeleteProjectImageByIdAsync(int projectId, int imageId)
        {
            if(projectId != 0 && imageId != 0)
            {
                MyProjectImage myProjectImage = await _dbContext.MyProjectImages.SingleOrDefaultAsync(x => x.Id == imageId && x.MyProject.Id == projectId);
                if(myProjectImage != null)
                {
                    var imgaePath = Path.Combine(_environment.WebRootPath, myProjectImage.Location);

                    if (File.Exists(imgaePath))
                    {
                        File.Delete(imgaePath);
                    }

                    _dbContext.MyProjectImages.Remove(myProjectImage);
                    await _dbContext.SaveChangesAsync();
                }

                return (false, "Image or Project not found");
            }

            return (false, "Image or Project not found");
        }

        public async Task<MyProject> GetByUrlWithImagesAsync(string url)
        {
            if(url != null)
            {
                string[] splitUrl = url.Split('-', 2);

                if(splitUrl.Length == 2)
                {
                    int id;

                    bool idParseResult = int.TryParse(splitUrl[0], out id);

                    if (idParseResult)
                    {

                        MyProject myProject = await _dbContext.MyProjects.SingleOrDefaultAsync(x => x.Id == id && string.Compare(splitUrl[1], x.Title) == 0);
                        if (myProject != null)
                        {
                            myProject.MyProjectImages = await _dbContext.MyProjectImages.Where(x => x.MyProject.Id == myProject.Id).ToListAsync();
                        }

                        return myProject;
                    }
                }    
            }

            return null;
        }

        #region Private

        /// <summary>
        /// Save images to server.
        /// </summary>
        /// <param name="formFiles"></param>
        /// <returns></returns>
        private async Task<List<MyProjectImage>> SaveImagesAsync(List<IFormFile> formFiles, MyProject myProject)
        {
            List<MyProjectImage> myProjectImages = new List<MyProjectImage>();

            if (formFiles.Count > 0)
            {
                string imagePath = Path.Combine("uploads", "images", "monthly_" + DateTime.Now.ToString("yyyy_MM"));

                string fullPath = Path.Combine(_environment.WebRootPath, imagePath);

                if (!Directory.Exists(fullPath))
                {
                    Directory.CreateDirectory(fullPath);
                }

                for (int i = 0; i < formFiles.Count; i++)
                {
                    string ext = formFiles[i].FileName.Substring(formFiles[i].FileName.LastIndexOf('.'));

                    // Only save files with allowed extensions.
                    if (permittedImageExtensions.Contains(ext.ToLower()))
                    {
                        string fileName = DateTimeOffset.Now.ToUnixTimeSeconds() + "_" + RandomString.GenerateRandomString() + ext;

                        var file = Path.Combine(fullPath, fileName);

                        using (var fileStream = new FileStream(file, FileMode.Create))
                        {
                            await formFiles[i].CopyToAsync(fileStream);
                        }

                        myProjectImages.Add(new MyProjectImage()
                        {
                            MyProject = myProject,
                            Time = DateTime.Now,
                            Location = Path.Combine(imagePath, fileName)
                        }); ;
                    }
                }
            }                    

            return myProjectImages;
        }

        #endregion
    }
}
