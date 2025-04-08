using MoviesApp.Models;
using MoviesApp.Data;
using Microsoft.EntityFrameworkCore;
public class Program
{
    static MoviesContext context = new MoviesContext();
    public static void Main(String[] args)
    {
        while (true)
        {
            Console.WriteLine("==== Movie Booking System ====");
            Console.WriteLine("1. Login");
            Console.WriteLine("2. Register");
            Console.WriteLine("3. Exit");
            Console.Write("Select an option: ");
            int choice=Convert.ToInt32(Console.ReadLine());
            switch(choice)
            {
                case 1:
                    Login();
                    break;
                case 2:
                    Register();
                    break;
                case 3:
                    return;
                default:
                    System.Console.WriteLine("Invalid Option. Press Enter valid key from (1-3)");
                    break;
            }
        }
    }
    static void Register()
    {
        System.Console.WriteLine("==== Register New User");

        System.Console.Write("First Name: ");
        var firstName = Console.ReadLine();

        System.Console.Write("Last Name: ");
        var lastName = Console.ReadLine();

        System.Console.Write("Username: ");
        var username = Console.ReadLine();

        System.Console.Write("Password: ");
        var password = Console.ReadLine();

        System.Console.Write("Date of Birth (yyyy-MM-dd): ");
        DateTime dob = DateTime.Parse(Console.ReadLine());

        System.Console.Write("Email: ");
        var email = Console.ReadLine();

        System.Console.Write("Phone: ");
        var phone = Console.ReadLine();

        var newUser = new User
        {
            FirstName = firstName,
            LastName = lastName,
            UserName = username,
            Password = password,
            DateOfBirth = DateOnly.FromDateTime(dob),
            Email = email,
            Phone = phone
        };
        context.Users.Add(newUser);
        context.SaveChanges();

        System.Console.WriteLine("Registered successfully! Press Enter to continue...");
    }
    static void Login()
    {
        System.Console.WriteLine("== Login ==");

        System.Console.Write("Username: ");
        var username = Console.ReadLine();

        System.Console.Write("Password: ");
        var password = Console.ReadLine();

        var user = context.Users.FirstOrDefault(user => user.UserName == username && user.Password == password);
        if (user == null || user.UserName != username || user.Password != password)
        {
            Console.WriteLine("Invalid credentials. Press Enter to continue...");
        }
        System.Console.WriteLine($"Welcome: {user.UserName}");
        System.Console.WriteLine("---------------------");
        if (user.Role == "Admin")
        {
            AdminDashboard();
        }
        else
        {
            UserDashboard(user);
        }
    }
    static void AdminDashboard()
    {
        while (true)
        {
            System.Console.WriteLine("=== Admin Dashboard ===");
            System.Console.WriteLine("1. Add Movies");
            System.Console.WriteLine("2. View Movies");
            System.Console.WriteLine("3. Number of users");
            System.Console.WriteLine("4. Remove movies");
            System.Console.WriteLine("5. Logout");
            System.Console.Write("Select an option: ");
            int choice=Convert.ToInt32(Console.ReadLine());
            switch (choice)
            {
                case 1:
                    AdMovies();
                    break;
                
                case 2:
                    ViewMovies();
                    
                    break;
                case 3:
                    RegisteredUsers();
                    break;
                case 4:
                    RemoveMovies();
                    break;
                case 5:
                    return;
                default:
                    System.Console.WriteLine("Invalid option. Press Enter...");
                    break;
            }
        }
        System.Console.WriteLine("--------------");
    }
    static void AdMovies()
    {
        System.Console.WriteLine("Enter full path of the CSV file: ");
        var filepath = Console.ReadLine();
        if (!File.Exists(filepath))
        {
            System.Console.WriteLine("File not Found!!!");
            return;
        }

        var lines = File.ReadAllLines(filepath);

        for (int i = 1; i < lines.Length; i++)
        {
            string[] col = lines[i].Split(',');

            if (col.Length != 7)
            {
                System.Console.WriteLine($"Invalid: {lines}");
                continue;
            }
                var movies = new Movie
                {
                    MovieName = col[0].Trim(),
                    Genre = col[1].Trim(),
                    Duration = col[2].Trim(),
                    AgeRestriction = int.Parse(col[3]),
                    ShowTime = col[4].Trim(),
                    NumberOfTickets=int.Parse(col[5]),
                    Price=decimal.Parse(col[6])
                };
                context.Movies.Add(movies);    
        }
        context.SaveChanges();
        System.Console.WriteLine("Movies add successfully!");
    }
    static void ViewMovies(){
        System.Console.WriteLine("Movies that are added:");
        var movies=context.Movies.ToList();

        foreach(var movie in movies){
            System.Console.WriteLine($"Movie ID: {movie.MovieId}\nMovie Name: {movie.MovieName}\nGenre: {movie.Genre}");
            System.Console.WriteLine("--------------------------");
        }
    }
     static void RegisteredUsers()
    {
        var users = context.Users
        .Where(user => user.Role == "User")
        .Select(user => new
        {
            user.UserId,
            user.UserName,
            user.Email,
            user.Phone,
            user.DateOfBirth
        }).ToList();

        System.Console.WriteLine("====Registered Users====");
        foreach (var user in users)
        {
            System.Console.WriteLine($"user ID:{user.UserId}\nUserName: {user.UserName}\nEmail: {user.Email}\n Phone: {user.Phone}\n DateOfBirth: {user.DateOfBirth.ToShortDateString()}\n {new string('-', 40)}");
        }
        System.Console.WriteLine($"Total Users: {users.Count}");
    }
    static void RemoveMovies()
    {
        
        System.Console.WriteLine("==== Remove Movie ====");
        ViewMovies();
        System.Console.Write("Enter Movie ID from above movies list to remove : ");
    
        var movieIds = Console.ReadLine().Split(',')
                            .Select(id => id.Trim())
                            .ToList();

        foreach (var id in movieIds)
        {
            try
            {
                int movieId = Convert.ToInt32(id);

                var movieToRemove = context.Movies.FirstOrDefault(m => m.MovieId == movieId);

                if (movieToRemove != null)
                {
                    var bookingsToRemove = context.Bookings.Where(b => b.MovieId == movieId).ToList();
                    foreach (var booking in bookingsToRemove)
                    {
                        context.Bookings.Remove(booking);
                    }
                    context.Movies.Remove(movieToRemove);
                    System.Console.WriteLine($"Movie with ID {movieId} have been removed successfully.");
                }
                else
                {
                    System.Console.WriteLine($"Movie with ID {movieId} not found.");
                }
            }
            catch (Exception e)
            {
                System.Console.WriteLine($"Invalid Movie ID: {e.Message}.");
            }
        }

        context.SaveChanges();
        System.Console.WriteLine("Movies removed successfully!.");
    }
    static void UserDashboard(User user)
    {
        bool hasBooking = context.Bookings.Any(b => b.UserId == user.UserId);
        while (true)
        {
          
            if (hasBooking)
            {
                System.Console.WriteLine("1. Book Movie");
                System.Console.WriteLine("2. Booking History");
                System.Console.WriteLine("3. Logout");
            }
            else
            {
                System.Console.WriteLine("1. Book Movie");
                System.Console.WriteLine("2. Logout");
            }
            System.Console.WriteLine("Enter your choice");
            int choice=Convert.ToInt32(Console.ReadLine());
            switch (choice)
            {
                case 1:
                    BookMovie(user);
                    break;
                case 2:
                    if (hasBooking)
                    {
                        BookingHistory(user);
                    }
                    else
                    {
                        return;
                    }
                    break;
                case 3:
                    if (hasBooking)
                        return;
                    break;
                default:
                    System.Console.WriteLine("Invalid choice. Please valid one");
                    break;
            }
        }
    }
    static void BookMovie(User user)
    {
        System.Console.WriteLine("==== Book a Movie ====");

        var today = DateOnly.FromDateTime(DateTime.Today);
        int age = today.Year - user.DateOfBirth.Year;
        if (user.DateOfBirth > today.AddYears(-age))
        {
            age--;
        }

        var availableMovies = context.Movies
            .Where(m => m.AgeRestriction <= age && m.NumberOfTickets > 0)
            .ToList();

        if (!availableMovies.Any())
        {
            System.Console.WriteLine("No movies available for your age.");
            return;
        }

        System.Console.WriteLine("\nAvailable Movies:");
        foreach (var movie in availableMovies)
        {
            System.Console.WriteLine($"Movie ID: {movie.MovieId}\n Name: {movie.MovieName}\n Genre: {movie.Genre}\n Duration: {movie.Duration}\n Tickets: {movie.NumberOfTickets}\n Price: ₹{movie.Price}");
            System.Console.WriteLine("---------------------");
        }

        System.Console.Write("\nEnter Movie ID to book: ");
        if (!int.TryParse(Console.ReadLine(), out int movieId))
        {
            System.Console.WriteLine("Invalid input.");
            return;
        }

        var selectedMovie = availableMovies.FirstOrDefault(m => m.MovieId == movieId);
        if (selectedMovie == null)
        {
            System.Console.WriteLine("Movie not found.");
            return;
        }

        var showTimeList = selectedMovie.ShowTime.Split(';').Select(s => s.Trim()).ToList();
        if (!showTimeList.Any())
        {
            System.Console.WriteLine("No showtimes available for this movie.");
            return;
        }

        System.Console.WriteLine("\nAvailable Showtimes:");
        for (int i = 0; i < showTimeList.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {showTimeList[i]}");
            
        }
        System.Console.Write("Select showtime by number: ");
        if (!int.TryParse(Console.ReadLine(), out int showIndex) || showIndex < 1 || showIndex > showTimeList.Count)
        {
            Console.WriteLine("Invalid showtime selection.");
            return;
        }

        string selectedShowTime = showTimeList[showIndex - 1];
        System.Console.Write("Enter number of tickets: ");
        if (!int.TryParse(Console.ReadLine(), out int ticketCount) || ticketCount <= 0)
        {
            System.Console.WriteLine("Invalid ticket number.");
            return;
        }

        if (ticketCount > selectedMovie.NumberOfTickets)
        {
            System.Console.WriteLine("Not enough tickets available.");
            return;
        }
        decimal totalPrice = ticketCount * selectedMovie.Price;
        var booking = new Booking
        {
            UserId = user.UserId,
            MovieId = selectedMovie.MovieId,
            BookingTime = DateTime.Now,
            ShowTime=selectedShowTime,
            Totalprice = totalPrice
        };

        context.Bookings.Add(booking);
        selectedMovie.NumberOfTickets -= ticketCount;
        context.Movies.Update(selectedMovie);
        context.SaveChanges();

        System.Console.WriteLine($"\nBooking successful!");
        System.Console.WriteLine($"Movie: {selectedMovie.MovieName}\nShowtime: {selectedShowTime}\nTickets: {ticketCount}\nTotal Price: {totalPrice}");

    }
    static void BookingHistory(User user)
    {
        System.Console.WriteLine("== Your Booking History ==");

        var bookings = context.Bookings
        .Include(b=>b.Movie)
            .Where(book => book.UserId == user.UserId)
            .ToList();

        if (!bookings.Any())
        {
            System.Console.WriteLine("No bookings found.");
            return;
        }

        foreach (var booking in bookings)
        {
            if(booking.Movie!=null){
                System.Console.WriteLine("Tickets Booked successfully");
                System.Console.WriteLine("---------------------------------");
                System.Console.WriteLine($"Booking ID: {booking.BookingId}");
                System.Console.WriteLine($"Movie Name: {booking.Movie.MovieName}");
                System.Console.WriteLine($"Show Time: {booking.ShowTime}");
                System.Console.WriteLine($"Booked Time: {booking.BookingTime}");
                System.Console.WriteLine($"totalPrice: {booking.Totalprice}");
                System.Console.WriteLine("====================================");
            }
            else{
                System.Console.WriteLine($"Booking ID {booking.BookingId} has been removed by admin!.");

            }
        }
    }   
}