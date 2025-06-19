using System;
using System.Collections.Generic;
using System.Collections;
using System.Globalization;
using static System.Console;
using System.Linq;
using System.Text;
using System.Data;
using System.Xml.Linq;
using System.Security.Cryptography.X509Certificates;

var cars = new List<Car>
{
    new Sportcar("Lamborghini"),
    new Passenger("MClaren"),
    new Truck("Mercedes"),
    new Bus("Dodge")
};

var race = new Race_game(cars);

race.info += Car_info;
race.info += Check_winner;

WriteLine("Гонка начинается!\n");

race.start_race();

WriteLine("\nРезультаты заезда:");
Race_results(race);

WriteLine("\nКонец гонки!");

void Car_info(Car Car)
{
    WriteLine(($"Машина: {Car.name} {Car.distance} километров пройдено, скорость машины: {Car.speed} км/ч"));
}


void Check_winner(Car Car)
{
    if (Car.distance >= 100)
    {
        WriteLine($"\n {Car.name} финишировал!");
    }
}

void Race_results(Race_game Race)
{
    foreach (var new_car in Race.get_cars_by_distance())
    {
        WriteLine($"{new_car.name} {new_car.distance} км");
    }
}

public abstract class Car
{
    public string name { get; set; }
    public int speed { get; set; }
    public int distance { get; set; }
    public event Action<Car> finish_event;

    public Car(string name_p)
    {
        name = name_p;
        speed = 0;
        distance = 0;
    }

    public virtual void start_race()
    {
        speed = new Random().Next(5, 20);
    }

    public void move()
    {
        distance += speed;
        speed += new Random().Next(-3, 4);
        if (speed < 5) speed = 5;

        if (distance >= 100)
        {
            distance = 100;
            finish_event?.Invoke(this);
        }
    }
}

public class Sportcar : Car
{
    public Sportcar(string name) : base(name) { }

    public override void start_race()
    {
        speed = new Random().Next(10, 25);
    }
}

public class Passenger : Car
{
    public Passenger(string name) : base(name) { }

    public override void start_race()
    {
        speed = new Random().Next(5, 20);
    }
}

public class Truck : Car
{
    public Truck(string name) : base(name) { }

    public override void start_race()
    {
        speed = new Random().Next(1, 10);
    }
}

public class Bus : Car
{
    public Bus(string name) : base(name) { }

    public override void start_race()
    {
        speed = new Random().Next(3, 15);
    }
}

public class Race_game
{
    List<Car> cars;
    bool race_finished;

    public delegate void car_action(Car car);

    public event car_action info;

    public Race_game(List<Car> cars_s)
    {
        cars = cars_s;
        foreach (var car in cars_s)
        {
            car.finish_event += on_car_finished;
        }
    }

    public void start_race()
    {
        race_finished = false;
        WriteLine("На старт! Внимание! ПОЕХАЛИ!\n");

        foreach (var car in cars)
        {
            car.start_race();
        }

        while (!race_finished)
        {
            update_race();
        }
    }

    private void update_race()
    {
        foreach (var car in cars)
        {
            car.move();
            info?.Invoke(car);
        }
    }

    private void on_car_finished(Car car)
    {
        if (!race_finished)
        {
            race_finished = true;
        }
    }

    public IEnumerable<Car> get_cars_by_distance()
    {
        return cars.OrderByDescending(c => c.distance);
    }
}