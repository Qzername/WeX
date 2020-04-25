﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Database
{
    public class Config
    {
        public string token;
        public string api;
    }

    public class EightBall
    {
        public int id;
        public string text;
    }

    public class Cat
    {
        public List<object> breeds;
        public List<object> categories;
        public string id;
        public string url;
        public int width;
        public int height;
    }

    public class Dog
    {
        public string message;
        public object status;
    }

    public class Bird
    {
        public string link;
    }

    public class Food
    {
        public string image;
    }

    public class MALUSer
    {
        public string username;
        public string image_url;
        public AnimeList anime_stats;
    }

    public class AnimeList
    {
        public double days_watched;
        public double mean_score;
        public int watching;
        public int completed;
        public int dropped;
        public int total_entries;
    }
}