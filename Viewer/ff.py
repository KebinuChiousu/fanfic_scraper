#!/usr/bin/python
import os
import sys
import shutil
import fnmatch
import subprocess
from ConfigParser import SafeConfigParser
from datetime import datetime
import sqlite3
import textwrap


category=''
folder=''
dfolder=''
sfolder=''
tfile=''
editor='vi'

basePath = '/home/ubuntu/OneDrive/'
arcRoot = 'FF_Archive'

def get_entry(text,value):
    #Substitute {0} in text with value.
    return text.format(value)

def pause():
    #Wait for user input.
    programPause = raw_input("Press the <ENTER> key to continue...")

def submenu(values,prompt):
    inc=9
    start=0
    stop=inc    
    np=0
    pp=0
 
    while True:
        print(prompt)
        print('')        
        for i in range(start,len(values)):
            print i, values[i]
            j=i
            # limit number of entries to stop var.
            if i >= stop:
                # Stop offering next if the last entry is reached.
                if i < (len(values)-1):
                    np=stop+1
                    i=i+1
                    print np, "Next Page"
                    break
        #if stop > inc, allow prev page.        
        if stop > inc:
            pp=i+1
            i=i+1
            print pp, "Prev Page"
        print('')
        #parse input into number, blank is invalid.
        try:
            sel=int(raw_input("Enter Selection: "))
        except:
            sel = j+1
        # Validate input is valid for current menu.
        if (sel >= start) and (sel <= j): 
            ret = values[sel]
            break
        else:
         # Move 1 page ahead
         if sel == np:
             start=start+10
             stop=start+9
             _=os.system("clear")
         # Move 1 page back
         elif sel == pp:
             start=start-10
             stop=start+9
             _=os.system("clear")
         else:    
             print "Unknown Option Selected!"
             pause()
             _=os.system("clear")

    return ret

def choose_value(path,value):
    #Get sub-folders in path
    subdirs = next(os.walk(path))[1]
    subdirs.sort()    
    
    if len(subdirs) > 0:
        ret = submenu(subdirs,value)
    else:
        ret = ''
        print "No folders found!"
        pause()

    return ret    

def choose_file(path,value):
    global sfolder
    global tfile
    
    #get files in path
    lfiles = get_files(path,'*.txt')    
    ret=''
 
    if len(lfiles) > 0:
        lfiles.append('*.txt')
        lfiles.sort()
        ret = submenu(lfiles,value)
    else:
        lfiles = get_files(path,'*.*')
        if len(lfiles) == 0:
            os.rmdir(path)

        sfolder=''
        tfile=''
        ret=tfile

    return ret

def get_files(path,value):
    matches = [] 
    for root, dirs, files in os.walk(path):
        for filename in fnmatch.filter(files,value):
            matches.append(filename)
    # Sort files alphabetically
    matches.sort()

    return matches

def get_folders(path,value):
    matches = []
    for root, dirs, files in os.walk(path):
        for dirs in fnmatch.filter(files,value):
            matches.append(dirs)
    # Sort folder alphabetically
    matches.sort()

    return matches

def list_files(path,value):
    matches = get_files(path,value)
    print_list(matches)

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

def run_script(value):
    subprocess.call(value,shell=True)
    pause()

def format_file(value):

    print "Processing File... please wait."

    t_file = value+".tmp"

    f1 = open(value, "r")
    f2 = open(t_file, "w")

    fdata = f1.read()
    flines = fdata.splitlines()
    for line in flines:
        data1 = textwrap.fill(line,80)
        data2 = data1.splitlines()
        for i in range(len(data2)):
            f2.write(data2[i]+"\n")
            if i == (len(data2)-1):
                f2.write("\n")
    f2.flush()
    os.fsync(f2.fileno())

    f1.close
    f2.close

    os.remove(value)
    shutil.move(t_file,value)

def read_file(value):
    subprocess.call(editor+' '+value,shell=True)

def ensure_dir(path):     
    if not os.path.exists(path):
        os.makedirs(path)

def mov_files(path,dfolder,value):
    global tfile
    # Build destination path
    dpath = os.path.join(basePath,arcRoot,dfolder,category,sfolder)    
    # Create destination folder if it doesn't exist.
    ensure_dir(dpath)

    list_files(path,value)
    confirm=raw_input("Enter Y to proceed: ") 
    if confirm.lower() == 'y':
        print "Archiving files"
        try:
            matches = get_files(path,value)             
            for i in range(len(matches)):
                print "Archiving: ", matches[i]
                shutil.move(os.path.join(path,matches[i]),os.path.join(dpath,matches[i]))
            print "Archive complete."
        except:
            print "Problem Archiving File."

    tfile = ''
    pause()

