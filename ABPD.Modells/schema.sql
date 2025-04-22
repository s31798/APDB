CREATE TABLE ElectronicDevice (
                                  Id NVARCHAR(50) PRIMARY KEY,
                                  Name NVARCHAR(100) NOT NULL,
                                  IsOn BIT NOT NULL
);

CREATE TABLE EmbeddedDevice (
                                Id NVARCHAR(50) PRIMARY KEY,
                                FOREIGN KEY (Id) REFERENCES ElectronicDevice(Id),
                                Ip NVARCHAR(45),
                                NetworkName NVARCHAR(100)
);

CREATE TABLE PersonalComputer (
                                  Id NVARCHAR(50) PRIMARY KEY,
                                  FOREIGN KEY (Id) REFERENCES ElectronicDevice(Id),
                                  OperatingSystem NVARCHAR(100) NOT NULL
);

CREATE TABLE SmartWatch (
                            Id NVARCHAR(50) PRIMARY KEY,
                            FOREIGN KEY (Id) REFERENCES ElectronicDevice(Id),
                            Battery INT NOT NULL
);
-- Insert into base table
INSERT INTO ElectronicDevice (Id, Name, IsOn)
VALUES ('dev-001', 'Sensor Node A', 1);

-- Insert into EmbeddedDevice
INSERT INTO EmbeddedDevice (Id, Ip, NetworkName)
VALUES ('dev-001', '192.168.1.10', 'IoT-Net');
