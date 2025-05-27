USE master;
GO

IF EXISTS (SELECT * FROM sys.databases WHERE name = 'decorDB')
BEGIN
    ALTER DATABASE decorDB SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE decorDB;
END
GO

CREATE DATABASE decorDB;
GO

USE decorDB;
GO

CREATE TABLE ProductTypes (
    Id INT PRIMARY KEY IDENTITY(1,1),
    TypeName NVARCHAR(100) NOT NULL,
    Coefficient DECIMAL(10,4) NOT NULL DEFAULT 1.0
);
GO

CREATE TABLE MaterialTypes (
    Id INT PRIMARY KEY IDENTITY(1,1),
    TypeName NVARCHAR(100) NOT NULL,
    WastePercentage DECIMAL(5,2) NOT NULL DEFAULT 0.0
);
GO

CREATE TABLE Materials (
    Id INT PRIMARY KEY IDENTITY(1,1),
    MaterialTypeId INT NOT NULL,
    Name NVARCHAR(200) NOT NULL,
    Unit NVARCHAR(50) NOT NULL,
    Cost DECIMAL(10,2) NOT NULL,
    StockQuantity DECIMAL(10,2) NOT NULL DEFAULT 0,
    MinQuantity DECIMAL(10,2) NOT NULL DEFAULT 0,
    FOREIGN KEY (MaterialTypeId) REFERENCES MaterialTypes(Id)
);
GO

CREATE TABLE Products (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Article NVARCHAR(50) NOT NULL UNIQUE,
    ProductTypeId INT NOT NULL,
    Name NVARCHAR(200) NOT NULL,
    Description NVARCHAR(1000),
    MinPartnerCost DECIMAL(10,2) NOT NULL,
    RollWidth DECIMAL(10,2) NOT NULL,
    ProductionTime INT DEFAULT 0,
    WorkshopNumber INT DEFAULT 1,
    WorkersCount INT DEFAULT 1,
    FOREIGN KEY (ProductTypeId) REFERENCES ProductTypes(Id)
);
GO

CREATE TABLE ProductMaterials (
    Id INT PRIMARY KEY IDENTITY(1,1),
    ProductId INT NOT NULL,
    MaterialId INT NOT NULL,
    Quantity DECIMAL(10,4) NOT NULL,
    FOREIGN KEY (ProductId) REFERENCES Products(Id) ON DELETE CASCADE,
    FOREIGN KEY (MaterialId) REFERENCES Materials(Id)
);
GO

CREATE TABLE Users (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Login NVARCHAR(50) NOT NULL UNIQUE,
    Password NVARCHAR(100) NOT NULL,
    Role NVARCHAR(20) NOT NULL DEFAULT 'User'
);
GO

INSERT INTO MaterialTypes (TypeName, WastePercentage) VALUES 
('Бумага', 0.70),
('Краска', 0.50),
('Клей', 0.15),
('Дисперсия', 0.20);
GO

INSERT INTO ProductTypes (TypeName, Coefficient) VALUES 
('Декоративные обои', 5.5),
('Фотообои', 7.54),
('Обои под покраску', 3.25),
('Стеклообои', 2.5);
GO

INSERT INTO Materials (MaterialTypeId, Name, Unit, Cost, StockQuantity, MinQuantity) VALUES 
(1, 'Бумага-основа с покрытием для флизелиновых обоев', 'рул', 1700.00, 2500.00, 1000.00),
(1, 'Бумага-основа для флизелиновых обоев', 'рул', 1500.00, 3000.00, 1000.00),
(1, 'Бумага обойная для вспененных виниловых обоев', 'рул', 1200.00, 1500.00, 1000.00),
(2, 'Концентрат водоразбавляемой печатной краски', 'кг', 1500.00, 550.00, 500.00),
(2, 'Перламутровый пигмент', 'кг', 3100.00, 200.00, 100.00),
(2, 'Цветная пластизоль', 'кг', 650.00, 200.00, 100.00),
(2, 'Водорастворимая краска водная', 'кг', 500.00, 400.00, 300.00),
(2, 'Металлический пигмент', 'кг', 4100.00, 250.00, 100.00),
(3, 'Сухой клей на основе ПВС', 'кг', 360.00, 700.00, 500.00),
(3, 'Дисперсия анионно-стабилизированного стирол-акрилового сополимера', 'л', 170.00, 800.00, 660.00),
(4, 'Флизелин', 'рул', 1600.00, 2000.00, 1000.00),
(4, 'Стирол-акриловая дисперсия для производства обоев', 'л', 14.90, 2000.00, 880.00),
(4, 'Стирол-акриловая дисперсия для гидрофобных покрытий', 'л', 14.90, 1200.00, 880.00);
GO

