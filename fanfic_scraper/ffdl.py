#!/usr/bin/env python3
import os
import sys
import shutil
import fnmatch
import subprocess
import argparse
from datetime import date
from collections import defaultdict
from configparser import SafeConfigParser
from fanfic_scraper import current_fanfic
import tldextract
from urllib.parse import urlparse
import requests
from pony.orm import *
from fanfic_scraper.db_pony import DataBaseLogic, Category, Fanfic
from fanfic_scraper import cui
import textwrap
import sqlite3
from time import sleep


basePath = '/home/ubuntu/OneDrive/'
arcRoot = 'FF_Archive'
db = None
db_name = 'FanfictionDB.db'
db_folder = 'Read'
dl_folder = 'htm'

def ensure_dir(path):
    if not os.path.exists(path):
        os.makedirs(path)

def get_config():
    #create config dir if it doesn't exist and return path to config.
    home = os.path.expanduser("~")
    cfg = os.path.join(home,".ff")
    ensure_dir(cfg)

    return os.path.join(cfg,"config.ini")

def config_load():
    global basePath
    global arcRoot
    global db_folder
    global db_name
    global dl_folder
    t_path = ''
    t_root = ''
    t_dbname = ''
    t_dbfolder = ''
    t_dlfolder = ''
    config = SafeConfigParser()

    cfg = get_config()

    if os.path.isfile(cfg):
        config.read(cfg)

        try:
            t_path = config.get('path','path')
            t_root = config.get('path','rootfolder')
            t_dbfolder = config.get('path','dbfolder')
            t_dbname = config.get('path','dbname')
            t_dlfolder = config.get('path','downloadfolder')
        except:
            i=0

        if t_path:
            basePath = t_path
        if t_root:
            arcRoot = t_root
        if t_dbfolder:
            db_folder = t_dbfolder
        if t_dbname:
            db_name = t_dbname
        if t_dlfolder:
            dl_folder = t_dlfolder

def config_save():
    cfg = get_config()

    config = SafeConfigParser()

    config.read(cfg)

    if not config.has_section('path'):
        config.add_section('path')

    config.set('path','path',basePath)
    config.set('path','rootfolder',arcRoot)
    config.set('path','dbfolder',db_folder)
    config.set('path','dbname',db_name)
    config.set('path','downloadfolder',dl_folder)

    with open(cfg, 'w') as f:
        config.write(f)

def IsNotStale(update_date, last_checked):
    wait = 0
    ret = False

    if not last_checked:
        ret = True

    if not ret:
        if not update_date:
            ret = True

    if not ret:

        d0 = update_date
        d1 = last_checked
        delta = d1 - d0

        if delta.days > 360:
            wait = 180
        if delta.days > 180:
            wait = 90
        if delta.days > 90:
            wait = 60
        if delta.days > 60:
            wait = 30
        if delta.days > 30:
            wait = 7
        if delta.days < 30:
            wait = 0

        d0 = last_checked
        d1 = date.today()
        delta = d1 - d0

        if delta.days > wait and wait > 0:
            ret = False
        else:
            ret = True

    return ret

def check_url(url):
    ret = False

    if 'fanfiction.net' in url:
        return True
    if 'hpfanficarchive.com' in url:
        return True

    return ret

def set_ffargs(location,folder):

    parser = argparse.ArgumentParser(
    description=('fanfic_scraper args'))

    parser.add_argument(
        "-f", "--folder", required=True, help="Name of folder to place fanfic in.")
    parser.add_argument(
        "-l", "--location", default=os.getcwd(), help="set download location")
    #parser.add_argument(
    #    "-c", "--chapters", default=False,
    #    help="Specify chapters to download separated by : (10:15).")
    parser.add_argument(
        "-ct", "--chapterthreads", default=1,
        help="Number of parallel chapters downloads.")
    parser.add_argument(
        "-wt", "--waittime", default=15,
        help="Wait time before retry if encountered with an error")
    parser.add_argument(
        "-rt", "--retries", default=10,
        help="Number of retries before giving up")

    args = parser.parse_args(['--location='+location,'--folder='+folder])

    return args

def print_list(matches):
    divisor = 6
    result = ''

    for i in range(int(len(matches)/divisor)):
            j = divisor * (i *1)
            result = ''
            for j in range(j,j+(divisor)):
                result=result+matches[j]+'\t'
            print(result)

    k = len(matches) % divisor
    l = (int(len(matches) / divisor)*divisor)

    result = ''
    for m in range(l,len(matches)):
        result=result+matches[m]+'\t'
    print(result)

