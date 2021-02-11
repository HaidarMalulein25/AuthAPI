-- phpMyAdmin SQL Dump
-- version 5.0.4
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Feb 12, 2021 at 12:45 AM
-- Server version: 10.4.17-MariaDB
-- PHP Version: 7.4.14

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `authdb`
--

-- --------------------------------------------------------

--
-- Table structure for table `users`
--

CREATE TABLE `users` (
  `id` varchar(36) NOT NULL,
  `created` datetime NOT NULL,
  `modified` datetime NOT NULL,
  `username` varchar(250) NOT NULL,
  `password` varchar(350) NOT NULL,
  `name` varchar(350) NOT NULL,
  `email` varchar(350) NOT NULL,
  `salt` varchar(250) NOT NULL,
  `token` varchar(250) NOT NULL,
  `refresh_token` varchar(250) NOT NULL,
  `token_expiration` datetime NOT NULL,
  `refresh_token_expiration` datetime NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Dumping data for table `users`
--

INSERT INTO `users` (`id`, `created`, `modified`, `username`, `password`, `name`, `email`, `salt`, `token`, `refresh_token`, `token_expiration`, `refresh_token_expiration`) VALUES
('d8ba6034-8e1c-4e90-afc0-b15e334363ea', '2021-02-12 01:10:43', '2021-02-12 01:10:47', 'HaidarNew', 'r/unikPAe6CZWhGKLX5eNKxo5KuCv222UskhcmCZFdk=', 'Haidar New', 'haidar@1223.com', 'Jc1N3N4UefQkjVzn1CEs', 'QxlUOm6nr8OIUIajkixqEqNJdw8YakdbXpfKPCi5KqI45gzEEjYsYoER9L1I5gJWCTqvX73PEEzz6NVL/OP+5g==', 'gKGUyILq86DTbjE4xAhXETWN3BrqsQEFHWr1Fw8fUT3/lkEb/iv6gBATiDdPzaoTHIqMPnzp65D3djb2NiTDIA==', '2021-02-13 01:10:47', '2021-02-19 01:10:47'),
('de688cd9-66b6-45cf-bf07-44e69566066c', '2021-02-12 01:16:16', '2021-02-12 01:17:43', 'FinalTest', 'GDiQTRoNK/Hl5u/WhOuMeL0fSssagQw3LdR8q3vzT6w=', 'finaltest@gmail.com', 'finaltest@gmail.com', 'iuxI+hPlA/xL6wmBEpg1', 'tiZeQ+rIDaQLSjhSzn7O/Av9I4/oMn35lBrU3B4p3pY2G55BaZCGU9hsMtR0S7KvIT8jO3ktSmufmOd184l1kA==', '19gqPiVHjgqQ/DoYaL29h5wPaQBqp3a99A4ucy85pGLHHrE+9RGMmjbazJqmslviY20tzO8z4O0pTTkhbT40iA==', '2021-02-13 01:17:43', '2021-02-19 01:17:43'),
('e7e73df9-05aa-40a3-b386-4e02dcaeed08', '2021-02-12 01:07:17', '2021-02-12 01:07:53', 'HaidarTest', 'Nz7/LsSsc5Fas4l6ieEpwFjBSAS8WzQaJF4a09GyGA8=', 'haidar@test.com', 'haidar@test.com', 'hbK4F6H4/+PqiHmLgjEg', 'bnwir3ruF/0mhHhdd8G049NbBeW/KVwKTWpt6XmuNDqS4nodykr/sYnw6zLzRgbIcvuveLGmEpOH1RTIg5ZVtg==', '8N+jomWWJq4M52uoPpAPwCBycNKMBDdTEhIRHXLu1dlWbYaVkWA/wI/kHNVxVJHtoQhqaEOZFo6/PytJhZ8ANg==', '2021-02-13 01:07:24', '2021-02-19 01:07:24');

--
-- Indexes for dumped tables
--

--
-- Indexes for table `users`
--
ALTER TABLE `users`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `username` (`username`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
