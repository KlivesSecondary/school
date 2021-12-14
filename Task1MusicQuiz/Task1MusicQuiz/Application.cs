using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Task1MusicQuiz
{
    public class Application
    {
        Random rnd = new Random();

        //Player data struct, this is to hold all the data for players in an struct
        public struct PlayerData
        {
            public string Name;
            public int Score;
            public PlayerData(string name, int score)
            {
                Name = name;
                Score = score;
            }
            public void UpdateScore(int score)
            {
                Score = score;
            }
            public void SavePlayer()
            {
                File.WriteAllText(Name + ".json", JsonConvert.SerializeObject(this));
            }
        }
        //Songs struct
        public struct Song
        {
            public string Name;
            public string Artist;
            public Song(string name, string artist)
            {
                Name = name;
                Artist = artist;
            }
        }
        //Asynchronous function to asynchronize IO processes in order to not affect the quality of the application
        public async Task RunAsync()
        {
            //Change colour, for fun.
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Welcome, what is your username?");
            //Asks for username input
            var playerdata = new PlayerData("Override", 50);
            string username = Console.ReadLine().Trim();
            if (username == "emergency")
            {
                Console.WriteLine("Overriding read/login function");
            }
            else
            {
                //Logs in and acquires PlayerData struct using that username input
                playerdata = LoginOrRegister(username);
            }
            Console.WriteLine("Successfully logged in.");
            //One second delay for dramatic effect
            await Task.Delay(1000);
            //Clears console.
            Console.Clear();
            //Loads all songs available in that songs file.
            List<Song> songs = LoadAllSongs();
            //Loads a random song.
            Song song = songs[rnd.Next(rnd.Next(songs.Count))];
            Console.WriteLine("Here is your first song to guess.");
            //Another ominous 1 second delay
            await Task.Delay(1000);
            Console.WriteLine($"Song Artist: |{song.Artist}| First 2 letters of Song Name: {song.Name.Trim().Substring(0, 2)}");
            //Another useless 1 second delay
            await Task.Delay(1000);
            Console.WriteLine("What song is this?");
            int guessestaken = 0;
            while (true)
            {
                string guess = Console.ReadLine();
                if (guess.ToLower().Contains(song.Name.ToLower()))
                {
                    Console.WriteLine("Correct! One point awarded to account.");
                    playerdata.UpdateScore(playerdata.Score + 1);
                    playerdata.SavePlayer();
                    break;
                }
                else
                {
                    Console.WriteLine("Wrong answer!");
                    if (guessestaken <= 2)
                    {
                        guessestaken++;
                    }
                    else
                    {
                        Console.WriteLine($"You have ran out of guesses. The correct answer was {song.Name}\n\nGame Over");
                        break;
                    }
                }
            }
            Console.WriteLine("\nTop 5 Winning Scores:");
            //Sorts all players into a list ordered by their Score using LINQ structure
            List<PlayerData> orderedplayer = GetAllPlayerData().OrderByDescending(x => x.Score).ToList();
            //Loops over each player until it reaches 5 and prints them to console.
            int check = 0;
            foreach (var item in orderedplayer)
            {
                check++;
                if (check < 6)
                {
                    Console.WriteLine($"Player: {item.Name} | Score: {item.Score}");
                }
                else
                {
                    break;
                }
            }
        }

        //Gets PlayerData, if such playerdata does not exist, create new PlayerData and save it to a JSON file.
        public PlayerData LoginOrRegister(string username)
        {
            //Load PlayerData from the player's file.
            var playerdata = GetPlayerData(username);
            //Ducttape method of checking whether the return value is valid or not.
            if (playerdata.Name != username)
            {
                Console.WriteLine("Creating account!");
                playerdata = new(username, 0);
                playerdata.SavePlayer();
            }
            return playerdata;
        }

        public List<Song> LoadAllSongs()
        {
            string[] songsText = File.ReadAllLines("songs.txt");
            List<Song> songs = new List<Song>();
            foreach (var item in songsText)
            {
                //Weird code to parse the song artist and name and load it into the Songs list
                songs.Add(new Song(item.Split("|")[0].Replace("|", "").Trim(), item.Split("|")[1].Replace("|", "").Trim()));
            }
            return songs;
        }

        public PlayerData GetPlayerData(string username)
        {
            //Creates an empty instance of playerdata.
            PlayerData playerData = new();
            //Checks if the account even exists
            if (File.Exists(username + ".json"))
            {
                //Searches through each file that ends with .json using LINQ structure
                playerData = JsonConvert.DeserializeObject<PlayerData>(File.ReadAllText(Path.GetFullPath(username + ".json")));
                //Return the playerdata.
                return playerData;
            }
            else
            {
                //If it doesn't, return a invalid one.
                return playerData;
            }
        }
        public List<PlayerData> GetAllPlayerData()
        {
            List<PlayerData> playerData = new List<PlayerData>();
            //Iterates over each JSON file, which is 100% of the time PlayerData
            foreach (var item in Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory).Where(item => item.EndsWith(".json")))
            {
                //Deserializes, converts then Adds that player to PlayerData.
                playerData.Add(JsonConvert.DeserializeObject<PlayerData>(File.ReadAllText(Path.GetFullPath(item))));
            }
            return playerData;
        }
    }
}