def get_subfolders(path):

    subfolders = [f.name for f in os.scandir(path) if f.is_dir() ]
    subfolders.sort()

    return subfolders

def get_files(path,value):
    matches = []
    for root, dirs, files in os.walk(path):
        for filename in fnmatch.filter(files,value):
            matches.append(filename)
    # Sort files alphabetically
    matches.sort()

    return matches

def convert_file(source,htm):

    txt = htm.split('.')[0]+'.txt'
    ifile = [os.path.abspath(os.path.join(source,htm))]
    ofile = os.path.abspath(os.path.join(source,txt))
    cmd = ['html2text','--ignore-emphasis','-b 80']
    msg = 'Converting file: {0} to text file: {1}'

    print(msg.format(htm,txt))

    with open(ofile, "w") as outfile:
        subprocess.call(cmd+ifile , stdout=outfile)

    for i in ifile:
        os.remove(i)

class FanficDB:

    def __init__(self):
        db_path = os.path.join(basePath,arcRoot,db_folder,db_name)
        dbhandle = DataBaseLogic(db_path)

    def add_story(self, folder, cat_id, url, info):

        with db_session:
            Fanfic(Title = info['Title'],
                   Author = info['Author'],
                   Folder = folder,
                   Count = 0,
                   Description = info['Description'],
                   Internet = url,
                   StoryId = info['StoryId'],
                   Publish_Date = info['Publish_Date'],
                   Update_Date = info['Update_Date'],
                   Last_Checked = info['Publish_Date'],
                   Category_Id = cat_id)

    def get_categories(self):
        d = defaultdict(list)

        with db_session:
            categories = select(c for c in Category).order_by(Category.Name)

            for c in categories:
                for k,v in c.to_dict().items():
                    d[k].append(v)

        return d['Name']

    def get_folder(self, cat_id, folderName):

        with db_session:
            folders = count(f.Folder for f in Fanfic
                            if f.Category_Id == cat_id
                            and f.Folder == folderName)

        return folders


    def get_cat_id(self, categoryName):

        with db_session:
            cat = Category.get(Name=categoryName)
            ret = cat.Id

        return ret

    def get_fanfics(self,cat_id):
        d = defaultdict(list)
        ret = []

        with db_session:
            ff = select(f for f in Fanfic
                        if f.Abandoned == 0
                        and f.Complete == 0
                        and f.Category_Id == cat_id)
            ff = ff.order_by(Fanfic.Folder)

            for f in ff:
                d = defaultdict(list)
                for k,v in f.to_dict().items():
                    d[k].append(v)
                ret.append(d)

        return ret

    def update_chapter_count(self, fic_id, chapters):

        with db_session:
            ff = Fanfic.get(Id=fic_id)
            ff.Count = chapters

    def update_last_checked(self, fic_id, last):

        with db_session:
            ff = Fanfic.get(Id=fic_id)
            ff.Last_Checked = last

    def update_last_updated(self, fic_id, update):

        with db_session:
            ff = Fanfic.get(Id=fic_id)
            ff.Update_Date = update

def download_stories(args):

    cat = db.get_categories()

    for cat_name in cat:
        #print("Processing:",cat_name)
        download_by_category(cat_name)
        convert_by_category(cat_name)

