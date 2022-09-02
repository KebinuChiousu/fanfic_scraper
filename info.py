#!/usr/bin/env python3
import sys
import argparse
import tomli

info = None

def parseargs():

    parser = argparse.ArgumentParser(
        description=('Python Project Info Parser'))

    parser.add_argument('--name', required=False, action='store_true',
                        help='Show Name')

    parser.add_argument('--version', required=False, action='store_true',
                        help='Show Version')

    args = parser.parse_args()

    if args.name:
        print(info["name"])
    if args.version:
        print(info["version"])

    parser.print_help()

def loaddata():

    global info

    with open("pyproject.toml", mode="rb") as fp:
        config = tomli.load(fp)

    info = config["tool"]["poetry"]

def main():

    loaddata()

    parseargs()

if __name__ == "__main__":
    sys.exit(main())
