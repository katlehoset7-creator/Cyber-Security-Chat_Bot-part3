
CREATE DATABASE IF NOT EXISTS cyberbot_db;

USE cyberbot_db;

CREATE TABLE IF NOT EXISTS tasks (
    id INT AUTO_INCREMENT PRIMARY KEY,
    title VARCHAR(150) NOT NULL,
    description VARCHAR(500) NOT NULL,
    reminder_date DATETIME NULL,
    is_completed TINYINT(1) NOT NULL DEFAULT 0,
    created_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP
);

-- Test task
INSERT INTO tasks (title, description, reminder_date)
VALUES (
    'Enable two-factor authentication',
    'Turn on 2FA for your email and banking accounts.',
    DATE_ADD(NOW(), INTERVAL 3 DAY)
);

SELECT * FROM tasks;
