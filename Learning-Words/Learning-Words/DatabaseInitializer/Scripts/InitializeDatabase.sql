-- Add a new collection
INSERT INTO Collections (Name)
VALUES ('User_Collection');

-- Get the id of the newly created collection
INSERT INTO Chapters (NameChapter, Description, CollectionId)
VALUES 
('Example 1', 'First example chapter', (SELECT Id FROM Collections WHERE Name = 'User_Collection')),
('Example 2', 'Second example chapter', (SELECT Id FROM Collections WHERE Name = 'User_Collection')),
('Example 3', 'Third example chapter', (SELECT Id FROM Collections WHERE Name = 'User_Collection'));

-- Add words to the chapters
INSERT INTO Words (NameWord, Translation, Transcription, ChapterId)
VALUES 
-- Words for the first chapter
('apple', 'яблуко', '[ˈæpl]', (SELECT Id FROM Chapters WHERE NameChapter = 'Example 1' AND CollectionId = (SELECT Id FROM Collections WHERE Name = 'User_Collection'))),
('banana', 'банан', '[bəˈnænə]', (SELECT Id FROM Chapters WHERE NameChapter = 'Example 1' AND CollectionId = (SELECT Id FROM Collections WHERE Name = 'User_Collection'))),
('cherry', 'вишня', '[ˈʧɛri]', (SELECT Id FROM Chapters WHERE NameChapter = 'Example 1' AND CollectionId = (SELECT Id FROM Collections WHERE Name = 'User_Collection'))),
('date', 'фінік', '[deɪt]', (SELECT Id FROM Chapters WHERE NameChapter = 'Example 1' AND CollectionId = (SELECT Id FROM Collections WHERE Name = 'User_Collection'))),
('elderberry', 'бузина', '[ˈɛldərˌbɛri]', (SELECT Id FROM Chapters WHERE NameChapter = 'Example 1' AND CollectionId = (SELECT Id FROM Collections WHERE Name = 'User_Collection'))),
('fig', 'інжир', '[fɪɡ]', (SELECT Id FROM Chapters WHERE NameChapter = 'Example 1' AND CollectionId = (SELECT Id FROM Collections WHERE Name = 'User_Collection'))),
('grape', 'виноград', '[ɡreɪp]', (SELECT Id FROM Chapters WHERE NameChapter = 'Example 1' AND CollectionId = (SELECT Id FROM Collections WHERE Name = 'User_Collection'))),
('honeydew', 'медова роса', '[ˈhʌniˌdu]', (SELECT Id FROM Chapters WHERE NameChapter = 'Example 1' AND CollectionId = (SELECT Id FROM Collections WHERE Name = 'User_Collection'))),
('kiwi', 'ківі', '[ˈkiːwi]', (SELECT Id FROM Chapters WHERE NameChapter = 'Example 1' AND CollectionId = (SELECT Id FROM Collections WHERE Name = 'User_Collection'))),
('lemon', 'лимон', '[ˈlɛmən]', (SELECT Id FROM Chapters WHERE NameChapter = 'Example 1' AND CollectionId = (SELECT Id FROM Collections WHERE Name = 'User_Collection'))),

