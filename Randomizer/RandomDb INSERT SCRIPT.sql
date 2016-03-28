use RandomDb

INSERT INTO Participants (ParticipantId, Name) VALUES (1, 'Tarzan')
INSERT INTO Participants (ParticipantId, Name) VALUES (2, 'Faccio')
INSERT INTO Participants (ParticipantId, Name) VALUES (3, 'Trusser')
INSERT INTO Participants (ParticipantId, Name) VALUES (4, 'Nosser')
INSERT INTO Participants (ParticipantId, Name) VALUES (5, 'Leffo')
INSERT INTO Participants (ParticipantId, Name) VALUES (6, 'Tennedz')
INSERT INTO Participants (ParticipantId, Name) VALUES (7, 'Aallex')

INSERT INTO dbo.Events (EventName, Measure) VALUES ('Scoring', 'Other_3')
INSERT INTO dbo.Events (EventName, Measure) VALUES ('Assist', 'Other_2')
INSERT INTO dbo.Events (EventName, Measure) VALUES ('Shot OnTarget', 'Other_1')
INSERT INTO dbo.Events (EventName, Measure) VALUES ('Corner Taken', 'Other_1')
INSERT INTO dbo.Events (EventName, Measure) VALUES ('Medic', 'Own_1')
INSERT INTO dbo.Events (EventName, Measure) VALUES ('Offside', 'Own_1')
INSERT INTO dbo.Events (EventName, Measure) VALUES ('Foul Committed', 'Own_1')
INSERT INTO dbo.Events (EventName, Measure) VALUES ('Shot Off Target', 'Own_1')
INSERT INTO dbo.Events (EventName, Measure) VALUES ('Penalty Committed', 'Own_2')
INSERT INTO dbo.Events (EventName, Measure) VALUES ('Yellow', 'Own_2')
INSERT INTO dbo.Events (EventName, Measure) VALUES ('Substitution', 'Own_2')
INSERT INTO dbo.Events (EventName, Measure) VALUES ('Own Goal', 'Own_3')
INSERT INTO dbo.Events (EventName, Measure) VALUES ('Penalty Missed', 'Own_3')
INSERT INTO dbo.Events (EventName, Measure) VALUES ('Red', 'Own_3')


INSERT INTO Distribution (Measure, Small, Medium, Large, Walter) VALUES ('Own_1', 50, 30, 15, 5)
INSERT INTO Distribution (Measure, Small, Medium, Large, Walter) VALUES ('Own_2', 30, 30, 25, 15)
INSERT INTO Distribution (Measure, Small, Medium, Large, Walter) VALUES ('Own_3', 15, 25, 35, 25)
INSERT INTO Distribution (Measure, Small, Medium, Large, Walter) VALUES ('Other_1', 40, 30, 20, 10)
INSERT INTO Distribution (Measure, Small, Medium, Large, Walter) VALUES ('Other_2', 20, 30, 30, 20)
INSERT INTO Distribution (Measure, Small, Medium, Large, Walter) VALUES ('Other_3', 15, 25, 30, 30)






