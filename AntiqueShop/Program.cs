using System;
using System.Collections.Generic;
using System.ComponentModel.Design;

namespace AntiqueShop {
    public class Book {
        public int price;
        public bool evaluated = false;
        private string name;
        private int rarity; //Value between 0 - 5
        private string category;
        private int actualValue; //Value between 0 - 100
        private bool cursed;
        private Random generator = new Random();

        private static readonly string[] names = {
            "sample name 1",
        };
        private static readonly string[] categories = {
            "sample category 1",
        };

        public Book() {
            name = names[generator.Next(names.Length)]; //Get random name
            category = categories[generator.Next(categories.Length)]; //Get random category
            rarity = generator.Next(5);
            actualValue = generator.Next(100);
            cursed = generator.Next(0, 1) == 1 ? true : false;
        }

        public int Evaluate() {
            int correctPrice = actualValue * rarity;
            correctPrice *= generator.Next(50, 150);
            correctPrice = correctPrice / 100;
            price = correctPrice; //Apply the newly estimated price

            evaluated = true;
            return correctPrice;
        }

        public string GetCategory => category;
        public bool IsCursed() => generator.Next(10) > 2 ? cursed : !cursed;

        public override string ToString() {
            return $"Name: {name}, Rarity: {rarity}, Category: {category}, Price: {(evaluated ? price.ToString() : "?")}kr";
        }
    }
    public class Customer {
        public Book bookToSell = new Book();
        public int sellPrice { get; private set; }
        private Random generator = new Random();

        public Customer() {
            sellPrice = generator.Next(60, 200);
        }

        public int Bargain() {
            int modifiedPrice = sellPrice;
            modifiedPrice *= generator.Next(50, 150);
            modifiedPrice = modifiedPrice / 100;
            sellPrice = modifiedPrice;
            return sellPrice;
        }
    }
    class Program {
        static void Main(string[] args) {
            List<Customer> customers = new List<Customer> {
                new Customer(),
                new Customer(),
                new Customer(),
                new Customer(),
                new Customer()
            };

            bool notDead = true;
            bool bargaining = true;
            int money = 400;

            while (notDead && customers.Count > 0) {
                Console.WriteLine($"Customers left: {customers.Count}");
                Console.WriteLine($"Your money: {money}kr");

                Console.WriteLine(customers[^1].bookToSell.ToString());
                Console.WriteLine($"Customer wants {customers[^1].sellPrice}kr for the book");

                bargaining = true;
                
                while (bargaining) {
                    Console.WriteLine($"{(customers[^1].bookToSell.evaluated ? "" : "1: Evaluate,")}\n2: Reject offer,\n3: Purchase,\n4: Bargain");
                    while (true) {
                        if (Console.KeyAvailable) {
                            ConsoleKeyInfo key = Console.ReadKey(true);

                            if (key.Key == ConsoleKey.D1) {
                                if (!customers[^1].bookToSell.evaluated) {
                                    Console.WriteLine($"You think the book costs: {customers[^1].bookToSell.Evaluate()}kr");
                                }
                                break;
                            }
                            else if (key.Key == ConsoleKey.D2) {
                                Console.WriteLine("You rejected the customer's offer.");
                                bargaining = false;
                                break;
                            }
                            else if (key.Key == ConsoleKey.D3) {
                                if (money >= customers[^1].sellPrice) {
                                    Console.WriteLine($"You purchased the customer's book for: {customers[^1].sellPrice}");
                                    bargaining = false;
                                    money -= customers[^1].sellPrice;
                                } else {
                                    Console.WriteLine("You don't have enough money!");
                                    notDead = false;
                                }
                                break;
                            }
                            else if (key.Key == ConsoleKey.D4) {
                                int oldPrice = customers[^1].sellPrice;
                                customers[^1].Bargain();
                                Console.WriteLine($"The customer's new price is: {customers[^1].sellPrice}, was; {oldPrice}. ({oldPrice - customers[^1].sellPrice})");
                                break;
                            }
                        }
                    }
                }



                Console.ReadLine();
                Console.Clear();
                customers.RemoveAt(customers.Count - 1);
            }
        }
    }
}
