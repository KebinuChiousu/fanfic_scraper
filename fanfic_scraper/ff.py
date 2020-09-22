#!/usr/bin/python3

import os
import sys
import shutil
import fnmatch
import subprocess
import textwrap
import json
import argparse
from datetime import date
from collections import defaultdict
from configparser import SafeConfigParser, Error
from fanfic_scraper import current_fanfic
from pony.orm import db_session, select, count
from fanfic_scraper.db_pony import DataBaseLogic, Category, Fanfic
from fanfic_scraper import cui


category = ''
folder = ''
dfolder = ''
sfolder = ''
tfile = ''
editor = 'vi'
sync_server = ''
sync_path = ''
sync_safe = True
use_proxy = None

# Initial Values
basePath = '/home/ubuntu/OneDrive/'
arcRoot = 'FF_Archive'
db = None
db_name = 'FanfictionDB.db'
db_folder = 'Read'


class FanficDB:

    def __init__(self):
        db_path = os.path.join(basePath, arcRoot, db_folder, db_name)
        dbhandle = DataBaseLogic(db_path)

    def get_cat_id(self, categoryName):

        with db_session:
            cat = Category.get(Name=categoryName)
            ret = cat.Id

        return ret

    def set_complete(self, fic_id):

        with db_session:
            fic = Fanfic.get(Id=fic_id)
            c = fic.Complete

            if c == 1:
                c = 0
            else:
                c = 1

            fic.Complete = c

    def set_abandoned(self, fic_id):

        with db_session:
            fic = Fanfic.get(Id=fic_id)
            a = fic.Abandoned

            if a == 1:
                a = 0
            else:
                a = 1

            fic.Abandoned = a

    def reset_update(self, fic_id):

        with db_session:
            fic = Fanfic.get(Id=fic_id)
            p = fic.Publish_Date
           
            fic.Last_Checked = p

    def get_fic_id(self, cat_id, folder):

        with db_session:
            ff = select(f for f in Fanfic
                        if f.Category_Id == cat_id
                        and f.Folder == folder)

            for f in ff:
                ret = f.Id

        return ret

    def get_fanfic(self, cat_id, folder):
        d = defaultdict(list)
        ret = []

        with db_session:
            ff = select(f for f in Fanfic
                        if f.Category_Id == cat_id
                        and f.Folder == folder)

            for f in ff:
                d = defaultdict(list)
                for k, v in f.to_dict().items():
                    d[k].append(v)
                ret.append(d)

        return ret[0]
    
    def get_fanfic_byId(self, fic_id):
        d = defaultdict(list)
        ret = []

        with db_session:
            ff = select(f for f in Fanfic
                        if f.Id == fic_id)

            for f in ff:
                d = defaultdict(list)
                for k, v in f.to_dict().items():
                    d[k].append(v)
                ret.append(d)

        return ret[0]

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


def set_ffargs(location, folder, use_proxy):

    parser = argparse.ArgumentParser(description=('fanfic_scraper args'))

    parser.add_argument(
        "-f", "--folder", required=True,
        help="Name of folder to place fanfic in.")
    parser.add_argument(
        "-l", "--location", default=os.getcwd(),
        help="set download location")
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
    parser.add_argument(
        "-p", "--useproxy", default=use_proxy,
        help="Use proxy when connecting.")

    args = parser.parse_args(['--location=' + location, '--folder=' + folder])

    return args


def about_story():

    source = os.path.join(basePath, arcRoot, folder, category, sfolder)
    cat = category.replace('_', ' ')
    cat_id = db.get_cat_id(cat)
    r = db.get_fanfic(cat_id, sfolder)

    _ = os.system("clear")

    if r["Abandoned"][0] == 1:
        print("WARNING: This story is ABANDONED and will NOT be updated.")
        print("")

    print("Title: ", r["Title"][0])
    print("Author: ", r["Author"][0])
    print("Folder Name: ", r["Folder"][0])
    print("Chapter Count: ", r["Count"][0])

    if r["Complete"][0] == 1:
        print("Complete: Yes")
    else:
        print("Complete: No")

    print("Last Updated: ", r["Update_Date"][0])
    print("Summary: ", textwrap.fill(r["Description"][0]))
    print("")

    cui.pause()

    menu_story()


