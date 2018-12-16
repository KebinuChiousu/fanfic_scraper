import sys
import datetime
from pony.orm import Database, DatabaseError, Required, PrimaryKey, Optional
from pony.orm import sql_debug

db = Database()


def init_db(filename):
    bind_db(filename)
    db.generate_mapping(create_tables=True)


def bind_db(filename):

    try:
        db.bind(provider='sqlite', filename=filename, create_db=True)
    except DatabaseError as err:
        print("Error loading database!")
        sys.exit(1)


#############################################################
class Category(db.Entity):
    """
    Pony ORM model of the Category table
    """
    Name = Required(str)
    Id = PrimaryKey(int, auto=True)


#############################################################
class Fanfic(db.Entity):
    """
    Pony ORM model of the Fanfic table
    """
    Title = Required(str)
    Author = Required(str)
    Folder = Required(str)
    Chapter = Optional(str)
    Count = Optional(int)
    Matchup = Optional(str)
    Crossover = Optional(str)
    Description = Optional(str)
    Internet = Optional(str)
    StoryId = Optional(str)
    Abandoned = Optional(bool, default=0)
    Complete = Optional(bool, default=0)
    Publish_Date = Optional(datetime.date)
    Update_Date = Optional(datetime.date)
    Last_Checked = Optional(datetime.date)
    Id = PrimaryKey(int, auto=True)
    Category_Id = Required(int)


class DataBaseLogic:

    def __init__(self, db_filename):

        init_db(db_filename)


def main():

    bind_db('FanfictionDB.db')

    # turn on debug mode
    sql_debug(True)

    # map the models to the database
    # and create the tables, if they don't exist
    db.generate_mapping(create_tables=True)


if __name__ == '__main__':
    main()
