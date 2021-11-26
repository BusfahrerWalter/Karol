# C Gartenzaun Karol

## Dokumentation
- [Karol](api/Karol.yml)
- [Karol.Core](api/Karol.Core.yml)

Karol f&uuml;r C# <br>
Roboter k&ouml;nnen in einer Welt tolle sachen machen...

## Screenshots
#### Welt mit 2 Robotern
![Ups.. hier sollte ein bild sein!](images/img1.png)

#### Ein Labyrinth
![Ups.. hier sollte ein bild sein!](images/img2.png)

#### 2D Ansicht
![Ups.. hier sollte ein bild sein!](images/img3.png)

#### Verschiedene Farben
![Ups.. hier sollte ein bild sein!](images/img4.png)

## Beispiel Code
#### Erstellen einer Welt mit Roboter
```C#
World world = new World(10, 5, 10);
Robot adam = new Robot(3, 2, world, Direction.South);
```

#### Roboter baut eine Wand
```C#
World world = new World(10, 5, 10);
Robot sandler = new Robot(0, 0, world);

sandler.Delay = 10; // Damit es nich so lang dauert...

while (sandler.Position.Y < world.Height - 1)
{
    if (sandler.HasWall)
    {
        sandler.TurnRight();
    }

    sandler.Place();
    sandler.Move();
}
```

#### Mehr oder weniger Intelligentes l&ouml;sen eines Labyrinths
```C#
World world = World.Load("Pfad zu Welt Datei"); // Welt Dateiformat .kdw oder .cskw
Robot egon34 = world.GetRobot(0); // Erster Roboter in der Welt
Random rand = new Random();       // Zufallszahlen generator

egon34.Delay = 0; // Damit es nich so lang dauert...

while (!egon34.HasMark)
{
    if (!egon34.HasWall)
    {
        egon34.Move();
    }

    int num = rand.Next(100);
    if (num <= 30)
    {
        egon34.TurnLeft();
    }
    else if (num >= 70)
    {
        egon34.TurnRight();
    }
}
```

#### Markieren des gesammten Bodens
```C#
World world = new World(15, 5, 10);     // Welt erzeugen
Robot robo = new Robot(0, 0, world);    // Roboter an der Position 0, 0, 0 in "world" erzeugen

robo.Delay = 30;    // Damit es nich so lang dauert...

for(int i = 0; i < world.Width; i++)
{
    while (!robo.HasWall)
    {
        robo.PlaceMark();
        robo.Move();
    }

    if (i == world.Width - 1)
        break;

    robo.PlaceMark();

    if (i % 2 == 0)
    {
        robo.TurnRight();
        robo.Move();
        robo.TurnRight();
    }
    else
    {
        robo.TurnLeft();
        robo.Move();
        robo.TurnLeft();
    }
}

robo.PlaceMark();
```

#### Welten k&ouml;nnen auch aus strings geladen werden!
```C#
string str = "C_Gartenzaun_Karol_World\n" +
             "Size: 3,2,3\n" +
             "---  \n" +
             "_ _ R(2)\n" +
             "Q Q _\n" +
             "_ Q Q\n";

MemoryStream stream = new MemoryStream(Encoding.ASCII.GetBytes(str));
World world = World.Load(stream);
```

ergibt folgende Welt...
<br>
![Ups.. hier sollte ein bild sein!](images/img5.png)