-- Words for the second chapter
('cat', 'кіт', '[kæt]', (SELECT Id FROM Chapters WHERE NameChapter = 'Example 2' AND CollectionId = (SELECT Id FROM Collections WHERE Name = 'User_Collection'))),
('dog', 'собака', '[dɔɡ]', (SELECT Id FROM Chapters WHERE NameChapter = 'Example 2' AND CollectionId = (SELECT Id FROM Collections WHERE Name = 'User_Collection'))),
('elephant', 'слон', '[ˈɛləfənt]', (SELECT Id FROM Chapters WHERE NameChapter = 'Example 2' AND CollectionId = (SELECT Id FROM Collections WHERE Name = 'User_Collection'))),
('fox', 'лисиця', '[fɒks]', (SELECT Id FROM Chapters WHERE NameChapter = 'Example 2' AND CollectionId = (SELECT Id FROM Collections WHERE Name = 'User_Collection'))),
('giraffe', 'жирафа', '[ʤɪˈræf]', (SELECT Id FROM Chapters WHERE NameChapter = 'Example 2' AND CollectionId = (SELECT Id FROM Collections WHERE Name = 'User_Collection'))),
('hippopotamus', 'гіпопотам', '[ˌhɪpəˈpɒtəməs]', (SELECT Id FROM Chapters WHERE NameChapter = 'Example 2' AND CollectionId = (SELECT Id FROM Collections WHERE Name = 'User_Collection'))),
('iguana', 'ігуана', '[ɪˈɡwɑːnə]', (SELECT Id FROM Chapters WHERE NameChapter = 'Example 2' AND CollectionId = (SELECT Id FROM Collections WHERE Name = 'User_Collection'))),
('jaguar', 'ягуар', '[ˈʤæɡwɑːr]', (SELECT Id FROM Chapters WHERE NameChapter = 'Example 2' AND CollectionId = (SELECT Id FROM Collections WHERE Name = 'User_Collection'))),
('kangaroo', 'кенгуру', '[ˌkæŋɡəˈruː]', (SELECT Id FROM Chapters WHERE NameChapter = 'Example 2' AND CollectionId = (SELECT Id FROM Collections WHERE Name = 'User_Collection'))),
('lion', 'лев', '[ˈlaɪən]', (SELECT Id FROM Chapters WHERE NameChapter = 'Example 2' AND CollectionId = (SELECT Id FROM Collections WHERE Name = 'User_Collection'))),

-- Words for the third chapter
('red', 'червоний', '[rɛd]', (SELECT Id FROM Chapters WHERE NameChapter = 'Example 3' AND CollectionId = (SELECT Id FROM Collections WHERE Name = 'User_Collection'))),
('blue', 'синій', '[bluː]', (SELECT Id FROM Chapters WHERE NameChapter = 'Example 3' AND CollectionId = (SELECT Id FROM Collections WHERE Name = 'User_Collection'))),
('green', 'зелений', '[ɡriːn]', (SELECT Id FROM Chapters WHERE NameChapter = 'Example 3' AND CollectionId = (SELECT Id FROM Collections WHERE Name = 'User_Collection'))),
('yellow', 'жовтий', '[ˈjɛloʊ]', (SELECT Id FROM Chapters WHERE NameChapter = 'Example 3' AND CollectionId = (SELECT Id FROM Collections WHERE Name = 'User_Collection'))),
('orange', 'помаранчевий', '[ˈɔːrɪndʒ]', (SELECT Id FROM Chapters WHERE NameChapter = 'Example 3' AND CollectionId = (SELECT Id FROM Collections WHERE Name = 'User_Collection'))),
('purple', 'фіолетовий', '[ˈpɜːpl]', (SELECT Id FROM Chapters WHERE NameChapter = 'Example 3' AND CollectionId = (SELECT Id FROM Collections WHERE Name = 'User_Collection'))),
('pink', 'рожевий', '[pɪŋk]', (SELECT Id FROM Chapters WHERE NameChapter = 'Example 3' AND CollectionId = (SELECT Id FROM Collections WHERE Name = 'User_Collection'))),
('brown', 'коричневий', '[braʊn]', (SELECT Id FROM Chapters WHERE NameChapter = 'Example 3' AND CollectionId = (SELECT Id FROM Collections WHERE Name = 'User_Collection'))),
('black', 'чорний', '[blæk]', (SELECT Id FROM Chapters WHERE NameChapter = 'Example 3' AND CollectionId = (SELECT Id FROM Collections WHERE Name = 'User_Collection'))),
('white', 'білий', '[waɪt]', (SELECT Id FROM Chapters WHERE NameChapter = 'Example 3' AND CollectionId = (SELECT Id FROM Collections WHERE Name = 'User_Collection')));
