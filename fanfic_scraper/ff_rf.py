#!/usr/bin/python3

import os
import sys
import argparse
import shutil
import fnmatch
import re
from collections import defaultdict

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
            f = root+'/'+filename
            matches.append(f)

    # Sort files alphabetically
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

def get_prefix(file):
    matches = []
    r = re.compile("([a-zA-Z]+[0-9]+[a-zA-Z]+-)|([a-zA-Z]+[0-9]+-)|([a-zA-Z]+)")

    m = r.match(file)
    
    if m:
        if (m.group(1)):
            t = m.group(1)
        elif (m.group(2)):
            t = m.group(2)
        elif (m.group(3)):
            t = m.group(3)

    return t

def main():

    """Rename fanfic chapters."""
    parser = argparse.ArgumentParser(
        description=('Utility to rename fanfic chapters'))

    parser.add_argument('folder', help='fanfic folder to process')

    args = parser.parse_args()

    source = args.folder

    txt = get_files(source,"*.txt")
    htm = get_files(source,"*.htm")

    files = txt+htm

    for f in files:
        d = os.path.dirname(f)
        f = os.path.basename(f)
        p = get_prefix(f)
        n = int(f.replace(p,'').split('.')[0])
        p = p.split('-')[0] + '-'
        num = '%03d' % n
        ext = f.replace(p,'').split('.')[1]
        target = p+num+'.'+ext
        print("Renaming: " + f + " to " + target)
        shutil.move(os.path.join(d,f),os.path.join(d,target))

if __name__ == "__main__":
    sys.exit(main())
