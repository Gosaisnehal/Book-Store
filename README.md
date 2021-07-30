# Book-Store
Book Store Console Based 

 An online bookstore currently sells books for 3 different categories: Crime, Romance, Fantasy. They have future plans to add more categories into their collection. 
Currently all books within the Crime category are discounted by 5%. 

The following are the additional charges that would be applied to an order:

•	10% goods services tax (GST) on the total price

•	$5.95 delivery fee for orders less than $20

## Assumptions 

• Database present and books table are created  with the following properties.  

   My database name is Book Store and table name is books.
   
   Table books has following properties. 
   
	[bookId] [int] IDENTITY(1,1) NOT NULL,
	[bookTitle] [varchar](max) NOT NULL,
	[bookAuthor] [varchar](max) NULL,
	[bookGenre] [varchar](max) NULL,
	[bookUnitPrice] [decimal](18, 2) NULL.
  
 
	


## Getting Started

To Clone:
`git clone https://github.com/Gosaisnehal/Book-Store
- Once cloned, you will need to modify connection string in the app.config to drirect the application to your database
- You will also need to populate your database with seed data of your choice .



## Features

-user's can display all the books

-user's can print order

-user's can add a new book

-discount can be applied to a book category


	

