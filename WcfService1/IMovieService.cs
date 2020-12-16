using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace MoviesService2020
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IMovieService
    {
        [OperationContract]
        int CreateMovie(Movie movie);
        [OperationContract]
        List<Movie> GetMovies();
        [OperationContract]
        Movie GetMovie(int ID);
        [OperationContract]
        bool UpdateMovie(Movie movie);
        [OperationContract]
        bool DeleteMovie(int ID);
    }
}
