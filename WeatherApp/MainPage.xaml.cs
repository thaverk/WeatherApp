using Newtonsoft.Json;
using WeatherApp.Models;
using WeatherApp.Services;

namespace WeatherApp
{
    public partial class MainPage : ContentPage
    {
        private GPSModule _gpsmodule;
        private HttpClient _httpClient;

        private string _country;
        public string country { get { return _country; } set {_country=value; OnPropertyChanged(); } }

        private int _temp;
        public int Temp { get {return _temp; } set { _temp=value ;OnPropertyChanged(); } }

        private string _description;
        public string description { get {return _description; } set {_description=value;OnPropertyChanged(); } }

        private int _clouds;
        public int Clouds { get {return _clouds; } set { _clouds = value;OnPropertyChanged(); } }

        private double _speed;
        public double windspeed { get {return _speed; } set { _speed = value; OnPropertyChanged(); } }

        private int _rise;
        public int rise { get { return _rise; } set { _rise = value;OnPropertyChanged(); } }

        private int _set;
        public int set { get { return _set; } set { _set = value; OnPropertyChanged(); } }

        private double _pressure;
        public double pressure { get { return _pressure; } set { _pressure = value; OnPropertyChanged(); } }

        private double _humidity;
        public double humidity { get {return _humidity; } set { _humidity = value;OnPropertyChanged(); } }

        private DateAndTimeClass _dataAndTimeClass;
        public DateAndTimeClass dateAndTimeClass { get { return _dataAndTimeClass; } set { _dataAndTimeClass = value; OnPropertyChanged(); } }

        
        private long dateModified;
        public long DateModified
        {
            get => dateModified;
            set
            {
                if (dateModified.Equals(value)) return;
                dateModified = value;
                OnPropertyChanged();
            }
        }
       
        public MainPage()
        {
            InitializeComponent();
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            _gpsmodule = new GPSModule();
            GetLatestWeather();
            BindingContext = this;
        }

       

        public async void GetLatestWeather() 
        {
            DateAndTimeClass currenttime = new DateAndTimeClass();
            Location location = await _gpsmodule.GetCurrentLocation();
            double lat = location.Latitude;
            double lng = location.Longitude;

            string appid = "84e1ae5b22423295b04911bcbcb78422";
            string response = await _httpClient.GetStringAsync(new Uri($"https://api.openweathermap.org/data/2.5/weather?lat={lat}&lon={lng}&appid={appid}&units=metric"));

            WeatherData currentweather = JsonConvert.DeserializeObject<WeatherData>(response);

            if (currentweather != null ) 
            {
                Temp = (int)Math.Round(currentweather.main.temp);
                country =currentweather.sys.country;
                Clouds = currentweather.clouds.all;
                windspeed=currentweather.wind.speed;
                rise=currentweather.sys.sunrise;
                set = currentweather.sys.sunset;
                pressure=currentweather.main.pressure;
                humidity= currentweather.main.humidity;
                  
            }

            if (currentweather.weather.Count > 0)
            {
                description = currentweather.weather[0].description;
            }

        }
    }

}
