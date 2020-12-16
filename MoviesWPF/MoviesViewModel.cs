using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.ServiceModel;
using System.ServiceModel.Security;
using System.Text;
using System.Windows;
using MoviesService;


namespace MoviesWPF
{
    public class MoviesViewModel : INotifyPropertyChanged

    {
        ObservableCollection<Movie> _movies;
        private Movie _current;
        //private string _Username;
        //private DelegateComamnd _loginCmd;
        private DelegateCommand _deleteSelectedCmd;
        private DelegateCommand _newMovieCmd;
        private DelegateCommand _updateCurrentCmd;
        private MovieServiceClient proxy;

        public Movie Current
        {
            get { return _current; }
            set { _current = value; NotifyPropertyChanged(); }
        }

        public DelegateCommand DeleteSelectedCmd
        {
            get { return _deleteSelectedCmd; }
            set { _deleteSelectedCmd = value; NotifyPropertyChanged(); }
        }

        public DelegateCommand NewMovieCmd
        {
            get { return _newMovieCmd; }
            set { _newMovieCmd = value; NotifyPropertyChanged(); }
        }

        public DelegateCommand UpdateCurrentCmd
        {
            get { return _updateCurrentCmd; }
            set { _updateCurrentCmd = value; NotifyPropertyChanged(); }
        }

        public ObservableCollection<Movie> Movies {
            get { return _movies; }
            set {  _movies = value; NotifyPropertyChanged(); }
        }

        public MoviesViewModel()
        {
            Movies = new ObservableCollection<Movie>();
            DeleteSelectedCmd = new DelegateCommand(x => true, DeleteSelected);
            NewMovieCmd = new DelegateCommand(x => true, CreateMovie);
            UpdateCurrentCmd = new DelegateCommand(x => true, UpdateCurrentMovie);
            proxy = new MovieServiceClient();
            proxy.ClientCredentials.ServiceCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.None;
            LoadMovie(null);
        }

        private async void CreateMovie(object o)
        {
            NewMovie newMovie = new NewMovie();
            var result = newMovie.ShowDialog();
            string title = newMovie.tbTitle.Text;
            string genre = newMovie.tbGenre.Text;
            DateTime? releaseDate = newMovie.dpReleaseDate.SelectedDate;
            Movie movie = new Movie()
            {
                Id = -1,
                Title = title,
                Genre = genre,
                ReleaseDate = releaseDate == null ? DateTime.Now : releaseDate.Value
    
            };
            int id = await proxy.CreateMovieAsync(movie);
            LoadMovie(id);
        }
        private async void DeleteSelected(object o)
        {
            var Movie = Current;
            Movies.Remove(Current);
            await proxy.DeleteMovieAsync(Movie.Id);
        }
        private async void UpdateCurrentMovie(object o)
        {
            var Movie = Current;
            await proxy.UpdateMovieAsync(Movie);
            LoadMovie(Movie.Id);
        }

        private void SetSelectedItemById(int id)
        {
            for (int i = 0; i < Movies.Count; i++) {
                if(Movies[i].Id == id)
                {
                    Current = Movies[i];
                    break;
                }
            }
        }

        private async void LoadMovie(object o)
        {
            try
            {
               
                //await proxy.OpenAsync();
                var movies = await proxy.GetMoviesAsync();
                Movies.Clear();
                foreach (var movie in movies)
                {
                    Movies.Add(movie);
                }
                if (o != null && o.GetType() == typeof(int))
                {
                    SetSelectedItemById((int)o);
                }
                else
                {
                    Current = Movies[0];
                }
            }
            catch (Exception ex )
            {
                MessageBox.Show("error:" + ex.Message);
            }
        }

        #region INPC
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        { 
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
    }
}
