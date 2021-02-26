﻿using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClassLibrary
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class ApiProductsController : ControllerBase
    {

        private readonly IProductService _productService;

        public ApiProductsController(IProductService productService)
        {
            _productService = productService;
        }

        /// <include file='ApiDoc.xml' path='docs/members[@name="ProductsController"]/GetAllProductsAsync/*'/>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Product>>> GetAllProductsAsync()
        {
            IEnumerable<Product> products = await _productService.GetAllProductsAsync();
            return Ok(products);
        }

        /// <include file='ApiDoc.xml' path='docs/members[@name="ProductsController"]/GetProductByIdAsync/*'/>
        [HttpGet("{productId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Product>> GetProductByIdAsync(int productId)
        {
            bool doesProductIdExist = await _productService.DoesProductIdExist(productId);

            if (doesProductIdExist == true)
            {
                Product product = await _productService.GetProductByIdAsync(productId);
                return Ok(product);
            }
            else
            {
                return NotFound();
            }
        }

        /// <include file='ApiDoc.xml' path='docs/members[@name="ProductsController"]/BuyProductByIdAsync/*'/>
        [HttpPost("buy")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<int>> BuyProductByIdAsync([FromBody] int productId)
        {
            bool doesProductIdExist = await _productService.DoesProductIdExist(productId);

            if (doesProductIdExist == true)
            {
                await _productService.BuyProductByIdAsync(productId);
                Product product = await _productService.GetProductByIdAsync(productId);
                return Ok(product.Quantity);
            }
            else
            {
                return NotFound();
            }
        }

        /// <include file='ApiDoc.xml' path='docs/members[@name="ProductsController"]/AddProductAsync/*'/>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<Product>> AddProductAsync([FromBody] Product product)
        {
            await _productService.AddProductAsync(product);
            return CreatedAtAction("GetProductById", new { productId = product.ProductId }, product);
        }

        /// <include file='ApiDoc.xml' path='docs/members[@name="ProductsController"]/DeleteProductByIdAsync/*'/>
        [HttpDelete("{productId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteProductByIdAsync(int productId)
        {
            bool doesProductIdExist = await _productService.DoesProductIdExist(productId);

            if (doesProductIdExist == true)
            {
                await _productService.DeleteProductByIdAsync(productId);
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }

        /// <include file='ApiDoc.xml' path='docs/members[@name="ProductsController"]/UpdateProductAsync/*'/>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Product>> UpdateProductAsync([FromBody] Product product)
        {
            bool doesProductIdExist = await _productService.DoesProductIdExist(product.ProductId);

            if (doesProductIdExist == true)
            {
                await _productService.UpdateProductAsync(product);
                Product updatedProduct = await _productService.GetProductByIdAsync(product.ProductId);
                return Ok(updatedProduct);
            }
            else
            {
                return NotFound();
            }
        }
    }
}