INSERT INTO Products (Article, ProductTypeId, Name, Description, MinPartnerCost, RollWidth, ProductionTime, WorkshopNumber, WorkersCount) VALUES 
('1549922', 1, 'Обои из природного материала Традиционный принт светло-коричневые', '', 16950.00, 0.91, 120, 1, 3),
('2018556', 2, 'Фотообои флизелиновые Горы 500x270 см', '', 15880.00, 0.50, 90, 1, 2),
('3028272', 3, 'Обои под покраску флизелиновые Рельеф', '', 11080.00, 0.75, 60, 2, 2),
('4029272', 4, 'Стеклообои Рогожка белые', '', 5898.00, 1.00, 240, 3, 4);
GO

INSERT INTO Users (Login, Password, Role) VALUES
('admin', 'admin123', 'Admin'),
('manager', 'manager123', 'Manager'),
('user', 'user123', 'User');
GO

INSERT INTO ProductMaterials (ProductId, MaterialId, Quantity) VALUES 
(1, 1, 1.20),
(1, 4, 0.33),
(2, 2, 0.50),
(2, 5, 2.90),
(3, 3, 1.40),
(3, 6, 1.70),
(4, 7, 0.30),
(4, 8, 1.00);
GO

CREATE FUNCTION CalculateMaterialNeed
(
    @ProductTypeId INT,
    @MaterialTypeId INT,
    @ProductQuantity INT,
    @Parameter1 DECIMAL(10,2),
    @Parameter2 DECIMAL(10,2),
    @StockQuantity DECIMAL(10,2)
)
RETURNS INT
AS
BEGIN
    DECLARE @Result INT = -1;
    DECLARE @Coefficient DECIMAL(10,4);
    DECLARE @WastePercentage DECIMAL(5,2);
    DECLARE @RequiredQuantity DECIMAL(10,2);
    DECLARE @TotalRequired DECIMAL(10,2);
    
    IF NOT EXISTS (SELECT 1 FROM ProductTypes WHERE Id = @ProductTypeId)
        RETURN -1;
    
    IF NOT EXISTS (SELECT 1 FROM MaterialTypes WHERE Id = @MaterialTypeId)
        RETURN -1;
    
    IF @ProductQuantity <= 0 OR @Parameter1 <= 0 OR @Parameter2 <= 0 OR @StockQuantity < 0
        RETURN -1;
    
    SELECT @Coefficient = Coefficient FROM ProductTypes WHERE Id = @ProductTypeId;
    SELECT @WastePercentage = WastePercentage FROM MaterialTypes WHERE Id = @MaterialTypeId;
    
    SET @RequiredQuantity = @Parameter1 * @Parameter2 * @Coefficient;
    SET @TotalRequired = @RequiredQuantity * @ProductQuantity * (1 + @WastePercentage / 100.0);
    SET @TotalRequired = @TotalRequired - @StockQuantity;
    
    IF @TotalRequired <= 0
        SET @Result = 0;
    ELSE
        SET @Result = CEILING(@TotalRequired);
    
    RETURN @Result;
END;
GO

CREATE VIEW ProductsWithCost 
AS
SELECT 
    p.Id,
    p.Article,
    pt.TypeName AS ProductType,
    p.Name,
    p.Description,
    p.MinPartnerCost,
    p.RollWidth,
    ISNULL(mc.MaterialCost, 0) AS MaterialCost,
    p.ProductionTime,
    p.WorkshopNumber,
    p.WorkersCount
FROM Products p
INNER JOIN ProductTypes pt ON p.ProductTypeId = pt.Id
LEFT JOIN (
    SELECT 
        pm.ProductId,
        SUM(pm.Quantity * m.Cost) AS MaterialCost
    FROM ProductMaterials pm
    INNER JOIN Materials m ON pm.MaterialId = m.Id
    GROUP BY pm.ProductId
) mc ON p.Id = mc.ProductId;
GO

CREATE VIEW ProductMaterialsView 
AS
SELECT 
    p.Id AS ProductId,
    p.Article,
    p.Name AS ProductName,
    m.Name AS MaterialName,
    pm.Quantity,
    m.Unit,
    m.Cost,
    (pm.Quantity * m.Cost) AS TotalCost
FROM Products p
INNER JOIN ProductMaterials pm ON p.Id = pm.ProductId
INNER JOIN Materials m ON pm.MaterialId = m.Id;
GO