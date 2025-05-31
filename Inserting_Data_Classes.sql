-- Inserting classes in the Schedules table for our DSPS class and another DSPS class schedule to collaborate the schedule

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