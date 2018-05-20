#!/usr/bin/env python3
import argparse
import requests
import os
from urllib.parse import urlparse
from urllib3.exceptions import InsecureRequestWarning
from fanfic_scraper import current_fanfic

def main():
    """Parse input and download fanfic(s)."""
    parser = argparse.ArgumentParser(
        description=(
            'Downloads all fanfic chapters from'
            'the given url (currently works with fanfiction.net'
            '). Example - A url input '
            ' http://www.fanfiction.net/s/12861961/1/Magical-Player looks '
            'for the fanfic chapters in the url and downloads them all.'))

    parser.add_argument('urls', metavar='url', nargs='+',
                        help='fanfic urls to download')
    parser.add_argument(
        "-f", "--folder", required=True, help="Name of folder to place fanfic in.")
    parser.add_argument(
        "-l", "--location", default=os.getcwd(), help="set download location")
    parser.add_argument(
        "-c", "--chapters", default=False,
        help="Specify chapters to download separated by : (10:15).")
    parser.add_argument(
        "-ct", "--chapterthreads", default=5,
        help="Number of parallel chapters downloads.")    
    parser.add_argument(
        "-wt", "--waittime", default=15,
        help="Wait time before retry if encountered with an error")
    parser.add_argument(
        "-rt", "--retries", default=10,
        help="Number of retries before giving up")

    args = parser.parse_args()

    for url in args.urls:
        # If https, check before using verify False
        urlscheme = urlparse(url)
        verify_https = False
        if urlscheme.scheme == 'https':
            try:
                requests.get(url)
                verify_https = True
            except requests.exceptions.SSLError:
                verify_https = False
                print('Could not validate https certificate for url:' +
                      '%s. Proceeding with Insecure certificate.' % (url))
                requests.packages.urllib3.disable_warnings(
                    category=InsecureRequestWarning)

        fanfic = current_fanfic.fanfic(url, args, verify_https)
        fanfic.get_chapters()
        print('Downloading fanfic: ' + fanfic.name)

        # Get chapters to download
        if args.chapters:
            try:
                start_stop = args.chapters.split(':')
                if len(start_stop) == 1:
                    potential_keys = [int(start_stop[0])]
                elif len(start_stop) == 2:
                    potential_keys = [
                        i * 1 for i in range(int(start_stop[0]),int(start_stop[1])+1)
                        ]
                else:
                    raise SyntaxError(
                        "Chapter inputs should be separated by ':'")
            except TypeError:
                raise SyntaxError("Chapter inputs should be separated by ':'")

            fanfic.set_download_chapters(potential_keys)
        else:
            fanfic.set_download_chapters()


        #print(fanfic.chapter_count)

        fanfic.download_fanfic()
        print('Downloaded fanfic: ' + fanfic.title)


if __name__ == '__main__':
    main()
