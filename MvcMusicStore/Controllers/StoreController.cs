﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcMusicStore.Models;

namespace MvcMusicStore.Controllers {
    public class StoreController : Controller {
        MusicStoreEntities storeDB = new MusicStoreEntities();
        //
        // GET: /Store/

        public ActionResult Details(int id) 
        {
            var album = storeDB.Albums.Find(id);
            if (album == null) {
                return HttpNotFound();
            }

            return View(album);
        }

        public ActionResult Browse(string genre) {
            // Retrieve Genre and its Associated Albums from database
            var genreModel = storeDB.Genres.Include("Albums")
                .Single(g => g.Name == genre);

            return View(genreModel);
        }

        public ActionResult Index() 
        {
            var genres = storeDB.Genres;
            return View(genres);
        }

        [ChildActionOnly]
        public ActionResult GenreMenu()
        {
            var genres = storeDB.Genres
                .OrderByDescending(
                    g => g.Albums.Sum(
                    a => a.OrderDetails.Sum(
                    od => od.Quantity)))
                .Take(9)
                .ToList();

            return PartialView(genres);
        }

    }
}
