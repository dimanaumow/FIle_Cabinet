# FileCabinetApp

The main project I did during the .NET training at EPAM. The project is done under the instructions which you can see there https://github.com/epam-dotnet-lab/file-cabinet-task.
From my point of view, my result is really far from being perfect, but I learned a lot during it. It gave me a lot of practice and skills, which I use to my future development.
The project looks like a console database. It works with the users' records - creates, stores, updates, displays, deletes, has the functions of import and export from and to xml or csv formats, can use different validators of data extracted from a .json file, command line arguments for configuration, two places of storage - memory or filesystem, uses logging and has a compound search with 'where' commands and support of 'and's and 'or's.
From design point, the project implements different kinds of design patterns - Chain of Responsibility, Decorator, Strategy Pattern, Composite Pattern, Template Method, Iterator. The code is easy to expand, has a good structure and documentation. You can see the development of the project via commits(it actually changed a lot).

USAGE:
1) Command line arguments:
• '--validation-rules' or '-v' : default or custom;
• '--storage' or '-s' : memory(by default) or filesystem;
• '--use-stopwatch' or '-uw' : to measure the execution time of the commands;
• '--use-logger' or '-ul' : loggs the info about the called methods to the text file;
examples:
- --validation-rules=custom --storage=memory --use-stopwatch=true --use-logger=false
- -v custom -s memory -us true -ul false
- --validation-rules=dEfAuLt --storage=FILESYSTEM
- (arguments may be ommited) 

2) >import/export <format> <filepath>
<format> can be either xml or csv.
examples: 
- import xml records.xml
- export csv C:\csharp\records.csv

3) >select <property>, <property> where <property>='value' or/and <property>='value'
examples:
- select id, firstname, lastname where firstname = 'John' and lastname = 'Doe'
- select id, firstname, dateofbirth where firstname='John' or donations='0'
- select where firstname='John' and lastname='Doe' or firstname='Mike' and lastname = 'Wazowski' (prints all the properties of found records)
- select where (prints all the records with all the properties)

4) 'update' and 'delete' work the same as 'select' command.

5)  'create', 'stat', 'purge', 'help', 'exit' don't have any arguments
- 'create' - creates a single record
- 'stat' - displays the number of records
- 'purge' - purges the deleted records from the list. it is useful in filesystem service for cleaning the defragmentation. when the records from the filesystem service are deleted, they are only marked as deleted, but do still exist. When the 'purge' is called, the marked records are finally deleted physically and are shifted close to each other.

Additional: RecordGenerator
Generates random records and exports them to the file.
Command Line Arguments:
- '--output-type' or 't' : can be csv or xml
- '--output' or 'o' : path to the new file
- '--records-amount' or 'a' : amount of records to generate
- '--start-id' or 'i' : the integer-id of the first record (should not be negative)
examples:
- --output-type=csv --output=d:\data\records.csv --records-amount=100 --start-id=1
- -t xml -o records.xml -a 50 -i 30
