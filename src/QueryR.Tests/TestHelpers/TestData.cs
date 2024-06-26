﻿using System.Collections.Generic;
using System.Linq;

namespace QueryR.Tests.TestHelpers
{
    internal class TestData
    {
        public class Person
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public int Age { get; set; }
            public List<Pet> Pets { get; set; }
            public List<string> AltNames { get; set; }
        }
        public class Pet
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public int OwnerId { get; set; }
            public Person Owner { get; set; }
            public int PetTypeId { get; set; }
            public PetType PetType { get; set; }
            public List<string> AltNames { get; set; }
        }
        public class PetType
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        public PetType Cat { get; init; }
        public PetType Dog { get; init; }
        public PetType Bird { get; init; }
        public List<PetType> PetTypes { get; init; }

        public Person Craig { get; init; }
        public Person Bob { get; init; }
        public List<Person> Persons { get; init; }

        public Pet Titan { get; init; }
        public Pet Rufus { get; init; }
        public Pet Meowswers { get; init; }
        public Pet Kitty { get; init; }
        public Pet Tweeter { get; init; }
        public List<Pet> Pets { get; init; }

        public TestData()
        {
            Cat = new PetType { Id = 1, Name = "Cat" };
            Dog = new PetType { Id = 2, Name = "Dog" };
            Bird = new PetType { Id = 3, Name = "Bird" };

            Craig = new Person { Id = 1, Name = "Craig", Age = 20, AltNames = ["Greg"] };
            Bob = new Person { Id = 2, Name = "Bob", Age = 25, AltNames = ["Robert", "Robbie"] };

            Titan = new Pet { Id = 1, Name = "Titan", OwnerId = 1, PetTypeId = 2, AltNames = ["Mr Slobber", "Stinky"] };
            Rufus = new Pet { Id = 2, Name = "Rufus", OwnerId = 2, PetTypeId = 2, AltNames = ["Stinky", "Sir Barks-a-lot"] };
            Meowswers = new Pet { Id = 3, Name = "Meowswers", OwnerId = 1, PetTypeId = 1, AltNames = ["BAD CAT!"] };
            Kitty = new Pet { Id = 4, Name = "Kitty", OwnerId = 2, PetTypeId = 1, AltNames = ["Puddin'"] };
            Tweeter = new Pet { Id = 5, Name = "Tweeter", OwnerId = 1, PetTypeId = 3, AltNames = ["Chirp Chirp"] };

            PetTypes = new List<PetType> { Cat, Dog, Bird };
            Persons = new List<Person> { Craig, Bob };
            Pets = new List<Pet> { Titan, Rufus, Meowswers, Kitty, Tweeter };

            foreach (var person in Persons)
            {
                person.Pets = Pets.Where(p => p.OwnerId == person.Id).ToList();
            }

            foreach (var pet in Pets)
            {
                pet.Owner = Persons.First(p => p.Id == pet.OwnerId);
                pet.PetType = PetTypes.First(p => p.Id == pet.PetTypeId);
            }
        }
    }
}
