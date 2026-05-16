DROP DATABASE IF EXISTS amarris_kitchen;
CREATE DATABASE amarris_kitchen;
USE amarris_kitchen;

CREATE TABLE Admin (
    admin_id INT PRIMARY KEY AUTO_INCREMENT,
    admin_name VARCHAR(255) NOT NULL,
    role VARCHAR(100) NOT NULL,
    email VARCHAR(255) UNIQUE NOT NULL,
    password VARCHAR(255) NOT NULL,
    created_at DATETIME DEFAULT CURRENT_TIMESTAMP,
    updated_at DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
);

CREATE TABLE Category (
    category_id INT PRIMARY KEY AUTO_INCREMENT,
    category_name VARCHAR(255) NOT NULL,
    admin_id INT,
    created_at DATETIME DEFAULT CURRENT_TIMESTAMP,
    updated_at DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    FOREIGN KEY (admin_id) REFERENCES Admin(admin_id)
);

CREATE TABLE Product (
    product_id INT PRIMARY KEY AUTO_INCREMENT,
    product_name VARCHAR(255) NOT NULL,
    unit_price DECIMAL(10,2) NOT NULL,
    image_url VARCHAR(255),
    category_id INT NOT NULL,
    admin_id INT NOT NULL,
    created_at DATETIME DEFAULT CURRENT_TIMESTAMP,
    updated_at DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    FOREIGN KEY (category_id) REFERENCES Category(category_id),
    FOREIGN KEY (admin_id) REFERENCES Admin(admin_id)
);

CREATE TABLE Combo_Item (
    combo_id INT,
    product_id INT,
    quantity INT NOT NULL,
    created_at DATETIME DEFAULT CURRENT_TIMESTAMP,
    updated_at DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    PRIMARY KEY (combo_id, product_id),
    FOREIGN KEY (combo_id) REFERENCES Product(product_id),
    FOREIGN KEY (product_id) REFERENCES Product(product_id)
);

CREATE TABLE `Order` (
    order_id INT PRIMARY KEY AUTO_INCREMENT,
    order_date DATE NOT NULL,
    order_time TIME NOT NULL,
    order_mode VARCHAR(50) NOT NULL,
    status VARCHAR(50) DEFAULT 'Pending',
    price DECIMAL(10,2) NOT NULL,
    created_at DATETIME DEFAULT CURRENT_TIMESTAMP,
    updated_at DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
);

CREATE TABLE Order_Item (
    order_id INT,
    product_id INT,
    quantity INT NOT NULL,
    created_at DATETIME DEFAULT CURRENT_TIMESTAMP,
    updated_at DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    PRIMARY KEY (order_id, product_id),
    FOREIGN KEY (order_id) REFERENCES `Order`(order_id),
    FOREIGN KEY (product_id) REFERENCES Product(product_id)
);

CREATE TABLE Payment (
    payment_id INT PRIMARY KEY AUTO_INCREMENT,
    order_id INT,
    payment_method VARCHAR(100),
    trn VARCHAR(255),
    amount_paid DECIMAL(10,2) NOT NULL,
    payment_date DATETIME,
    discount DECIMAL(10,2),
    vat DECIMAL(10,2),
    created_at DATETIME DEFAULT CURRENT_TIMESTAMP,
    updated_at DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    FOREIGN KEY (order_id) REFERENCES `Order`(order_id)
);

INSERT INTO Admin (admin_name, role, email, password) VALUES
('Greta', 'Manager', 'greta@sampleamarriskitchen.com', 'password123'),
('Efren', 'Cashier', 'efren@sampleamarriskitchen.com', 'gandako321'),
('Jeana', 'Cook', 'jeana@sampleamarriskitchen.com', 'tapsilog213'),
('Francis', 'Cook', 'francis@sampleamarriskitchen.com', 'chikin67'),
('Diorene', 'Cashier', 'diorene@sampleamarriskitchen.com', 'cutie111');

INSERT INTO Category (category_name, admin_id, created_at, updated_at) VALUES
('Silog Meals', 1, NOW(), NOW()),
('Pork Tonkatsu', 1, NOW(), NOW()),
('Drinks', 2, NOW(), NOW()),
('Unlimited Wings', 2, NOW(), NOW()),
('Ala Carte Wings', 3, NOW(), NOW()),
('Wings to Share', 3, NOW(), NOW()),
('Wing Flavors', 4, NOW(), NOW()),
('Extras & Add-ons', 5, NOW(), NOW());

