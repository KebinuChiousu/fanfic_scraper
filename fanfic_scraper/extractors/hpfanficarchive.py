"""Extractor for hpfanficarchive.com."""

from fanfic_scraper.base_fanfic import BaseFanfic, BaseChapter
from urllib.parse import urlparse, urljoin, parse_qs
from bs4 import BeautifulSoup, Comment
from collections import defaultdict
import re
import os
from datetime import datetime


def chapter_nav(tag):
    test = (tag.name == 'select')
    test = (test and 'chap_select' in tag['id'])

    return test


class HPFanficArchive(BaseFanfic):

    def get_fanfic_title(self, r):
        soup = BeautifulSoup(r.text, 'html5lib')

        for div in soup.find_all('div', {'id': 'pagetitle'}):
            ch_regex = re.compile(r'^viewstory.php\?sid=')
            title = div.find_all('a', href=ch_regex)[0]
            title = title.get_text()
            break

        return title

    def get_story_url(self, storyid):
        base_url = 'http://www.hpfanficarchive.com/stories/viewstory.php?sid='
        return base_url + storyid

    def extract_chapters(self):
        """Extract chapters function (backbone)."""
        fanfic_name = self.name

        url = self.url
        urlscheme = urlparse(url)

        # Set story_id from url
        self.fanfic_id = parse_qs(urlscheme.query,
                                  keep_blank_values=True)['sid'][0]

        # Get chapters
        r = self.send_request(url)
        soup = BeautifulSoup(r.text, 'html5lib')
        self.title = self.get_fanfic_title(r)
        chapters = defaultdict(HPFanficArchiveChapter)

        try:

            ch_regex = re.compile(r'^viewstory.php\?sid=')
            chapter_list = soup.find_all('a', href=ch_regex)

            for link in chapter_list:
                chapter = link.get('href')
                if 'chapter' in chapter:
                    chapter_link = urljoin(
                        urlscheme.scheme + "://" + urlscheme.netloc,
                        'stories/' + str(chapter))
                    ch_qs = parse_qs(urlparse(chapter_link).query)
                    chapter_num = ch_qs['chapter'][0]
                    chapter_num = int(chapter_num)
                    chapters[chapter_num] = HPFanficArchiveChapter(
                        self, chapter_num, chapter_link)

            return chapters

        except:
            return chapters

    def get_update_date(self):
        r = self.send_request(self.url)
        soup = BeautifulSoup(r.text, 'lxml')

        for c in soup.find_all(text=lambda text: isinstance(text, Comment)):
            if c in [' UPDATED START ']:
                update_date = c.next_element.strip()
                update_date = datetime.strptime(update_date, '%B %d, %Y')
                break

        return update_date