def download_story():
    global basePath
    global folder
    global sfolder
    global category
    global use_proxy

    location = os.path.join(basePath, arcRoot, folder, category)
    source = os.path.join(location, sfolder)
    cat = category.replace('_', ' ')
    cat_id = db.get_cat_id(cat)
    fic = db.get_fanfic(cat_id, sfolder)

    _ = os.system("clear")
    
    abandon = fic['Abandoned'][0]
    complete = fic['Complete'][0]
    title = fic['Title'][0]
    author = fic['Author'][0]
    sfolder = fic['Folder'][0]
    chapters = fic['Count'][0]
    update = fic['Update_Date'][0]
    last = fic['Last_Checked'][0]
    fic_id = fic['Id'][0]

    if abandon == 1:
        print("WARNING: This story is ABANDONED and will NOT be updated.")
        print("")

    print("Title: ", title)
    print("Author: ", author)
    print("Folder Name: ", sfolder)
    print("Chapter Count: ", chapters)

    if complete == 1:
        print("Complete: Yes")
    else:
        print("Complete: No")

    print("Last Updated: ", update)
    print("")

    if (abandon == 1) or (complete == 1):
        cui.pause()
    else:
        ret = cui.menu_yesno('Download?', False)
        if ret == "Yes":
            chapter = input("Start Chapter [{0}]: ".format(chapters))
            if chapter == "":
                chapter = chapters
            else:
                chapter = int(chapter)
            process_story(location, fic_id, chapter, use_proxy)
            convert_story(source)
            cui.pause()
            menu_story()
        else:
            menu_story()

def process_story(location, fic_id, chapter, use_proxy):

    fic = db.get_fanfic_byId(fic_id)

    potential_keys = []

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

    url_check = current_fanfic.check_url(url)

    if url_check is True:

        ffargs = set_ffargs(location, folder, use_proxy)

        # Initialize class so we can retrieve actual story url
        fanfic = current_fanfic.fanfic(url, ffargs)
            
        # Get fanfic url
        url = fanfic.get_story_url(storyid)
            
        # Initialize class with correct url.
        fanfic = current_fanfic.fanfic(url, ffargs)
            
        # Download list of chapters
        fanfic.get_chapters()
        # Get current chapter count from site
        cc = fanfic.chapter_count

        if cc >= chapter:
            if cc == chapter:
                potential_keys = [chapter]
            else:
                potential_keys = [
                    i * 1 for i in range(chapter, (cc + 1))]
        
        # Show story about to be updated.
        update = fanfic.get_update_date()
                
        print('', end='\n')
        print(title, '/', folder, '/', update)
        
        # Set chapters to download
        fanfic.set_download_chapters(potential_keys)
        
        print('Downloading fanfic: ', title, '/', folder)

        # Download Story
        fanfic.download_fanfic()
        
        print('Downloaded fanfic: ', title, '/', folder)
        
        db.update_chapter_count(fic_id, cc)
        db.update_last_updated(fic_id, update)
        db.update_last_checked(fic_id, date.today())

        print('', end='\n')


def convert_story(location):

    files = get_files(location, "*.htm")

    for htm in files:
        convert_file(location, htm)


def convert_file(source, htm):

    txt = htm.split('.')[0] + '.txt'
    ifile = [os.path.abspath(os.path.join(source, htm))]
    ofile = os.path.abspath(os.path.join(source, txt))
    cmd = ['html2text', '--ignore-emphasis', '-b 80']
    msg = 'Converting file: {0} to text file: {1}'

    print(msg.format(htm, txt))

    with open(ofile, "w") as outfile:
        subprocess.call(cmd + ifile, stdout=outfile)

    for i in ifile:
        os.remove(i)


def toggle_complete():

    cat = category.replace('_', ' ')
    cat_id = db.get_cat_id(cat)
    fic_id = db.get_fic_id(cat_id, sfolder)

    db.set_complete(fic_id)

    menu_story()


