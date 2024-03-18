using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using SV20T1020242.DomainModels;

namespace SV20T1020242.DataLayers.SQLServer
{
    public class OrderDAL : _BaseDAL, IOrderDAL
    {
        public OrderDAL(string connectionString) : base(connectionString)
        {
        }

        public int Add(Order data)
        {
            int id = 0;
            using (var connection = OpenConnection())
            {
                var sql = @"insert into Orders(CustomerID, OrderTime, DeliveryProvince, DeliveryAddress, EmployeeID, Status)
                    values(@CustomerID, getdate(), @DeliveryProvince, @DeliveryAddress, @EmployeeID, @Status);
                    select @@identity";
                var parameters = new
                {
                    CustomerID = data.CustomerID,
                    DeliveryProvince = data.DeliveryProvince,
                    DeliveryAddress = data.DeliveryAddress,
                    EmployeeID = data.EmployeeID,
                    Status = data.Status
                };
                id = connection.ExecuteScalar<int>(sql, param: parameters);
            }
            return id;
        }

        public int Count(int status = 0, DateTime? fromTime = null, DateTime? toTime = null, string searchValue = "")
        {
            int count = 0;
            if (!string.IsNullOrEmpty(searchValue))
                searchValue = "%" + searchValue + "%";
            using (var connection = OpenConnection())
            {
                var sql = @"select count(*)
                            from Orders as o
                            left join Customers as c on o.CustomerID = c.CustomerID
                            left join Employees as e on o.EmployeeID = e.EmployeeID
                            left join Shippers as s on o.ShipperID = s.ShipperID
                            where c.CustomerName like @SearchValue
                            or e.FullName like @SearchValue
                            or s.ShipperName like @SearchValue";
                count = connection.Query<int>(sql, new { SearchValue = searchValue }).FirstOrDefault();
            }
            return count;
        }

        public bool Delete(int id)
        {
            bool result = false;
            using (var connection = OpenConnection())
            {
                var sql = @"delete from OrderDetails where OrderID = @OrderID;
                            delete from Orders where OrderID = @OrderID";
                result = connection.Execute(sql, new { OrderID = id }) > 0;
            }
            return result;
        }
        public bool DeleteDetail(int orderID, int productID)
        {
            bool result = false;
            using (var connection = OpenConnection())
            {
                var sql = @"DELETE FROM OrderDetails WHERE OrderID = @OrderID AND ProductID = @ProductID";
                result = connection.Execute(sql, new { OrderID = orderID, ProductID = productID }) > 0;
            }
            return result;
        }


        public Order? Get(int id)
        {
            Order? data = null;
            using (var connection = OpenConnection())
            {
                var sql = @"select o.*, c.CustomerName, c.ContactName as CustomerContactName,
                                    c.Address as CustomerAddress, c.Phone as CustomerPhone, c.Email as CustomerEmail,
                                    e.FullName as EmployeeName, s.ShipperName, s.Phone as ShipperPhone
                            from Orders as o
                            left join Customers as c on o.CustomerID = c.CustomerID
                            left join Employees as e on o.EmployeeID = e.EmployeeID
                            left join Shippers as s on o.ShipperID = s.ShipperID
                            where o.OrderID = @OrderID";
                data = connection.Query<Order>(sql, new { OrderID = id }).FirstOrDefault();
            }
            return data;
        }
        public OrderDetail? GetDetail(int orderID, int productID)
        {
            OrderDetail? data = null;
            using (var connection = OpenConnection())
            {
                var sql = @"select od.*, p.ProductName, p.Photo, p.Unit
                    from OrderDetails as od
                    join Products as p on od.ProductID = p.ProductID
                    where od.OrderID = @OrderID and od.ProductID = @ProductID";
                data = connection.Query<OrderDetail>(sql, new { OrderID = orderID, ProductID = productID }).FirstOrDefault();
            }
            return data;
        }


        public bool IsUsed(int id)
        {
            using (var connection = OpenConnection())
            {
                var sql = "SELECT COUNT(*) FROM YourOtherTable WHERE OrderID = @OrderID";
                int count = connection.QuerySingle<int>(sql, new { OrderID = id });
                return count > 0;
            }
        }