def get_config():
    #create config dir if it doesn't exist and return path to config.
    home = os.path.expanduser("~")
    cfg = os.path.join(home,".ff")
    ensure_dir(cfg)
     
    return os.path.join(cfg,"config.ini")

def load_config():
    global basePath
    global folder
    global dfolder
    global sfolder
    global category    
    global tfile
    global editor
    t_editor =''
    t_path = ''
    config = SafeConfigParser()    

    cfg = get_config()

    if os.path.isfile(cfg):            
        config.read(cfg)        

        try:
            t_folder = config.get('ff','sourcefolder')
            t_category = config.get('ff','category')
            t_sfolder = config.get('ff','storyfolder') 
            t_dfolder = config.get('ff','destfolder')
            t_tfile = config.get('ff','textfile')
            t_editor = config.get('ff','editor')
            t_path = config.get('ff','path')
        except:
            i=0

        if t_editor:
            editor = t_editor
            
        if t_path:
            basePath = t_path

        spath = os.path.join(basePath,arcRoot,t_folder)
        dpath = os.path.join(basePath,arcRoot,t_dfolder)

        #Validate parameters in config file are valid.
        if os.path.exists(dpath):
            dfolder = t_dfolder    
        if os.path.exists(spath):        
            folder = t_folder
            spath = os.path.join(spath,t_category)
            if os.path.exists(spath):
                category = t_category
                spath = os.path.join(spath,t_sfolder)
                if os.path.exists(spath):
                    sfolder = t_sfolder     
                    sfile = os.path.join(spath,t_tfile)
                    if os.path.isfile(sfile):
                        tfile = t_tfile
             
def save_config():    
    cfg = get_config()    

    config = SafeConfigParser()

    config.read(cfg)

    if not config.has_section('ff'):
        config.add_section('ff')

    config.set('ff','path',basePath)
    config.set('ff','sourcefolder',folder)
    config.set('ff','category',category)
    config.set('ff','storyfolder',sfolder)
    config.set('ff','destfolder',dfolder)
    config.set('ff','textfile',tfile)
    config.set('ff','editor',editor)

    with open(cfg, 'w') as f:
        config.write(f)

def sync_menu():

    sync = ['Download','Upload','Clean Folder','Main Menu']
    ret = submenu(sync,"Choose OneDrive Option")

    home = os.path.expanduser("~")
    if ret == "Download":        
        run_script(os.path.join(home,"bin/sync_dl.sh"))
    if ret == "Upload":        
        run_script(os.path.join(home,"bin/sync_ul.sh"))
    if ret == "Clean Folder":
        run_script(os.path.join(home,"bin/clean-onedrive"))

def story_menu():

    menu = ['About Story', 'Toggle Complete', "Format File", "Main Menu"]
    ret = submenu(menu,"Chose Story Option")

    if ret == "About Story":
       about_story()
    if ret == "Toggle Complete":
       toggle_complete()
    if ret == "Format File":
        if tfile:
            format_file(os.path.join(basePath,arcRoot,folder,category,sfolder,tfile))
        else:
            print "File must be selected first!"

def edit_menu():
    global editor

    menu = ['vi','gedit', 'geany', 'scite']
    ret = submenu(menu,"Choose Editor")

    editor = ret

def options_menu():
    menu = ['Path','Editor', 'Main Menu']
    ret = submenu(menu, "Choose Option")

    if ret == "Editor":
        edit_menu()
    if ret == "Path":
        set_path()

def set_path():
    global basePath
    
    msg = 'Sync Folder: {0}'
    print msg.format(basePath)
    
    root=raw_input("Enter root path to sync folder: ") 
    
    if os.path.exists(root):
        basePath = root
    else:
        print "Invalid path, please try again."
        pause()
        _=os.system("clear")
        set_path()

def toggle_complete():
    
    db_name = 'FanfictionDB.db'
    db_folder = 'Read'
    db_path = os.path.join(basePath,arcRoot,db_folder,db_name)

    if os.path.isfile(db_path):

        conn = sqlite3.connect(db_path)
        conn.row_factory = sqlite3.Row
        c = conn.cursor()        

        cat = (category.replace('_',' '),)             
  
        c.execute("SELECT * FROM Category WHERE Name=?;",cat)
        
        r = c.fetchone()  

        cat_id = r['Id']

        f = (cat_id,sfolder)

        c.execute("Select Complete from Fanfic Where Category_Id=? And Folder=?;",f)
        r = c.fetchone()

        t = r['Complete']

        if t == 1:
            t = 0
        else:
            t = 1

        f = (t,cat_id,sfolder)

        c.execute("UPDATE Fanfic set Complete = ? Where Category_Id=? And Folder=?;",f)
        conn.commit()

    about_story()