def toggle_abandoned():

    cat = category.replace('_', ' ')
    cat_id = db.get_cat_id(cat)
    fic_id = db.get_fic_id(cat_id, sfolder)

    db.set_abandoned(fic_id)

    menu_story()

def reset_update_date():
    cat = category.replace('_', ' ')
    cat_id = db.get_cat_id(cat)
    fic_id = db.get_fic_id(cat_id, sfolder)

    db.reset_update(fic_id)

    menu_story()

def get_entry(text, value):
    # Substitute {0} in text with value.
    return text.format(value)


def menu_editor():
    global editor

    menu = ['vi', 'gedit', 'geany']
    ret = cui.submenu(menu, "Choose Editor")

    editor = ret


def menu_options():
    menu = ['Path', 'Editor', 'Main Menu']
    ret = cui.submenu(menu, "Choose Option")

    if ret == "Editor":
        menu_editor()
    if ret == "Path":
        set_path()


def menu_story():

    menu = ['About Story', 'Download Story']
    menu = menu + ['Toggle Complete', 'Toggle Abandoned']
    menu = menu + ['Reset Updated']
    menu = menu + ["Format File", "Main Menu"]
    ret = cui.submenu(menu, "Chose Story Option")

    if ret == "About Story":
        about_story()
    if ret == "Download Story":
        download_story()
    if ret == "Toggle Complete":
        toggle_complete()
    if ret == "Toggle Abandoned":
        toggle_abandoned()
    if ret == "Reset Updated":
        reset_update_date()
    if ret == "Format File":
        if tfile:
            format_file(os.path.join(basePath, arcRoot,
                                     folder, category,
                                     sfolder, tfile))
        else:
            print("File must be selected first!")


def menu_sync():

    ret = subprocess.run(["which", "rclone"],
                         stdout=subprocess.PIPE,
                         stderr=subprocess.PIPE)
    check = ret.stdout.decode('utf-8')

    if not check:
        print("Command: rclone is required for sync to work!")
        cui.pause()
    else:
        if sync_server == '' or sync_path == '':
            sync = ["Config", "Main Menu"]
        else:
            sync = ["Config",
                    "Sync from Remote",
                    "Sync to Remote",
                    "Main Menu"]

        ret = cui.submenu(sync, "Choose Sync Option")

        if ret == "Config":
            setup_sync()
        if ret == "Sync from Remote":
            source = sync_server + ":" + sync_path + "/" + arcRoot
            dest = basePath + "/" + arcRoot
            run_sync(source, dest, sync_safe)
        if ret == "Sync to Remote":
            source = basePath + "/" + arcRoot
            dest = sync_server + ":" + sync_path + "/" + arcRoot
            run_sync(source, dest, sync_safe)
        if ret == "Main Menu":
            save_config()
            mainmenu()

   


def setup_sync():
    global sync_server
    global sync_path
    global sync_safe

    while True:
        menu = {}
        menu[1] = get_entry("Server: {0}", sync_server)
        menu[2] = get_entry("Path: {0}", sync_path)
        menu[3] = get_entry("Safe Sync: {0}", sync_safe)
        menu[4] = "Previous Menu"

        options = menu.keys()
        _ = os.system("clear")
        print("Setup Sync Config")
        print('')

        for entry in options:
            opt = '%02d' % entry
            print(opt, menu[entry])
        print('')
        selection = input("Please Select: ")

        nodes = []

        if selection == "1":
            ret = subprocess.run(["rclone", "config", "dump"],
                                 stdout=subprocess.PIPE,
                                 stderr=subprocess.PIPE)
            cfg = ret.stdout.decode('utf-8')
            cfg2 = json.loads(cfg)
            for node in cfg2:
                nodes.append(node)

            print("")
            sync_server = cui.submenu(nodes, "Select Server")

        if selection == "2":
            if sync_server == "":
                print("Please specify server first!")
                cui.pause()
            else:
                sync_path = set_sync_path(sync_server)

        if selection == "3":
            if sync_path == "":
                print("Please specify path first!")
                cui.pause()
            else:
                if cui.menu_yesno("Do not overrite database?") == "Yes":
                    sync_safe = True
                else:
                    sync_safe = False

        if selection == "4":
            menu_sync()


