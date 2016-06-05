use RandomDb


CREATE TABLE Participants
(
	ParticipantId INT IDENTITY PRIMARY KEY,
	Name VARCHAR(100),
	Red INT,
	Green INT,
	Blue INT,
	MatchId INT REFERENCES Matches (MatchId)
)

CREATE TABLE Matches
(
	MatchId INT IDENTITY(1,1) PRIMARY KEY,
	TeamNames VARCHAR(100),
	Created DATETIME DEFAULT GETDATE()
)

CREATE TABLE Players
(
	PlayerId INT IDENTITY PRIMARY KEY,
	Name VARCHAR(100),
	Number INT,
	MatchId INT REFERENCES dbo.Matches (MatchId),
	PlayerIndex INT
)

CREATE TABLE Events
(
	EventName VARCHAR(20) PRIMARY KEY,
	Measure VARCHAR(20),
	SoundClipUrl VARCHAR(300) DEFAULT ''
)

CREATE TABLE Measures
(
	Measure VARCHAR(20) PRIMARY KEY,
	Small INT,
	Medium INT,
	Large INT,
	Walter INT
)

CREATE TABLE Graph (
	MatchId INT FOREIGN KEY REFERENCES Matches (MatchId),
	ParticipantId INT FOREIGN KEY REFERENCES Participants (ParticipantId),
	GameMinute INT,
	MeasureZips INT,
	EventNumber INT
)

CREATE TABLE Owners (
	MatchId INT REFERENCES Matches (MatchId),
	ParticipantId INT FOREIGN KEY REFERENCES Participants (ParticipantId),
	PlayerId INT FOREIGN KEY REFERENCES Players (PlayerId)
	)
GO

CREATE PROCEDURE [dbo].[CalculateGraph]
@MatchId INT
AS
	BEGIN
	SELECT ParticipantId, GameMinute, MeasureZips, SUM(MeasureZips) OVER(PARTITION BY ParticipantId ORDER BY ParticipantId, GameMinute) AS CurrentTotal
	FROM Graph
	WHERE MatchId = @MatchId
	GROUP BY ParticipantId, GameMinute, MeasureZips
	END
GO

CREATE PROCEDURE [dbo].[UndoLatest]
AS
	BEGIN
		DECLARE @LatestEvent INT
		SELECT @LatestEvent = MAX(EventNumber) FROM Graph

		DELETE Graph WHERE EventNumber = @LatestEvent
	END
GO

CREATE PROCEDURE [dbo].[MapPlayerToOwner]
@MatchId INT,
@PlayerName VARCHAR(100),
@OwnerName VARCHAR(100)
AS
	BEGIN
		DECLARE @playerId INT, @ownerId INT
		SELECT @playerId = PlayerId FROM Players WHERE Name = @PlayerName AND MatchId = @MatchId
		SELECT @ownerId = ParticipantId FROM Participants WHERE Name = @OwnerName AND MatchId = @MatchId

		INSERT INTO Owners (MatchId, ParticipantId, PlayerId) VALUES (@MatchId, @ownerId, @playerId)
	END
GO

CREATE PROCEDURE [dbo].[GetOwnerFromPlayerAndGame]
@MatchId INT,
@PlayerName VARCHAR(100)
AS
	BEGIN		
		SELECT P.ParticipantId, P.Name
		FROM Participants P
		INNER JOIN Owners O ON P.ParticipantId = O.ParticipantId
		INNER JOIN Players PL ON PL.PlayerId = O.PlayerId AND PL.MatchId = O.MatchId
		WHERE PL.Name = @PlayerName AND O.MatchId = @MatchId
	END
GO

CREATE PROCEDURE [dbo].[LogRandomizingOutcomeToGraph]
@MatchId INT,
@OwnerName VARCHAR(100),
@Zips INT,
@Time INT,
@EventNumber INT
AS
	BEGIN
		DECLARE @ParticipantId INT
		SELECT @ParticipantId = P.ParticipantId
		FROM Owners O INNER JOIN Participants P ON O.ParticipantId = P.ParticipantId
		WHERE O.MatchId = @MatchId AND P.Name = @OwnerName

		INSERT INTO Graph (MatchId, ParticipantId, MeasureZips, GameMinute, EventNumber)
		VALUES (@MatchId, @ParticipantId, @Zips, @Time, @EventNumber)
	END
GO

CREATE PROCEDURE [dbo].[GetPlayersAndOwners]
@MatchId INT
AS
	BEGIN
		SELECT PL.Name AS PlayerName, PL.PlayerIndex, PA.Name AS OwnerName
		FROM Players PL INNER JOIN Owners O ON PL.PlayerId = O.PlayerId
		INNER JOIN Participants PA ON O.ParticipantId = PA.ParticipantId
		INNER JOIN Matches M ON O.MatchId = M.MatchId
		WHERE M.MatchId = @MatchId
	END
GO

CREATE PROCEDURE [dbo].[GetOldGames]
AS
	BEGIN
		SELECT M.MatchId, M.TeamNames, M.Created, ISNULL(MAX(EventNumber),0) AS LatestEvent
		FROM Matches M LEFT JOIN Graph G ON M.MatchId = G.MatchId
		GROUP BY M.MatchId, M.TeamNames, M.Created
	END
GO

--select top 5 * from owners
--select top 5 * from Participants
--select top 5 * from Players

--ALTER TABLE Matches ADD MatchId INT

--delete graph
--delete Owners
--delete Players
--delete matches
--select * from graph

--select * from matches

--select * from Participants

--DROP TABLE Measures
--DROP TABLE MatchLog
--DROP TABLE Graph
--DROP TABLE Owners
--DROP TABLE Players
--DROP TABLE Matches
--DROP TABLE Events
--DROP TABLE Teams
--DROP TABLE Participants

--DROP PROCEDURE CalculateGraph
--DROP PROCEDURE UndoLatest
--DROP PROCEDURE MapPlayerToOwner
--DROP PROCEDURE GetOwnerFromPlayer
--DROP PROCEDURE LogRandomizingOutcomeToGraph
--DROP PROCEDURE GetPlayersAndOwners


--select * from graph where matchid = 'e-q'

--select * from participants