        public IList<Order> List(int page = 1, int pageSize = 0, int status = 0, DateTime? fromTime = null, DateTime? toTime = null, string searchValue = "")
        {
            List<Order> list = new List<Order>();
            if (!string.IsNullOrEmpty(searchValue))

                searchValue = "%" + searchValue + "%";

            using (var connection = OpenConnection())
            {
                var sql = @"with cte as
                            (
                                select row_number() over(order by o.OrderTime desc) as RowNumber, o.*,
                                        c.CustomerName, c.ContactName as CustomerContactName,
                                        c.Address as CustomerAddress, c.Phone as CustomerPhone, c.Email as CustomerEmail,
                                        e.FullName as EmployeeName, s.ShipperName, s.Phone as ShipperPhone
                                from Orders as o
                                left join Customers as c on o.CustomerID = c.CustomerID
                                left join Employees as e on o.EmployeeID = e.EmployeeID
                                left join Shippers as s on o.ShipperID = s.ShipperID
                                where c.CustomerName like @SearchValue
                                or e.FullName like @SearchValue
                                or s.ShipperName like @SearchValue
                            )
                            select * from cte
                            where (@PageSize = 0)
                            or (RowNumber between (@Page - 1) * @PageSize + 1 and @Page * @PageSize)
                            order by RowNumber";
                var parameters = new 
                {
                    Page = page,
                    PageSize = pageSize,
                    Status = status,
                    FromTime = fromTime,
                    ToTime = toTime,
                    SearchValue = searchValue
                };
                list = connection.Query<Order>(sql: sql, param: parameters, commandType: CommandType.Text).ToList();
                connection.Close();
            }
            return list;
        }
        public IList<OrderDetail> ListDetails(int orderID)
        {
            List<OrderDetail> list = new List<OrderDetail>();
            using (var connection = OpenConnection())
            {
                var sql = @"select od.*, p.ProductName, p.Photo, p.Unit
                    from OrderDetails as od
                    join Products as p on od.ProductID = p.ProductID
                    where od.OrderID = @OrderID";
                list = connection.Query<OrderDetail>(sql, new { OrderID = orderID }).ToList();
            }
            return list;
        }

        public bool Update(Order data)
        {
            bool result = false;
            using (var connection = OpenConnection())
            {
                var sql = @"UPDATE Orders
                    SET CustomerID = @CustomerID,
                        OrderTime = @OrderTime,
                        DeliveryProvince = @DeliveryProvince,
                        DeliveryAddress = @DeliveryAddress,
                        EmployeeID = @EmployeeID,
                        AcceptTime = @AcceptTime,
                        ShipperID = @ShipperID,
                        ShippedTime = @ShippedTime,
                        FinishedTime = @FinishedTime,
                        Status = @Status
                    WHERE OrderID = @OrderID";
                result = connection.Execute(sql, data) > 0;
            }
            return result;
        }
        public bool SaveDetail(int orderID, int productID, int quantity, decimal salePrice)
        {
            bool result = false;
            using (var connection = OpenConnection())
            {
                var existingDetail = GetDetail(orderID, productID);
                if (existingDetail == null)
                {
                    // If the detail does not exist, insert a new one
                    var sql = @"INSERT INTO OrderDetails (OrderID, ProductID, Quantity, SalePrice) 
                        VALUES (@OrderID, @ProductID, @Quantity, @SalePrice)";
                    result = connection.Execute(sql, new { OrderID = orderID, ProductID = productID, Quantity = quantity, SalePrice = salePrice }) > 0;
                }
                else
                {
                    // If the detail already exists, update its quantity and sale price
                    var sql = @"UPDATE OrderDetails 
                        SET Quantity = @Quantity, 
                            SalePrice = @SalePrice 
                        WHERE OrderID = @OrderID AND ProductID = @ProductID";
                    result = connection.Execute(sql, new { OrderID = orderID, ProductID = productID, Quantity = quantity, SalePrice = salePrice }) > 0;
                }
            }
            return result;
        }

    }
}