def set_sync_path(server):

    tpath = ""

    while True:
        tpath = input("Enter root path to sync folder: ")
        ret = subprocess.run(["rclone", "lsf", "--dirs-only"] +
                             ["--max-depth", str(1)] +
                             [server + ":" + tpath],
                             stdout=subprocess.PIPE,
                             stderr=subprocess.PIPE)
        check = ret.stdout.decode('utf-8').splitlines()
        for folder in check:
            if arcRoot in folder:
                return tpath
        else:
            print("Invalid Folder!")
            cui.pause()


def run_sync(source, dest, update):

    cmd1 = ["rclone", "sync", "--progress", "--checksum"]

    # Prevent upload from overwriting htm folder or database as a safeguard.
    if update:
        cmd2 = ["--exclude", "htm/**", "--exclude", "FanfictionDB.db"]
        cmd = cmd1 + cmd2 + [source, dest]
    else:
        cmd = cmd1 + [source, dest]

    print(cmd)
    subprocess.call(cmd)
    cui.pause()

    menu_sync()


def choose_value(path, value):
    # Get sub-folders in path
    subdirs = next(os.walk(path))[1]
    subdirs.sort()

    if len(subdirs) > 0:
        ret = cui.submenu(subdirs, value)
    else:
        ret = ''
        print("No folders found!")
        cui.pause()

    return ret


def choose_file(path, value):
    global sfolder
    global tfile

    if not os.path.exists(path):
        print("No folder found!")
        ret = ''
        cui.pause()
    else:
        # get files in path
        lfiles1 = get_files(path, '*.txt')
        lfiles2 = get_files(path, '*.pdf')

        lfiles = lfiles1 + lfiles2

        ret = ''

        if len(lfiles) > 0:
            lfiles.append('*.txt')
            lfiles.append('*.pdf')
            lfiles.sort()
            ret = cui.submenu(lfiles, value)
        else:
            lfiles = get_files(path, '*.*')
            if len(lfiles) == 0:
                os.rmdir(path)

            sfolder = ''
            tfile = ''
            ret = tfile

    return ret


def get_files(path, value):
    matches = []
    for root, dirs, files in os.walk(path):
        for filename in fnmatch.filter(files, value):
            matches.append(filename)
    # Sort files alphabetically
    matches.sort()

    return matches


def get_folders(path, value):
    matches = []
    for root, dirs, files in os.walk(path):
        for dirs in fnmatch.filter(files, value):
            matches.append(dirs)
    # Sort folder alphabetically
    matches.sort()

    return matches


def list_files(path, value):
    matches = get_files(path, value)
    print_list(matches)


def print_list(matches):
    divisor = 6
    result = ''

    for i in range(int(len(matches) / divisor)):
            j = divisor * (i * 1)
            result = ''
            for j in range(j, j + (divisor)):
                result = result + matches[j] + '\t'
            print(result)

    k = len(matches) % divisor
    l = (int(len(matches) / divisor) * divisor)

    result = ''
    for m in range(l, len(matches)):
        result = result + matches[m] + '\t'
    print(result)


def run_script(value):
    subprocess.call(value, shell=True)
    cui.pause()


def format_file(value):

    print("Processing File... please wait.")

    t_file = value + ".tmp"

    f1 = open(value, "r")
    f2 = open(t_file, "w")

    fdata = f1.read()
    flines = fdata.splitlines()
    for line in flines:
        data1 = textwrap.fill(line, 80)
        data2 = data1.splitlines()
        for i in range(len(data2)):
            f2.write(data2[i] + "\n")
            if i == (len(data2) - 1):
                f2.write("\n")
    f2.flush()
    os.fsync(f2.fileno())

    f1.close
    f2.close

    os.remove(value)
    shutil.move(t_file, value)


def read_file(value):

    filename, file_extension = os.path.splitext(value)

    if file_extension == ".pdf":
        subprocess.call('xdg-open ' + value, shell=True)
    else:
        subprocess.call(editor + ' ' + value, shell=True)


def ensure_dir(path):
    if not os.path.exists(path):
        os.makedirs(path)