def about_story():

    db_name = 'FanfictionDB.db'
    db_folder = 'Read'
    db_path = os.path.join(basePath,arcRoot,db_folder,db_name)    

    abort = False

    if os.path.isfile(db_path):
    
        conn = sqlite3.connect(db_path)
        conn.row_factory = sqlite3.Row
        c = conn.cursor()        
    
        cat = (category.replace('_',' '),)             
    
        c.execute("SELECT * FROM Category WHERE Name=?;",cat)
        
        r = c.fetchone()  
    
        cat_id = r['Id']
    
        f = (cat_id,sfolder)
    
        c.execute("SELECT * FROM Fanfic Where Category_Id=? And Folder=?;",f)
        r= c.fetchone()
    
        try:
            dt = r["Update_Date"]
        except:
            abort = True
        
        if abort == False:
        
            dt = datetime.strptime(dt, "%Y-%m-%d %H:%M:%S")
        
            _=os.system("clear")
            if r["Abandoned"] == 1:
                print "WARNING: This story is ABANDONED and will NOT be updated."
            print "Title: ", r["Title"]
            print "Author: ", r["Author"]
            print "Folder Name: ", r["Folder"]
            print "Chapter Count: ", r["Count"]
            if r["Complete"] == 1:
                print "Complete: Yes"
            else:
                print "Complete: No"
            print "Last Updated: ", dt.strftime("%B %d, %Y")
            print "Summary: ", textwrap.fill(r["Description"])
            print ""
        
            pause()

            if r["Abandoned"] == 1:
                print "Abandoned story, entering archive mode..."
                tfile='*.txt'
                mov_files(os.path.join(basePath,arcRoot,folder,category,sfolder),dfolder,tfile)
        
        else:
            print "Unable to locate story."
            pause()

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
        menu[' 1'] = get_entry("Source Folder: {0}",folder)
        menu[' 2'] = get_entry("Source Category: {0}",category)
        menu[' 3'] = get_entry("Story: {0}",sfolder)
        menu[' 4'] = get_entry("Dest Folder: {0}",dfolder+"/"+category+"/"+sfolder)
        menu[' 5'] = get_entry("Text File: {0}",tfile)
        menu[' 6'] = "Read File"
        menu[' 7'] = "Move File"
        menu[' 8'] = "Story Options"
        menu[' 9'] = "Sync Options"
        menu['10'] = "Config Options"        
        menu['11'] = "Exit"
        options=menu.keys()
        options.sort()
        _=os.system("clear")
        print("Manage FanFiction Archive")
        print('')
        print(get_entry("Sync Folder: {0}",basePath))
        print('')
        for entry in options: 
            print entry, menu[entry]
        print('')
        selection=raw_input("Please Select: ") 
        if selection =='1': 
            root = os.path.join(basePath,arcRoot)
            if not os.path.exists(root):
                print("Please set a valid Sync Folder first!")
                basePath = ''
            else:
                folder = choose_value(os.path.join(basePath,arcRoot),"Choose Folder:")
        elif selection == '2':
            if not folder:
                print("Please choose a folder first!")
                pause()
            else:
                category = choose_value(os.path.join(basePath,arcRoot,folder),"Choose Category:")
                sfolder = ''
                tfile = ''
        elif selection == '3':
            if not category:
                print("Please choose a category first!")
                pause()
            else:
                sfolder = choose_value(os.path.join(basePath,arcRoot,folder,category),"Choose Story Folder:")
                tfile = ''
        elif selection == '4':
            if not sfolder:
                print("Please choose a story first!")
                pause()
            else:
                 dfolder = choose_value(os.path.join(basePath,arcRoot),"Choose Destination Folder:")
        elif selection == '5':
            if not sfolder:
                print("Please choose a story first!")
                pause()
            else:
                 tfile = choose_file(os.path.join(basePath,arcRoot,folder,category,sfolder),"Choose File:")
        elif selection == '6':
            if not tfile:
                print("Please select a file first!")
                pause()
            else:
                read_file(os.path.join(basePath,arcRoot,folder,category,sfolder,tfile))
        elif selection == '7':
            if not tfile:
                print("Please select a file first!")
                pause()
            else:
                mov_files(os.path.join(basePath,arcRoot,folder,category,sfolder),dfolder,tfile)
        elif selection == '8':
            story_menu()
        elif selection == '9':
            sync_menu()
        elif selection == '10':
            options_menu()
        elif selection == '11':
            save_config()
            break
        else: 
            print "Unknown Option Selected!"    
            pause()

mainmenu()