INSERT INTO Product (product_name, unit_price, image_url, 
category_id, admin_id, created_at, updated_at)
VALUES
('Tapsilog', 199.00, 'tapsilog.jpg', 1, 1, NOW(), NOW()),
('Longsilog', 120.00, 'longsilog.jpg', 1, 1, NOW(), NOW()),
('Chicken Tonkatsu', 299.00, 'chicken_tonkatsu.jpg', 2, 2,
NOW(), NOW()),
('Pork Tonkatsu', 89.00, 'pork_tonkatsu.jpg', 2, 2,
NOW(), NOW()),
('Unlimited Wings', 349.00, 'unli_wings.jpg', 4, 3,
NOW(), NOW()),
('Iced Tea', 150.00, 'iced_tea.jpg', 3, 2,
NOW(), NOW()),
('Garlic Wings (6pcs)', 249.00, 'garlic_wings.jpg', 5, 4,
NOW(), NOW()),
('Wings to Share (12pcs)', 199.00, 'wings_share.jpg', 6, 4,
NOW(), NOW()),
('BBQ Combo Wings', 399.00, 'bbq_combo.jpg', 6, 5,
NOW(), NOW()),
('Extra Rice', 30.00, 'extra_rice.jpg', 8, 5, NOW(), NOW()),
('Buffalo Sauce', 20.00, 'buffalo_sauce.jpg', 7, 3,
NOW(), NOW());

INSERT INTO Combo_Item (combo_id, product_id, quantity,
created_at, updated_at) VALUES
(1, 7, 2, NOW(), NOW()),
(2, 11, 1, NOW(), NOW());

INSERT INTO `Order` (order_date, order_time, order_mode, status, price, 
created_at, updated_at) VALUES
('2026-05-05', '10:30:00', 'Dine-in',  'Completed', 597.00, NOW(), NOW()),
('2026-05-05', '11:00:00', 'Take-out', 'Completed', 120.00, NOW(), NOW()),
('2026-05-06', '12:15:00', 'Dine-in',  'Completed', 598.00, NOW(), NOW()),
('2026-05-06', '13:45:00', 'Take-out', 'Completed',  89.00, NOW(), NOW()),
('2026-05-06', '15:00:00', 'Dine-in',  'Pending',   698.00, NOW(), NOW()),
('2026-05-07', '09:20:00', 'Take-out', 'Completed', 600.00, NOW(), NOW()),
('2026-05-07', '10:10:00', 'Dine-in',  'Completed', 249.00, NOW(), NOW()),
('2026-05-07', '14:30:00', 'Dine-in',  'Completed', 399.00, NOW(), NOW());

INSERT INTO Order_Item (order_id, product_id, quantity,
created_at, updated_at) VALUES
(1, 1, 3, NOW(), NOW()),
(2, 2, 1, NOW(), NOW()),
(3, 3, 2, NOW(), NOW()),
(4, 4, 1, NOW(), NOW()),
(5, 5, 2, NOW(), NOW()),
(6, 6, 4, NOW(), NOW()),
(7, 7, 1, NOW(), NOW()),
(8, 8, 1, NOW(), NOW());
 
INSERT INTO Payment 
(order_id, payment_method, trn, amount_paid, payment_date, discount, vat)
VALUES
(1, 'Cash',  'TRN001', 597.00, NOW(),  0,  0),
(2, 'Cash',  'TRN002', 120.00, NOW(),  0,  0),
(3, 'GCash', 'TRN003', 598.00, NOW(), 10, 12),
(4, 'Cash',  'TRN004',  89.00, NOW(),  0,  0),
(5, 'GCash', 'TRN005', 698.00, NOW(),  0,  0),
(6, 'Cash',  'TRN006', 600.00, NOW(),  5,  8),
(7, 'GCash', 'TRN007', 249.00, NOW(),  0,  0),
(8, 'GCash', 'TRN008', 399.00, NOW(), 20, 15);

-- =========================================================
SHOW Tables;
SELECT * FROM Admin;
SELECT * FROM Category;
SELECT * FROM Combo_Item;
SELECT * FROM `Order`;
SELECT * FROM Order_Item;
SELECT * FROM Payment;
SELECT * FROM Product;

-- -------------------------------------------------------
-- EASY
-- -------------------------------------------------------
-- 1. List all products with their IDs
SELECT product_id, product_name FROM Product;

-- 2. List all categories and their IDs
SELECT category_id, category_name FROM Category;

-- 3. Show all products under Silog Meals (category_id = 1)
SELECT product_name, unit_price FROM Product WHERE category_id = 1;

-- 4. Show products under Pork Tonkatsu (category_id = 2) priced at least 50
SELECT product_name, unit_price FROM Product WHERE category_id = 2 AND unit_price >= 50.00;

-- 5. Find a product by name
SELECT product_name, unit_price FROM Product WHERE product_name = 'Tapsilog';

-- 6. Show all products sorted by price (highest first)
SELECT product_name, unit_price FROM Product ORDER BY unit_price DESC;

-- 7. Show all orders placed today
SELECT * FROM `Order` WHERE order_date = CURRENT_DATE;

-- 8. Show admin name and role
SELECT admin_name, role FROM Admin;

-- 9. Show all Take-out orders
SELECT * FROM `Order` WHERE order_mode = 'Take-out';

-- 10. Show all payments made via GCash
SELECT * FROM Payment WHERE payment_method = 'GCash';

---------------------------------------------------------
-- MODERATE
---------------------------------------------------------

-- 1. Show product name, price, and its category name
SELECT p.product_name, p.unit_price, c.category_name
FROM Product p
JOIN Category c ON p.category_id = c.category_id;

