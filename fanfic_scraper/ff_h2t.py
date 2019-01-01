#!/usr/bin/python3

import os
import sys
import fnmatch
import subprocess
import argparse


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
            matches.append(filename)
    # Sort files alphabetically
    matches.sort()

    return matches


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


def main():

    """Rename fanfic chapters."""
    parser = argparse.ArgumentParser(
        description=('Utility to conver html fanfic chapters to text'))

    parser.add_argument('folder', help='fanfic folder to process')

    args = parser.parse_args()

    source = args.folder

    if not os.path.exists(source):
        sys.stderr.write("ERROR: Folder %r was not found!" % (source,))
        return 1

    files = get_files(source, "*.htm")

    # print_list(files)

    source = os.path.realpath(source)

    for htm in files:
        convert_file(source, htm)


if __name__ == "__main__":
    sys.exit(main())
