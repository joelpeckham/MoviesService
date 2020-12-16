using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace MoviesService2020
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class MovieService : IMovieService
    {
        private Movies2020Entities context = new Movies2020Entities();
        int IMovieService.CreateMovie(Movie movie)
        {
            context.Movies1.Add(movie);
            context.SaveChanges();
            return movie.Id;
        }

        bool IMovieService.DeleteMovie(int ID)
        {
            try
            {
                context.Movies1.Remove(context.Movies1.Where(x => x.Id == ID).FirstOrDefault());
                context.SaveChanges();
            }
            catch (Exception) {
                return false;
            }
            return true;
        }

        Movie IMovieService.GetMovie(int ID)
        {
            return context.Movies1.Where(x => x.Id == ID).FirstOrDefault();
        }

        List<Movie> IMovieService.GetMovies()
        {
            return context.Movies1.ToList();
        }

        bool IMovieService.UpdateMovie(Movie movie)
        {
            try
            {
                context.Movies1.AddOrUpdate(movie);
                context.SaveChanges();
            }
            catch (Exception) {
                return false;
            }
            return true;
        }
    }
}