-- 2. Count how many products are in each category
SELECT c.category_name, COUNT(p.product_id) AS total_items
FROM Category c
LEFT JOIN Product p ON c.category_id = p.category_id
GROUP BY c.category_name;

-- 3. Show full details of a specific order (order_id = 1)
SELECT o.order_id, p.product_name, oi.quantity, p.unit_price,
       (p.unit_price * oi.quantity) AS total
FROM `Order` o
JOIN Order_Item oi ON o.order_id = oi.order_id
JOIN Product p ON oi.product_id = p.product_id
WHERE o.order_id = 1;

-- 4. List which admin created each category
SELECT a.admin_name, c.category_name
FROM Admin a
JOIN Category c ON a.admin_id = c.admin_id;

-- 5. Show total sales collected per payment method
SELECT payment_method, SUM(amount_paid) AS total_collected
FROM Payment
GROUP BY payment_method;

-- 6. Find the most expensive product in each category
SELECT c.category_name, MAX(p.unit_price) AS highest_price
FROM Category c
JOIN Product p ON c.category_id = p.category_id
GROUP BY c.category_name;

-- 7. List orders placed in the last 7 days
SELECT * FROM `Order`
WHERE order_date >= DATE_SUB(CURDATE(), INTERVAL 7 DAY);

-- 8. Identify combo items and their component products
SELECT main.product_name AS Combo_Name, sub.product_name AS Component
FROM Combo_Item ci
JOIN Product main ON ci.combo_id = main.product_id
JOIN Product sub  ON ci.product_id = sub.product_id;

-- 9. Find high value orders (total payment above 200)
SELECT order_id, amount_paid
FROM Payment
WHERE amount_paid > 200;

-- 10. Show all orders with their payment method and amount paid
SELECT o.order_id, o.order_date, o.order_mode, p.payment_method, p.amount_paid
FROM `Order` o
JOIN Payment p ON o.order_id = p.order_id;

---------------------------------------------------------
-- DIFFICULT
---------------------------------------------------------

-- 1. Show each order's computed total vs amount_paid
SELECT o.order_id,
       SUM(p.unit_price * oi.quantity) AS computed_total,
       pay.amount_paid
FROM `Order` o
JOIN Order_Item oi ON o.order_id = oi.order_id
JOIN Product p     ON oi.product_id = p.product_id
JOIN Payment pay   ON o.order_id = pay.order_id
GROUP BY o.order_id, pay.amount_paid;

-- 2. Rank products by total quantity sold
SELECT p.product_name, SUM(oi.quantity) AS total_sold
FROM Product p
JOIN Order_Item oi ON p.product_id = oi.product_id
GROUP BY p.product_name
ORDER BY total_sold DESC;

-- 3. Show total revenue per day
SELECT o.order_date, SUM(pay.amount_paid) AS daily_revenue
FROM `Order` o
JOIN Payment pay ON o.order_id = pay.order_id
GROUP BY o.order_date
ORDER BY o.order_date;

-- 4. Show products that have never been ordered
SELECT p.product_name
FROM Product p
LEFT JOIN Order_Item oi ON p.product_id = oi.product_id
WHERE oi.order_id IS NULL;

-- 5. Average order value per order mode
SELECT o.order_mode, AVG(pay.amount_paid) AS avg_order_value
FROM `Order` o
JOIN Payment pay ON o.order_id = pay.order_id
GROUP BY o.order_mode;

-- 6. Orders where a discount was applied
SELECT o.order_id, pay.amount_paid, pay.discount,
       (pay.amount_paid + pay.discount) AS original_price
FROM `Order` o
JOIN Payment pay ON o.order_id = pay.order_id
WHERE pay.discount > 0;

-- 7. Total VAT collected per payment method
SELECT payment_method, SUM(vat) AS total_vat_collected
FROM Payment
GROUP BY payment_method;

-- 8. Total revenue per category
SELECT c.category_name, SUM(p.unit_price * oi.quantity) AS total_revenue
FROM Category c
JOIN Product p     ON c.category_id = p.category_id
JOIN Order_Item oi ON p.product_id  = oi.product_id
GROUP BY c.category_name
ORDER BY total_revenue DESC;

-- 9. Most ordered product per order mode
SELECT o.order_mode, p.product_name, SUM(oi.quantity) AS total_qty
FROM `Order` o
JOIN Order_Item oi ON o.order_id   = oi.order_id
JOIN Product p     ON oi.product_id = p.product_id
GROUP BY o.order_mode, p.product_name
ORDER BY o.order_mode, total_qty DESC;

-- 10. Full order summary
SELECT o.order_id, o.order_date, o.order_mode,
       p.product_name, oi.quantity, p.unit_price,
       (p.unit_price * oi.quantity) AS line_total,
       pay.payment_method, pay.discount, pay.vat, pay.amount_paid
FROM `Order` o
JOIN Order_Item oi ON o.order_id    = oi.order_id
JOIN Product p     ON oi.product_id = p.product_id
JOIN Payment pay   ON o.order_id    = pay.order_id
ORDER BY o.order_date, o.order_time;