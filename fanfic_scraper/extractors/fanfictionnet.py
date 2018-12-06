"""Extractor for fanfiction.net."""

from fanfic_scraper.base_fanfic import BaseFanfic, BaseChapter
from urllib.parse import urlparse, urljoin
import requests
import bs4 as bsoup
from collections import defaultdict
import re
import os
import shutil
import textwrap
import datetime
from random import shuffle, uniform, randint
from copy import deepcopy
from time import sleep
from fake_useragent import UserAgent


def chapter_nav(tag):
    test = (tag.name == 'select')
    test = (test and 'chap_select' in tag['id'])

    return test


class FanfictionNetFanfic(BaseFanfic):

    def get_story_url(self, storyid):
        return 'https://www.fanfiction.net/s/'+storyid

    def extract_chapters(self):
        """Extract chapters function (backbone)."""
        fanfic_name = self.name
        url = self.url
        self.title = url.split('/')[-1]
        urlscheme = urlparse(url)

        #set story_id from url
        self.fanfic_id = urlscheme.path.split('/')[2]

        # Get chapters
        r = self.send_request(url)
        soup = bsoup.BeautifulSoup(r.text, 'html5lib')

        chapters = defaultdict(FanfictionNetChapter)

        #try:

        chapter_list = soup.find_all(chapter_nav)

        if chapter_list:
            chapter_list = chapter_list[0]

        if chapter_list:
            for link in chapter_list:
                chapter_num = int(link.get('value'))
                chapter_link = urljoin(
                    urlscheme.scheme + "://" + urlscheme.netloc,
                    "s/" + self.fanfic_id + "/" + str(chapter_num))
                chapters[chapter_num] = FanfictionNetChapter(
                    self, chapter_num, chapter_link)
        else:
            chapter_num = 1
            chapter_link = urljoin(
                urlscheme.scheme + "://" + urlscheme.netloc,
                "s/" + self.fanfic_id + "/" + str(chapter_num))
            chapters[chapter_num] = FanfictionNetChapter(
                self, chapter_num, chapter_link)

        return chapters

        #except:
        #    return chapters

    def get_update_date(self):
        r = self.send_request(self.url)
        soup = bsoup.BeautifulSoup(r.text, 'html5lib')

        for div in soup.find_all('div', {'id':'profile_top'}):
            span = div.find_all('span')[-2]
            ts = span.get('data-xutime')
            update_date = datetime.datetime.fromtimestamp(float(ts)).strftime('%Y-%m-%d')

            break

        return update_date

class FanfictionNetChapter(BaseChapter):
    """Base chapter class."""

    def get_fanfic_title(self,r):
        soup = bsoup.BeautifulSoup(r.text, 'html5lib')

        for div in soup.find_all('div', {'id':'profile_top'}):
            title = div.find_all('b')[0].get_text()
            break

        return title

    def get_fanfic_author(self,r):
        soup = bsoup.BeautifulSoup(r.text, 'html5lib')

        for div in soup.find_all('div', {'id':'profile_top'}):
            for link in div.find_all('a'):
                if '/u' in link.get('href'):
                    author = link.get_text()
                    break

        return author

    def get_fanfic_category(self,r):
        soup = bsoup.BeautifulSoup(r.text, 'html5lib')

        for div in soup.find_all('div', {'id':'pre_story_links'}):
            for link in div.find_all('a'):
                category = link.get_text()
                break

        return category

    def get_fanfic_description(self,r):
        soup = bsoup.BeautifulSoup(r.text, 'html5lib')

        for div in soup.find_all('div', {'id':'profile_top'}):
            description = div.find_all('div')[1].get_text()
            break

        return description

    def get_update_date(self,r):
        soup = bsoup.BeautifulSoup(r.text, 'html5lib')

        for div in soup.find_all('div', {'id':'profile_top'}):
            span = div.find_all('span')[-2]
            ts = span.get('data-xutime')

            if ts is None:
                span = div.find_all('span')[-1]
                ts = span.get('data-xutime')

            update_date = datetime.datetime.fromtimestamp(float(ts)).strftime('%Y-%m-%d')

            break

        return update_date

    def get_publish_date(self,r):
        soup = bsoup.BeautifulSoup(r.text, 'html5lib')

        for div in soup.find_all('div', {'id':'profile_top'}):
            span = div.find_all('span')[-1]
            ts = span.get('data-xutime')
            publish_date = datetime.datetime.fromtimestamp(float(ts)).strftime('%Y-%m-%d')

            break

        return publish_date

    def get_chapter_title(self,r):
        soup = bsoup.BeautifulSoup(r.text, 'html5lib')

        chapter_list = soup.find_all(chapter_nav)[0]
        for link in chapter_list:
            if int(link.get('value')) == self.chapter_num:
                chapter_title = link.get_text()
                break

        return chapter_title

    def get_chapter_count(self,r):
        soup = bsoup.BeautifulSoup(r.text, 'html5lib')

        chapter_list = soup.find_all(chapter_nav)

        if chapter_list:
            chapter_count = len(chapter_list[0])
        else:
            chapter_count = 1

        return chapter_count

    def get_chapter_html(self,r):
        soup = bsoup.BeautifulSoup(r.text, 'html5lib')

        story = soup.find_all('div',{'id':'storytext'})[0]

        return str(story)

    def render_p(self,value):
        return '<p>'+value+'</p>'

    def story_info(self):
        r = self.send_request(self.chapter_url)

        title = self.get_fanfic_title(r)
        author = self.get_fanfic_author(r)
        category = self.get_fanfic_category(r)
        desc = self.get_fanfic_description(r)
        update_date = self.get_update_date(r)
        publish_date = self.get_publish_date(r)
        chapter_count = self.get_chapter_count(r)

        info = {}

        info['StoryId'] = self.fanfic_id
        info['Title'] = title
        info['Author'] = author
        info['Category'] = category
        info['Description'] = desc
        info['Publish_Date'] = publish_date
        info['Update_Date'] = update_date
        info['Count'] = chapter_count

        return info

    def download_chapter(self):

        filename = self.fanfic_name+'-%03d.htm' % (self.chapter_num)

        r = self.send_request(self.chapter_url)

        title = self.get_fanfic_title(r)
        author = self.get_fanfic_author(r)
        category = self.get_fanfic_category(r)
        desc = self.get_fanfic_description(r)
        chapter_title = self.get_chapter_title(r)
        update_date = self.get_update_date(r)
        publish_date = self.get_publish_date(r)
        chapter_count = self.get_chapter_count(r)
        story = self.get_chapter_html(r)

        #print(title)
        #print(author)
        #print(category)
        #print("Summary: ", textwrap.fill(desc))
        #print('Chapter '+chapter_title)
        #print('Published: '+publish_date)
        #print('Updated: '+update_date)
        #print(chapter_count)
        #print(story)

        target = os.path.join(self.fanfic_download_location,filename)

        if os.path.isfile(target):
            os.remove(target)

        f1 = open(target, "w")

        f1.write('<html>')
        f1.write('<body>')
        f1.write(self.render_p(title))
        f1.write(self.render_p(author))
        f1.write(self.render_p(category))
        f1.write(self.render_p('Summary: '+desc))
        f1.write(self.render_p('Chapter '+chapter_title))
        if self.chapter_num == 1:
            f1.write(self.render_p('Published: '+publish_date))
        if self.chapter_num == chapter_count:
            f1.write(self.render_p('Updated: '+update_date))
        f1.write(self.render_p('========='))
        f1.write(story)


        f1.flush()
        os.fsync(f1.fileno())
        f1.close