def download_by_category(category):

    potential_keys = []

    cat_folder = category.replace(' ','_')

    location = os.path.join(basePath,arcRoot,dl_folder,cat_folder)

    cat_id = db.get_cat_id(category)

    fics = db.get_fanfics(cat_id)

    fic_idx = 0
    fic_count = len(fics)
    msg = "Processing: {0} - {1} of {2}."

    for fic in fics:
        fic_idx = fic_idx + 1
        print(msg.format(category,fic_idx,fic_count),end='\r')
        title = fic['Title'][0]
        author = fic['Author'][0]
        folder = fic['Folder'][0]
        chapters = fic['Count'][0]
        update = fic['Update_Date'][0]
        last = fic['Last_Checked'][0]
        if fic['Internet'][0]:
            tmp = fic['Internet'][0]
            tmp2 = tmp.split('#')
            if len(tmp2) > 1:
                url = fic['Internet'][0].split('#')[1]
            else:
                url = tmp
        else:
            url = ''
        storyid = fic['StoryId'][0]
        fic_id = fic['Id'][0]

        date_check = IsNotStale(update,last)
        url_check = check_url(url)

        if url_check == True and date_check == True:

            ffargs = set_ffargs(location,folder)

            next_chapter = chapters + 1

            #Initialize class so we can retrieve actual story url
            fanfic = current_fanfic.fanfic(url, ffargs)
            #Get fanfic url
            url = fanfic.get_story_url(storyid)
            #Initialize class with correct url.
            fanfic = current_fanfic.fanfic(url, ffargs)
            #Download list of chapters
            fanfic.get_chapters()
            # Get current chapter count from site
            cc = fanfic.chapter_count

            if cc >= next_chapter:
                if cc == next_chapter:
                    potential_keys = [next_chapter]
                else:
                    potential_keys = [
                        i * 1 for i in range(next_chapter,(cc + 1))
                        ]
                #Show story about to be updated.
                update = fanfic.get_update_date()
                print('',end='\n')
                print(title, '/', folder, '/', update)
                #Set chapters to download
                fanfic.set_download_chapters(potential_keys)
                #Download Story
                fanfic.download_fanfic()
                print('Downloaded fanfic: ',category,'/',title,'/',folder)
                db.update_chapter_count(fic_id,cc)
                db.update_last_updated(fic_id,update)
                db.update_last_checked(fic_id,date.today())
            else:
                db.update_last_checked(fic_id,date.today())

            sleep(2)
        else:
            sleep(0.2)

    print('',end='\n')

def convert_by_category(category):

    cat_folder = category.replace(' ','_')
    location = os.path.join(basePath,arcRoot,dl_folder,cat_folder)

    folders = get_subfolders(location)

    for folder in folders:
        target = os.path.realpath(os.path.join(location,folder))
        files = get_files(target,"*.htm")

        for htm in files:
            convert_file(target,htm)

def add_story(category, cat_id, folder):
    _=os.system("clear")
    url = input("Enter Story URL: ")
    url_check = check_url(url)
    if url_check:
        ffargs = set_ffargs('/tmp',folder)
        fanfic = current_fanfic.fanfic(url, ffargs, True)
        info = fanfic.story_info()
        print("ID: {0}".format(info['StoryId']))
        print("Title: {0}".format(info['Title']))
        print("Author: {0}".format(info['Author']))
        print("Summary: ", textwrap.fill(info['Description']))
        print("Published: {0}".format(info['Publish_Date']))
        print("Updated: {0}".format(info['Update_Date']))
        print("Chapter Count: {0}".format(info['Count']))
        cui.pause()
        confirm = cui.menu_yesno("Proceed?")

        if confirm == "Yes":
            db.add_story(folder, cat_id, url, info)
            print("Story added to database.")
            cui.pause()

    else:
        print("Site not supported!!!")
        cui.pause()

def config_menu():
    category = "None"
    folder = "None"

    while True:
        menu = []
        menu.append(cui.get_entry("Category: {0}", category))
        menu.append(cui.get_entry("Folder: {0}", folder))
        menu.append("Add Story")
        menu.append("Exit")

        _=os.system("clear")

        print("Fanfic Downloader - Config")
        print("")
        ret = cui.submenu(menu, "Enter Selection")

        if "Category:" in ret:
            cat = db.get_categories()
            _=os.system("clear")
            category = cui.submenu(cat,"Choose Category: ")
            cat_id = db.get_cat_id(category)
        if "Folder:" in ret:
            if category != "None":
                folder = input("Enter Folder Name: ")
                folder_count = db.get_folder(cat_id, folder)
                if folder_count > 0:
                    print("Folder: {0} already exists!".format(folder))
                    folder = "None"
                    cui.pause()
            else:
                print("Please select category first!")
                cui.pause()
        if ret == "Add Story":
            if category != "None" and folder != "None":
                add_story(category, cat_id, folder)
                folder = "None"
            else:
                print("Please ensure Category and Folder are not set to None!")
                cui.pause()
        if ret == "Exit":
            sys.exit(0)


def main():
    global db
    parser = argparse.ArgumentParser(
        description=(
            'Automates fanfic downloads latest chapters using fanfic_scraper. '
            '(Currently works with fanfiction.net)'))
    parser.add_argument(
        "-c", '--config', required=False, action='store_true', help="Configures Fanfic Downloader.")

    config_load()
    db = FanficDB()

    args = parser.parse_args()

    if args.config:
        config_menu()
    else:
        download_stories(args)


if __name__ == "__main__":
    sys.exit(main())
