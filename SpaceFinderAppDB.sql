
DROP DATABASE IF EXISTS SpaceFinderAppDB;
CREATE DATABASE SpaceFinderAppDB;
USE SpaceFinderAppDB;

CREATE TABLE Users (
    user_id INT PRIMARY KEY AUTO_INCREMENT,
    email VARCHAR(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL UNIQUE,
    password VARCHAR(300) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL, 
    fullname VARCHAR(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL, 
    r_number VARCHAR(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL UNIQUE,
    phone_number VARCHAR(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL,
    profile_picture_url VARCHAR(255) NULL, 
    created_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE PasswordResetTokens (
    password_reset_token_id INT PRIMARY KEY AUTO_INCREMENT,
    user_id INT NOT NULL,
    token VARCHAR(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL UNIQUE,
    expires_at DATETIME NOT NULL,
    created_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    is_used BOOLEAN NOT NULL DEFAULT FALSE,
    CONSTRAINT FK_PasswordResetTokens_Users FOREIGN KEY (user_id) REFERENCES Users(user_id)
);

CREATE TABLE Campuses (
    campus_id INT PRIMARY KEY AUTO_INCREMENT,
    campus_name VARCHAR(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL UNIQUE
);

CREATE TABLE Features (
    feature_id INT PRIMARY KEY AUTO_INCREMENT,
    feature_name VARCHAR(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL UNIQUE
);

CREATE TABLE Classrooms (
    classroom_id INT PRIMARY KEY AUTO_INCREMENT,
    room_number VARCHAR(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
    capacity INT,
    campus_id INT NOT NULL,
    CONSTRAINT FK_Classrooms_Campuses FOREIGN KEY (campus_id) REFERENCES Campuses(campus_id)
);

CREATE TABLE ClassroomFeatures (
    classroom_id INT NOT NULL,
    feature_id INT NOT NULL,
    CONSTRAINT PK_ClassroomFeatures PRIMARY KEY (classroom_id, feature_id),
    CONSTRAINT FK_ClassroomFeatures_Classrooms FOREIGN KEY (classroom_id) REFERENCES Classrooms(classroom_id),
    CONSTRAINT FK_ClassroomFeatures_Features FOREIGN KEY (feature_id) REFERENCES Features(feature_id)
);

CREATE TABLE Schedules (
    schedule_id INT PRIMARY KEY AUTO_INCREMENT,
    user_id INT NOT NULL,
    classroom_id INT NOT NULL,
    course_name VARCHAR(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
    day_of_week VARCHAR(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
    start_time TIME NOT NULL,
    end_time TIME NOT NULL,
    start_date DATE NOT NULL,
    end_date DATE NOT NULL,
    CONSTRAINT FK_Schedules_Users FOREIGN KEY (user_id) REFERENCES Users (user_id),
    CONSTRAINT FK_Schedules_Classrooms FOREIGN KEY (classroom_id) REFERENCES Classrooms (classroom_id)
);

CREATE TABLE Bookings (
    booking_id INT PRIMARY KEY AUTO_INCREMENT,
    user_id INT NOT NULL,
    classroom_id INT NOT NULL,
    start_time DATETIME NOT NULL,
    end_time DATETIME NOT NULL,
    booking_date DATE NOT NULL,
    people_amount INT,
    status VARCHAR(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL DEFAULT 'Confirmed',
    created_at DATETIME DEFAULT NOW(),
    CONSTRAINT FK_Bookings_Users FOREIGN KEY (user_id) REFERENCES Users (user_id),
    CONSTRAINT FK_Bookings_Classrooms FOREIGN KEY (classroom_id) REFERENCES Classrooms (classroom_id),
    CONSTRAINT UQ_Booking_Time_Room UNIQUE (classroom_id, start_time, end_time)
);

SET @institutional_user_id = 1;


SET @course_start_date = '2025-02-10'; 
SET @course_end_date = '2025-06-16';   


INSERT IGNORE INTO Campuses (campus_name) VALUES
('Campus Kruidtuin'),
('Campus De Vest'),
('Campus De Ham');

-- Campus Kruidtuin 
-- Campus Kruidtuin (campus_id 1)
INSERT IGNORE INTO Classrooms (room_number, capacity, campus_id) VALUES
('T0.01', 20, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus Kruidtuin')),
('T0.02', 20, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus Kruidtuin')),
('T0.03', 20, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus Kruidtuin')),
('T0.04', 20, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus Kruidtuin')),
('T0.05', 20, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus Kruidtuin')),
('T0.06', 20, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus Kruidtuin')),
('T0.07', 20, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus Kruidtuin')),
('T0.08', 20, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus Kruidtuin')),
('T0.09', 20, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus Kruidtuin')),
('T0.10', 20, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus Kruidtuin')),
('T1.01', 20, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus Kruidtuin')),
('T1.02', 50, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus Kruidtuin')),
('T1.03', 20, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus Kruidtuin')),
('T1.04', 20, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus Kruidtuin')),
('T1.05', 20, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus Kruidtuin')),
('T1.06', 20, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus Kruidtuin')),
('T1.07', 20, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus Kruidtuin')),
('T1.08', 20, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus Kruidtuin')),
('T1.09', 20, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus Kruidtuin')),
('T1.10', 20, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus Kruidtuin')),
('T2.01', 20, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus Kruidtuin')),
('T2.02', 20, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus Kruidtuin')),
('T2.03', 20, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus Kruidtuin')),
('T2.04', 20, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus Kruidtuin')),
('T2.05', 20, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus Kruidtuin')),
('T2.06', 20, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus Kruidtuin')),
('T2.07', 20, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus Kruidtuin')),
('T2.08', 20, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus Kruidtuin')),
('T2.09', 20, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus Kruidtuin')),
('T2.10', 20, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus Kruidtuin')),
('G0.01', 30, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus Kruidtuin')),
('G0.02', 30, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus Kruidtuin')),
('G0.03', 30, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus Kruidtuin')),
('G0.04', 30, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus Kruidtuin')),
('G0.05', 30, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus Kruidtuin')),
('G0.06', 30, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus Kruidtuin')),
('G0.07', 30, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus Kruidtuin')),
('G0.08', 30, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus Kruidtuin')),
('G0.09', 30, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus Kruidtuin')),
('G0.10', 30, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus Kruidtuin')),
('G0.11', 30, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus Kruidtuin')),
('G0.12', 30, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus Kruidtuin')),
('G0.13', 30, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus Kruidtuin')),
('G0.14', 30, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus Kruidtuin')),
('G0.15', 30, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus Kruidtuin')),
('G1.16', 30, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus Kruidtuin')),
('G1.17', 30, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus Kruidtuin')),
('G1.18', 30, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus Kruidtuin')),
('G1.19', 30, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus Kruidtuin')),
('G1.20', 30, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus Kruidtuin')),
('G1.21', 30, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus Kruidtuin')),
('G1.22', 30, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus Kruidtuin')),
('G1.23', 30, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus Kruidtuin')),
('G1.24', 30, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus Kruidtuin')),
('G1.25', 30, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus Kruidtuin')), 
('G1.26', 30, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus Kruidtuin')),
('G1.27', 30, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus Kruidtuin')),
('G1.28', 30, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus Kruidtuin')),
('G1.29', 30, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus Kruidtuin')),
('G1.30', 30, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus Kruidtuin')),
('K1.03', 40, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus Kruidtuin')),
('K3.05', 30, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus Kruidtuin')),
('C3.10', 50, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus Kruidtuin')), -- C3.10 is in Campus Kruidtuin for 'Digital Skills'
('-K1.01', 20, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus Kruidtuin')),
('-K1.02', 20, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus Kruidtuin')),
('-K1.03', 20, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus Kruidtuin')),
('-K1.04', 20, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus Kruidtuin')),
('-K1.05', 20, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus Kruidtuin')),
('-K1.06', 20, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus Kruidtuin')),
('-K1.07', 20, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus Kruidtuin')),
('K0.01', 20, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus Kruidtuin')),
('K0.02', 20, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus Kruidtuin')),
('K0.03', 20, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus Kruidtuin')),
('K0.04', 20, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus Kruidtuin')),
('K0.05', 20, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus Kruidtuin')),
('K0.06', 20, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus Kruidtuin')),
('K0.07', 20, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus Kruidtuin')),
('K1.01', 20, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus Kruidtuin')),
('K1.02', 20, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus Kruidtuin')),
('K1.04', 20, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus Kruidtuin')),
('K1.05', 20, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus Kruidtuin')),
('K1.06', 20, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus Kruidtuin')),
('K1.07', 20, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus Kruidtuin')),
('K2.01', 20, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus Kruidtuin')),
('K2.02', 20, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus Kruidtuin')),
('K2.03', 20, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus Kruidtuin')),
('K2.04', 20, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus Kruidtuin')),
('K2.05', 20, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus Kruidtuin')),
('K2.06', 20, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus Kruidtuin')),
('K2.07', 20, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus Kruidtuin')),
('K3.01', 20, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus Kruidtuin')),
('K3.02', 20, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus Kruidtuin')),
('K3.03', 20, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus Kruidtuin')),
('K3.04', 20, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus Kruidtuin')),
('K3.06', 20, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus Kruidtuin')),
('K3.07', 20, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus Kruidtuin'));


-- Campus De Vest (campus_id 2)
INSERT IGNORE INTO Classrooms (room_number, capacity, campus_id) VALUES
('C1.01', 20, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus De Vest')),
('C1.02', 20, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus De Vest')),
('C1.03', 20, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus De Vest')),
('C1.04', 20, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus De Vest')),
('C1.05', 20, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus De Vest')),
('C1.06', 20, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus De Vest')),
('C1.07', 20, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus De Vest')),
('C1.08', 20, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus De Vest')),
('C1.09', 20, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus De Vest')),
('C1.10', 20, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus De Vest')),
('C1.11', 20, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus De Vest')),
('C1.12', 20, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus De Vest')),
('C1.13', 20, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus De Vest')),
('C1.14', 20, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus De Vest')),
('C1.15', 20, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus De Vest')),
('C1.16', 20, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus De Vest')),
('C1.17', 20, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus De Vest')),
('C1.18', 20, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus De Vest')),
('C1.19', 20, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus De Vest')),
('C1.20', 20, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus De Vest')),
('C1.21', 20, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus De Vest')),
('C1.22', 20, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus De Vest')),
('C1.23', 20, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus De Vest')),
('C1.24', 20, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus De Vest')),
('C1.25', 20, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus De Vest')),
('C1.26', 20, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus De Vest')),
('C1.27', 20, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus De Vest')),
('C1.28', 20, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus De Vest')),
('C1.29', 20, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus De Vest')),
('C1.30', 20, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus De Vest')),
('C2.01', 20, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus De Vest')),
('C2.02', 20, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus De Vest')),
('C2.03', 20, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus De Vest')),
('C2.04', 20, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus De Vest')),
('C2.05', 20, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus De Vest')),
('C2.06', 20, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus De Vest')),
('C2.07', 20, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus De Vest')), -- for Business Intelligence Fundamentals
('C2.08', 20, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus De Vest')),
('C2.09', 20, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus De Vest')),
('C2.10', 20, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus De Vest')),
('C2.11', 20, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus De Vest')),
('C2.12', 20, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus De Vest')),
('C2.13', 20, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus De Vest')),
('C2.14', 20, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus De Vest')),
('C2.15', 20, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus De Vest')),
('C2.16', 20, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus De Vest')),
('C2.17', 20, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus De Vest')),
('C2.18', 20, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus De Vest')),
('C2.19', 20, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus De Vest')),
('C2.20', 20, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus De Vest')),
('C2.21', 20, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus De Vest')),
('C2.22', 20, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus De Vest')),
('C2.23', 20, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus De Vest')),
('C2.24', 20, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus De Vest')),
('C2.25', 20, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus De Vest')), -- for Personal Development
('C2.26', 20, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus De Vest')),
('C2.27', 20, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus De Vest')),
('C2.28', 20, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus De Vest')), -- for Data Processing & Analysis, Monitoraat DSPS
('C2.29', 20, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus De Vest')),
('C2.30', 20, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus De Vest')),
('C3.01', 20, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus De Vest')),
('C3.02', 20, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus De Vest')),
('C3.03', 60, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus De Vest')),
('C3.04', 20, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus De Vest')),
('C3.05', 20, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus De Vest')), -- Used for Cyber Crime Fundamentals
('C3.06', 20, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus De Vest')),
('C3.07', 20, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus De Vest')),
('C3.08', 20, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus De Vest')),
('C3.09', 20, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus De Vest')),
('C3.10', 50, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus De Vest')), -- C3.10 is in Campus De Vest for 'Identity & Access' and 'Nederlands 2' and 'Computer Fundamentals'
('C3.11', 20, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus De Vest')), -- Used for Inspiration Lab, Scripting (V)
('C3.12', 20, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus De Vest')),
('C3.13', 20, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus De Vest')),
('C3.14', 20, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus De Vest')),
('C3.15', 20, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus De Vest')),
('C3.16', 20, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus De Vest')),
('C3.17', 20, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus De Vest')),
('C3.18', 20, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus De Vest')),
('C3.19', 20, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus De Vest')),
('C3.20', 20, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus De Vest')),
('C3.21', 20, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus De Vest')),
('C3.22', 20, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus De Vest')),
('C3.23', 20, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus De Vest')),
('C3.24', 20, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus De Vest')),
('C3.25', 20, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus De Vest')),
('C3.26', 20, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus De Vest')),
('C3.27', 20, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus De Vest')),
('C3.28', 20, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus De Vest')),
('C3.29', 20, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus De Vest')),
('C3.30', 20, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus De Vest'));

-- Campus De Ham (campus_id 3)
INSERT IGNORE INTO Classrooms (room_number, capacity, campus_id) VALUES
('Z1.13', 40, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus De Ham')),
('Z1.14', 150, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus De Ham')),
('Z1.15', 150, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus De Ham')),
('Z1.01', 25, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus De Ham')),
('Z1.02', 25, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus De Ham')),
('Z1.03', 25, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus De Ham')),
('Z2.03', 50, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus De Ham')),
('Z2.09', 45, (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus De Ham'));


INSERT IGNORE INTO Features (feature_name) VALUES
('projectors'),
('whiteboards'),
('smartboards'),
('window_blinds'),
('chair with extendable table'),
('historical artifacts'),
('height adjustable chairs'),
('chargers along the tables'),
('classroom sound system (speakers)'),
('huge projector'),
('big whiteboard');

INSERT INTO ClassroomFeatures (classroom_id, feature_id)
SELECT c.classroom_id, f.feature_id
FROM Classrooms c
JOIN Features f ON f.feature_name = 'window_blinds'
ON DUPLICATE KEY UPDATE classroom_id = classroom_id;

INSERT INTO ClassroomFeatures (classroom_id, feature_id)
SELECT c.classroom_id, f.feature_id
FROM Classrooms c
JOIN Features f ON f.feature_name IN ('projectors', 'whiteboards', 'smartboards')
ON DUPLICATE KEY UPDATE classroom_id = classroom_id;

INSERT INTO ClassroomFeatures (classroom_id, feature_id)
SELECT c.classroom_id, f.feature_id
FROM Classrooms c
JOIN Campuses cp ON c.campus_id = cp.campus_id
JOIN Features f ON f.feature_name = 'historical artifacts'
WHERE cp.campus_name = 'Campus Kruidtuin' AND c.room_number = 'T2.10'
ON DUPLICATE KEY UPDATE classroom_id = classroom_id;

DELETE cf
FROM ClassroomFeatures cf
JOIN Classrooms c ON cf.classroom_id = c.classroom_id
JOIN Campuses cp ON c.campus_id = cp.campus_id
JOIN Features f ON cf.feature_id = f.feature_id
WHERE cp.campus_name = 'Campus Kruidtuin'
  AND c.room_number BETWEEN 'G1.16' AND 'G1.30'
  AND f.feature_name IN ('whiteboards', 'smartboards');


INSERT INTO ClassroomFeatures (classroom_id, feature_id)
SELECT c.classroom_id, f.feature_id
FROM Classrooms c
JOIN Campuses cp ON c.campus_id = cp.campus_id
JOIN Features f ON f.feature_name IN ('height adjustable chairs', 'classroom sound system (speakers)')
WHERE cp.campus_name = 'Campus De Vest'
ON DUPLICATE KEY UPDATE classroom_id = classroom_id;

DELETE cf
FROM ClassroomFeatures cf
JOIN Classrooms c ON cf.classroom_id = c.classroom_id
JOIN Campuses cp ON c.campus_id = cp.campus_id
JOIN Features f ON cf.feature_id = f.feature_id
WHERE cp.campus_name = 'Campus De Ham'
  AND f.feature_name IN ('projectors', 'whiteboards', 'smartboards');

INSERT INTO ClassroomFeatures (classroom_id, feature_id)
SELECT c.classroom_id, f.feature_id
FROM Classrooms c
JOIN Campuses cp ON c.campus_id = cp.campus_id
JOIN Features f ON f.feature_name IN ('huge projector', 'big whiteboard', 'chair with extendable table')
WHERE cp.campus_name = 'Campus De Ham'
ON DUPLICATE KEY UPDATE classroom_id = classroom_id;

INSERT INTO ClassroomFeatures (classroom_id, feature_id)
SELECT c.classroom_id, f.feature_id
FROM Classrooms c
JOIN Campuses cp ON c.campus_id = cp.campus_id
JOIN Features f ON f.feature_name = 'chargers along the tables'
WHERE (cp.campus_name = 'Campus De Vest')
   OR (cp.campus_name = 'Campus Kruidtuin' AND c.room_number LIKE 'G0.%')
   OR (cp.campus_name = 'Campus Kruidtuin' AND c.room_number LIKE 'G1.%')
ON DUPLICATE KEY UPDATE classroom_id = classroom_id;

INSERT INTO Schedules (user_id, classroom_id, course_name, day_of_week, start_time, end_time, start_date, end_date)
VALUES (
    @institutional_user_id,
    (SELECT classroom_id FROM Classrooms WHERE room_number = 'K1.03' AND campus_id = (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus Kruidtuin')),
    'Nederlands 2',
    'Monday', '10:45:00', '12:45:00', @course_start_date, @course_end_date
);

INSERT INTO Schedules (user_id, classroom_id, course_name, day_of_week, start_time, end_time, start_date, end_date)
VALUES (
    @institutional_user_id,
    (SELECT classroom_id FROM Classrooms WHERE room_number = 'Z2.03' AND campus_id = (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus De Ham')),
    'Management Skills',
    'Monday', '13:45:00', '15:45:00', @course_start_date, @course_end_date
);

INSERT INTO Schedules (user_id, classroom_id, course_name, day_of_week, start_time, end_time, start_date, end_date)
VALUES (
    @institutional_user_id,
    (SELECT classroom_id FROM Classrooms WHERE room_number = 'T1.02' AND campus_id = (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus Kruidtuin')),
    'Procurement & Supply Chain Management',
    'Monday', '16:00:00', '18:00:00', @course_start_date, @course_end_date
);

INSERT INTO Schedules (user_id, classroom_id, course_name, day_of_week, start_time, end_time, start_date, end_date)
VALUES (
    @institutional_user_id,
    (SELECT classroom_id FROM Classrooms WHERE room_number = 'C3.03' AND campus_id = (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus De Vest')),
    'Data Engineering',
    'Tuesday', '08:30:00', '18:00:00', @course_start_date, @course_end_date
);

INSERT INTO Schedules (user_id, classroom_id, course_name, day_of_week, start_time, end_time, start_date, end_date)
VALUES (
    @institutional_user_id,
    (SELECT classroom_id FROM Classrooms WHERE room_number = 'K3.05' AND campus_id = (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus Kruidtuin')),
    'Programming Fundamentals',
    'Wednesday', '08:30:00', '12:45:00', @course_start_date, @course_end_date
);

INSERT INTO Schedules (user_id, classroom_id, course_name, day_of_week, start_time, end_time, start_date, end_date)
VALUES (
    @institutional_user_id,
    (SELECT classroom_id FROM Classrooms WHERE room_number = 'C3.10' AND campus_id = (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus De Vest')),
    'Identity & Access Management',
    'Wednesday', '13:45:00', '15:45:00', @course_start_date, @course_end_date
);

INSERT INTO Schedules (user_id, classroom_id, course_name, day_of_week, start_time, end_time, start_date, end_date)
VALUES (
    @institutional_user_id,
    (SELECT classroom_id FROM Classrooms WHERE room_number = 'Z2.09' AND campus_id = (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus De Ham')),
    'Professional Communication',
    'Thursday', '08:30:00', '10:30:00', @course_start_date, @course_end_date
);

INSERT INTO Schedules (user_id, classroom_id, course_name, day_of_week, start_time, end_time, start_date, end_date)
VALUES (
    @institutional_user_id,
    (SELECT classroom_id FROM Classrooms WHERE room_number = 'C3.10' AND campus_id = (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus Kruidtuin')),
    'Digital Skills',
    'Thursday', '10:45:00', '12:45:00', @course_start_date, @course_end_date
);

INSERT INTO Schedules (user_id, classroom_id, course_name, day_of_week, start_time, end_time, start_date, end_date)
VALUES (
    @institutional_user_id,
    (SELECT classroom_id FROM Classrooms WHERE room_number = 'C3.10' AND campus_id = (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus De Vest')),
    'Nederlands 2',
    'Thursday', '16:00:00', '17:00:00', @course_start_date, @course_end_date
);

INSERT INTO Schedules (user_id, classroom_id, course_name, day_of_week, start_time, end_time, start_date, end_date)
VALUES (
    @institutional_user_id,
    (SELECT classroom_id FROM Classrooms WHERE room_number = 'C2.15' AND campus_id = (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus De Vest')),
    'AI Tools',
    'Friday', '08:30:00', '10:30:00', @course_start_date, @course_end_date
);


INSERT INTO Schedules (user_id, classroom_id, course_name, day_of_week, start_time, end_time, start_date, end_date)
VALUES (
    @institutional_user_id,
    (SELECT classroom_id FROM Classrooms WHERE room_number = 'C2.28' AND campus_id = (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus De Vest')),
    'Data Processing & Analysis (V)',
    'Monday', '08:30:00', '10:30:00', @course_start_date, @course_end_date
);

INSERT INTO Schedules (user_id, classroom_id, course_name, day_of_week, start_time, end_time, start_date, end_date)
VALUES (
    @institutional_user_id,
    (SELECT classroom_id FROM Classrooms WHERE room_number = 'C3.11' AND campus_id = (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus De Vest')),
    'Inspiration Lab (V)',
    'Monday', '10:45:00', '12:45:00', @course_start_date, @course_end_date
);

INSERT INTO Schedules (user_id, classroom_id, course_name, day_of_week, start_time, end_time, start_date, end_date)
VALUES (
    @institutional_user_id,
    (SELECT classroom_id FROM Classrooms WHERE room_number = 'C3.05' AND campus_id = (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus De Vest')),
    'Cyber Crime Fundamentals (V)',
    'Monday', '13:45:00', '15:45:00', @course_start_date, @course_end_date
);

INSERT INTO Schedules (user_id, classroom_id, course_name, day_of_week, start_time, end_time, start_date, end_date)
VALUES (
    @institutional_user_id,
    (SELECT classroom_id FROM Classrooms WHERE room_number = 'C3.11' AND campus_id = (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus De Vest')),
    'Scripting (V)',
    'Monday', '16:00:00', '18:00:00', @course_start_date, @course_end_date
);

INSERT INTO Schedules (user_id, classroom_id, course_name, day_of_week, start_time, end_time, start_date, end_date)
VALUES (
    @institutional_user_id,
    (SELECT classroom_id FROM Classrooms WHERE room_number = 'T1.02' AND campus_id = (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus Kruidtuin')),
    'Introduction to Financial Management (V)',
    'Tuesday', '08:30:00', '10:30:00', @course_start_date, @course_end_date
);

INSERT INTO Schedules (user_id, classroom_id, course_name, day_of_week, start_time, end_time, start_date, end_date)
VALUES (
    @institutional_user_id,
    (SELECT classroom_id FROM Classrooms WHERE room_number = 'T2.10' AND campus_id = (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus Kruidtuin')),
    'Français 2 (V)',
    'Wednesday', '10:45:00', '12:45:00', @course_start_date, @course_end_date
);

INSERT INTO Schedules (user_id, classroom_id, course_name, day_of_week, start_time, end_time, start_date, end_date)
VALUES (
    @institutional_user_id,
    (SELECT classroom_id FROM Classrooms WHERE room_number = 'G1.25' AND campus_id = (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus Kruidtuin')),
    'AI Tools (V)',
    'Wednesday', '13:34:00', '15:45:00', @course_start_date, @course_end_date
);

INSERT INTO Schedules (user_id, classroom_id, course_name, day_of_week, start_time, end_time, start_date, end_date)
VALUES (
    @institutional_user_id,
    (SELECT classroom_id FROM Classrooms WHERE room_number = 'C3.10' AND campus_id = (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus De Vest')),
    'Computer Fundamentals (V)',
    'Wednesday', '16:00:00', '18:00:00', @course_start_date, @course_end_date
);

INSERT INTO Schedules (user_id, classroom_id, course_name, day_of_week, start_time, end_time, start_date, end_date)
VALUES (
    @institutional_user_id,
    (SELECT classroom_id FROM Classrooms WHERE room_number = 'C2.25' AND campus_id = (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus De Vest')),
    'Personal Development (V)',
    'Thursday', '08:30:00', '10:30:00', @course_start_date, @course_end_date
);

INSERT INTO Schedules (user_id, classroom_id, course_name, day_of_week, start_time, end_time, start_date, end_date)
VALUES (
    @institutional_user_id,
    (SELECT classroom_id FROM Classrooms WHERE room_number = 'C3.11' AND campus_id = (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus De Vest')),
    'Scripting (V)',
    'Thursday', '11:45:00', '12:45:00', @course_start_date, @course_end_date
);

INSERT INTO Schedules (user_id, classroom_id, course_name, day_of_week, start_time, end_time, start_date, end_date)
VALUES (
    @institutional_user_id,
    (SELECT classroom_id FROM Classrooms WHERE room_number = 'C2.28' AND campus_id = (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus De Vest')),
    'Monitoraat DSPS (V)',
    'Thursday', '16:00:00', '18:00:00', @course_start_date, @course_end_date
);

INSERT INTO Schedules (user_id, classroom_id, course_name, day_of_week, start_time, end_time, start_date, end_date)
VALUES (
    @institutional_user_id,
    (SELECT classroom_id FROM Classrooms WHERE room_number = 'C2.07' AND campus_id = (SELECT campus_id FROM Campuses WHERE campus_name = 'Campus De Vest')),
    'Business Intelligence Fundamentals (V)',
    'Friday', '10:45:00', '12:45:00', @course_start_date, @course_end_date
);


