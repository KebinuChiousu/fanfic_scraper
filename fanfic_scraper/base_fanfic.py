"""Base Fanfic class."""
import sys
import os
from collections import OrderedDict
import concurrent.futures
import requests
from requests.auth import HTTPBasicAuth
from requests.adapters import HTTPAdapter
import urllib.parse


class BaseFanfic:
    """ Base Fanfic class. Contains chapters."""

    def __init__(self, fanfic_url, program_args):
        """Init function. Creates chapters for given fanfic."""
        self.title = ''
        self.url = fanfic_url
        self.name = program_args.folder
        self.location = program_args.location
        # Set download location
        self.download_location = os.path.abspath(
            os.path.join(self.location, self.name))
        # Set threads and retry values
        self.chapter_threads = program_args.chapterthreads
        self.wait_time = program_args.waittime
        self.max_retries = program_args.retries
        # Set verify mode
        self.verify_https = True
        self.all_chapters = None
        self.use_proxy = program_args.useproxy
        self.chapter_count = 0
        self.fanfic_id = 0

    def send_request(self, url):
        r = None

        if int(self.use_proxy) == 1:

            # https://github.com/TeamHG-Memex/aquarium

            s = requests.Session()
            s.mount('http://127.0.0.1:8050', HTTPAdapter(max_retries=5))

            proxy_url = 'http://127.0.0.1:8050/render.html?'
            proxy_url = proxy_url + 'proxy=tor&url={0}'.format(url)
            auth = HTTPBasicAuth('user', 'userpass')

            r = requests.get(proxy_url, auth=auth)

        else:

            r = requests.get(url)

        return r

    def get_chapters(self):
        self.all_chapters = self.extract_chapters()
        self.chapter_count = len(self.all_chapters)

    def set_download_chapters(self, potential_keys=None, silent=False):
        """Set chapters to download."""
        if potential_keys:
            keys = list(set(potential_keys) & set(self.all_chapters.keys()))
        else:
            keys = list(self.all_chapters.keys())

        # Sort keys to make it ascending order and make it a new dict
        unsorted_chapters = {key: self.all_chapters[key]
                             for key in keys}

        self.chapters_to_download = OrderedDict(
            sorted(unsorted_chapters.items(), key=lambda t: t[0]))
        # Print downloading chapters
        if not silent:
            print("Downloading the below chapters:")
            print(sorted(keys))

    def story_info(self):

        self.get_chapters()
        potential_keys = [1]
        self.set_download_chapters(potential_keys, True)

        for chapter_num, chapter in self.chapters_to_download.items():
            ret = chapter.story_info()
            break

        return ret

    def download_fanfic(self):
        """Begin download of chapters in the fanfic."""

        if not os.path.exists(self.download_location):
            os.makedirs(self.download_location)

        with concurrent.futures.ThreadPoolExecutor(
                max_workers=self.chapter_threads) as executor:
            future_to_chapter = {
                executor.submit(chapter.download_chapter): chapter_num
                for chapter_num, chapter in self.chapters_to_download.items()}

            for future in concurrent.futures.as_completed(future_to_chapter):
                chapter_num = future_to_chapter[future]
                try:
                    future.result()
                except Exception as exc:
                    print('Chapter-%g generated an exception: %s'
                          % (chapter_num, exc))
                    import traceback
                    exc_info = sys.exc_info()
                    traceback.print_exception(*exc_info)
                else:
                    print('Downloaded: Chapter-%g' % (chapter_num))

    def extract_chapters(self):
        """Extract chapters function (backbone)."""
        pass


class BaseChapter:
    """Base Chapter class."""

    def __init__(self, fanfic, chapter_num, chapter_url):
        """Initialize constants required for download."""
        # Extract necessary information from the fanfic object
        self.fanfic_url = fanfic.url
        self.fanfic_id = fanfic.fanfic_id
        self.use_proxy = fanfic.use_proxy
        self.fanfic_name = fanfic.name
        self.fanfic_download_location = fanfic.download_location
        # Create chapter specific variables
        self.chapter_num = chapter_num
        self.chapter_url = chapter_url
        # Threads and retry time
        self.wait_time = fanfic.wait_time
        self.max_retries = fanfic.max_retries
        # Set verify mode
        self.verify_https = fanfic.verify_https
        # Get download chapter location
        self.chapter_location = fanfic.download_location

    def send_request(self, url):
        r = None
        new_url = url

        if int(self.use_proxy) == 1:

            s = requests.Session()
            s.mount('http://127.0.0.1:8050', HTTPAdapter(max_retries=5))

            new_url = urllib.parse.quote_plus(url)

            # https://github.com/TeamHG-Memex/aquarium

            proxy_url = 'http://127.0.0.1:8050/render.html?'
            proxy_url = proxy_url + 'proxy=tor&url={0}'.format(new_url)
            auth = HTTPBasicAuth('user', 'userpass')

            r = requests.get(proxy_url, auth=auth)

        else:

            r = requests.get(url)

        return r


    def download_chapter(self):
        """Download chapter (backbone)."""
        pass

    def story_info(self):
        pass