def mov_files(path, dfolder, value):
    global tfile
    # Build destination path
    dpath = os.path.join(basePath, arcRoot, dfolder, category, sfolder)
    # Create destination folder if it doesn't exist.
    ensure_dir(dpath)

    list_files(path, value)
    confirm = input("Enter Y to proceed: ")
    if confirm.lower() == 'y':
        print("Archiving files")
        try:
            matches = get_files(path, value)
            for i in range(len(matches)):
                print("Archiving: ", matches[i])
                shutil.move(os.path.join(path, matches[i]),
                            os.path.join(dpath, matches[i]))
            print("Archive complete.")
        except Error as err:
            print("Problem Archiving File.")

    tfile = ''
    cui.pause()


def get_config():
    # create config dir if it doesn't exist and return path to config.
    home = os.path.expanduser("~")
    cfg = os.path.join(home, ".ff")
    ensure_dir(cfg)

    return os.path.join(cfg, "config.ini")


def load_config():
    global basePath
    global folder
    global dfolder
    global sfolder
    global category
    global tfile
    global editor
    global db_folder
    global db_name
    global sync_server
    global sync_path
    global sync_safe
    global use_proxy

    t_editor = ''
    t_path = ''
    t_dbname = ''
    t_dbfolder = ''
    t_sync_server = ''
    t_sync_path = ''
    t_sync_safe = '1'
    t_useproxy = ''
    config = SafeConfigParser()

    cfg = get_config()

    if os.path.isfile(cfg):
        config.read(cfg)

        try:
            t_folder = config.get('archive', 'sourcefolder')
            t_category = config.get('archive', 'category')
            t_sfolder = config.get('archive', 'storyfolder')
            t_dfolder = config.get('archive', 'destfolder')
            t_tfile = config.get('archive', 'textfile')
            t_editor = config.get('options', 'editor')
            t_path = config.get('path', 'path')
            t_dbfolder = config.get('path', 'dbfolder')
            t_dbname = config.get('path', 'dbname')
            t_sync_server = config.get('sync', 'server')
            t_sync_path = config.get('sync', 'path')
            t_sync_safe = config.get('sync', 'safe')
            t_useproxy = config.get('download', 'proxy_enable')
        except:
            i = 0

        if t_sync_server:
            sync_server = t_sync_server

        if t_sync_path:
            sync_path = t_sync_path

        if t_editor:
            editor = t_editor

        if t_path:
            basePath = t_path

        if t_dbfolder:
            db_folder = t_dbfolder

        if t_dbname:
            db_name = t_dbname

        if t_sync_safe == '1':
            sync_safe = True
        else:
            sync_safe = False

        if t_useproxy:
            use_proxy = t_useproxy

        spath = os.path.join(basePath, arcRoot, t_folder)
        dpath = os.path.join(basePath, arcRoot, t_dfolder)

        # Validate parameters in config file are valid.
        if os.path.exists(dpath):
            dfolder = t_dfolder
        if os.path.exists(spath):
            folder = t_folder
            spath = os.path.join(spath, t_category)
            if os.path.exists(spath):
                category = t_category
                spath = os.path.join(spath, t_sfolder)
                if os.path.exists(spath):
                    sfolder = t_sfolder
                    sfile = os.path.join(spath, t_tfile)
                    if os.path.isfile(sfile):
                        tfile = t_tfile


def save_config():
    cfg = get_config()

    config = SafeConfigParser()

    config.read(cfg)

    if use_proxy is None:
        use_proxy = 0

    if not config.has_section('archive'):
        config.add_section('archive')
    if not config.has_section('options'):
        config.add_section('options')
    if not config.has_section('path'):
        config.add_section('path')
    if not config.has_section('sync'):
        config.add_section('sync')
    if not config.has_section('download'):
        config.add_section('download')

    config.set('archive', 'sourcefolder', folder)
    config.set('archive', 'category', category)
    config.set('archive', 'storyfolder', sfolder)
    config.set('archive', 'destfolder', dfolder)
    config.set('archive', 'textfile', tfile)
    config.set('options', 'editor', editor)
    config.set('path', 'path', basePath)
    config.set('path', 'rootfolder', arcRoot)
    config.set('path', 'dbfolder', db_folder)
    config.set('path', 'dbname', db_name)
    config.set('sync', 'server', sync_server)
    config.set('sync', 'path', sync_path)
    config.set('download', 'proxy_enable', str(use_proxy))

    if sync_safe:
        config.set('sync', 'safe', '1')
    else:
        config.set('sync', 'safe', '0')

    with open(cfg, 'w') as f:
        config.write(f)


