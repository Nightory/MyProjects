create database fitnes_club;
use fitnes_club;

CREATE TABLE Abonement
(
	ID_buy_abonement     TINYINT NOT NULL,
	Date_buy             DATE NOT NULL,
	Date_end             DATE NOT NULL,
	ID_client            SMALLINT NOT NULL,
	ID_abonement         TINYINT NOT NULL
);

ALTER TABLE Abonement
ADD PRIMARY KEY (ID_buy_abonement);

CREATE TABLE Client
(
	ID_client            SMALLINT NOT NULL,
	Secondname           VARCHAR(30) NOT NULL,
	Date_birthday        DATE NOT NULL,
	Sex                  VARCHAR(10) NOT NULL,
	Phone_number         VARCHAR(15) NOT NULL,
	Name                 VARCHAR(20) NOT NULL,
	Patronymic           VARCHAR(30) NULL
);

ALTER TABLE Client
ADD PRIMARY KEY (ID_client);

CREATE TABLE Course
(
	ID_course            TINYINT NOT NULL,
	Name_course          VARCHAR(40) NOT NULL
);

ALTER TABLE Course
ADD PRIMARY KEY (ID_course);

CREATE TABLE Education
(
	ID_education         TINYINT NOT NULL,
	Name_education       VARCHAR(40) NOT NULL
);

ALTER TABLE Education
ADD PRIMARY KEY (ID_education);

CREATE TABLE Gym
(
	ID_gym               TINYINT NOT NULL,
	FIO_trainer          VARCHAR(50) NOT NULL,
	Type_gym             VARCHAR(20) NOT NULL,
	Working_time         VARCHAR(20) NOT NULL
);

ALTER TABLE Gym
ADD PRIMARY KEY (ID_gym);
CREATE TABLE Price_list
(
	Type_abonement       VARCHAR(30) NOT NULL,
	Price                DECIMAL(5,2) NOT NULL,
	Number_visit         TINYINT NOT NULL,
	ID_abonement         TINYINT NOT NULL,
	Validity_period      TINYINT NOT NULL,
	Name_service         VARCHAR(30) NOT NULL
);

ALTER TABLE Price_list
ADD PRIMARY KEY (ID_abonement);

CREATE TABLE Trainer
(
	ID_trainer           TINYINT NOT NULL,
	Sex                  VARCHAR(10) NOT NULL,
	Secondname           VARCHAR(30) NOT NULL,
	Salary               DECIMAL(6,2) NOT NULL,
	Experience           TINYINT NOT NULL,
	Date_birthday        DATE NOT NULL,
	Name                 VARCHAR(20) NOT NULL,
	Patronymic           VARCHAR(30) NULL
);

ALTER TABLE Trainer
ADD PRIMARY KEY (ID_trainer);

CREATE TABLE Trainer_course
(
	ID_course            TINYINT NOT NULL,
	ID_trainer           TINYINT NOT NULL
);
ALTER TABLE Trainer_course
ADD PRIMARY KEY (ID_course,ID_trainer);

CREATE TABLE Trainer_education
(
	ID_education         TINYINT NOT NULL,
	ID_trainer           TINYINT NOT NULL
);

ALTER TABLE Trainer_education
ADD PRIMARY KEY (ID_education,ID_trainer);

CREATE TABLE Visit
(
	ID_workout           SMALLINT NOT NULL,
	ID_trainer           TINYINT NOT NULL,
	ID_buy_abonement     TINYINT NOT NULL
);
ALTER TABLE Visit
ADD PRIMARY KEY (ID_workout,ID_trainer,ID_buy_abonement);

CREATE TABLE Workout
(
	ID_workout           SMALLINT NOT NULL,
	Name_workout         VARCHAR(30) NOT NULL,
	ID_gym               TINYINT NOT NULL,
	ID_trainer           TINYINT NOT NULL,
	Date_workout         DATETIME NOT NULL
);

ALTER TABLE Workout
ADD PRIMARY KEY (ID_workout,ID_trainer);

ALTER TABLE Abonement
ADD FOREIGN KEY (ID_client) REFERENCES Client (ID_client);

ALTER TABLE Abonement
ADD FOREIGN KEY (ID_abonement) REFERENCES Price_list (ID_abonement);

ALTER TABLE Trainer_course
ADD FOREIGN KEY (ID_course) REFERENCES Course (ID_course);

ALTER TABLE Trainer_course
ADD FOREIGN KEY (ID_trainer) REFERENCES Trainer (ID_trainer);

ALTER TABLE Trainer_education
ADD FOREIGN KEY (ID_education) REFERENCES Education (ID_education);

ALTER TABLE Trainer_education
ADD FOREIGN KEY (ID_trainer) REFERENCES Trainer (ID_trainer);

ALTER TABLE Visit
ADD FOREIGN KEY (ID_workout, ID_trainer) REFERENCES Workout (ID_workout, ID_trainer);

ALTER TABLE Visit
ADD FOREIGN KEY (ID_buy_abonement) REFERENCES Abonement (ID_buy_abonement);

ALTER TABLE Workout
ADD FOREIGN KEY (ID_gym) REFERENCES Gym (ID_gym);

ALTER TABLE Workout
ADD FOREIGN KEY (ID_trainer) REFERENCES Trainer (ID_trainer);

/* Триггер: дата окончания абонемента должна быть больше даты приобретения абонемента.*/
DELIMITER //
CREATE TRIGGER DateAbonement
before insert on abonement for each row
begin
if datediff(NEW.Date_end,NEW.Date_buy)<0 then
signal sqlstate '45000' set message_text = 'Date_end should be more than Date_buy';
end if;
end //
DELIMITER ;

