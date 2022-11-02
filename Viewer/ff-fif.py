#!/usr/bin/python3

import os
import sys
import shutil
import fnmatch
import re

def pause():
    #Wait for user input.
    programPause = input("Press the <ENTER> key to continue...")

def print_format(text,value):
    #Substitute {0} in text with value.
    print(text.format(value))

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

def ensure_dir(path):     
    if not os.path.exists(path):
        os.makedirs(path)

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

def mov_files(path,dpath,value):    
    # Create destination folder if it doesn't exist.
    ensure_dir(dpath)
    
    #list_files(path,value)    
    #confirm=input("Enter Y to proceed: ")
    confirm = 'y'
    
    if confirm.lower() == 'y':
        try:
            matches = get_files(path,value)
            for i in range(len(matches)):
                source = os.path.join(path,matches[i])
                if os.path.isfile(source):
                    print("Moving: ", matches[i])
                    dest = os.path.join(dpath,matches[i])
                    shutil.move(source,dest)
            print("Move complete.")
        except:
            print("Problem moving files.")

    #pause()

def get_prefixes(files):
    matches = []
    t = []
    r = re.compile("([a-zA-Z]+[0-9]+-)|([a-zA-Z]+)")

    for file in files:
        m = r.match(file)
        if m:
            if (m.group(1)):
                t.append(m.group(1))
            else:
                t.append(m.group(2))

    matches = list(set(t))
    matches.sort(reverse=True)
    
    #print_list(matches)
    #print()
    #pause()
    
    return matches

def main(argv):
    if len(argv) < 2:
        sys.stderr.write("Usage: %s <folder_path>" % (argv[0],))
        return 1

    if not os.path.exists(argv[1]):
        sys.stderr.write("ERROR: Folder %r was not found!" % (argv[1],))
        return 1
    
    source = argv[1]
    
    files = get_files(source,"*.txt")
    prefs = get_prefixes(files)
    
    for p in prefs:
        d = p.split("-")[0]
        dest = os.path.join(source,d)
        print_format("Moving files into folder: {0}",d)
        mov_files(source,dest,p+"*.txt")

if __name__ == "__main__":
    sys.exit(main(sys.argv))
