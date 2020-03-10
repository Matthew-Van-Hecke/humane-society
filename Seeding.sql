INSERT INTO Categories VALUES ('Cat'), ('Dog'), ('Bird'), ('Fish'), ('Snake')
INSERT INTO DietPlans VALUES ('Medium Seafood', 'Fish', 3), ('Small Seafood', 'Shrimp', 1), ('Large Vegetarian', 'Broccoli and Black Beans', 5), ('Medium Carnivorous', 'Meat', 3), ('Small Bird', 'Bird Seed', 1), ('Medium Snake', 'Mice', 3)
INSERT INTO Employees VALUES ('George', 'Washington', 'WashingtonG', 'Delaware76', 85, 'g.washington@adoptapetmke.com'), ('Brad', 'Pitt', 'PittB', 'PitBull1990', 24, 'b.pitt@adoptapetmke.com'), ('Genghis', 'Khan', 'KhanG', 'IConquerTheWorld#1', 1, 'g.khan@adoptapetmke.com'), ('Giannis', 'Antetokounmpo', 'AntetokounmpoG', 'BucksN6', 34, 'g.antetokounmpo@adoptapetmke.com'), ('Kyle', 'Kuopus', 'KuopusK', 'Password', 29, 'k.kuopus@adoptapetmke.com'), ('Matthew', 'Van Hecke', 'VanHeckeM', 'ThisIsMyPassword', 12, 'm.vanhecke@adoptapetmke.com')
INSERT INTO Animals VALUES ('Charlotte', 50, 15, 'Playful, Nervous, Superstitous', 1, 1, 'Female', 'Pending', 2, 4, 6), ('Winston', 64, 2, 'Independent, Quiet', 1, 1, 'Male', 'Available', 2, 1, 5), ('Hank', 5, 3, 'Narcissistic, Aggressive, Flirtatious', 0, 0, 'Male', 'Very Available', 3, 2, 3), ('Heinrich', 15, 18, 'Grumpy, Self-Centered, Lazy', 1, 0, 'Male', 'Adopted', 1, 1, 1), ('Jaws', 20, 5, 'Ferocious, Anxious, Perpetually Hungry', 0, 0, 'Female', 'Available', 4, 1, 2)
INSERT INTO Rooms VALUES (101, 3), (102, 4), (103, 1), (104, 2), (105, null), (106, null), (107, null), (108, null), (109, null), (110, 5)
INSERT INTO Addresses VALUES ('2267 38th St.', 'Chicago', (SELECT USStateId FROM USStates WHERE Name = 'Illinois'), 60632), ('1600 Pennsylvania Ave.', 'Washington DC', (SELECT USStateId FROM USStates WHERE Name = 'Maryland'), 20500), ('2100 Woodward Ave.', 'Detroit', (SELECT USStateId FROM USStates WHERE Name = 'Michigan'), 48201), ('700 N Art Museum Dr.', 'Milwaukee', (SELECT USStateId FROM USStates WHERE Name = 'Wisconsin'), 53202), ('2800 S Taylor Dr.', 'Sheboygan', (SELECT USStateId FROM USStates WHERE Name = 'Wisconsin'), 53081)
INSERT INTO Clients VALUES ('John', 'Harrison', 'JHarrison', 'IHeartChicago26', 1, 'JHarrison@yahoo.com'), ('Grover', 'Cleveland', 'GCleveland', 'Presidentx2', 2, 'g.cleveland@usa.gov'), ('Bobby', 'Higginson', 'BHigginson', 'WorldSeries84', 3, 'b.higginson@worldseries.net'), ('Leonardo', 'DaVinci', 'LDaVinci', 'CubismStinks', 4, 'davinci@monalisa.net'), ('Henry', 'Thompson', 'HThompson', 'WashingtonFoundingFather', 5, 'h.thompson@harvard.edu')