def set_path():
    global basePath

    msg = 'Sync Folder: {0}'
    print(msg.format(basePath))

    root = input("Enter root path to sync folder: ")

    if os.path.exists(root):
        basePath = root
    else:
        print("Invalid path, please try again.")
        cui.pause()
        _ = os.system("clear")
        set_path()


def mainmenu():
    global basePath
    global folder
    global dfolder
    global sfolder
    global category
    global tfile

    load_config()

    while True:
        menu = {}
        menu[1] = get_entry("Source Folder: {0}", folder)
        menu[2] = get_entry("Source Category: {0}", category)
        menu[3] = get_entry("Story: {0}", sfolder)
        menu[4] = get_entry("Dest Folder: {0}",
                            dfolder + "/" + category + "/" + sfolder)
        menu[5] = get_entry("Text File: {0}", tfile)
        menu[6] = "Read File"
        menu[7] = "Move File"
        menu[8] = "Story Options"
        menu[9] = "Sync Options"
        menu[10] = "Config Options"
        menu[11] = "Exit"

        options = menu.keys()
        _ = os.system("clear")
        print("Manage FanFiction Archive")
        print('')
        print(get_entry("Sync Folder: {0}", basePath))
        print('')
        for entry in options:
            opt = '%02d' % entry
            print(opt, menu[entry])
        print('')
        selection = input("Please Select: ")
        if selection == '1':
            root = os.path.join(basePath, arcRoot)
            if not os.path.exists(root):
                print("Please set a valid Sync Folder first!")
                basePath = ''
            else:
                folder = choose_value(os.path.join(basePath, arcRoot),
                                      "Choose Folder:")
        elif selection == '2':
            if not folder:
                print("Please choose a folder first!")
                cui.pause()
            else:
                category = choose_value(os.path.join(basePath, arcRoot,
                                        folder), "Choose Category:")
                sfolder = ''
                tfile = ''
        elif selection == '3':
            if not category:
                print("Please choose a category first!")
                cui.pause()
            else:
                sfolder = choose_value(os.path.join(basePath, arcRoot,
                                                    folder, category),
                                       "Choose Story Folder:")
                tfile = ''
        elif selection == '4':
            if not sfolder:
                print("Please choose a story first!")
                cui.pause()
            else:
                dfolder = choose_value(os.path.join(basePath, arcRoot),
                                       "Choose Destination Folder:")
        elif selection == '5':
            if not sfolder:
                print("Please choose a story first!")
                cui.pause()
            else:
                tfile = choose_file(os.path.join(basePath, arcRoot, folder,
                                                 category, sfolder),
                                    "Choose File:")
        elif selection == '6':
            if not tfile:
                print("Please select a file first!")
                cui.pause()
            else:
                read_file(os.path.join(basePath, arcRoot, folder,
                                       category, sfolder, tfile))
        elif selection == '7':
            if not tfile:
                print("Please select a file first!")
                cui.pause()
            else:
                mov_files(os.path.join(basePath, arcRoot, folder,
                                       category, sfolder), dfolder, tfile)
        elif selection == '8':
            if not sfolder:
                print("Please choose a story first!")
                cui.pause()
            else:
                menu_story()
        elif selection == '9':
            menu_sync()
        elif selection == '10':
            menu_options()
        elif selection == '11':
            save_config()
            sys.exit(0)
        else:
            print("Unknown Option Selected!")
            cui.pause()


def main():
    global db

    load_config()

    if not os.path.exists(basePath):
        set_path()
        save_config()
        ensure_dir(os.path.join(basePath, arcRoot))
        ensure_dir(os.path.join(basePath, arcRoot, db_folder))

    # Initialize DB
    db = FanficDB()
    # Call Main Menu
    mainmenu()


if __name__ == "__main__":
    sys.exit(main())
