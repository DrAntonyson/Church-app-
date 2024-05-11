DBCC CHECKIDENT (Orders,RESEED,0);
DBCC CHECKIDENT (InvoiceModels,RESEED,0);

DELETE FROM Orders WHERE Qty = 1 OR Unit_Price = 4500 OR FkProdId = 2;
DELETE FROM InvoiceModels WHERE FKUserID = '7d655621-96a9-4c09-a009-99ff8369f144' OR Total_Bill = 3500;

select * from Orders;
select * from InvoiceModels;