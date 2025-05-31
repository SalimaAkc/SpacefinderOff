-- Inserting Campuse names into the Campuses table
USE SpaceFinderAppDB;
INSERT IGNORE INTO Campuses (campus_name) VALUES
('Campus Kruidtuin'),
('Campus De Vest'),
('Campus De Ham');

-- Inserting Classroom Features
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
