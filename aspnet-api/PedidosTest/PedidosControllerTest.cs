using System;
using System.Collections.Generic;
using AspnetApi.API.Controllers;
using AspnetApi.Domain.Models.DTO;
using AspnetApi.Domain.Repository;
using AspnetApi.Domain.Services;
using AspnetApi.Infra.Context;
using AspnetApi.Infra.Repository;
using AspnetApi.Service;
using Microsoft.AspNetCore.Mvc;
using Xunit;


namespace PedidosTest
{
    public class PedidosControllerTest
    {
        public FastFoodContext _context;
        public IPedidoRepository _repository;
        public IPedidoService _service;
        public PedidosController _controller;

        private IActionResult _res1;
        private IActionResult _res2;
        private IActionResult _res3;

        private PostPedido postItem;
        private PostPedido postItem2;
        private PostPedido postItem3;

        private void Initialize()
        {
            if (_context == null)
            {
                _context = new FastFoodContext();
                _repository = new PedidoRepository(_context);
                _service = new PedidoService(_repository);
                _controller = new PedidosController(_service);

                postItem = new PostPedido(){SolicitanteId = "0123-4567", Lanche = "Cheeseburger", Bebida = "Coquinha"};
                postItem2 = new PostPedido(){SolicitanteId = "0123-4567", Lanche = "Cheeseburger", Bebida = "Pepsi"};
                postItem3 = new PostPedido(){SolicitanteId = "0123-4567", Lanche = "X-Bacon", Bebida = "Guaraná"};
                _res1 = _controller.Post(postItem);
                _res2 = _controller.Post(postItem2);
                _res3 = _controller.Post(postItem3);
            }
        }

        [Fact]
        public void TestCreate()
        {
            // Arrange
            Initialize();
            var postItem4 = new PostPedido(){SolicitanteId = "0123-4567", Lanche = "Sanduíche de presunto", Bebida = "Fanta uva"};

            // Act
            var result = _controller.Post(postItem4);
            var okResult = (ObjectResult) result;
            var pedidoResult = (PedidoSummary) okResult.Value;

            var getResult = _controller.GetAll();
            var getOkResult = (ObjectResult) getResult;
            var listResult = (List<PedidoSummary>) getOkResult.Value;

            // Assert
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(postItem4.Lanche, pedidoResult.Lanche);
            Assert.Equal(postItem4.Bebida, pedidoResult.Bebida);

            Assert.Equal(200, getOkResult.StatusCode);
            Assert.Equal(4, listResult.Count);
            Assert.Contains<PedidoSummary>((_res1 as ObjectResult).Value as PedidoSummary, listResult);
            Assert.Contains<PedidoSummary>((_res2 as ObjectResult).Value as PedidoSummary, listResult);
            Assert.Contains<PedidoSummary>((_res3 as ObjectResult).Value as PedidoSummary, listResult);
            Assert.Contains<PedidoSummary>(pedidoResult, listResult);
        }

        [Fact]
        public void TestGetAll()
        {
            // Arrange
            Initialize();
            
            // Act
            var result = _controller.GetAll();
            var okResult = (ObjectResult) result;
            var listResult = (List<PedidoSummary>) okResult.Value;
            
            // Assert
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(3, listResult.Count);
            Assert.Contains<PedidoSummary>((_res1 as ObjectResult).Value as PedidoSummary, listResult);
            Assert.Contains<PedidoSummary>((_res2 as ObjectResult).Value as PedidoSummary, listResult);
            Assert.Contains<PedidoSummary>((_res3 as ObjectResult).Value as PedidoSummary, listResult);
        }

        [Fact]
        public void TestGetFilterBySolicitante()
        {
            // Arrange
            Initialize();
            var postItem4 = new PostPedido(){SolicitanteId = "0000-4567", Lanche = "Sanduíche de presunto", Bebida = "Fanta uva"};
            var _res4 = _controller.Post(postItem4);

            // Act
            var result1 = _controller.GetFiltered("0123-4567");
            var okResult1 = (ObjectResult) result1;
            var listResult1 = (List<PedidoSummary>) okResult1.Value;

            var result2 = _controller.GetFiltered("0000-4567");
            var okResult2 = (ObjectResult) result2;
            var listResult2 = (List<PedidoSummary>) okResult2.Value;

            
            // Assert
            Assert.Equal(200, okResult1.StatusCode);
            Assert.Equal(3, listResult1.Count);
            Assert.Contains<PedidoSummary>((_res1 as ObjectResult).Value as PedidoSummary, listResult1);
            Assert.Contains<PedidoSummary>((_res2 as ObjectResult).Value as PedidoSummary, listResult1);
            Assert.Contains<PedidoSummary>((_res3 as ObjectResult).Value as PedidoSummary, listResult1);

            Assert.Equal(200, okResult2.StatusCode);
            Assert.Equal(1, listResult2.Count);
            Assert.Contains<PedidoSummary>((_res4 as ObjectResult).Value as PedidoSummary, listResult2);
        }

        [Fact]
        public void TestDelete()
        {
            // Arrange
            Initialize();
            var expectedRes2 = (_res2 as ObjectResult).Value as PedidoSummary;
            expectedRes2.Posicao--;
            var expectedRes3 = (_res3 as ObjectResult).Value as PedidoSummary;
            expectedRes3.Posicao--;

            // Act
            var deleteResult = _controller.Delete(1);

            var getResult = _controller.GetAll();
            var getOkResult = (ObjectResult) getResult;
            var listResult = (List<PedidoSummary>) getOkResult.Value;

            // Assert
            Assert.IsAssignableFrom<OkResult>(deleteResult);

            Assert.Equal(200, getOkResult.StatusCode);
            Assert.Equal(2, listResult.Count);
            Assert.Contains<PedidoSummary>(expectedRes2, listResult);
            Assert.Contains<PedidoSummary>(expectedRes3, listResult);
        }
    }
}