class HPFanficArchiveChapter(BaseChapter):
    """Base chapter class."""

    def get_fanfic_title(self, r):
        soup = BeautifulSoup(r.text, 'html5lib')

        regex = re.compile(r'^viewstory.php\?sid=')

        for div in soup.find_all('div', {'id': 'pagetitle'}):
            title = div.find_all('a', href=regex)[0]
            title = title.get_text()
            break

        return title

    def get_fanfic_author(self, r):
        soup = BeautifulSoup(r.text, 'html5lib')

        regex = re.compile(r'^viewuser.php\?uid=')

        for div in soup.find_all('div', {'id': 'pagetitle'}):
            author = div.find_all('a', href=regex)[0]
            author = author.get_text()
            break

        return author

    def get_fanfic_category(self, r):
        soup = BeautifulSoup(r.text, 'html5lib')
        category = ''

        regex = re.compile(r'^browse.php\?type=categories')

        desc = soup.find_all('div', {'class': 'content'})[2]
        cat = desc.find_all('a', href=regex)
        cat2 = []
        for a in cat:
            cat2.append(a.get_text())
        s = ', '
        category = s.join(cat2)

        return category

    def get_fanfic_genre(self, r):
        soup = BeautifulSoup(r.text, 'html5lib')
        category = ''

        regex = re.compile(r'type_id=1')

        desc = soup.find_all('div', {'class': 'content'})[2]
        cat = desc.find_all('a', href=regex)
        cat2 = []
        for a in cat:
            cat2.append(a.get_text())
        s = ', '
        category = s.join(cat2)

        return category

    def get_fanfic_description(self, r):
        soup = BeautifulSoup(r.text, 'html5lib')

        desc = soup.find_all('div', {'class': 'content'})[2]
        para = desc.find_all('p')
        temp = []
        for p in para:
            temp.append(p.get_text())
        desc = "".join(temp)
        return desc

    def get_update_date(self, r):
        soup = BeautifulSoup(r.text, 'lxml')

        for c in soup.find_all(text=lambda text: isinstance(text, Comment)):
            if c in [' UPDATED START ']:
                update_date = c.next_element.strip()
                break

        return update_date

    def get_publish_date(self, r):
        soup = BeautifulSoup(r.text, 'lxml')

        for c in soup.find_all(text=lambda text: isinstance(text, Comment)):
            if c in [' PUBLISHED START ']:
                publish_date = c.next_element.strip()
                break

        return publish_date

    def get_chapter_title(self, r):
        soup = BeautifulSoup(r.text, 'html5lib')

        chapters = soup.find_all('select', {'name': 'chapter'})[0]
        chapter_list = chapters.find_all('option')
        for option in chapter_list:
            if int(option.get('value')) == self.chapter_num:
                chapter_title = option.get_text()
                break

        return chapter_title

    def get_chapter_count(self, r):
        """Extract chapters function (backbone)."""

        soup = BeautifulSoup(r.text, 'html5lib')

        chapters = 0

        try:

            ch_regex = re.compile(r'^viewstory.php\?sid=')
            chapter_list = soup.find_all('a', href=ch_regex)

            for link in chapter_list:
                chapter = link.get('href')
                if 'chapter' in chapter:
                    chapters = chapters + 1

            return chapters

        except:
            return chapters

    def get_chapter_html(self, r):
        soup = BeautifulSoup(r.text, 'html5lib')

        story = soup.find_all('div', {'id': 'story'})[0]

        return str(story)

    def render_p(self, value):
        return '<p>' + value + '</p>'

    def story_info(self):

        r = self.send_request(self.fanfic_url)

        title = self.get_fanfic_title(r)
        author = self.get_fanfic_author(r)
        category = self.get_fanfic_category(r)
        genre = self.get_fanfic_genre(r)
        desc = self.get_fanfic_description(r)
        update_date = self.get_update_date(r)
        publish_date = self.get_publish_date(r)
        chapter_count = self.get_chapter_count(r)

        info = {}

        info['StoryId'] = self.fanfic_id
        info['Title'] = title
        info['Author'] = author
        info['Description'] = desc
        info['Publish_Date'] = publish_date
        info['Update_Date'] = update_date
        info['Count'] = chapter_count

        return info

    def download_chapter(self):

        filename = self.fanfic_name + '-%03d.htm' % (self.chapter_num)

        print(self.chapter_url)
        r = self.send_request(self.fanfic_url)

        title = self.get_fanfic_title(r)
        author = self.get_fanfic_author(r)
        category = self.get_fanfic_category(r)
        genre = self.get_fanfic_genre(r)
        desc = self.get_fanfic_description(r)
        update_date = self.get_update_date(r)
        publish_date = self.get_publish_date(r)
        chapter_count = self.get_chapter_count(r)

        r = self.send_request(self.chapter_url)

        chapter_title = self.get_chapter_title(r)
        story = self.get_chapter_html(r)

        # print(title)
        # print(author)
        # print('Categories: '+category)
        # print('Genres: '+genre)
        # print("Summary: ", textwrap.fill(desc))
        # print('Chapter '+chapter_title)
        # print('Published: '+publish_date)
        # print('Updated: '+update_date)
        # print(chapter_count)
        # print(story)

        target = os.path.join(self.fanfic_download_location, filename)

        if os.path.isfile(target):
            os.remove(target)

        f1 = open(target, "w")

        f1.write('<html>')
        f1.write('<body>')
        f1.write(self.render_p(title))
        f1.write(self.render_p(author))
        f1.write(self.render_p('Categories: ' + category))
        f1.write(self.render_p('Summary: ' + desc))
        f1.write(self.render_p('Chapter ' + chapter_title))
        if self.chapter_num == 1:
            f1.write(self.render_p('Published: ' + publish_date))
        if self.chapter_num == chapter_count:
            f1.write(self.render_p('Updated: ' + update_date))
        f1.write(self.render_p('========='))
        f1.write(story)

        f1.flush()
        os.fsync(f1.fileno())
        f1.close
