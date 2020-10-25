﻿using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly Random randomNumberGenerator = new Random();

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        
        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _productRepository.GetAllProductsAsync();
        }

        public async Task<Product> GetProductByIdAsync(int productId)
        {
            return await _productRepository.GetProductByIdAsync(productId);
        }
       
        public async Task BuyProductByIdAsync(int productId)
        {
            Product product = await GetProductByIdAsync(productId);

            if(product != null)
            {
                product.Quantity -= 1;
                await _productRepository.UpdateProductAsync(product);
            }
        }

        private String ChooseRandomImageUrlFromPossibleOptions()
        {
            int randomNumber = randomNumberGenerator.Next();
            if (randomNumber % 3 == 0)
            {
                return "book.png";
            }
            else if (randomNumber % 3 == 1)
            {
                return "lamp.png";
            }
            else if (randomNumber % 3 == 2)
            {
                return "duck.png";
            }
            return "";
        }

        public async Task AddProductAsync(Product product)
        {
            if (product != null)
            {
                // product.Id has to be 0 otherwise there will be an error. This is 
                // because you are not allowed to specify an ID value when adding a product
                // to the database. An ID value will automatically be given to the product.
                product.Id = 0;
                product.ImageUrl = ChooseRandomImageUrlFromPossibleOptions();
                await _productRepository.AddProductAsync(product);
            }
        }
   
        public async Task DeleteProductByIdAsync(int id)
        {
            Product product = await GetProductByIdAsync(id);

            if(product != null)
            {
                await _productRepository.DeleteProductByIdAsync(product);
            }
        }

        public async Task UpdateProductAsync(Product product)
        {
            if(product != null) {
                Product existingProduct = await GetProductByIdAsync(product.Id);
                if (existingProduct != null)
                {
                    existingProduct.CopyProductPropertiesExcludingImageUrl(product);
                    await _productRepository.UpdateProductAsync(existingProduct);
                }
            }
        }

        public async Task<bool> DoesProductIdExist(int productId)
        {
            Product product = await GetProductByIdAsync(productId);
            if(product == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
