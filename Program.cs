using System.Data;
using Npgsql;
using Sozezdiya;

const int width = 50;
const int height = 26;

var connection = new NpgsqlConnection(
    @"server=dpg-cedr1b9a6gdgn5f892hg-a.frankfurt-postgres.render.com;
    userid=sweeper;
    password=5uHR6lvAAaVA1VnK83vEeQzaszMt6CLR;
    database=scrapyard"
);

connection.Open();

Start:

var reader = new NpgsqlCommand("SELECT * FROM constellations ORDER BY id;", connection).ExecuteReader();
var constellations = new List<Constellation>();

while (reader.Read())
{
    var constellation = new Constellation();
    constellation.Id = reader.GetInt32(0);
    constellation.Name = reader.GetString(1);
    constellation.Description = reader.GetString(2);
    
    constellations.Add(constellation);
}

reader.Close();

Console.WriteLine("Созвездия:");

for (int i = 0; i < constellations.Count; i++)
{
    Console.WriteLine($"{constellations.ElementAt(i).Id}. {constellations.ElementAt(i).Name}");
}

Console.Write("ИД Созвездия: ");

var id = int.Parse(Console.ReadLine()!);

var constellation1 = constellations.Find(a => a.Id == id);
var reader1 = new NpgsqlCommand($"SELECT * FROM stars WHERE cid = {id} ORDER BY id", connection).ExecuteReader();
var stars = new List<Star>();

while (reader1.Read())
{
    var star = new Star();
    star.Id = reader1.GetInt32(0);
    star.Cid = reader1.GetInt32(1);
    star.X = reader1.GetInt32(2);
    star.Y = reader1.GetInt32(3);
    star.Name = reader1.GetString(4);
    star.Description = reader1.GetString(5);
    star.Size = reader1.GetInt32(6);
    
    stars.Add(star);
}

reader1.Close();

Console.Clear();

for (var h = 0; h < height; h++)
{
    for (var w = 0; w < width; w++)
    {
        if (h is 0 or (height - 1))
            switch (h)
            {
                case 0 when w is 0:
                    Console.Write("#");
                    break;
                case 0 when w is 49:
                    Console.Write("#");
                    break;
                default:
                {
                    switch (w)
                    {
                        case 49 when h is 25:
                            Console.Write("#");
                            break;
                        case 0 when h is 25:
                            Console.Write("#");
                            break;
                        default:
                            Console.Write("=");
                            break;
                    }

                    break;
                }
            }
        
        else if (w is 0 or (width - 1)) Console.Write("|");
        else Console.Write(" ");
    }
    
    Console.Write("\n");
}

for (int i = 0; i < stars.Count; i++)
{
    var star = stars.ElementAt(i);
    Console.SetCursorPosition(star.X, star.Y);
    Console.WriteLine((star.Size == 1) ? "x" : "*");
}

Console.SetCursorPosition(0, 26);

Console.WriteLine($"Созвездие: {constellation1.Name}");
Console.WriteLine($"\tОписание: {constellation1.Description}");

for (int i = 0; i < stars.Count; i++)
{
    Console.WriteLine($"Звезда: {stars.ElementAt(i).Name}");
    Console.WriteLine($"\tОписание: {stars.ElementAt(i).Description}");
}

Console.Write("\nНажмите ENTER чтобы продолжить");
Console.Read();
Console.Clear();

goto Start;