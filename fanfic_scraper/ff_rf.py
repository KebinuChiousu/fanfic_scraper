#!/usr/bin/python3

import os
import sys
import argparse
import shutil
import fnmatch
import re


def pause():
    # Wait for user input.
    programPause = input("Press the <ENTER> key to continue...")


def print_format(text, value):
    # Substitute {0} in text with value.
    print(text.format(value))


def get_files(path, value):
    matches = []
    for root, dirs, files in os.walk(path):
        for filename in fnmatch.filter(files, value):
            f = root + '/' + filename
            matches.append(f)

    # Sort files alphabetically
    matches.sort()

    return matches


def ensure_dir(path):
    if not os.path.exists(path):
        os.makedirs(path)


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


def get_prefix(file):
    matches = []
    t = ''

    regex1 = "([a-zA-Z]+[0-9]+[a-zA-Z]+-)"
    regex2 = "([a-zA-Z]+[0-9]+-)"
    regex3 = "([a-zA-Z]+-)"
    regex4 = "([a-zA-Z]+)"
    regex5 = [regex1, regex2, regex3, regex4]

    r = re.compile('|'.join(regex5))

    m = r.match(file)

    if m:
        if (m.group(1)):
            t = m.group(1)
        elif (m.group(2)):
            t = m.group(2)
        elif (m.group(3)):
            t = m.group(3)
        elif (m.group(4)):
            t = m.group(4)

    return t


def main():

    """Rename fanfic chapters."""
    parser = argparse.ArgumentParser(
        description=('Utility to rename fanfic chapters'))

    parser.add_argument('folder', help='fanfic folder to process')

    args = parser.parse_args()

    source = args.folder

    txt = get_files(source, "*.txt")
    htm = get_files(source, "*.htm")

    files = txt + htm

    abort = 0

    for f in files:
        d = os.path.dirname(f)
        f = os.path.basename(f)
        cat = os.path.basename(d)

        if '_' in f:
            target = f.replace('_', '-')
            ren_file(d, f, target)
            f = target

        p = get_prefix(f)

        if p == '':
            abort = 1
        else:
            try:
                n = int(f.replace(p, '').split('.')[0])
            except:
                abort = 1

        if (abort == 0):
            p = p.split('-')[0] + '-'
            num = '%03d' % n
            ext = f.replace(p, '').split('.')[1]
            target = p + num + '.' + ext
            ren_file(d, f, target)
        else:
            abort = 0


def ren_file(folder, orig, new):
    source = os.path.join(folder, orig)
    target = os.path.join(folder, new)

    print("Fanfic: " + folder)
    print("Renaming: " + orig + " to " + new)

    shutil.move(source, target)


if __name__ == "__main__":
    sys.exit(main())
