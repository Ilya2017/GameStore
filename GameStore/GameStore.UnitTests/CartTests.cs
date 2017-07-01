﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameStore.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using GameStore.WebUI.Models;
using GameStore.WebUI.Controllers;
using System.Web.Mvc;
using GameStore.Domain.Abstract;
using Moq;

namespace GameStore.UnitTests
{
	[TestClass]
	public class CartTests
	{
		[TestMethod]
		public void Can_Add_New_Lines()
		{
			// Организация - создание нескольких тестовых игр
			Game game1 = new Game { GameId = 1, Name = "Игра1" };
			Game game2 = new Game { GameId = 2, Name = "Игра2" };

			// Организация - создание корзины
			Cart cart = new Cart();

			// Действие
			cart.AddItem(game1, 1);
			cart.AddItem(game2, 1);
			List<CartLine> results = cart.Lines.ToList();

			// Утверждение
			Assert.AreEqual(results.Count(), 2);
			Assert.AreEqual(results[0].Game, game1);
			Assert.AreEqual(results[1].Game, game2);
		}

		[TestMethod]
		public void Can_Add_Quantity_For_Existing_Lines()
		{
			// Организация - создание нескольких тестовых игр
			Game game1 = new Game { GameId = 1, Name = "Игра1" };
			Game game2 = new Game { GameId = 2, Name = "Игра2" };

			// Организация - создание корзины
			Cart cart = new Cart();

			// Действие
			cart.AddItem(game1, 1);
			cart.AddItem(game2, 1);
			cart.AddItem(game1, 5);
			List<CartLine> results = cart.Lines.OrderBy(c => c.Game.GameId).ToList();

			// Утверждение
			Assert.AreEqual(results.Count(), 2);
			Assert.AreEqual(results[0].Quantity, 6);    // 6 экземпляров добавлено в корзину
			Assert.AreEqual(results[1].Quantity, 1);
		}

		[TestMethod]
		public void Can_Remove_Line()
		{
			// Организация - создание нескольких тестовых игр
			Game game1 = new Game { GameId = 1, Name = "Игра1" };
			Game game2 = new Game { GameId = 2, Name = "Игра2" };
			Game game3 = new Game { GameId = 3, Name = "Игра3" };

			// Организация - создание корзины
			Cart cart = new Cart();

			// Организация - добавление нескольких игр в корзину
			cart.AddItem(game1, 1);
			cart.AddItem(game2, 4);
			cart.AddItem(game3, 2);
			cart.AddItem(game2, 1);

			// Действие
			cart.RemoveLine(game2);

			// Утверждение
			Assert.AreEqual(cart.Lines.Where(c => c.Game == game2).Count(), 0);
			Assert.AreEqual(cart.Lines.Count(), 2);
		}

		[TestMethod]
		public void Calculate_Cart_Total()
		{
			// Организация - создание нескольких тестовых игр
			Game game1 = new Game { GameId = 1, Name = "Игра1", Price = 100 };
			Game game2 = new Game { GameId = 2, Name = "Игра2", Price = 55 };

			// Организация - создание корзины
			Cart cart = new Cart();

			// Действие
			cart.AddItem(game1, 1);
			cart.AddItem(game2, 1);
			cart.AddItem(game1, 5);
			decimal result = cart.ComputeTotalValue();

			// Утверждение
			Assert.AreEqual(result, 655);
		}

		[TestMethod]
		public void Can_Clear_Contents()
		{
			// Организация - создание нескольких тестовых игр
			Game game1 = new Game { GameId = 1, Name = "Игра1", Price = 100 };
			Game game2 = new Game { GameId = 2, Name = "Игра2", Price = 55 };

			// Организация - создание корзины
			Cart cart = new Cart();

			// Действие
			cart.AddItem(game1, 1);
			cart.AddItem(game2, 1);
			cart.AddItem(game1, 5);
			cart.Clear();

			// Утверждение
			Assert.AreEqual(cart.Lines.Count(), 0);
		}


		/// <summary>
		/// Проверяем добавление в корзину
		/// </summary>
		[TestMethod]
		public void Can_Add_To_Cart()
		{
			// Организация - создание имитированного хранилища
			Mock<IGameRepository> mock = new Mock<IGameRepository>();
			mock.Setup(m => m.Games).Returns(new List<Game> {
		new Game {GameId = 1, Name = "Игра1", Category = "Кат1"},
	}.AsQueryable());

			// Организация - создание корзины
			Cart cart = new Cart();

			// Организация - создание контроллера
			CartController controller = new CartController(mock.Object);

			// Действие - добавить игру в корзину
			controller.AddToCart(cart, 1, null);

			// Утверждение
			Assert.AreEqual(cart.Lines.Count(), 1);
			Assert.AreEqual(cart.Lines.ToList()[0].Game.GameId, 1);
		}

		/// <summary>
		/// После добавления игры в корзину, должно быть перенаправление на страницу корзины
		/// </summary>
		[TestMethod]
		public void Adding_Game_To_Cart_Goes_To_Cart_Screen()
		{
			// Организация - создание имитированного хранилища
			Mock<IGameRepository> mock = new Mock<IGameRepository>();
			mock.Setup(m => m.Games).Returns(new List<Game> {
		new Game {GameId = 1, Name = "Игра1", Category = "Кат1"},
	}.AsQueryable());

			// Организация - создание корзины
			Cart cart = new Cart();

			// Организация - создание контроллера
			CartController controller = new CartController(mock.Object);

			// Действие - добавить игру в корзину
			RedirectToRouteResult result = controller.AddToCart(cart, 2, "myUrl");

			// Утверждение
			Assert.AreEqual(result.RouteValues["action"], "Index");
			Assert.AreEqual(result.RouteValues["returnUrl"], "myUrl");
		}

		// Проверяем URL
		[TestMethod]
		public void Can_View_Cart_Contents()
		{
			// Организация - создание корзины
			Cart cart = new Cart();

			// Организация - создание контроллера
			CartController target = new CartController(null);

			// Действие - вызов метода действия Index()
			CartIndexViewModel result
				= (CartIndexViewModel)target.Index(cart, "myUrl").ViewData.Model;

			// Утверждение
			Assert.AreSame(result.Cart, cart);
			Assert.AreEqual(result.ReturnUrl, "myUrl");
		}
	}
}