/* Триггер: фамилия, имя, отчество должны состоять из букв.*/
DELIMITER //
CREATE TRIGGER ClientValidator
before insert on client for each row
begin
if NEW.Secondname regexp '^[a-zA-Z]+$' = '' or NEW.Name regexp '^[a-zA-Z]+$' = '' or NEW.Patronymic regexp '^[a-zA-Z]+$' = '' then
signal sqlstate '45000' set message_text = 'Names/Secondname/Patronymic isn\'t valid!';
end if;
end //
DELIMITER ;

/* Триггер: номер телефона должен содержать только цифры.*/
DELIMITER //
CREATE TRIGGER PhoneValidator
before insert on client for each row
begin
if NEW.Phone_number regexp '^[0-9]+$' = ''  then
signal sqlstate '45000' set message_text = 'Phone_number isn\'t valid!';
end if;
end //
DELIMITER ;

/* Триггер: срок действия абонемента должен быть 1 месяц, 3 месяца, 6 месяцев, 12 месяцев.*/
DELIMITER //
CREATE TRIGGER PeriodCheck
before insert on price_list for each row
begin
if NEW.Validity_period!=1 or NEW.Validity_period!=3 or NEW.Validity_period!=6 or NEW.Validity_period!=12 then
signal sqlstate '45000' set message_text = 'Phone_number isn\'t valid!';
end if;
end //
DELIMITER ;

/* Процедура: вывод всех клиентов, купивших абонемент по выбранному занятию.*/
DELIMITER //
create procedure getAllClientOnService(IN service varchar(20))
begin
select Secondname, Name, Patronymic, Name_service from client 
inner join abonement on(client.ID_client=abonement.ID_client)
inner join price_list on (abonement.ID_abonement=price_list.ID_abonement)
where Name_service=service;
end; //
DELIMITER ;

/* Процедура: вывод всех клиентов пришедших на занятия в определенное время, а также название занятия.*/

DELIMITER //
create procedure getAllClientVisitInTime(IN times datetime)
begin
select Secondname, Name, Patronymic, Name_workout from client 
inner join abonement on(client.ID_client=abonement.ID_client)
inner join visit on (abonement.ID_buy_abonement=visit.ID_buy_abonement)
inner join workout on(workout.ID_workout=visit.ID_workout)
where Date_workout=times;
end; //
DELIMITER ;

INSERT INTO client () VALUES ('1', 'Белоусов', '2001-11-01', 'Мужской', '89624808829', 'Алексей', 'Сергеевич'), ('2', 'Токмакова', '2002-02-10', 'Женский', '89103090999', 'Наталья', 'Михайловна'), ('3', 'Пономарёв', '2001-01-23', 'Мужской', '89624809323', 'Дмитрий', 'Александрович'), ('4', 'Волков', '2001-08-16', 'Мужской', '89192077833', 'Вадим', 'Вячеславович'), ('5', 'Цуканов', '2001-08-12', 'Мужской', '89092277005', 'Артём', 'Александрович');

INSERT INTO course () VALUES ('1', 'Психология'),('2', 'Йога'),('3', 'Пилатес'),('4', 'Сайкл'),('5', 'Тяжелая атлетика');
INSERT INTO education () VALUES ('1', 'Педагогическое'),('2', 'Медицинское'), ('3', 'Физкультурное');

INSERT INTO trainer () VALUES ('1', 'Мужской', 'Камоликов', '30000', '20', '1969-05-14', 'Андрей', 'Евгеньевич'),('2', 'Женский', 'Волкова', '35000', '20', '1978-05-31', 'Елена', 'Викторовна'),('3', 'Женский', 'Пилипенко', '20000', '2', '1999-02-27', 'Светлана', 'Игоревна'), ('4', 'Мужской', 'Жуков', '12000', '0', '2002-07-13', 'Иван', 'Вадимович');

INSERT INTO gym () VALUES ('1', 'Волкова', 'Кардиозал', '10-17'),('2', 'Камоликов', 'Силовой зал', '12-15'),('3', 'Пилипенко', 'Зал свободного веса', '9-18');
INSERT INTO price_list () VALUES ('Одиночный', '5600', '1', '6', 'Тренажёрный зал'),('Групповой', '3000', '10', '2', '1', 'Пилатес'),('Групповой', '3500', '10', '3', '1', 'Сайкл'),('Одиночный', '10000', '4', '12', 'Тренажёрный зал'),('Одиночный', '2750', '10', '5', '1', 'Тренажёрный зал');
INSERT INTO trainer_course () VALUES ('3', '2'),('4', '2'), ('1', '2'),('1', '1'),('1', '3'),('5', '1'),('2', '3');

INSERT INTO trainer_education () VALUES ('1', '1'),('2', '1'),('3', '1'),('1', '2'),('2', '2'),('3', '2'),('1', '3'),('2', '3'),('3', '3');
INSERT INTO abonement () VALUES ('1', '2021-05-29', '2021-11-29', '1', '1'),('2', '2021-05-24', '2021-06-24', '2', '2'),('3', '2021-06-01', '2021-07-01', '3', '5'),('4', '2021-05-19', '2021-06-19', '5', '3');

INSERT INTO workout () VALUES ('1', 'Пилатес', '3', '2', '2021-06-01 12:00:00'),('2', 'Сайкл', '1', '2', '2021-06-01 12:00:00'),('3', 'Тренажёрный зал', '2', '1', '2021-06-01 12:00:00'),('4', 'Сайкл', '1', '2', '2021-06-01 15:00:00'),('5', 'Тренажёрный зал', '3', '3', '2021-06-01 15:00:00'),('6', 'Пилатес', '2', '2', '2021-06-02 09:00:00'),('7', 'Пилатес', '3', '2', '2021-06-02 12:00:00');
INSERT INTO visit () VALUES ('1', '2', '2'),('2', '2', '3'),('3', '1', '1');


