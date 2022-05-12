# cryptocop
### This project composes of three services using RabbitMQ and Docker
* Web API in .NET core with authentication using JWT
   * [GET]
      * /cart [GET] - Gets all items within the shopping cart<br />
      * /addresses [GET] - Gets all addresses associated with authenticated user <br />
      * /payments [GET] - Gets all payment cards associated with the authenticated user <br />
      * /orders [GET] - Gets all orders associated with the authenticated user <br />
      * /account/signout [GET] - Logs the user out by voiding the provided JWT token
      * /cryptocurrencies [GET] - Gets all available cryptocurrencies - the only available cryptocurrencies in this platform are BitCoin (BTC), Ethereum (ETH), Tether (USDT) and Monero (XMR)
      * /exchanges [GET] - Gets all exchanges in a paginated envelope. 
    * [POST]
      * /cart [POST] - Adds an item to the shopping cart<br />
      * /addresses [POST] - Adds a new address associated with authenticated user<br />
      * /payments [POST] - Adds a new payment card associated with the authenticated user <br />
      * /orders [POST] - Adds a new order associated with the authenticated user<br />
      * /account/register [POST] - Registers a user within the application
      * /account/signin [POST] - Signs the user in by checking the credentials provided 
    * [DELETE]
      * /cart/{id} [DELETE] - Deletes an item from the shopping cart <br />
      * /cart [DELETE] - Clears the cart - all items within the cart should be deleted <br />
      * /addresses/{id} [DELETE] - Deletes an address by id  <br />
    * [PATCH]
      * /cart/{id} [PATCH] - Updates the quantity for a shopping cart item <br />
* A simple Payment Service in JS
* Email Service in Python








