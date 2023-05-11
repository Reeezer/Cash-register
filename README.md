# CashRegister

CashRegister is a cross-platform cash register application developed in C# for small companies. The main objectives of this project are to provide a versatile cash register solution that can run on multiple platforms (iOS, Android, Windows), utilize an Azure server as a MySQL database, handle article scanning through portable devices and webcams, send receipts to email addresses, and automate currency management.

The application features three distinct modes: Cashier mode, Customer mode, and Administration mode.

In Cashier mode, users can add articles to a customer's cart and process their payment. This mode supports scanning articles or manually adding them, removing articles, displaying ongoing articles with their details (name, description, quantity, total price, unit price), making payments (using the Twint platform), and adding new articles to the database by scanning their barcodes through an API (e.g., Open Food Facts).

Customer mode allows customers to add articles themselves and make payments at the cash register. Similar to the "Passabene" system in Swiss stores like "Coop" (also known as Coopé or Là Coopéy), customers can scan their items at a terminal before finalizing the purchase.

Administration mode is designed for stock management. It enables users to add new products, track available quantities, manage stock levels, perform inventory checks by comparing physical stock with database records, and view sales statistics.

The project's scope includes the development of the backend database and Azure server, implementing functionalities such as adding, removing, and viewing articles in the cart, barcode scanning, adding new articles through an API, generating and sending receipts via email, handling payments, and creating an administration mode for currency management.

CashRegister provides a comprehensive cash register solution that combines convenience, cross-platform compatibility, and efficient stock management for small businesses.
