-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Mar 10, 2026 at 06:53 AM
-- Server version: 10.4.32-MariaDB
-- PHP Version: 8.2.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `coffee_shop_db`
--

DELIMITER $$
--
-- Procedures
--
CREATE DEFINER=`root`@`localhost` PROCEDURE `RestockProduct` (IN `p_id` INT, IN `p_quantity` INT)   BEGIN
UPDATE Products
SET stock_quantity = stock_quantity + p_quantity
WHERE product_id = p_id;
END$$

--
-- Functions
--
CREATE DEFINER=`root`@`localhost` FUNCTION `GetCustomerTotalSpend` (`cust_id` INT) RETURNS DECIMAL(10,2) DETERMINISTIC BEGIN
DECLARE total DECIMAL(10,2);
SELECT SUM(si.quantity * si.unit_price) INTO total
FROM Sales s
JOIN Sale_Items si ON s.sale_id = si.sale_id
WHERE s.customer_id = cust_id;
RETURN IFNULL(total, 0);
END$$

DELIMITER ;

-- --------------------------------------------------------

--
-- Table structure for table `customers`
--

CREATE TABLE `customers` (
  `customer_id` int(11) NOT NULL,
  `first_name` varchar(50) DEFAULT NULL,
  `last_name` varchar(50) DEFAULT NULL,
  `email` varchar(100) DEFAULT NULL,
  `loyalty_points` int(11) DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `customers`
--

INSERT INTO `customers` (`customer_id`, `first_name`, `last_name`, `email`, `loyalty_points`) VALUES
(1, 'John', 'Doe', 'john.doe@email.com', 15),
(2, 'Jane', 'Smith', 'jane.s@email.com', 40),
(3, 'Michael', 'Brown', 'mbrown@email.com', 10),
(4, 'Emily', 'Davis', 'emily.d@email.com', 55),
(5, 'Chris', 'Wilson', 'cwilson@email.com', 0),
(6, 'Sarah', 'Miller', 'smiller@email.com', 25),
(7, 'David', 'Moore', 'dmoore@email.com', 100),
(8, 'Laura', 'Taylor', 'ltaylor@email.com', 5),
(9, 'James', 'Anderson', 'janderson@email.com', 12),
(10, 'Linda', 'Thomas', 'lthomas@email.com', 30),
(11, 'Robert', 'Jackson', 'rjack@email.com', 0),
(12, 'Barbara', 'White', 'bwhite@email.com', 45),
(13, 'William', 'Harris', 'wharris@email.com', 60),
(14, 'Elizabeth', 'Martin', 'emartin@email.com', 10),
(15, 'Richard', 'Thompson', 'rthompson@email.com', 20),
(16, 'Susan', 'Garcia', 'sgarcia@email.com', 85),
(17, 'Joseph', 'Martinez', 'jmart@email.com', 5),
(18, 'Jessica', 'Robinson', 'jrob@email.com', 15),
(19, 'Thomas', 'Clark', 'tclark@email.com', 0),
(20, 'Sarah', 'Rodriguez', 'srod@email.com', 50),
(21, 'Charles', 'Lewis', 'clewis@email.com', 35),
(22, 'Karen', 'Lee', 'klee@email.com', 12),
(23, 'Christopher', 'Walker', 'cwalker@email.com', 8),
(24, 'Nancy', 'Hall', 'nhall@email.com', 22),
(25, 'Matthew', 'Allen', 'mallen@email.com', 90),
(26, 'Lisa', 'Young', 'lyoung@email.com', 14),
(27, 'Anthony', 'King', 'aking@email.com', 3),
(28, 'Betty', 'Wright', 'bwright@email.com', 28),
(29, 'Mark', 'Lopez', 'mlopez@email.com', 11),
(30, 'Sandra', 'Hill', 'shill@email.com', 42);

-- --------------------------------------------------------

--
-- Table structure for table `employees`
--

CREATE TABLE `employees` (
  `employee_id` int(11) NOT NULL,
  `first_name` varchar(50) DEFAULT NULL,
  `last_name` varchar(50) DEFAULT NULL,
  `role` varchar(30) DEFAULT NULL,
  `hire_date` date DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `employees`
--

INSERT INTO `employees` (`employee_id`, `first_name`, `last_name`, `role`, `hire_date`) VALUES
(1, 'Alice', 'Smith', 'Manager', '2023-01-15'),
(2, 'Bob', 'Jones', 'Barista', '2023-02-01'),
(3, 'Charlie', 'Brown', 'Barista', '2023-03-10'),
(4, 'Diana', 'Prince', 'Shift Lead', '2023-01-20'),
(5, 'Ethan', 'Hunt', 'Barista', '2023-05-05'),
(6, 'Fiona', 'Gallagher', 'Barista', '2023-06-12'),
(7, 'George', 'Costanza', 'Inventory', '2023-01-10'),
(8, 'Hannah', 'Abbott', 'Barista', '2023-07-01'),
(9, 'Ian', 'Malcolm', 'Security', '2023-08-15'),
(10, 'Julia', 'Child', 'Chef', '2023-02-20');

-- --------------------------------------------------------

--
-- Table structure for table `products`
--

CREATE TABLE `products` (
  `product_id` int(11) NOT NULL,
  `product_name` varchar(100) DEFAULT NULL,
  `category` varchar(50) DEFAULT NULL,
  `price` decimal(10,2) DEFAULT NULL,
  `stock_quantity` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Triggers `products`
--
DELIMITER $$
CREATE TRIGGER `trg_before_update_product_stock` BEFORE UPDATE ON `products` FOR EACH ROW BEGIN
    IF NEW.stock_quantity < 0 THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Error: Stock quantity cannot be negative.';
    END IF;
END
$$
DELIMITER ;

-- --------------------------------------------------------

--
-- Table structure for table `sales`
--

CREATE TABLE `sales` (
  `sale_id` int(11) NOT NULL,
  `sale_date` timestamp NOT NULL DEFAULT current_timestamp(),
  `customer_id` int(11) DEFAULT NULL,
  `employee_id` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `sales`
--

INSERT INTO `sales` (`sale_id`, `sale_date`, `customer_id`, `employee_id`) VALUES
(1, '2024-01-01 00:30:00', 1, 2),
(2, '2024-01-01 01:15:00', 2, 3),
(3, '2024-01-01 02:00:00', 3, 2),
(4, '2024-01-02 00:00:00', 4, 5),
(5, '2024-01-02 03:30:00', 5, 6),
(6, '2024-01-02 06:20:00', 6, 8),
(7, '2024-01-02 23:45:00', 7, 3),
(8, '2024-01-03 01:50:00', 8, 2),
(9, '2024-01-03 04:10:00', 9, 5),
(10, '2024-01-04 00:15:00', 10, 6),
(11, '2024-01-04 07:00:00', 11, 8),
(12, '2024-01-05 01:00:00', 12, 3),
(13, '2024-01-05 02:30:00', 13, 2),
(14, '2024-01-05 05:45:00', 14, 5),
(15, '2024-01-06 00:20:00', 15, 6),
(16, '2024-01-06 03:10:00', 16, 8),
(17, '2024-01-06 23:30:00', 17, 3),
(18, '2024-01-07 01:40:00', 18, 2),
(19, '2024-01-08 00:50:00', 19, 5),
(20, '2024-01-08 06:00:00', 20, 6),
(21, '2024-01-09 02:15:00', 21, 8),
(22, '2024-01-09 08:30:00', 22, 3),
(23, '2024-01-09 23:15:00', 23, 2),
(24, '2024-01-10 01:25:00', 24, 5),
(25, '2024-01-11 00:40:00', 25, 6),
(26, '2024-01-11 04:00:00', 26, 8),
(27, '2024-01-12 03:00:00', 27, 3),
(28, '2024-01-12 05:20:00', 28, 2),
(29, '2024-01-13 01:10:00', 29, 5),
(30, '2024-01-13 07:45:00', 30, 6);

-- --------------------------------------------------------

--
-- Table structure for table `sale_items`
--

CREATE TABLE `sale_items` (
  `item_id` int(11) NOT NULL,
  `sale_id` int(11) DEFAULT NULL,
  `product_id` int(11) DEFAULT NULL,
  `quantity` int(11) DEFAULT NULL,
  `unit_price` decimal(10,2) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Triggers `sale_items`
--
DELIMITER $$
CREATE TRIGGER `trg_after_delete_sale_item` AFTER DELETE ON `sale_items` FOR EACH ROW BEGIN
    UPDATE products
    SET stock_quantity = stock_quantity + OLD.quantity
    WHERE product_id = OLD.product_id;
END
$$
DELIMITER ;
DELIMITER $$
CREATE TRIGGER `trg_after_insert_sale_item` AFTER INSERT ON `sale_items` FOR EACH ROW BEGIN
    UPDATE products
    SET stock_quantity = stock_quantity - NEW.quantity
    WHERE product_id = NEW.product_id;
END
$$
DELIMITER ;

-- --------------------------------------------------------

--
-- Stand-in structure for view `view_daily_sales`
-- (See below for the actual view)
--
CREATE TABLE `view_daily_sales` (
`Sale_Date` date
,`Transaction_Count` bigint(21)
,`Total_Revenue` decimal(42,2)
);

-- --------------------------------------------------------

--
-- Stand-in structure for view `view_low_stock`
-- (See below for the actual view)
--
CREATE TABLE `view_low_stock` (
`product_name` varchar(100)
,`category` varchar(50)
,`stock_quantity` int(11)
);

-- --------------------------------------------------------

--
-- Stand-in structure for view `view_staff_performance`
-- (See below for the actual view)
--
CREATE TABLE `view_staff_performance` (
`employee_id` int(11)
,`Employee_Name` varchar(101)
,`Total_Sales_Handled` bigint(21)
,`Total_Revenue_Generated` decimal(42,2)
);

-- --------------------------------------------------------

--
-- Structure for view `view_daily_sales`
--
DROP TABLE IF EXISTS `view_daily_sales`;

CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `view_daily_sales`  AS SELECT cast(`s`.`sale_date` as date) AS `Sale_Date`, count(`s`.`sale_id`) AS `Transaction_Count`, sum(`si`.`quantity` * `si`.`unit_price`) AS `Total_Revenue` FROM (`sales` `s` join `sale_items` `si` on(`s`.`sale_id` = `si`.`sale_id`)) GROUP BY cast(`s`.`sale_date` as date) ;

-- --------------------------------------------------------

--
-- Structure for view `view_low_stock`
--
DROP TABLE IF EXISTS `view_low_stock`;

CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `view_low_stock`  AS SELECT `products`.`product_name` AS `product_name`, `products`.`category` AS `category`, `products`.`stock_quantity` AS `stock_quantity` FROM `products` WHERE `products`.`stock_quantity` < 15 ;

-- --------------------------------------------------------

--
-- Structure for view `view_staff_performance`
--
DROP TABLE IF EXISTS `view_staff_performance`;

CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `view_staff_performance`  AS SELECT `e`.`employee_id` AS `employee_id`, concat(`e`.`first_name`,' ',`e`.`last_name`) AS `Employee_Name`, count(`s`.`sale_id`) AS `Total_Sales_Handled`, sum(`si`.`quantity` * `si`.`unit_price`) AS `Total_Revenue_Generated` FROM ((`employees` `e` left join `sales` `s` on(`e`.`employee_id` = `s`.`employee_id`)) left join `sale_items` `si` on(`s`.`sale_id` = `si`.`sale_id`)) GROUP BY `e`.`employee_id`, `e`.`first_name`, `e`.`last_name` ;

--
-- Indexes for dumped tables
--

--
-- Indexes for table `customers`
--
ALTER TABLE `customers`
  ADD PRIMARY KEY (`customer_id`);

--
-- Indexes for table `employees`
--
ALTER TABLE `employees`
  ADD PRIMARY KEY (`employee_id`);

--
-- Indexes for table `products`
--
ALTER TABLE `products`
  ADD PRIMARY KEY (`product_id`);

--
-- Indexes for table `sales`
--
ALTER TABLE `sales`
  ADD PRIMARY KEY (`sale_id`),
  ADD KEY `customer_id` (`customer_id`),
  ADD KEY `employee_id` (`employee_id`);

--
-- Indexes for table `sale_items`
--
ALTER TABLE `sale_items`
  ADD PRIMARY KEY (`item_id`),
  ADD KEY `sale_id` (`sale_id`),
  ADD KEY `product_id` (`product_id`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `customers`
--
ALTER TABLE `customers`
  MODIFY `customer_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=31;

--
-- AUTO_INCREMENT for table `employees`
--
ALTER TABLE `employees`
  MODIFY `employee_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=11;

--
-- AUTO_INCREMENT for table `products`
--
ALTER TABLE `products`
  MODIFY `product_id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `sales`
--
ALTER TABLE `sales`
  MODIFY `sale_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=31;

--
-- AUTO_INCREMENT for table `sale_items`
--
ALTER TABLE `sale_items`
  MODIFY `item_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=31;

--
-- Constraints for dumped tables
--

--
-- Constraints for table `sales`
--
ALTER TABLE `sales`
  ADD CONSTRAINT `sales_ibfk_1` FOREIGN KEY (`customer_id`) REFERENCES `customers` (`customer_id`),
  ADD CONSTRAINT `sales_ibfk_2` FOREIGN KEY (`employee_id`) REFERENCES `employees` (`employee_id`);

--
-- Constraints for table `sale_items`
--
ALTER TABLE `sale_items`
  ADD CONSTRAINT `sale_items_ibfk_1` FOREIGN KEY (`sale_id`) REFERENCES `sales` (`sale_id`),
  ADD CONSTRAINT `sale_items_ibfk_2` FOREIGN KEY (`product_id`) REFERENCES `products` (`product_id`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
